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
    /// Lógica de interacción para Information.xaml
    /// </summary>
    public partial class Information : Page
    {
        public Information()
        {
            InitializeComponent();
            
        }



        private void liquidacion_tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var ls = db.InformeLiquidaciones.ToList();
            }
        }

        private void venta_tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            venta_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteVentas.ToList();
                List<string> header = new List<string>{ "Codigo" ,"Cant. BE", "Cant. ME","BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(source, header, path);
                    venta_sp.Children.Add(p);
                }
            }
        }

        private void devoluciones_tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var ls = db.ReporteDevoluciones.ToList();
            }
        }

        private void deuda_tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var ds = db.ReporteDeudas.ToList();
            }
        }

        private void transferencia_tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var ts = db.ReporteTransferencias.ToList();
            }
        }

        private void entrada_tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var es = db.ReporteEntradas.ToList();
            }
        }

        private void TC_Loaded(object sender, RoutedEventArgs e)
        {
            venta_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteVentas.Where(x=>x.ShopId==1).ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(source, header, path);
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Trabajador.Nombre;
                    p.pago_inform.Content = "Pago: " + item.CostoTotal.ToString() + " $";
                    venta_sp.Children.Add(p);
                }
            }
            devoluciones_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteDevoluciones.ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(source, header, path);
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Cant. de Productos: "+item.CantidadTotal.ToString();
                    devoluciones_sp.Children.Add(p);
                }
            }
            deuda_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteDeudas.ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(source, header, path);
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Pagado: " + item.Pagado.ToString() +" $";
                    var l = new Label();
                    l.Content = "Deuda: " + item.CostoTotal.ToString()+" $";
                    l.Style = (Style)FindResource("Label_inf");
                    p.dp.Children.Add(l);
                    deuda_sp.Children.Add(p);
                }
            }
            transferencia_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteTransferencias.ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(source, header, path);
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Cant. de Productos: " + item.CantidadTotal.ToString();
                    transferencia_sp.Children.Add(p);
                }
            }
        }
    }
}
