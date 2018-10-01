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


        void fillVentas(Func<ReporteVenta, bool> pred)
        {
            int prods = 0;
            double total = 0;
            venta_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteVentas.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso", "PrecioBuenEstado", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    List<objeto_venta> source2 = new List<objeto_venta>();
                    foreach (var i in item.Articulos)
                    {
                        source2.Add(new objeto_venta(i.ArticuloVentaId));
                        source2.Last().CantidadBuenEstado = i.CantidadBuenEstado;
                        source2.Last().CantidadDefectuoso = i.CantidadDefectuoso;
                    }
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source2, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Trabajador.Nombre;
                    p.pago_inform.Content = "Pago: " + item.CostoTotal.ToString() + " $";
                    p.Background = Brushes.AntiqueWhite;
                    venta_sp.Children.Add(p);
                    prods += item.CantidadTotal;
                    total += item.CostoTotal;
                }
            }
            venta_prod_total.Content = prods.ToString();
            venta_ganancia_total.Content = total.ToString();
        }

        void fillDevoluciones(Func<ReporteDevolucion, bool> pred) {
            int total = 0;
            devoluciones_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteDevoluciones.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME" };
                List<string> path = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso" };
                foreach (var item in vs)
                {
                    List<objeto_devolucion> source2 = new List<objeto_devolucion>();
                    foreach (var i in item.Articulos)
                    {
                        source2.Add(new objeto_devolucion(i.ArticuloDevolucionId));
                        source2.Last().CantidadBuenEstado = i.CantidadBuenEstado;
                        source2.Last().CantidadDefectuoso = i.CantidadDefectuoso;
                    }
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source2, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Cant. de Productos: " + item.CantidadTotal.ToString();
                    p.Background = Brushes.AntiqueWhite;
                    devoluciones_sp.Children.Add(p);
                    total += item.CantidadTotal;
                }
            }
            devolucion_total.Content = total.ToString();
        }

        void fillDeuda(Func<ReporteDeuda, bool> pred) {
            double total = 0;
            double pagada = 0;
            deuda_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteDeudas.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                List<string> path = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso", "PrecioBuenEstado", "PrecioDefectuoso" };
                foreach (var item in vs)
                {
                    List<objeto_deuda> source2 = new List<objeto_deuda>();
                    foreach (var i in item.Articulos)
                    {
                        source2.Add(new objeto_deuda(i.ArticuloDeudaId));
                        source2.Last().CantidadBuenEstado = i.CantidadBuenEstado;
                        source2.Last().CantidadDefectuoso = i.CantidadDefectuoso;
                    }
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source2, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Pagado: " + item.Pagado.ToString() + " $";
                    var l = new Label();
                    l.Content = "Deuda: " + item.CostoTotal.ToString() + " $";
                    l.Style = (Style)FindResource("Label_inf");
                    p.dp.Children.Add(l);
                    p.Background = Brushes.AntiqueWhite;
                    deuda_sp.Children.Add(p);
                    total += item.CostoTotal;
                    pagada += item.Pagado;
                }
            }
            deuda_total.Content = total.ToString();
            deuda_pagado_total.Content = pagada.ToString();
        }

        class objeto_transferencia
        {
            public string Codigo { get; set; }
            public string Descripcion { get; set; }
            public int CantidadBuenEstado { get; set; }
            public int CantidadDefectuoso { get; set; }
            public int CantidadTotal { get { return this.CantidadDefectuoso + this.CantidadBuenEstado; } }

            public objeto_transferencia(int obj_id) {
                using (var db = new TiendaDbContext())
                {
                    var existencia = db.Existencias.Find(obj_id);
                    this.Codigo = existencia.Codigo;
                    this.Descripcion = existencia.Producto.Descripcion;
                }
                this.CantidadBuenEstado = 0;
                this.CantidadDefectuoso = 0;
            }
        }

        void fillTranferencia(Func<ReporteTransferencia, bool> pred)
        {
            int total = 0;
            transferencia_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteTransferencias.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME"};
                List<string> path = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso" };
                foreach (var item in vs)
                {
                    List<objeto_transferencia> source2 = new List<objeto_transferencia>();
                    foreach (var i in item.Articulos)
                    {
                        source2.Add(new objeto_transferencia(i.ArticuloTransferenciaId));
                        source2.Last().CantidadBuenEstado = i.CantidadBuenEstado;
                        source2.Last().CantidadDefectuoso = i.CantidadDefectuoso;
                    }
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source2, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre;
                    p.pago_inform.Content = "Cant. de Productos: " + item.CantidadTotal.ToString();
                    p.Background = Brushes.AntiqueWhite;
                    transferencia_sp.Children.Add(p);
                    total += item.CantidadTotal;
                }
            }
            transferencia_total.Content = total.ToString();
        }

        void fillEntrada(Func<ReporteEntrada, bool> pred)
        {
            int total = 0;
            entrada_sp.Children.Clear();
            using (var db = new TiendaDbContext())
            {
                var vs = db.ReporteEntradas.Where(pred).ToList();
                List<string> header = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME" };
                List<string> path = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso" };
                foreach (var item in vs)
                {
                    List<objeto_transferencia> source2 = new List<objeto_transferencia>();
                    foreach (var i in item.Articulos)
                    {
                        source2.Add(new objeto_transferencia(i.ArticuloEntradaId));
                        source2.Last().CantidadBuenEstado = i.CantidadBuenEstado;
                        source2.Last().CantidadDefectuoso = i.CantidadDefectuoso;
                    }
                    var p = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source2, header, path));
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = "Almacen";
                    p.pago_inform.Content = "Cant. de Productos: " + item.CantidadTotal.ToString();
                    p.Background = Brushes.AntiqueWhite;
                    entrada_sp.Children.Add(p);
                    total += item.CantidadTotal;
                }
            }
            entrada_total.Content = total.ToString();
        }

        void fillLiquidacion(Func<InformeLiquidacion, bool> pred)
        {
            int art = 0;
            double deuda=0;
            double ganancia=0;
            liquidacion_sp.Children.Clear();
            List<UIElement> t;
            using(var db = new TiendaDbContext())
            {
                var vs = db.InformeLiquidaciones.Where(pred).ToList();
                foreach (var item in vs)
                {
                    t = new List<UIElement>();
                    if (item.ReporteVenta.CantidadTotal > 0) {
                        List<string> header1 = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                        List<string> path1 = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso", "PrecioBuenEstado", "PrecioDefectuoso" };
                        List<objeto_venta> source1 = new List<objeto_venta>();
                        foreach (var i in item.ReporteVenta.Articulos)
                        {
                            source1.Add(new objeto_venta(i.ArticuloVentaId));
                            source1.Last().CantidadBuenEstado = i.CantidadBuenEstado;
                            source1.Last().CantidadDefectuoso = i.CantidadDefectuoso;
                        }
                        var ventas = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source1, header1, path1));
                        ventas.fecha_inform.Content = item.ReporteVenta.Fecha.ToString();
                        ventas.nombre_inform.Content = item.ReporteVenta.Tienda.Nombre;
                        ventas.pago_inform.Content = "Pago: " + item.ReporteVenta.CostoTotal.ToString() + " $";
                        ventas.Background = Brushes.White;
                        t.Add(ventas);
                        art += item.ReporteVenta.CantidadTotal;
                        ganancia += item.ReporteVenta.CostoTotal;
                    }
                    if (item.ReporteDevolucion.CantidadTotal > 0)
                    {
                        List<string> header2 = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME"};
                        List<string> path2 = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso"};
                        List<objeto_devolucion> source2 = new List<objeto_devolucion>();
                        foreach (var i in item.ReporteDevolucion.Articulos)
                        {
                            source2.Add(new objeto_devolucion(i.ArticuloDevolucionId));
                            source2.Last().CantidadBuenEstado = i.CantidadBuenEstado;
                            source2.Last().CantidadDefectuoso = i.CantidadDefectuoso;
                        }
                        var devoluciones = new UserControllers.ReporteVentaController(Metodos_Auxiliares.make_dg(source2, header2, path2));
                        devoluciones.fecha_inform.Content = item.ReporteDevolucion.Fecha.ToString();
                        devoluciones.nombre_inform.Content = item.ReporteDevolucion.Tienda.Nombre;
                        devoluciones.pago_inform.Content = "Cant. de Productos: " + item.ReporteDevolucion.CantidadTotal.ToString();
                        devoluciones.Background = Brushes.White;
                        t.Add(devoluciones);
                        art += item.ReporteDevolucion.CantidadTotal;
                    }
                    if (item.ReporteDeuda.CantidadTotal > 0)
                    {
                        List<string> header3 = new List<string> { "Codigo", "Descripcion", "Cant. BE", "Cant. ME", "BE $", "ME $" };
                        List<string> path3 = new List<string> { "Codigo", "Descripcion", "CantidadBuenEstado", "CantidadDefectuoso", "PrecioBuenEstado", "PrecioDefectuoso" };
                        List<objeto_deuda> source3 = new List<objeto_deuda>();
                        foreach (var i in item.ReporteDeuda.Articulos)
                        {
                            source3.Add(new objeto_deuda(i.ReporteDeudaId));
                            source3.Last().CantidadBuenEstado = i.CantidadBuenEstado;
                            source3.Last().CantidadDefectuoso = i.CantidadDefectuoso;
                        }
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
                        art += item.ReporteDeuda.CantidadTotal;
                        deuda += item.ReporteDeuda.CostoTotal;
                    }

                    var p = new UserControllers.ReporteVentaController(t.ToArray());
                    p.Background = Brushes.AntiqueWhite;
                    p.fecha_inform.Content = item.Fecha.ToString();
                    p.nombre_inform.Content = item.Tienda.Nombre.ToString();
                    p.pago_inform.Content = "Cant. Articulos: " + (item.ReporteVenta.CantidadTotal + item.ReporteDevolucion.CantidadTotal + item.ReporteDeuda.CantidadTotal).ToString();
                    liquidacion_sp.Children.Add(p);
                    liquidacion_art.Content = art.ToString();
                    liquidacion_deuda.Content = deuda.ToString();
                    liquidacion_ganancia.Content = ganancia.ToString();
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

        private Tuple<DateTime,DateTime> calendar_change(DatePicker fecha_menor, DatePicker fecha_mayor) {
            DateTime? mayor =fecha_mayor.SelectedDate;
            DateTime? menor =fecha_menor.SelectedDate;
            if (menor > mayor)
            {
                DateTime? t = mayor;
                mayor = menor;
                menor = t;
            }
            if (mayor != null) mayor = mayor.Value.AddDays(1);
            else mayor = DateTime.MaxValue;
            if (menor == null) menor = DateTime.MinValue;
            return new Tuple<DateTime, DateTime>((DateTime)menor, (DateTime)mayor);
        }

        private void liquidacion_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            Tuple<DateTime,DateTime> t = calendar_change(liquidacion_dp_2, liquidacion_dp_1);
            DateTime a = t.Item1;
            DateTime b = t.Item2;
            fillLiquidacion(x => x.Fecha >= a && x.Fecha <= b);
        }

        private void Entrada_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            Tuple<DateTime, DateTime> t = calendar_change(entrada_dp_2, entrada_dp_1);
            DateTime a = t.Item1;
            DateTime b = t.Item2;
            fillEntrada(x => x.Fecha >= a && x.Fecha <= b);
        }

        private void venta_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            Tuple<DateTime, DateTime> t = calendar_change(venta_dp_2, venta_dp_1);
            DateTime a = t.Item1;
            DateTime b = t.Item2;
            fillVentas(x => x.Fecha >= a && x.Fecha <= b);
        }

        private void devoluciones_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            Tuple<DateTime, DateTime> t = calendar_change(devoluciones_dp_2, devoluciones_dp_1);
            DateTime a = t.Item1;
            DateTime b = t.Item2;
            fillDevoluciones(x => x.Fecha >= a && x.Fecha <= b);
        }

        private void deuda_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            Tuple<DateTime, DateTime> t = calendar_change(deuda_dp_2, deuda_dp_1);
            DateTime a = t.Item1;
            DateTime b = t.Item2;
            fillDeuda(x => x.Fecha >= a && x.Fecha <= b);
        }

        private void transferencia_dp_1_CalendarClosed(object sender, SelectionChangedEventArgs e)
        {
            Tuple<DateTime, DateTime> t = calendar_change(transferencia_dp_2, transferencia_dp_1);
            DateTime a = t.Item1;
            DateTime b = t.Item2;
            fillTranferencia(x => x.Fecha >= a && x.Fecha <= b);
        }

        private void TC_Loaded(object sender, RoutedEventArgs e)
        {
            fillLiquidacion(x => true);
            fillVentas(x => true);
            fillDevoluciones(x => true);
            fillDeuda(x => true);
            fillTranferencia(x => true);
            fillEntrada(x => true);
        }
    }
}
