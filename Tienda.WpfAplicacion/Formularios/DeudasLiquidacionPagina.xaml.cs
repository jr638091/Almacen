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
        private int shopId;
        public DeudasLiquidacionPagina()
        {
            InitializeComponent();
        }

        public DeudasLiquidacionPagina(int shopId)
        {
            InitializeComponent();

            source_deuda = new List<liquidacion_deuda>();
            this.shopId = shopId;

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
                dgrid_informacion.ItemsSource = db.ReporteDeudas.Find(objeto.ReporteDeudaId).Articulos.ToList();
            }
        }

        private void dgrid_deudas_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var reportes = db.ReporteDeudas.Where(r => r.ShopId == shopId && r.Pagado<r.CostoTotal).ToList();
                foreach (var item in reportes)
                {
                    source_deuda.Add(new liquidacion_deuda(item.ReporteDeudaId));
                }
            }
            dgrid_deudas.ItemsSource = source_deuda;
        }
    }
}
