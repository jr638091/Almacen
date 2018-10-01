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
    /// Lógica de interacción para MiTiendaPagina.xaml
    /// </summary>
    public partial class MiTiendaPagina : Page
    {
        private int indice_seleccion;
        private List<objeto_existencia> data_source;
        private List<Trabajador> data_trabajador;
        public MiTiendaPagina()
        {
            InitializeComponent();
        }
        

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                data_trabajador = db.Tiendas.First().Trabajadores.OrderBy(o => o.Nombre).Where(x => !x.eliminado).ToList();
                dg_trab.ItemsSource = data_trabajador;
                data_source = new List<objeto_existencia>();
                foreach(var item in db.Tiendas.First().Productos.Where(p => p.CantidadBuenEstado > 0 || p.CantidadDefectuoso > 0))
                {
                    data_source.Add(new objeto_existencia(item.ExistenciaId));
                }
                
                dgrid_productos.ItemsSource = data_source;
            }
        }

        private void btn_Vender_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RealizarVentaPagina());
        }

        private void btn_cambiarPrecio_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_productos.SelectedItem == null || !(dgrid_productos.SelectedItem is objeto_existencia))
            {
                MessageBox.Show("Tiene que seleccionar un producto");
                return;
            }
            var exist = dgrid_productos.SelectedItem as objeto_existencia;
            indice_seleccion = dgrid_productos.SelectedIndex;

            tbox_precioBuenEstado.Text = exist.PrecioBuenEstado.ToString();
            tbox_precioDefectuoso.Text = exist.PrecioDefectuoso.ToString();
            btn_cambiarPrecio.Visibility = Visibility.Hidden;
            gbox_modificarPrecio.Visibility = Visibility.Visible;
        }
        private void btn_cancelarPrecio_Click(object sender, RoutedEventArgs e)
        {
            gbox_modificarPrecio.Visibility = Visibility.Hidden;
            btn_cambiarPrecio.Visibility = Visibility.Visible;

        }

        private void btn_guardarPrecio_Click(object sender, RoutedEventArgs e)
        {
            double precioBE;
            double precioDef;
            if (!double.TryParse(tbox_precioBuenEstado.Text, out precioBE) || precioBE < 0 || !double.TryParse(tbox_precioDefectuoso.Text, out precioDef) || precioDef < 0)
            {
                MessageBox.Show("precio no valido");
                return;
            }
            var articulo = dgrid_productos.SelectedItem as objeto_existencia;
            var objeto_data_source = data_source.Find(p => p.ExistenciaId == articulo.ExistenciaId);
            objeto_data_source.PrecioBuenEstado = precioBE;
            objeto_data_source.PrecioDefectuoso = precioDef;

            using (var db = new TiendaDbContext())
            {
                var existencia = db.Existencias.Find(articulo.ExistenciaId);
                existencia.PrecioBuenEstado = precioBE;
                existencia.PrecioDefectuoso = precioDef;
                db.Entry(existencia).State = EntityState.Modified;
                db.SaveChanges();
            }
            dgrid_productos.Items.Refresh();
            gbox_modificarPrecio.Visibility = Visibility.Hidden;
            btn_cambiarPrecio.Visibility = Visibility.Visible;
            gbox_totales_Loaded(sender, e);
        }
        

        private void dg_trab_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var row = e.Row.Item as Trabajador;
            if (row.TrabajadorId == 0)
            {
                using( var db = new TiendaDbContext())
                {
                    var tienda = db.Tiendas.First();
                    var trabajador = new Trabajador {
                        Nombre = (e.EditingElement as TextBox).Text,
                        ShopId = tienda.ShopId
                    };
                    if (trabajador.Nombre == "")
                    {
                        e.Cancel = true;
                        return;
                    }
                    db.Trabajadores.Add(trabajador);
                    db.SaveChanges();
                    data_trabajador.Add(trabajador);
                    Metodos_Auxiliares.refresh(dg_trab, data_trabajador);
                }
            }
            else
                if(MessageBox.Show("Esta seguro","Cambio de Nombre", MessageBoxButton.YesNo) == MessageBoxResult.Yes){
                    using (var db = new TiendaDbContext())
                    {
                        db.Trabajadores.Find((dg_trab.SelectedItem as Trabajador).TrabajadorId).Nombre = (e.EditingElement as TextBox).Text;
                        db.SaveChanges();
                    }
                }
        }

        private void delete_empleado_Click(object sender, RoutedEventArgs e)
        {
            if (dg_trab.SelectedItem == null || !(dg_trab.SelectedItem is Trabajador))
                MessageBox.Show("Debe escoger un trabajador");
            else
            {
                if(MessageBox.Show("Esta seguro", "Eliminar Trabajador", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    using(var db = new TiendaDbContext())
                    {
                        db.Trabajadores.Find((dg_trab.SelectedItem as Trabajador).TrabajadorId).eliminado = true;
                        db.SaveChanges();
                        data_trabajador.Remove(dg_trab.SelectedItem as Trabajador);
                        Metodos_Auxiliares.refresh(dg_trab, data_trabajador);
                    }

            }
        }

        private void gbox_totales_Loaded(object sender, RoutedEventArgs e)
        {
            using(var db = new TiendaDbContext())
            {
                double valor_total = 0;
                int cantidad_total = 0;
                foreach(var item in db.Tiendas.First().Productos.Where(p => p.CantidadTotal>0))
                {
                    cantidad_total += item.CantidadTotal;
                    valor_total += item.CantidadBuenEstado * item.PrecioBuenEstado;
                    valor_total += item.CantidadDefectuoso * item.PrecioDefectuoso;
                }
                label_cantidad_total.Content = cantidad_total.ToString();
                label_valor_total.Content = valor_total.ToString("0.00");
            }
        }
    }
}
