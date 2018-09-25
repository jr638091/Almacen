using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tienda.Modelo;

namespace WpfAplicacion
{
    /// <summary>
    /// Lógica de interacción para RealizarVentaPagina.xaml
    /// </summary>
    public partial class RealizarVentaPagina : Page, IFormulario
    {
        private List<objeto_venta> productos_source;
        private List<objeto_venta> venta_source;
        private List<Trabajador> vendedores;

        private string codigo_src;
        private string descrip_src;

        public DateTime Fecha { get; set; }

        public RealizarVentaPagina()
        {
            codigo_src = "";
            descrip_src = "";

            venta_source = new List<objeto_venta>();
            productos_source = new List<objeto_venta>();
            vendedores = new List<Trabajador>();

            using (var db = new TiendaDbContext())
            {
                vendedores = db.Tiendas.First().Trabajadores.OrderBy(x => x.Nombre).ToList();
                var temp = db.Tiendas.First().Productos.Where(p => p.CantidadBuenEstado > 0 || p.CantidadDefectuoso > 0);
                foreach (var item in temp)
                {
                    productos_source.Add(new objeto_venta(item.ExistenciaId));
                }
            }

            InitializeComponent();
            cb_trabajadores.ItemsSource = vendedores;
            cb_trabajadores.SelectedIndex = 0;
            cb_trabajadores.DisplayMemberPath = "Nombre";
            
            dgrid_productos.ItemsSource = productos_source;
            dgrid_venta.ItemsSource = venta_source;

            Fecha = DateTime.Today;
        }
        private void btn_cancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btn_Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_productos.SelectedItem == null || !(dgrid_productos.SelectedItem is objeto_venta))
            {
                MessageBox.Show("Necesita seleccionar un producto para agregar");
                return;
            }
            var exist = dgrid_productos.SelectedItem as objeto_venta;
            productos_source.RemoveAt(productos_source.FindIndex(p => p.Codigo == exist.Codigo));
            venta_source.Add(exist);

            dgrid_venta.ItemsSource = null;
            dgrid_productos.ItemsSource = productos_source.Where(exis => exis.Codigo.Contains(codigo_src) && exis.Descripcion.Contains(descrip_src)).ToList();
            dgrid_venta.ItemsSource = venta_source;

            double costo_total = 0;
            venta_source.ForEach(p => costo_total += p.PrecioTotal);
            tbox_pagado.Text = costo_total.ToString();
        }

        private void btn_Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_venta.SelectedItem == null || !(dgrid_venta.SelectedItem is objeto_venta))
            {
                MessageBox.Show("Necesita seleccionar un producto para remover");
                return;
            }

            var obj_venta = dgrid_venta.SelectedItem as objeto_venta;

            productos_source.Add(obj_venta);

            venta_source.RemoveAt(venta_source.FindIndex(p => p.Codigo == obj_venta.Codigo));
            Metodos_Auxiliares.refresh(dgrid_productos, productos_source.Where(exis => exis.Codigo.ToLower().Contains(codigo_src) && exis.Descripcion.ToLower().Contains(descrip_src)).ToList());
            Metodos_Auxiliares.refresh(dgrid_venta, venta_source);
            double costo_total = 0;
            venta_source.ForEach(p => costo_total += p.PrecioTotal);
            tbox_pagado.Text = costo_total.ToString();
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            codigo_src = tbox_codigo.Text.ToLower();
            descrip_src = tbox_descripcion.Text.ToLower();
            Metodos_Auxiliares.refresh(dgrid_productos, productos_source.Where(exis => exis.Codigo.ToLower().Contains(codigo_src) && exis.Descripcion.ToLower().Contains(descrip_src)).ToList());
        }
        
        private void dgrid_venta_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int value;
            if (!int.TryParse((e.EditingElement as TextBox).Text, out value))
            {
                e.Cancel = true;

                return; 
            }
            var column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                int id = (dgrid_venta.SelectedItem as objeto_venta).ExistenciaId;
                var art = venta_source.Find(p => p.ExistenciaId == id);
                if (value < 0)
                    value = 0;


                var bindingPath = (column.Binding as Binding).Path.Path;
                if(bindingPath == "CantidadBuenEstado")
                {
                    if (art.CantidadExistenteBE < value)
                        value = art.CantidadExistenteBE;
                    venta_source.Find(s => s.ExistenciaId == id).CantidadBuenEstado = value;
                }
                else
                {
                    if (art.CantidadExistenteDefec < value)
                        value = art.CantidadExistenteDefec;
                    venta_source.Find(s => s.ExistenciaId == id).CantidadDefectuoso = value;
                }
            }
            Metodos_Auxiliares.refresh(dgrid_venta, venta_source);
            double costo_total = 0;
            venta_source.ForEach(p => costo_total += p.PrecioTotal);
            tbox_pagado.Text = costo_total.ToString();
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            if(venta_source.Count() == 0)
            {
                MessageBox.Show("No puede realizar una venta de 0 articulos");
                return;
            }
            int trabajador_id = (cb_trabajadores.SelectedItem as Trabajador).TrabajadorId;


            var reporte = objeto_venta.generar_reporte(1, trabajador_id, venta_source, Fecha);
            double pagado;
            if(double.TryParse(tbox_pagado.Text, out pagado))
            {
                reporte.Pagado = pagado;
                using (var db = new TiendaDbContext())
                {
                    db.Entry(reporte).State = EntityState.Modified;
                    db.SaveChanges();

                    // refeljando la venta en las existencias

                    foreach(var item in venta_source)
                    {
                        var existencia = db.Existencias.Find(item.ExistenciaId);
                        existencia.CantidadBuenEstado -= item.CantidadBuenEstado;
                        existencia.CantidadDefectuoso -= item.CantidadDefectuoso;
                        db.Entry(existencia).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
            }
            this.NavigationService.GoBack();
        }
    }
}
