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

namespace WpfAplicacion
{
    /// <summary>
    /// Lógica de interacción para MisTiendas.xaml
    /// </summary>
    public partial class MisTiendas : Page
    {
        public MisTiendas()
        {
            InitializeComponent();
        }

        private void Producto_Button_Click(object sender, RoutedEventArgs e)
        {
            var p = new Productos();
            this.NavigationService.Navigate(p);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var p = new SucursalPagina();
            this.NavigationService.Navigate(p);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NuevaTiendaPagina());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MiTiendaPagina());
        }
    }
}
