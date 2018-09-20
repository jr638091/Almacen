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
        public class infoProd
        {
            public string Nombre { get; set; }
            public string CantBE { get; set; }
            public string CantDef { get; set; }
            public infoProd(string Nombre, string CantBE, string CantDef)
            {
                this.Nombre = Nombre;
                this.CantBE = CantBE;
                this.CantDef = CantDef;
            }
        }

        private string codigo_src;
        private string descrip_src;
        private List<Producto> source_productos;
        public Productos()
        {
            InitializeComponent();
            codigo_src = "";
            descrip_src = "";
            source_productos = new List<Producto>();
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
                source_productos = db.Productos.Where(p => p.Codigo.Contains(codigo_src) && p.Descripcion.Contains(descrip_src)).ToList();
                dgrid_productos.ItemsSource = source_productos;
            }
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            codigo_src = tbox_codigo_src.Text;
            descrip_src = tbox_descripcion_src.Text;
            dgrid_productos.ItemsSource = source_productos.Where(exis => exis.Codigo.Contains(codigo_src) && exis.Descripcion.Contains(descrip_src)).ToList();
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
                    List<infoProd> infos = new List<infoProd>();
                    foreach (var item in tiendas)
                    {
                        infos.Add(new infoProd(item.Nombre, item.Productos.First(x => x.Codigo == producto.Codigo).CantidadBuenEstado.ToString(), item.Productos.First(x => x.Codigo == producto.Codigo).CantidadDefectuoso.ToString()));
                    }
                    dgrid_existencia.ItemsSource = infos;

                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MiTiendaPagina());
        }
    }
}
