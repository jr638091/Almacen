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
    /// Lógica de interacción para RealizarVentaPagina.xaml
    /// </summary>
    public partial class RealizarVentaPagina : Page, IFormulario
    {
        private List<objeto_venta> productos_source;
        private List<objeto_venta> venta_source;
        private List<Trabajador> vendedores;
        private string codigo_src;
        private string descrip_src;

        private bool is_editing;
        private bool need_refresh;

        public RealizarVentaPagina()
        {
            is_editing = false;
            need_refresh = false;
            codigo_src = "";
            descrip_src = "";

            venta_source = new List<objeto_venta>();
            productos_source = new List<objeto_venta>();
            vendedores = new List<Trabajador>();

            using (var db = new TiendaDbContext())
            {
                vendedores = db.Trabajadores.ToList();
                var temp = db.Tiendas.First().Productos.Where(p => p.CantidadBuenEstado > 0 || p.CantidadDefectuoso > 0);
                foreach (var item in temp)
                {
                    productos_source.Add(new objeto_venta(item.ExistenciaId));
                }
            }

            InitializeComponent();
            cb_trabajadores.ItemsSource = vendedores;
            cb_trabajadores.DisplayMemberPath = "Nombre";
            dgrid_productos.ItemsSource = productos_source;
            dgrid_venta.ItemsSource = venta_source;
        }
        private void btn_cancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btn_Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_productos.SelectedItem == null || !(dgrid_productos.SelectedItem is objeto_venta))
            {
                MessageBox.Show("Necesita seleccionar un producto para agregar");
                return;
            }
            var exist = dgrid_productos.SelectedItem as objeto_venta;
            productos_source.RemoveAt(productos_source.FindIndex(p => p.Codigo == exist.Codigo));
            venta_source.Add(exist);

            dgrid_venta.ItemsSource = null;
            dgrid_productos.ItemsSource = productos_source.Where(exis => exis.Codigo.Contains(codigo_src) && exis.Descripcion.Contains(descrip_src)).ToList();
            dgrid_venta.ItemsSource = venta_source;
        }

        private void btn_Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_venta.SelectedItem == null || !(dgrid_venta.SelectedItem is objeto_venta))
            {
                MessageBox.Show("Necesita seleccionar un producto para remover");
                return;
            }

            var obj_venta = dgrid_venta.SelectedItem as objeto_venta;

            productos_source.Add(obj_venta);

            venta_source.RemoveAt(venta_source.FindIndex(p => p.Codigo == obj_venta.Codigo));
            dgrid_venta.ItemsSource = null;
            dgrid_productos.ItemsSource = productos_source.Where(exis => exis.Codigo.Contains(codigo_src) && exis.Descripcion.Contains(descrip_src)).ToList();
            dgrid_venta.ItemsSource = venta_source;
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            codigo_src = tbox_codigo.Text;
            descrip_src = tbox_descripcion.Text;
            dgrid_productos.ItemsSource = productos_source.Where(exis => exis.Codigo.Contains(codigo_src) && exis.Descripcion.Contains(descrip_src)).ToList();
        }



        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!is_editing && need_refresh)
            {
                dgrid_venta.Items.Refresh();
                need_refresh = false;
            }
        }

        private void dgrid_venta_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            need_refresh = true;
            is_editing = true;
        }

        private void dgrid_venta_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int valor;
            if (!int.TryParse((e.EditingElement as TextBox).Text, out valor))
            {
                e.Cancel = true;

                return;
            }
            var column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                int index = dgrid_venta.SelectedIndex;
                var bindingPath = (column.Binding as Binding).Path.Path;

            }
            is_editing = false;
        }

        private void ComboBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
