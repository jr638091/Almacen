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
    /// Lógica de interacción para SucursalPagina.xaml
    /// </summary>
    public partial class SucursalPagina : Page
    {
        int tienda;
        public SucursalPagina()
        {
            InitializeComponent();
        }

        public SucursalPagina(int shopId)
        {
            InitializeComponent();
            tienda = shopId;
            using (var db = new TiendaDbContext())
            {
                var tienda = db.Tiendas.First(x => x.ShopId == shopId);
                Tienda_Encargado.Text = tienda.Encargado.Nombre;
                top.Title = tienda.Nombre;
                Tienda_Nombre.Text = tienda.Nombre;
                Tienda_Direccion.Text = tienda.Direccion;
            
                Tienda_Telefono.Text = tienda.Telefono.ToString();
            }
        }

        private void btn_Informe_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LiquidacionPagina(tienda));
        }

        private void btn_entrada_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaginaSucursalEntrda(tienda));

        }

        private void Edit_btn_Click(object sender, MouseButtonEventArgs e)
        {
            Tienda_Direccion.IsEnabled = true;
            Tienda_Encargado.IsEnabled = true;
            Tienda_Nombre.IsEnabled = true;
            Tienda_Telefono.IsEnabled = true;
            Edit_btn.Visibility = Visibility.Hidden;
            Accept_btn.Visibility = Visibility.Visible;
            Canel_btn.Visibility = Visibility.Visible;
        }

        private void Accept_btn_click(object sender, MouseButtonEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var t = db.Tiendas.First(x => x.ShopId == tienda);
                t.Nombre = Tienda_Nombre.Text;
                t.Telefono = int.Parse(Tienda_Telefono.Text);
                t.Direccion = Tienda_Direccion.Text;
                var c = db.Trabajadores.First(x => x.ShopId == t.ShopId);
                c.Nombre = Tienda_Encargado.Text;
                db.SaveChanges();
                top.Title = t.Nombre;
            }
            Tienda_Direccion.IsEnabled = false;
            Tienda_Encargado.IsEnabled = false;
            Tienda_Nombre.IsEnabled = false;
            Tienda_Telefono.IsEnabled = false;
            Edit_btn.Visibility = Visibility.Visible;
            Accept_btn.Visibility = Visibility.Hidden;
            Canel_btn.Visibility = Visibility.Hidden;
        }

        private void Cancel_btn_click(object sender, MouseButtonEventArgs e)
        {
            Tienda_Direccion.IsEnabled = false;
            Tienda_Encargado.IsEnabled = false;
            Tienda_Nombre.IsEnabled = false;
            Tienda_Telefono.IsEnabled = false;
            Edit_btn.Visibility = Visibility.Visible;
            Accept_btn.Visibility = Visibility.Hidden;
            Canel_btn.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Formularios.DeudasLiquidacionPagina(tienda));
        }

        class art_temp
        {
            public string Codigo { get; set; }
            public string Decripcion { get; set; }
            public int CantidadBuenEstado { get; set; }
            public int CantidadDefectuoso { get; set; }
            public int CantidadTotal { get; set; }
            public double PrecioBuenEstado { get; set; }
            public double PrecioDefectuoso { get; set; }
            public art_temp(string cod, string des, int cbe, int cme, int ct, double pbe, double pme)
            {
                Codigo = cod;
                Decripcion = des;
                CantidadBuenEstado = cbe;
                CantidadDefectuoso = cme;
                CantidadTotal = ct;
                PrecioBuenEstado = pbe;
                PrecioDefectuoso = pme;
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using(var db = new TiendaDbContext())
            {
                List<art_temp> t = new List<art_temp>();
                foreach (var item in db.Tiendas.Find(tienda).Productos.Where(x => x.CantidadTotal > 0))
                {
                    t.Add(new art_temp(item.Codigo, item.Producto.Descripcion, item.CantidadBuenEstado, item.CantidadDefectuoso, item.CantidadTotal, item.PrecioBuenEstado, item.PrecioDefectuoso));
                }
                dg_prods.ItemsSource = t;
            }
        }
    }
}
