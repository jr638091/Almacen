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
using LiveCharts;
using LiveCharts.Wpf;

namespace WpfAplicacion.UserControllers
{
    /// <summary>
    /// Interaction logic for PointShapeLineExample.xaml
    /// </summary>
    public partial class PointShapeLineExample : UserControl
    {
        TiendaDbContext db;
        DateTime CurrentDate;
        List<string> MonthAbrev = new List<string>() { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dec" };


        public PointShapeLineExample()
        {
            InitializeComponent();

            db = new TiendaDbContext();
            CurrentDate = DateTime.Now;

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {                   
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                }
            };

            Labels = GetLabels(5).ToArray<string>();
            YFormatter = value => value.ToString("C");


            //modifying any series values will also animate and update the chart


            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


        List<string> GetLabels(int n)
        {
            List<string> l = new List<string>();
            int idx = CurrentDate.Month, cnt = 0;
            while (cnt < n)
            {
                l.Add(MonthAbrev[idx - 1]);
                idx--;
                if (idx == 0)
                {
                    idx = 12;
                }
                cnt++;
            }
            return l.Reverse<string>().ToList<string>();
        }

        // Gets the sales on the n previous months
        ChartValues<double> GetSales(int n)
        {
            if (n > 12 || n < 1)
            {
                throw new Exception("Months should be beteween 1 and 12");
            }
            ChartValues<double> val = new ChartValues<double>();
            int idx = CurrentDate.Month, cnt = 0;
            while (cnt < n)
            {
                var x = from t in db.ReporteVentas where t.Fecha.Month == idx select t.Pagado;
                double total = 0;
                foreach (var item in x)
                {
                    total += item;
                }

                val.Add(total);

                idx--;
                if (idx == 0)
                {
                    idx = 12;
                }
                cnt++;
            }

            return val;
        }

    }
}
