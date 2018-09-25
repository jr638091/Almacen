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

namespace WpfAplicacion
{
    /// <summary>
    /// Interaction logic for Estadisticas.xaml
    /// </summary>
    public partial class Estadisticas : Page
    {
        TiendaDbContext db;

        

        public Estadisticas()
        {
            InitializeComponent();
            db = new TiendaDbContext();
            DataContext = this;

            VentasXMes(5);
            TiendasVentas();
        }
#region VentasXmes
        DateTime CurrentDate;
        List<string> MonthAbrev = new List<string>() { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dec" };
        public SeriesCollection Data { get; set; }
        public string[] XLabel { get; set; }
        public Func<double, string> YFormatter { get; set; }

        void VentasXMes(int cantMeses)
        {
            CurrentDate = DateTime.Now;
            YFormatter = value => value.ToString("C");
    
            XLabel = GetLabels(cantMeses).ToArray<string>();
            Data = new SeriesCollection { new LineSeries { Values = GetSales(cantMeses) } };

        }

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
            Stack<double> s = new Stack<double>();
            int idx = CurrentDate.Month, cnt = 0;
            while (cnt < n)
            {
                var x = from t in db.ReporteVentas where t.Fecha.Month == idx select t.Pagado;
                double total = 0;
                foreach (var item in x)
                {
                    total += item;
                }

                s.Push(total);

                idx--;
                if (idx == 0)
                {
                    idx = 12;
                }
                cnt++;
            }
            while(s.Count > 0)
            {
                val.Add(s.Pop());
            }
            return val;
        }
        #endregion


        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        Dictionary<string, double> datos;

        void TiendasVentas()
        {
            Formatter = value => value.ToString("N");

            datos = new Dictionary<string, double>();
            var shop = db.ReporteVentas.ToList();

            var aux = db.Tiendas.Select(x=>x.Nombre).ToList();
            foreach (var item in aux)
            {
                datos.Add(item, 0.0);
            }

            foreach(var s in shop)
            {
                string t = s.Tienda.Nombre;
                datos[t] += s.Pagado;
            }

            Labels = datos.Keys.ToArray<string>();

            ChartValues<double> chart = new ChartValues<double>();
            foreach (var item in datos.Values)
            {
                chart.Add(item);
            }

            SeriesCollection = new SeriesCollection {
               new ColumnSeries
                {
                    Values = chart
                }
            };

        }

        

        

    }
}


       
