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
    /// Lógica de interacción para MisTiendas.xaml
    /// </summary>
    /// 
    public partial class MisTiendas : Page
    {
        private List<Shop> tiendas;
        public MisTiendas()
        {
            InitializeComponent();
            tiendas = new List<Shop>();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NuevaTiendaPagina());
        }

        private void Sucursal_btn_click(object sender, RoutedEventArgs e)
        {
            var a = tiendas.Find(x => x.ShopId == int.Parse((e.Source as Button).Name.Split('_')[1]));
            NavigationService.Navigate(new SucursalPagina(a.ShopId));
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            var a = tiendas.Find(x => x.ShopId == int.Parse((e.Source as Button).Name.Split('_')[1]));
            TiendaNombre.Content = a.Nombre;
            TiendaDireccion.Content = a.Direccion;
            using (var db = new TiendaDbContext())
            {
                var t = db.Tiendas.Find(a.ShopId);
                TiendaRepresentante.Content = t.Encargado.Nombre;
            }
            TiendaTelfono.Content = a.Telefono.ToString();

        }

        private void MenuControl_Loaded(object sender, RoutedEventArgs e)
        {
            DPTiendas.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                tiendas = db.Tiendas.Where(x => x.ShopId > 1).ToList();
            }

            foreach (var i in tiendas)
            {
                var tem = new Button();
                tem.Name = "t_" + i.ShopId.ToString();
                tem.Style = (Style)(FindResource("Tienda_btn"));
                tem.Content = i.Nombre.ToString();
                tem.MouseEnter += Button_MouseEnter;
                tem.Click += Sucursal_btn_click;
                DPTiendas.Children.Add(tem);
            }
        }
    }
}
