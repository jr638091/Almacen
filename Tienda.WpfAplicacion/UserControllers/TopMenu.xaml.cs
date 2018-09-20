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
    /// Lógica de interacción para TopMenu.xaml
    /// </summary>
    public partial class TopMenu : UserControl
    {

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TopMenu));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public TopMenu()
        {
            InitializeComponent();
            this.DataContext = this;
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

        bool salir = false;


        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationWindow nw = GetDependencyObjectFromVisualTree(this, typeof(NavigationWindow)) as NavigationWindow;
            if (!salir)
                nw.DragMove();
            else
                salir = false;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var p = GetDependencyObjectFromVisualTree(this, typeof(Page)) as Page;
            salir = true;
            var a = GetDependencyObjectFromVisualTree(this, typeof(NavigationWindow)) as NavigationWindow;
            if (p is IFormulario)
            {
                var m = MessageBox.Show("Seguro que desea salir?", "Salir", MessageBoxButton.OKCancel);
                if (m == MessageBoxResult.OK)
                    a.Close();
            }
            else
                a.Close();
        }
    }
}
