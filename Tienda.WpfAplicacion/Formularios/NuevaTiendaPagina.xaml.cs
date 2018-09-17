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
    /// Lógica de interacción para NuevaTiendaPagina.xaml
    /// </summary>
    public partial class NuevaTiendaPagina : Page, IFormulario
    {
        public NuevaTiendaPagina()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool fallo = true;
            if(Tienda_Nombre.Text == "") {
                Tienda_Nombre.Background = Brushes.Pink;
                fallo = false;
            }
            else
            {
                Tienda_Nombre.Background = Brushes.White;
            }
            if (Tienda_telefono.Text == ""){
                Tienda_telefono.Background = Brushes.Pink;
                fallo = false;
            }
            else
            {
                Tienda_Nombre.Background = Brushes.White;
            }
            if (Tienda_Direccion.Text == "")
            {
                Tienda_Direccion.Background = Brushes.Pink;
                fallo = false;
            }
            else
            {
                Tienda_Nombre.Background = Brushes.White;
            }
            if (Tienda_Encargado.Text == "")
            {
                Tienda_Encargado.Background = Brushes.Pink;
                fallo = false;
            }
            else
            {
                Tienda_Nombre.Background = Brushes.White;
            }
            if (!fallo)
                Alert.Content = "**Faltan datos**";
            else
            {
                //var s = new Shop
                //{
                //    Direccion = Tienda_Direccion.Text,
                //    Encargado = Tienda_Encargado.Text,

                //};
                //using (var db = new TiendaDbContext())
                //{

                //}
            }
        }
    }
}
