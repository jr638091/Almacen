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
    /// Lógica de interacción para PaginaSucursalEntrda.xaml
    /// </summary>
    public partial class PaginaSucursalEntrda : Page, IFormulario
    {
        private List<objeto_existencia> source_producto;
        private List<entrada> source_entrada;

        private string codigo_src;
        private string descrip_src;

        private int TiendaId;

        public PaginaSucursalEntrda()
        {
            InitializeComponent();
        }
        public PaginaSucursalEntrda(int id_tienda)
        {
            InitializeComponent();

            source_entrada = new List<entrada>();
            source_producto = new List<objeto_existencia>();

            codigo_src = "";
            descrip_src = "";

            TiendaId = id_tienda;

            using(var db = new TiendaDbContext())
            {
                var productos = db.Tiendas.First().Productos.Where(p => p.CantidadTotal>0);
                foreach(var item in productos)
                {
                    source_producto.Add(new objeto_existencia(item.ExistenciaId));
                }
            }

            dgrid_productos.ItemsSource = source_producto;
            dgrid_entrada.ItemsSource = source_entrada;
        }

        private void CancelEntry_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btn_Agregar_Click(object sender, RoutedEventArgs e)
        {
            var objeto_select = dgrid_productos.SelectedItem;
            if(objeto_select == null || !(objeto_select is objeto_existencia))
            {
                MessageBox.Show("Antes de agregar necesita seleccionar un producto");
                return;
            }
            var exist = objeto_select as objeto_existencia;

            source_producto.RemoveAt(source_producto.FindIndex(p => p.ExistenciaId == exist.ExistenciaId));
            source_entrada.Add(new entrada(exist.Codigo, exist.Descripcion));

            Metodos_Auxiliares.refresh(dgrid_entrada, source_entrada);
            Metodos_Auxiliares.refresh(dgrid_productos, source_producto.Where(s => s.Codigo.ToLower().Contains(codigo_src) && s.Descripcion.ToLower().Contains(descrip_src)).ToList());
        }

        private void btn_Eliminar_Click_1(object sender, RoutedEventArgs e)
        {
            var objeto_select = dgrid_entrada.SelectedItem;
            if (objeto_select == null || !(objeto_select is entrada))
            {
                MessageBox.Show("Antes de retirar necesita seleccionar un producto");
                return;
            }
            var exist = objeto_select as entrada;

            int existencia_id;
            using (var db = new TiendaDbContext())
                existencia_id = db.Tiendas.First().Productos.Where(p => p.Codigo == exist.Codigo).First().ExistenciaId;

            source_entrada.RemoveAt(source_entrada.FindIndex(p => p.Codigo == exist.Codigo));
            source_producto.Add(new objeto_existencia(existencia_id));

            Metodos_Auxiliares.refresh(dgrid_entrada, source_entrada);
            Metodos_Auxiliares.refresh(dgrid_productos, source_producto.Where(s => s.Codigo.ToLower().Contains(codigo_src) && s.Descripcion.ToLower().Contains(descrip_src)).ToList());
        }

        private void dgrid_entrada_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int valor;
            if (!int.TryParse((e.EditingElement as TextBox).Text, out valor))
            {
                e.Cancel = true;
                return;
            }
            valor = Math.Max(valor, 0);
            var objeto = e.Row.Item as entrada;
            Existencia exist;

            using (var db = new TiendaDbContext())
                exist = db.Tiendas.First().Productos.Where(p => p.Codigo == objeto.Codigo).First();

            var column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                var bindingPath = (column.Binding as Binding).Path.Path;
                if (bindingPath == "CantidadBuenEstado")
                    source_entrada.Find(a => a.Codigo == objeto.Codigo).CantidadBuenEstado = Math.Min(valor, exist.CantidadBuenEstado);
                else
                    source_entrada.Find(a => a.Codigo == objeto.Codigo).CantidadDefectuoso = Math.Min(valor, exist.CantidadDefectuoso);
            }

            Metodos_Auxiliares.refresh(dgrid_entrada, source_entrada);
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            codigo_src = tbox_codigo_src.Text.ToLower();
            descrip_src = tbox_descripcion_src.Text.ToLower();
            dgrid_productos.ItemsSource = source_producto.Where(s => s.Codigo.ToLower().Contains(codigo_src) && s.Descripcion.ToLower().Contains(descrip_src)).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fecha = DateTime.Now;
            using (var db = new TiendaDbContext())
            {
                var reporte_transferencia = new ReporteTransferencia
                {
                    Articulos = new List<ArticuloTransferencia>(),
                    Fecha = fecha,
                    ShopId = TiendaId,
                };

                db.ReporteTransferencias.Add(reporte_transferencia);
                db.SaveChanges();

                foreach (var item in source_entrada)
                {
                    var existencia = db.Existencias.Where(s => s.Codigo == item.Codigo && s.ShopId == 1).First();
                    existencia.CantidadBuenEstado -= item.CantidadBuenEstado;
                    existencia.CantidadDefectuoso -= item.CantidadDefectuoso;
                    db.Entry(existencia).State = EntityState.Modified;
                    if(db.Existencias.Where(s => s.Codigo == item.Codigo && s.ShopId == TiendaId).Count() == 0)
                    {
                        var new_existencia = new Existencia
                        {
                            CantidadBuenEstado = item.CantidadBuenEstado,
                            CantidadDefectuoso = item.CantidadDefectuoso,
                            Codigo = item.Codigo,
                            PrecioBuenEstado = existencia.PrecioBuenEstado,
                            PrecioDefectuoso = existencia.PrecioDefectuoso,
                            ShopId = TiendaId
                        };
                        db.Existencias.Add(new_existencia);
                    }
                    else
                    {
                        var exist = db.Existencias.Where(s => s.Codigo == item.Codigo && s.ShopId == TiendaId).First();
                        exist.CantidadBuenEstado += item.CantidadBuenEstado;
                        exist.CantidadDefectuoso += item.CantidadDefectuoso;
                        db.Entry(exist).State = EntityState.Modified;
                    }

                    // agragando articulos al reporte de transferencia

                    var articulo_transferencia = new ArticuloTransferencia
                    {
                        CantidadBuenEstado = item.CantidadBuenEstado,
                        CantidadDefectuoso = item.CantidadDefectuoso,
                        Codigo = item.Codigo,
                    };
                    reporte_transferencia.Articulos.Add(articulo_transferencia);
                }

                db.Entry(reporte_transferencia).State = EntityState.Modified;

                db.SaveChanges();
                
            }

            this.NavigationService.GoBack();
        }
    }
}
