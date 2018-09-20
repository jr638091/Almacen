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

        void fillVentas()
        {
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteVentas.Where(x => x.ShopId == 1).ToList();
                List<string> header = new List<string> { "Codigo", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "CantidadBuenEstado", "CantidadDefectuoso", "Precio", "PrecioDefectuoso" };
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

        void fillDevoluciones() {
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteDevoluciones.ToList();
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

        void fillDeuda() {
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteDeudas.ToList();
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

        void fillTranferencia()
        {
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteTransferencias.ToList();
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

        void fillEntrada()
        {
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteEntradas.ToList();
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

        void fillLiquidacion()
        {
            List<UIElement> t;
            using(var db = new TiendaDbContext())
            {
                var vs = db.InformeLiquidaciones.ToList();
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
            venta_sp.Children.Clear(); devoluciones_sp.Children.Clear(); deuda_sp.Children.Clear(); transferencia_sp.Children.Clear(); entrada_sp.Children.Clear();
            int a = TC.SelectedIndex;
            switch (TC.SelectedIndex)
            {
                case 0:
                    fillLiquidacion();
                    break;
                case 1:
                    fillVentas();
                    break;
                case 2:
                    fillDevoluciones();
                    break;
                case 3:
                    fillDeuda();
                    break;
                case 4:
                    fillTranferencia();
                    break;
                case 5:
                    fillEntrada();
                    break;
            }
        }
    }
}
