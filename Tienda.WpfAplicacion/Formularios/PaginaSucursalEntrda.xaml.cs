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
    /// Lógica de interacción para PaginaSucursalEntrda.xaml
    /// </summary>
    public partial class PaginaSucursalEntrda : Page, IFormulario
    {
        public PaginaSucursalEntrda()
        {
            InitializeComponent();
        }
        public PaginaSucursalEntrda(int id_tienda = 0)
        {
            InitializeComponent();
        }

        private void CancelEntry_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btn_Eliminar_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
