﻿using System;
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


namespace WpfAplicacion
{
    /// <summary>
    /// Lógica de interacción para LiquidacionPagina.xaml
    /// </summary>
    public partial class LiquidacionPagina : Page, IFormulario
    {
        private List<objeto_venta> source_venta;
        private List<objeto_devolucion> source_devolucion;
        private List<objeto_deuda> source_deuda;
        private List<objeto_existencia> source_existencia;

        private int tienda_id;

        private double CostoVenta;
        private double CostoDeuda;
        private double CostoTotal
        {
            get { return CostoVenta + CostoDeuda; }
        }

        public LiquidacionPagina(int id_tienda)
        {
            InitializeComponent();

            this.tienda_id = id_tienda;

            source_venta = new List<objeto_venta>();
            source_devolucion = new List<objeto_devolucion>();
            source_deuda = new List<objeto_deuda>();
            source_existencia = new List<objeto_existencia>();

            using (var db = new TiendaDbContext())
            {
                var tienda = db.Tiendas.Find(id_tienda);
                var existencia = tienda.Productos;

                foreach(var item in existencia)
                {
                    if (item.CantidadTotal > 0)
                    {
                        source_existencia.Add(new objeto_existencia(item.ExistenciaId));
                    }
                }
            }

            dgrid_existencia.ItemsSource = source_existencia;
            dgrid_deuda.ItemsSource = source_deuda;
            dgrid_devolucion.ItemsSource = source_devolucion;
            dgrid_venta.ItemsSource = source_venta;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btn_agragar_venta_Click(object sender, RoutedEventArgs e)
        {
            if(dgrid_existencia.SelectedItem == null || !(dgrid_existencia.SelectedItem is objeto_existencia))
            {
                MessageBox.Show("Debe seleccionar un producto antes de agregar");
                return;
            }

            var existencia = dgrid_existencia.SelectedItem as objeto_existencia;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;

            if (existe_venta)
                return;
            var venta_new = new objeto_venta(existencia.ExistenciaId);
            source_venta.Add(venta_new);

            if (existe_devolucion && existe_deuda)
            {
                source_existencia.RemoveAt(source_existencia.FindIndex(p => p.ExistenciaId == existencia.ExistenciaId));
                Metodos_Auxiliares.refresh(dgrid_existencia, source_existencia);
            }
            Metodos_Auxiliares.refresh(dgrid_venta, source_venta);
        }

        private void btn_agragar_devolucion_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_existencia.SelectedItem == null || !(dgrid_existencia.SelectedItem is objeto_existencia))
            {
                MessageBox.Show("Debe seleccionar un producto antes de agregar");
                return;
            }

            var existencia = dgrid_existencia.SelectedItem as objeto_existencia;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;

            if (existe_devolucion)
                return;
            var devolucion_new = new objeto_devolucion(existencia.ExistenciaId);
            source_devolucion.Add(devolucion_new);

            if (existe_venta && existe_deuda)
            {
                source_existencia.RemoveAt(source_existencia.FindIndex(p => p.ExistenciaId == existencia.ExistenciaId));

                Metodos_Auxiliares.refresh(dgrid_existencia, source_existencia);
            }
            Metodos_Auxiliares.refresh(dgrid_devolucion, source_devolucion);
        }

