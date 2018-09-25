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
    /// 

    static class extensores
    {
        static string Descripcion(this object source) {
            return "Descripcion";
        }
        
    }

    public partial class Information : Page
    {
        public Information()
        {
            InitializeComponent();
        }

        class t
        {
            private string d { get { return "hola"; } }
        }
        class t2 {
            private t a {get; set;}
            public t2() { a = new t(); }
        }

        

        void fillVentas(Func<ReporteVenta,bool> pred)
        {
            venta_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteVentas.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Trabajador.Nombre;
                    p.pago_inform.Content = "Pago: " + item.CostoTotal.ToString() + " $";
                    p.Background = Brushes.AntiqueWhite;
                    venta_sp.Children.Add(p);
                }
            }
        }

        void fillDevoluciones(Func<ReporteDevolucion, bool> pred) {
            devoluciones_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteDevoluciones.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Cant. de Productos: " + item.CantidadTotal.ToString();
                    p.Background = Brushes.AntiqueWhite;
                    devoluciones_sp.Children.Add(p);
                }
            }
        }

        void fillDeuda(Func<ReporteDeuda, bool> pred) {
            deuda_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteDeudas.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Pagado: " + item.Pagado.ToString() + " $";
                    var l = new Label();
                    l.Content = "Deuda: " + item.CostoTotal.ToString() + " $";
                    l.Style = (Style)FindResource("Label_inf");
                    p.dp.Children.Add(l);
                    p.Background = Brushes.AntiqueWhite;
                    deuda_sp.Children.Add(p);
                }
            }
        }

        void fillTranferencia(Func<ReporteTransferencia, bool> pred)
        {
            transferencia_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteTransferencias.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Cant. de Productos: " + item.CantidadTotal.ToString();
                    p.Background = Brushes.AntiqueWhite;
                    transferencia_sp.Children.Add(p);
                }
            }
        }

        void fillEntrada(Func<ReporteEntrada, bool> pred)
        {
            entrada_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteEntradas.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "Cant. Total" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "CantidadTotal" };
                foreach (var item in vs)
                {
                    var source = item.Articulos;
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = "Almacen";
                    p.pago_inform.Content = "Cant. de Productos: " + item.CantidadTotal.ToString();
                    p.Background = Brushes.AntiqueWhite;
                    entrada_sp.Children.Add(p);
                }
            }
        }

        void fillLiquidacion(Func<InformeLiquidacion, bool> pred)
        {
            liquidacion_sp.Children.Clear();
            List<UIElement> t;
            using(var db = new TiendaDbContext())
            {
                var vs = db.InformeLiquidaciones.Where(pred).ToList();
                foreach (var item in vs)
                {
                    t = new List<UIElement>();
                    if (item.ReporteVenta.CantidadTotal > 0) {
                        List<string> header1 = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                        List<string> path1 = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                        var source1 = item.ReporteVenta.Articulos;
                        var ventas = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source1, header1, path1));
                        ventas.fecha_inform.Content = item.ReporteVenta.Fecha.ToString();
                        ventas.nombre_inform.Content = item.ReporteVenta.Trabajador.Nombre;
                        ventas.pago_inform.Content = "Pago: " + item.ReporteVenta.CostoTotal.ToString() + " $";
                        ventas.Background = Brushes.White;
                        t.Add(ventas);
                    }
                    if (item.ReporteDevolucion.CantidadTotal > 0)
                    {
                        var header2 = new List<string> { "Codigo", "Cant. BE", "Cant. ME" };
                        var path2 = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso" };
                        var source2 = item.ReporteDevolucion.Articulos;
                        var devoluciones = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source2, header2, path2));
                        devoluciones.fecha_inform.Content = item.ReporteDevolucion.Fecha.ToString();
                        devoluciones.nombre_inform.Content = item.ReporteDevolucion.Tienda.Nombre;
                        devoluciones.pago_inform.Content = "Cant. de Productos: " + item.ReporteDevolucion.CantidadTotal.ToString();
                        devoluciones.Background = Brushes.White;
                        t.Add(devoluciones);
                    }
                    if (item.ReporteDeuda.CantidadTotal > 0)
                    {
                        List<string> header3 = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                        List<string> path3 = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
                        var source3 = item.ReporteDeuda.Articulos;
                        var deudas = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source3, header3, path3));
                        deudas.fecha_inform.Content = item.ReporteDeuda.Fecha.ToString();
                        deudas.nombre_inform.Content = item.ReporteDeuda.Tienda.Nombre;
                        deudas.pago_inform.Content = "Pagado: " + item.ReporteDeuda.Pagado.ToString() + " $";
                        var l = new Label();
                        l.Content = "Deuda: " + item.ReporteDeuda.CostoTotal.ToString() + " $";
                        l.Style = (Style)FindResource("Label_inf");
                        deudas.dp.Children.Add(l);
                        deudas.Background = Brushes.White;
                        t.Add(deudas);
                    }

                    var p = new UserControllers.ReporteVentaController(t.ToArray());
                    p.Background = Brushes.AntiqueWhite;
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre.ToString();
                    p.pago_inform.Content = "Cant. Articulos: " + (item.ReporteVenta.CantidadTotal + item.ReporteDevolucion.CantidadTotal + item.ReporteDeuda.CantidadTotal).ToString();
                    liquidacion_sp.Children.Add(p);
                }
            }
        }

        private void TC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int a = TC.SelectedIndex;
            switch (TC.SelectedIndex)
            {
                case 0:
                    fillLiquidacion(x=>true);
                    break;
                case 1:
                    fillVentas(x => true);
                    break;
                case 2:
                    fillDevoluciones(x => true);
                    break;
                case 3:
                    fillDeuda(x => true);
                    break;
                case 4:
                    fillTranferencia(x => true);
                    break;
                case 5:
                    fillEntrada(x => true);
                    break;
            }
        }

        private void liquidacion_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            if (liquidacion_dp_1.SelectedDate != null && liquidacion_dp_2.SelectedDate != null)
            {
                if (liquidacion_dp_1.SelectedDate > liquidacion_dp_2.SelectedDate)
                    fillLiquidacion(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= liquidacion_dp_1.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= liquidacion_dp_2.SelectedDate));
                else
                    fillLiquidacion(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= liquidacion_dp_2.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= liquidacion_dp_1.SelectedDate));
            }
            else
                fillLiquidacion(x => true);
        }

        private void Entrada_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            if (entrada_dp_1.SelectedDate != null && entrada_dp_2.SelectedDate != null)
            {
                if (entrada_dp_1.SelectedDate > entrada_dp_2.SelectedDate)
                    fillEntrada(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= entrada_dp_1.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= entrada_dp_2.SelectedDate));
                else
                    fillEntrada(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= entrada_dp_2.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= entrada_dp_1.SelectedDate));
            }
            else
                fillEntrada(x => true);
        }

        private void venta_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            if (venta_dp_1.SelectedDate != null && venta_dp_2.SelectedDate != null)
            {
                if (venta_dp_1.SelectedDate > venta_dp_2.SelectedDate)
                    fillVentas(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= venta_dp_1.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= venta_dp_2.SelectedDate));
                else
                    fillVentas(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= venta_dp_2.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= venta_dp_1.SelectedDate));
            }
            else
                fillVentas(x => true);
        }

        private void devoluciones_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            if (devoluciones_dp_1.SelectedDate != null && devoluciones_dp_2.SelectedDate != null)
            {
                if (devoluciones_dp_1.SelectedDate > devoluciones_dp_2.SelectedDate)
                    fillDevoluciones(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= devoluciones_dp_1.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= devoluciones_dp_2.SelectedDate));
                else
                    fillDevoluciones(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= devoluciones_dp_2.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= devoluciones_dp_1.SelectedDate));
            }
            else
                fillDevoluciones(x => true);
        }

        private void deuda_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            if (deuda_dp_1.SelectedDate != null && deuda_dp_2.SelectedDate != null)
            {
                if (deuda_dp_1.SelectedDate > deuda_dp_2.SelectedDate)
                    fillDeuda(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= deuda_dp_1.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= deuda_dp_2.SelectedDate));
                else
                    fillDeuda(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= deuda_dp_2.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= deuda_dp_1.SelectedDate));
            }
            else
                fillDeuda(x => true);
        }

        private void transferencia_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            if (transferencia_dp_1.SelectedDate != null && transferencia_dp_2.SelectedDate != null)
            {
                if (transferencia_dp_1.SelectedDate > transferencia_dp_2.SelectedDate)
                    fillTranferencia(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= transferencia_dp_1.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= transferencia_dp_2.SelectedDate));
                else
                    fillTranferencia(x => (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) <= transferencia_dp_2.SelectedDate) && (new DateTime(x.Fecha.Year, x.Fecha.Month, x.Fecha.Day) >= transferencia_dp_1.SelectedDate));
            }
            else
                fillTranferencia(x => true);
        }

    }
}
