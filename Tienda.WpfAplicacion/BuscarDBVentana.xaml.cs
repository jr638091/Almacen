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
                string first = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=";
                string second = ";Integrated Security=True;Connect Timeout=30";
                string middle = form.SelectedPath + "\\Tienda.mdf";
                var connectionString = ConfigurationManager.ConnectionStrings["TiendaContext"];
                var field = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(connectionString, false);
                ConfigurationManager.ConnectionStrings["TiendaContext"].ConnectionString = first + middle + second;
                field.SetValue(connectionString, true);
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
                string first = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=";
                string second = ";Integrated Security=True;Connect Timeout=30";
                string middle = form.FileName;
                var connectionString = ConfigurationManager.ConnectionStrings["TiendaContext"];
                var field = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                field.SetValue(connectionString, false);
                ConfigurationManager.ConnectionStrings["TiendaContext"].ConnectionString = first + middle + second;
                field.SetValue(connectionString, true);
                try
                {
                    var a = new TiendaDbContext();
                    a.Trabajadores.Where(t => t.eliminado);
                }
                catch
                {
                    return;
                }
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