        private void btn_agragar_deuda_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_existencia.SelectedItem == null || !(dgrid_existencia.SelectedItem is objeto_existencia))
            {
                MessageBox.Show("Debe seleccionar un producto antes de agregar");
                return;
            }

            var existencia = dgrid_existencia.SelectedItem as objeto_existencia;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == existencia.ExistenciaId) != null;

            if (existe_deuda)
                return;
            var deuda_new = new objeto_deuda(existencia.ExistenciaId);
            source_deuda.Add(deuda_new);

            if (existe_devolucion && existe_venta)
            {
                source_existencia.RemoveAt(source_existencia.FindIndex(p => p.ExistenciaId == existencia.ExistenciaId));

                Metodos_Auxiliares.refresh(dgrid_existencia, source_existencia);
            }
            Metodos_Auxiliares.refresh(dgrid_deuda, source_deuda);
        }

        private void btn_retirar_venta_Click(object sender, RoutedEventArgs e)
        {
            if(dgrid_venta.SelectedItem == null || !(dgrid_venta.SelectedItem is objeto_venta))
            {
                MessageBox.Show("Necesita seleccionar un producto para retirar");
                return;
            }

            var objeto = dgrid_venta.SelectedItem as objeto_venta;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_existencia = source_existencia.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;

            if(!existe_existencia)
            {
                var existencia = new objeto_existencia(objeto.ExistenciaId);
                source_existencia.Add(existencia);

                dgrid_existencia.ItemsSource = null;
                dgrid_existencia.ItemsSource = source_existencia;
            }

            source_venta.RemoveAt(source_venta.FindIndex(p => p.ExistenciaId == objeto.ExistenciaId));

            dgrid_venta.ItemsSource = null;
            dgrid_venta.ItemsSource = source_venta;
        }

        private void btn_retirar_devolucion_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_devolucion.SelectedItem == null || !(dgrid_devolucion.SelectedItem is objeto_devolucion))
            {
                MessageBox.Show("Necesita seleccionar un producto para retirar");
                return;
            }

            var objeto = dgrid_devolucion.SelectedItem as objeto_devolucion;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_existencia = source_existencia.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;

            if (!existe_existencia)
            {
                var existencia = new objeto_existencia(objeto.ExistenciaId);
                source_existencia.Add(existencia);

                dgrid_existencia.ItemsSource = null;
                dgrid_existencia.ItemsSource = source_existencia;
            }

            source_devolucion.RemoveAt(source_devolucion.FindIndex(p => p.ExistenciaId == objeto.ExistenciaId));

            dgrid_devolucion.ItemsSource = null;
            dgrid_devolucion.ItemsSource = source_devolucion;
        }

        private void btn_retirar_deuda_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_deuda.SelectedItem == null || !(dgrid_deuda.SelectedItem is objeto_deuda))
            {
                MessageBox.Show("Necesita seleccionar un producto para retirar");
                return;
            }

            var objeto = dgrid_deuda.SelectedItem as objeto_deuda;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_existencia = source_existencia.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;

            if (!existe_existencia)
            {
                var existencia = new objeto_existencia(objeto.ExistenciaId);
                source_existencia.Add(existencia);

                dgrid_existencia.ItemsSource = null;
                dgrid_existencia.ItemsSource = source_existencia;
            }

            
            //dgrid_deuda.Items.Remove(source_deuda[0]);

            source_deuda.RemoveAt(source_deuda.FindIndex(p => p.ExistenciaId == objeto.ExistenciaId));

            dgrid_deuda.ItemsSource = null;
            dgrid_deuda.ItemsSource = source_deuda;
        }

        private void dgrid_venta_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int valor;
            if (!int.TryParse((e.EditingElement as TextBox).Text, out valor))
            {
                e.Cancel = true;
                return;
            }
            if (!(e.Row.Item is objeto_venta))
            {
                return;
            }
            var objeto = e.Row.Item as objeto_venta;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_existencia = source_existencia.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;

            int cantidadBuenEstado = 0;
            int cantidadDefectuoso = 0;

            if (existe_deuda)
            {
                cantidadBuenEstado += source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
                cantidadDefectuoso += source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadDefectuoso;

            }
            if (existe_devolucion)
            {
                cantidadBuenEstado += source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
                cantidadDefectuoso += source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
            }
            int cantidadBEmax;
            int cantidadDefmax;
            using (var db = new TiendaDbContext())
            {
                var exist = db.Existencias.Find(objeto.ExistenciaId);
                cantidadBEmax = exist.CantidadBuenEstado - cantidadBuenEstado;
                cantidadDefmax = exist.CantidadDefectuoso - cantidadDefectuoso;
            }
            cantidadBuenEstado = Math.Max(0, valor);
            cantidadDefectuoso = Math.Max(0, valor);
            cantidadBuenEstado = Math.Min(cantidadBEmax, valor);
            cantidadDefectuoso = Math.Min(cantidadDefmax, valor);
            
            var column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                var bindingPath = (column.Binding as Binding).Path.Path;
                if (bindingPath == "CantidadBuenEstado")
                    source_venta.Find(d => d.Codigo == objeto.Codigo).CantidadBuenEstado = cantidadBuenEstado;
                else
                    source_venta.Find(d => d.Codigo == objeto.Codigo).CantidadDefectuoso = cantidadDefectuoso;
            }

            Metodos_Auxiliares.refresh(dgrid_venta, source_venta);
            CostoVenta = 0;
            source_venta.ForEach(s => CostoVenta += s.PrecioTotal);

            tbox_importe_total.Text = CostoTotal.ToString();
        }

        private void dgrid_devolucion_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int valor;
            if (!int.TryParse((e.EditingElement as TextBox).Text, out valor))
            {
                e.Cancel = true;
                return;
            }
            if (!(e.Row.Item is objeto_devolucion))
            {
                return;
            }
            var objeto = e.Row.Item as objeto_devolucion;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_existencia = source_existencia.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;

            int cantidadBuenEstado = 0;
            int cantidadDefectuoso = 0;

            if (existe_deuda)
            {
                cantidadBuenEstado += source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
                cantidadDefectuoso += source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadDefectuoso;

            }
            if (existe_venta)
            {
                cantidadBuenEstado += source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
                cantidadDefectuoso += source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
            }
            int cantidadBEmax;
            int cantidadDefmax;
            using (var db = new TiendaDbContext())
            {
                var exist = db.Existencias.Find(objeto.ExistenciaId);
                cantidadBEmax = exist.CantidadBuenEstado - cantidadBuenEstado;
                cantidadDefmax = exist.CantidadDefectuoso - cantidadDefectuoso;
            }
            cantidadBuenEstado = Math.Max(0, valor);
            cantidadDefectuoso = Math.Max(0, valor);
            cantidadBuenEstado = Math.Min(cantidadBEmax, valor);
            cantidadDefectuoso = Math.Min(cantidadDefmax, valor);

            

            var column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                var bindingPath = (column.Binding as Binding).Path.Path;
                if (bindingPath == "CantidadBuenEstado")
                    source_devolucion.Find(s => s.Codigo == objeto.Codigo).CantidadBuenEstado = cantidadBuenEstado;
                else
                    source_devolucion.Find(s => s.Codigo == objeto.Codigo).CantidadDefectuoso = cantidadDefectuoso;
            }

            Metodos_Auxiliares.refresh(dgrid_devolucion, source_devolucion);
        }

        private void dgrid_deuda_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var columna = e.Column as DataGridBoundColumn;
            if(columna!= null && (columna.Binding as Binding).Path.Path == "Pagado")
            {
                double value = 0;
                if(!double.TryParse((e.EditingElement as TextBox).Text, out value))
                {
                    e.Cancel = true;
                    return;
                }
                if (!(e.Row.Item is objeto_deuda))
                {
                    return;
                }
                var obj = e.Row.Item as objeto_deuda;

                value = Math.Max(0, value);
                value = Math.Min(value, obj.PrecioTotal);


                source_deuda.Find(s => s.Codigo == obj.Codigo).Pagado = value;

                CostoDeuda = 0;
                source_deuda.ForEach(s => CostoDeuda += s.Pagado);
                tbox_importe_total.Text = CostoTotal.ToString();
                Metodos_Auxiliares.refresh(dgrid_deuda, source_deuda);
                return;
            }
            int valor;
            if (!int.TryParse((e.EditingElement as TextBox).Text, out valor))
            {
                e.Cancel = true;
                return;
            }
            if (!(e.Row.Item is objeto_deuda))
            {
                return;
            }
            var objeto = e.Row.Item as objeto_deuda;

            bool existe_venta = source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_deuda = source_deuda.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_devolucion = source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;
            bool existe_existencia = source_existencia.Find(p => p.ExistenciaId == objeto.ExistenciaId) != null;

            int cantidadBuenEstado = 0;
            int cantidadDefectuoso = 0;

            if (existe_venta)
            {
                cantidadBuenEstado += source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
                cantidadDefectuoso += source_venta.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadDefectuoso;

            }
            if (existe_devolucion)
            {
                cantidadBuenEstado += source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
                cantidadDefectuoso += source_devolucion.Find(p => p.ExistenciaId == objeto.ExistenciaId).CantidadBuenEstado;
            }
            int cantidadBEmax;
            int cantidadDefmax;
            using (var db = new TiendaDbContext())
            {
                var exist = db.Existencias.Find(objeto.ExistenciaId);
                cantidadBEmax = exist.CantidadBuenEstado - cantidadBuenEstado;
                cantidadDefmax = exist.CantidadDefectuoso - cantidadDefectuoso;
            }
            cantidadBuenEstado = Math.Max(0, valor);
            cantidadDefectuoso = Math.Max(0, valor);
            cantidadBuenEstado = Math.Min(cantidadBEmax, valor);
            cantidadDefectuoso = Math.Min(cantidadDefmax, valor);

            
            var column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                var bindingPath = (column.Binding as Binding).Path.Path;
                if (bindingPath == "CantidadBuenEstado")
                    source_deuda.Find(s => s.Codigo == objeto.Codigo).CantidadBuenEstado = cantidadBuenEstado;
                else
                    source_deuda.Find(s => s.Codigo == objeto.Codigo).CantidadDefectuoso = cantidadDefectuoso;
            }

            Metodos_Auxiliares.refresh(dgrid_deuda, source_deuda);

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var art_deuda = source_deuda.Where(p => p.CantidadTotal > 0).Count() > 0 ? source_deuda.Where(p => p.CantidadTotal > 0).ToList() : new List<objeto_deuda>();
                var art_devolucion = source_devolucion.Where(p => p.CantidadTotal > 0).Count() > 0 ? source_devolucion.Where(p => p.CantidadTotal > 0).ToList() : new List<objeto_devolucion>();
                var art_venta = source_venta.Where(p => p.CantidadTotal > 0).Count() > 0 ? source_venta.Where(p => p.CantidadTotal > 0).ToList() : new List<objeto_venta>();

                if(art_deuda.Count() + art_devolucion.Count() + art_venta.Count() == 0)
                {
                    MessageBox.Show("No se ha agregado nada");
                    return;
                }

                var reporte_deuda = objeto_deuda.generar_reporte(tienda_id, art_deuda);
                var reporte_devolucion = objeto_devolucion.generar_reporte(tienda_id ,art_devolucion);
                var tienda = db.Tiendas.Find(tienda_id);
                var reporte_venta = objeto_venta.generar_reporte(tienda_id, tienda.Encargado.TrabajadorId, art_venta);

                var informe = Metodos_Auxiliares.genera_informe(tienda_id, reporte_deuda, reporte_devolucion, reporte_venta);

                foreach(var item in art_deuda)
                {
                    var existencia = db.Existencias.Find(item.ExistenciaId);
                    existencia.CantidadBuenEstado -= item.CantidadBuenEstado;
                    existencia.CantidadDefectuoso -= item.CantidadDefectuoso;
                    db.Entry(existencia).State = EntityState.Modified;
                    db.SaveChanges();
                }

                foreach (var item in art_venta)
                {
                    var existencia = db.Existencias.Find(item.ExistenciaId);
                    existencia.CantidadBuenEstado -= item.CantidadBuenEstado;
                    existencia.CantidadDefectuoso -= item.CantidadDefectuoso;
                    db.Entry(existencia).State = EntityState.Modified;
                    db.SaveChanges();
                }

                foreach (var item in art_devolucion)
                {
                    var existencia_origen = db.Existencias.Find(item.ExistenciaId);
                    var existencia_destino = db.Existencias.Where(p => p.Codigo == item.Codigo).First();

                    existencia_origen.CantidadBuenEstado -= item.CantidadBuenEstado;
                    existencia_origen.CantidadDefectuoso -= item.CantidadDefectuoso;

                    existencia_destino.CantidadBuenEstado += item.CantidadBuenEstado;
                    existencia_destino.CantidadDefectuoso += item.CantidadDefectuoso;

                    db.Entry(existencia_origen).State = EntityState.Modified;
                    db.Entry(existencia_destino).State = EntityState.Modified;

                    db.SaveChanges();
                }
            }
            this.NavigationService.GoBack();
        }
    }
}
