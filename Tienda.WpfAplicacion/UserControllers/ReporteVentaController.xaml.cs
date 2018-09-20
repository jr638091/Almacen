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

        DataGrid dg = new DataGrid();
        public ReporteVentaController()
        {
            InitializeComponent();
            dg.HorizontalAlignment = HorizontalAlignment.Stretch;
            dg.VerticalAlignment = VerticalAlignment.Top;
            dg.Margin = new Thickness(0, 30, 0, 0);
        }

        public ReporteVentaController(object obj, List<string> header, List<string> path)
        {
            InitializeComponent();
            dg.Name = "dg_inform";
            dg.HorizontalAlignment = HorizontalAlignment.Stretch;
            dg.VerticalAlignment = VerticalAlignment.Top;
            dg.Margin = new Thickness(0, 30, 0, 0);
            dg.AutoGenerateColumns = false;
            dg.ItemsSource = (obj as IEnumerable<object>);
            for(int i = 0;i < path.Count;++i)
            {
                DataGridTextColumn t = new DataGridTextColumn();
                t.Binding = new Binding(path[i]);
                t.Header = header[i];
                t.IsReadOnly = true;
                dg.Columns.Add(t);
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
