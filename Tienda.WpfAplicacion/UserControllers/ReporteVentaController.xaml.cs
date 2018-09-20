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

namespace WpfAplicacion.UserControllers
{
    /// <summary>
    /// Lógica de interacción para ReporteVentaController.xaml
    /// </summary>
    public partial class ReporteVentaController : UserControl
    {
        StackPanel dg = new StackPanel();
        public ReporteVentaController()
        {
            InitializeComponent();
            dg.HorizontalAlignment = HorizontalAlignment.Stretch;
            dg.VerticalAlignment = VerticalAlignment.Top;
            dg.Margin = new Thickness(0, 30, 0, 0);
        }

        public ReporteVentaController(params UIElement[] args)
        {
            InitializeComponent();
            dg.HorizontalAlignment = HorizontalAlignment.Stretch;
            dg.VerticalAlignment = VerticalAlignment.Top;
            dg.Margin = new Thickness(0, 30, 0, 0);
            foreach(var i in args)
            {
                dg.Children.Add(i);
            }
            
        }

        private void expand_btn_Click(object sender, RoutedEventArgs e)
        {
            expand_btn.Visibility = Visibility.Hidden;
            colapse_btn.Visibility = Visibility.Visible;
            grid_base.Children.Add(dg);
        }

        private void colapse_btn_Click(object sender, RoutedEventArgs e)
        {
            expand_btn.Visibility = Visibility.Visible;
            colapse_btn.Visibility = Visibility.Hidden;
            grid_base.Children.Remove(dg);
        }
    }
}
