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
        private List<Existencia> data_source;
        public MiTiendaPagina()
        {
            InitializeComponent();
        }
        private void Mis_Tiendas_Click(object sender, RoutedEventArgs e)
        {
            var p = new MisTiendas();
            this.NavigationService.Navigate(p);
        }

        private void Producto_Button_Click(object sender, RoutedEventArgs e)
        {
            var pagina = new Productos();
            this.NavigationService.Navigate(pagina);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                data_source = db.Tiendas.First().Productos.Where(p => p.CantidadBuenEstado > 0 || p.CantidadDefectuoso > 0).ToList();
                
                dgrid_productos.ItemsSource = data_source;
            }
        }

        private void dgrid_productos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgrid_productos.SelectedItem is Existencia)
            {
                string cod = (dgrid_productos.SelectedItem as Existencia).Codigo;
                using (var db = new TiendaDbContext())
                {
                    tblock_descripcion.Text = db.Productos.Find(cod).Descripcion;
                }
            }
            else
            {
                tblock_descripcion.Text = "Seleccione un producto para ver su descripcion";
            }
        }

        private void btn_Vender_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RealizarVentaPagina());
        }

        private void btn_cambiarPrecio_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_productos.SelectedItem == null || !(dgrid_productos.SelectedItem is Existencia))
            {
                MessageBox.Show("Tiene que seleccionar un producto");
                return;
            }
            var exist = dgrid_productos.SelectedItem as Existencia;
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
            if (!double.TryParse(tbox_precioBuenEstado.Text, out precioBE) || precioBE <= 0 || !double.TryParse(tbox_precioDefectuoso.Text, out precioDef) || precioDef <= 0)
            {
                MessageBox.Show("precio no valido");
                return;
            }
            data_source[indice_seleccion].PrecioBuenEstado = precioBE;
            data_source[indice_seleccion].PrecioDefectuoso = precioDef;

            using (var db = new TiendaDbContext())
            {
                db.Entry(data_source[indice_seleccion]).State = EntityState.Modified;
                db.SaveChanges();
            }
            dgrid_productos.Items.Refresh();
            gbox_modificarPrecio.Visibility = Visibility.Hidden;
            btn_cambiarPrecio.Visibility = Visibility.Visible;
        }
    }
}
