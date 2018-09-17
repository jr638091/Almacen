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
    /// Lógica de interacción para SucursalPagina.xaml
    /// </summary>
    public partial class SucursalPagina : Page
    {
        public SucursalPagina()
        {
            InitializeComponent();
        }

        private void btn_Informe_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LiquidacionPagina());
        }

        private void btn_entrada_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaginaSucursalEntrda());

        }
    }
}
