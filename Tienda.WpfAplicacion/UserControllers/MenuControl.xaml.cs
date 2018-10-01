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
    /// Lógica de interacción para MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
        }

        private DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)

        {

            //Walk the visual tree to get the parent(ItemsControl)

            //of this control

            DependencyObject parent = startObject;

            while (parent != null)

            {

                if (type.IsInstanceOfType(parent))

                    break;

                else

                    parent = VisualTreeHelper.GetParent(parent);

            }

            return parent;

        }

        private void Productos_click_btn(object sender, MouseButtonEventArgs e)
        {
            Page pg = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;

            var p = new Productos();

            pg.NavigationService.Navigate(p);
        }
        private void Mi_Tienda_click_btn(object sender, MouseButtonEventArgs e)
        {
            Page pg = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;

            var p = new MiTiendaPagina();

            pg.NavigationService.Navigate(p);
        }
        private void Mis_Tiendas_click_btn(object sender, MouseButtonEventArgs e)
        {
            Page pg = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;

            var p = new MisTiendas();

            pg.NavigationService.Navigate(p);
        }
        private void Informes_click_btn(object sender, MouseButtonEventArgs e)
        {
            Page pg = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;

            var p = new Information();

            pg.NavigationService.Navigate(p);
        }


        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Page pg = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;

            var p = new Home();

            pg.NavigationService.Navigate(p);
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            var c = (e.Source as Grid);
            c.Cursor = Cursors.Hand;
        }

        private void Grid_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            Page pg = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;
            pg.NavigationService.Navigate(new Estadisticas());
        }

        private void Button_Click(object sender, MouseButtonEventArgs e)
        {
            var bd = new BuscarDBVentana();
            bd.ShowDialog();
        }
    }
}
