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

namespace WpfAplicacion.Formularios
{
    /// <summary>
    /// Lógica de interacción para DeudasLiquidacionPagina.xaml
    /// </summary>
    public partial class DeudasLiquidacionPagina : Page, IFormulario
    {
        private List<liquidacion_deuda> source_deuda;
        private List<articulo_info> source_info;
        public DeudasLiquidacionPagina()
        {
            InitializeComponent();
        }

        public DeudasLiquidacionPagina(int shopId)
        {
            InitializeComponent();

            source_deuda = new List<liquidacion_deuda>();
            using (var db = new TiendaDbContext())
            {
                var reportes = db.ReporteDeudas.Where(r => r.ShopId == shopId && !r.Saldada).ToList();
                foreach(var item in reportes)
                {
                    source_deuda.Add(new liquidacion_deuda(item.ReporteDeudaId));
                }
            }
            dgrid_deudas.ItemsSource = null;
            dgrid_deudas.ItemsSource = source_deuda;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void dgrid_deudas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seleccion = dgrid_deudas.SelectedItem;
            if (seleccion == null || !(seleccion is liquidacion_deuda))
                return;
            var objeto = seleccion as liquidacion_deuda;

            using (var db = new TiendaDbContext())
            {
                source_info = new List<articulo_info>();
                foreach(var item in db.ReporteDeudas.Find(objeto.ReporteDeudaId).Articulos)
                {
                    source_info.Add(new articulo_info(item.ArticuloDeudaId));
                }
                dgrid_informacion.ItemsSource = null;
                dgrid_informacion.ItemsSource = source_info;
            }
        }

        private void dgrid_deudas_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            double valor;
            var objeto = e.Row.Item as liquidacion_deuda;
            if (!double.TryParse((e.EditingElement as TextBox).Text, out valor))
            {
                Metodos_Auxiliares.refresh(dgrid_deudas, source_deuda);
            }
            else
            {
                valor = Math.Min(valor, objeto.CostoTotal - objeto.Pagado);

                valor = Math.Max(0, valor);

                source_deuda.Find(p => p.ReporteDeudaId == objeto.ReporteDeudaId).APagar = valor;
                Metodos_Auxiliares.refresh(dgrid_deudas, source_deuda);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach(var item in source_deuda)
            {
                item.GuardaCambios();
            }
            this.NavigationService.GoBack();
        }
    }
}
