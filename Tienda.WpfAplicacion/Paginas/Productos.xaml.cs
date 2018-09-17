using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : Page
    {
        public Productos()
        {
            InitializeComponent();            
        }

        private void Entrada_Button_Click(object sender, RoutedEventArgs e)
        {
            var p = new Entrada();
            this.NavigationService.Navigate(p);
        }

        private void Mis_Tiendas_Click(object sender, RoutedEventArgs e)
        {
            var p = new MisTiendas();
            this.NavigationService.Navigate(p);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var productos = db.Productos.Where(p => p.Codigo.Contains(tbox_codigo.Text));
                dgrid_productos.ItemsSource = productos.ToList();
            }
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var productos = db.Productos.Where(p => p.Codigo.Contains(tbox_codigo.Text));
                dgrid_productos.ItemsSource = productos.ToList();

            }
        }

        private void dgrid_productos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgrid_productos.SelectedItem != null && dgrid_productos.SelectedItem is Producto)
            {
                var producto = dgrid_productos.SelectedItem as Producto;
                dgrid_existencia.ItemsSource = new List<object>();
                using (var db = new TiendaDbContext())
                {
                    var tiendas = db.Tiendas.Where(t => t.Productos.Where(p => p.Codigo == producto.Codigo && (p.CantidadBuenEstado > 0 || p.CantidadDefectuoso > 0)).Count() > 0).ToList();
                    dgrid_existencia.ItemsSource = tiendas;

                }
            }
                    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MiTiendaPagina());
        }
    }
}
