using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using winForm = System.Windows.Forms;

namespace WpfAplicacion
{
    /// <summary>
    /// Lógica de interacción para BuscarDBVentana.xaml
    /// </summary>
    public partial class BuscarDBVentana : Window
    {
        public BuscarDBVentana()
        {
            InitializeComponent();
        }

        private void btn_nueva_Click(object sender, RoutedEventArgs e)
        {
            var form = new winForm.FolderBrowserDialog();
            if (form.ShowDialog() == winForm.DialogResult.OK)
            {
                string principio = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=";
                string final = ";Integrated Security=True;Connect Timeout=30";
                string medio = form.SelectedPath + "\\TiendaBD.mdf";

                string conexion = principio + medio + final;
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings["TiendaContext"].ConnectionString = conexion;

                config.Save();
                try
                {
                    var a = new TiendaDbContext();
                    a.SeedPublico(a);
                }
                catch
                {
                    return;
                }
                this.DialogResult = true;
                this.Close();
            }
        }

        private void btn_existente_Click(object sender, RoutedEventArgs e)
        {
            var form = new winForm.OpenFileDialog();
            form.CheckFileExists = true;
            form.Multiselect = false;
            form.DefaultExt = ".mdf";
            if (form.ShowDialog() == winForm.DialogResult.OK)
            {
                string principio = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=";
                string final = ";Integrated Security=True;Connect Timeout=30";
                string medio = form.FileName;

                string conexion = principio + medio + final;
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings["TiendaContext"].ConnectionString = conexion;
                config.Save();
                MessageBox.Show("Necesita reabrir la aplicacion para que los cambios surtan efecto");
                Application.Current.Shutdown();
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                if(!db.Database.Exists())
                    Application.Current.Shutdown();
            }
        }
    }
}
