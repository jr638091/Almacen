using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Tienda.Modelo;

namespace WpfAplicacion
{
    public class entrada
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadBuenEstado { get; set; }
        public int CantidadDefectuoso { get; set; }
        public int CantidadTotal { get { return this.CantidadDefectuoso + this.CantidadBuenEstado; } }
        public entrada(string Codigo, string Descripcion)
        {
            this.Codigo = Codigo;
            this.Descripcion = Descripcion;
            CantidadBuenEstado = 0;
            CantidadDefectuoso = 0;
        }

        public ArticuloEntrada genera_articulo_entrada(DateTime Fecha)
        {
            var articulo = new ArticuloEntrada
            {
                CantidadBuenEstado = this.CantidadBuenEstado,
                CantidadDefectuoso = this.CantidadDefectuoso,
                Codigo = this.Codigo
            };
            return articulo;
        }

        public ReporteEntrada genera_reporte(ICollection<ArticuloEntrada> articulos)
        {
            var reporte = new ReporteEntrada
            {
                Fecha = DateTime.Now,
            };
            foreach (var art in articulos)
            {
                reporte.Articulos.Add(art);
            }

            using (var db = new TiendaDbContext())
            {
                db.ArticuloEntradas.AddRange(articulos);
                db.ReporteEntradas.Add(reporte);
                db.SaveChanges();
            }
            return reporte;
        }

        public Producto genera_producto()
        {
            return new Producto { Codigo = this.Codigo, Descripcion = this.Descripcion };
        }
    }

    public class objeto_venta
    {
        internal int ExistenciaId;
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadBuenEstado { get; set; }
        public int CantidadDefectuoso { get; set; }
        public int CantidadTotal
        {
            get
            {
                return this.CantidadBuenEstado + this.CantidadDefectuoso;
            }
        }
        public double PrecioBuenEstado { get; set; }
        public double PrecioDefectuoso { get; set; }
        public double PrecioTotal
        {
            get
            {
                return this.PrecioBuenEstado * this.CantidadBuenEstado + this.PrecioDefectuoso * this.CantidadDefectuoso;
            }
        }

        public int CantidadExistenteBE { get; set; }
        public int CantidadExistenteDefec { get; set; }
        public int CantidadTotalExistente { get { return this.CantidadExistenteBE + this.CantidadExistenteDefec; } }

        public objeto_venta(int ExistenciaId)
        {
            this.ExistenciaId = ExistenciaId;
            using (var db = new TiendaDbContext())
            {
                var existencia = db.Existencias.Find(ExistenciaId);
                this.Codigo = existencia.Codigo;
                this.Descripcion = existencia.Producto.Descripcion;
                this.PrecioBuenEstado = existencia.PrecioBuenEstado;
                this.PrecioDefectuoso = existencia.PrecioDefectuoso;
                this.CantidadExistenteBE = existencia.CantidadBuenEstado;
                this.CantidadExistenteDefec = existencia.CantidadDefectuoso;
            }
            this.CantidadBuenEstado = 0;
            this.CantidadDefectuoso = 0;
        }

        public ArticuloVenta genera_articulo(objeto_venta articulo)
        {
            var art = new ArticuloVenta
            {
                Codigo = articulo.Codigo,
                CantidadBuenEstado = articulo.CantidadBuenEstado,
                CantidadDefectuoso = articulo.CantidadDefectuoso,
                Precio = articulo.PrecioBuenEstado,
                PrecioDefectuoso = articulo.PrecioDefectuoso,

            };
            
            return art;
        }

        public static ReporteVenta generar_reporte(int tienda_id, int trabajador_id, ICollection<objeto_venta> Articulos, DateTime Fecha)
        {
            var reporte = new ReporteVenta { ShopId = tienda_id, TrabajadorId = trabajador_id, Fecha = Fecha, Articulos = new List<ArticuloVenta>() };

            using (var db = new TiendaDbContext())
            {
                db.ReporteVentas.Add(reporte);
                db.SaveChanges();
                foreach (var art in Articulos)
                {
                    var art_venta = art.genera_articulo(art);
                    reporte.Articulos.Add(art_venta);
                }
                db.SaveChanges();
            }

            return reporte;
        }
    }

    public class objeto_existencia
    {
        internal int ExistenciaId;
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadBuenEstado { get; set; }
        public int CantidadDefectuoso { get; set; }
        public int CantidadTotal
        {
            get
            {
                return this.CantidadBuenEstado + this.CantidadDefectuoso;
            }
        }

        public objeto_existencia(int existencia_id)
        {
            this.ExistenciaId = existencia_id;
            using (var db = new TiendaDbContext())
            {
                var existencia = db.Existencias.Find(existencia_id);
                this.Codigo = existencia.Codigo;
                this.Descripcion = existencia.Producto.Descripcion;
                this.CantidadBuenEstado = existencia.CantidadBuenEstado;
                this.CantidadDefectuoso = existencia.CantidadDefectuoso;
            }
            
        }
    }

    public class objeto_deuda
    {
        internal int ExistenciaId;
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadBuenEstado { get; set; }
        public int CantidadDefectuoso { get; set; }
        public int CantidadTotal
        {
            get
            {
                return this.CantidadBuenEstado + this.CantidadDefectuoso;
            }
        }
        public double Pagado { get; set; }
        public double PrecioBuenEstado { get; set; }
        public double PrecioDefectuoso { get; set; }
        public double PrecioTotal { get { return CantidadDefectuoso * PrecioDefectuoso + CantidadBuenEstado * PrecioBuenEstado; } }

        public objeto_deuda(int existencia_id)
        {
            this.ExistenciaId = existencia_id;
            using (var db = new TiendaDbContext())
            {
                var existencia = db.Existencias.Find(existencia_id);
                this.Codigo = existencia.Codigo;
                this.Descripcion = existencia.Producto.Descripcion;
                this.PrecioBuenEstado = existencia.PrecioBuenEstado;
                this.PrecioDefectuoso = existencia.PrecioDefectuoso;
            }
            this.CantidadBuenEstado = 0;
            this.CantidadDefectuoso = 0;
        }

        public ArticuloDeuda generar_articulo()
        {
            var articulo = new ArticuloDeuda
            {
                CantidadBuenEstado = this.CantidadBuenEstado,
                CantidadDefectuoso = this.CantidadDefectuoso,
                Codigo = this.Codigo,
                PrecioDefectuoso = this.PrecioDefectuoso,
                Precio = this.PrecioBuenEstado,
            };
            return articulo;
        }

        public static ReporteDeuda generar_reporte(int tienda_id, ICollection<objeto_deuda> articulos, DateTime Fecha)
        {
            using (var db = new TiendaDbContext()) {
                var tienda = db.Tiendas.Find(tienda_id);
                var reporte = new ReporteDeuda
                {
                    Fecha = Fecha,
                    ShopId = tienda_id,
                    Articulos = new List<ArticuloDeuda>(),
                };
                double pagado = 0;
                foreach(var art in articulos)
                {
                    reporte.Articulos.Add(art.generar_articulo());
                    pagado += art.Pagado;
                }
                reporte.Pagado = pagado;
                db.ReporteDeudas.Add(reporte);
                db.SaveChanges();
                return reporte;
            }
        }
    }

    public class objeto_devolucion
    {
        internal int ExistenciaId;
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadBuenEstado { get; set; }
        public int CantidadDefectuoso { get; set; }
        public int CantidadTotal
        {
            get
            {
                return this.CantidadBuenEstado + this.CantidadDefectuoso;
            }
        }

        public objeto_devolucion(int existencia_id)
        {
            this.ExistenciaId = existencia_id;
            using (var db = new TiendaDbContext())
            {
                var existencia = db.Existencias.Find(existencia_id);
                this.Codigo = existencia.Codigo;
                this.Descripcion = existencia.Producto.Descripcion;
            }
            this.CantidadBuenEstado = 0;
            this.CantidadDefectuoso = 0;
        }

        public ArticuloDevolucion generar_articulo()
        {
            var articulo = new ArticuloDevolucion
            {
                CantidadBuenEstado = this.CantidadBuenEstado,
                CantidadDefectuoso = this.CantidadDefectuoso,
                Codigo = this.Codigo,
            };
            return articulo;
        }

        public static ReporteDevolucion generar_reporte(int tienda_id ,ICollection<objeto_devolucion> articulos, DateTime Fecha)
        {
            using (var db = new TiendaDbContext())
            {
                var reporte = new ReporteDevolucion
                {
                    Fecha = Fecha,
                    Articulos = new List<ArticuloDevolucion>(),
                    ShopId = tienda_id,
                };
                foreach (var art in articulos)
                {
                    reporte.Articulos.Add(art.generar_articulo());
                }
                db.ReporteDevoluciones.Add(reporte);
                db.SaveChanges();
                return reporte;
            }
        }
    }

    public class Metodos_Auxiliares
    {

        public static DataGrid make_dg(IEnumerable<object> source, List<string> header, List<string> path)
        {
            DataGrid dg = new DataGrid();
            dg.AutoGenerateColumns = false;
            dg.ItemsSource = source;
            for (int i = 0; i < path.Count; ++i)
            {
                DataGridTextColumn t = new DataGridTextColumn();
                t.Binding = new Binding(path[i]);
                t.Header = header[i];
                t.IsReadOnly = true;
                dg.Columns.Add(t);
            }
            return dg;
        }

        public static InformeLiquidacion genera_informe(int tienda_id, ReporteDeuda deuda, ReporteDevolucion devolucion, ReporteVenta venta, DateTime Fecha)
        {
            using (var db = new TiendaDbContext())
            {
                var informe = new InformeLiquidacion
                {
                    Fecha = Fecha,
                    ShopId = tienda_id,
                    ReporteDeudaId = deuda.ReporteDeudaId,
                    ReporteDevolucionId = devolucion.ReporteDevolucionId,
                    ReporteVentaId = venta.ReporteVentaId
                };
                db.InformeLiquidaciones.Add(informe);
                db.SaveChanges();
                return informe;
            }
        }
        public static void refresh<T>(DataGrid grid, List<T> source)
        {
            int index = grid.SelectedIndex;
            grid.ItemsSource = null;
            grid.ItemsSource = source;
            grid.SelectedIndex = Math.Min(index, source.Count()-1);
        }
    }

    public class liquidacion_deuda
    {
        public int ReporteDeudaId { get; set; }
        public string Fecha { get; set; }
        public int CantidadTotal { get; set; }
        public double CostoTotal { get; set; }
        public double Pagado { get; set; }
        public double APagar { get; set; }

        public liquidacion_deuda(int ReporteDeudaId)
        {
            this.ReporteDeudaId = ReporteDeudaId;
            this.APagar = 0;
            using (var db = new TiendaDbContext())
            {
                var reporte = db.ReporteDeudas.Find(this.ReporteDeudaId);
                this.CostoTotal = reporte.CostoTotal;
                this.CantidadTotal = reporte.CantidadTotal;
                this.Pagado = reporte.Pagado;
                this.Fecha = reporte.Fecha.ToShortDateString();
            }
        }

        public void GuardaCambios()
        {
            DateTime Date = DateTime.Today;
            if (APagar <= 0)
                return;
            using(var db = new TiendaDbContext())
            {
                var Informe = new InformePagoDeuda
                {
                    ReporteDeudaId = this.ReporteDeudaId,
                    Fecha = Date,
                    Pagado = this.APagar,
                };
                var reporte = db.ReporteDeudas.Find(this.ReporteDeudaId);
                reporte.Pagado += this.APagar;
                if (reporte.Pagado == reporte.CostoTotal)
                    reporte.Saldada = true;
                db.Entry(reporte).State = EntityState.Modified;
                db.InformePagoDeudas.Add(Informe);
                db.SaveChanges();
            }
        }
    }

    public class informacion_deuda
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadBuenEstado { get; set; }
        public int CantidadDefectuoso { get; set; }
        public int CantidadTotal
        {
            get
            {
                return this.CantidadBuenEstado + this.CantidadDefectuoso;
            }
        }
        public double PrecioBuenEstado { get; set; }
        public double PrecioDefectuoso { get; set; }
        public double PrecioTotal
        {
            get
            {
                return this.PrecioBuenEstado * this.CantidadBuenEstado + this.PrecioDefectuoso * this.CantidadDefectuoso;
            }
        }

        public informacion_deuda(int articulo_deuda_id)
        {
            using(var db = new TiendaDbContext())
            {
                var art = db.ArticuloDeudas.Find(articulo_deuda_id);
                this.Codigo = art.Codigo;
                this.Descripcion = art.Producto.Descripcion;
                this.CantidadBuenEstado = art.CantidadBuenEstado;
                this.CantidadDefectuoso = art.CantidadDefectuoso;
                this.PrecioBuenEstado = art.Precio;
                this.PrecioDefectuoso = art.PrecioDefectuoso;
            }
        }

    }


    public class articulo_info
    {
        public int ArticuloDeudaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadBuenEstado { get; set; }
        public int CantidadDefectuoso { get; set; }
        public int CantidadTotal { get { return this.CantidadBuenEstado + this.CantidadDefectuoso; } }
        public double PrecioBuenEstado { get; set; }
        public double PrecioDefectuoso { get; set; }

        public articulo_info(int ArticuloId)
        {
            this.ArticuloDeudaId = ArticuloId;
            using (var db = new TiendaDbContext())
            {
                var articulo = db.ArticuloDeudas.Find(ArticuloId);
                this.Codigo = articulo.Codigo;
                this.Descripcion = articulo.Producto.Descripcion;
                this.CantidadBuenEstado = articulo.CantidadBuenEstado;
                this.CantidadDefectuoso = articulo.CantidadDefectuoso;
                this.PrecioBuenEstado = articulo.Precio;
                this.PrecioDefectuoso = articulo.PrecioDefectuoso;
            }
        }
    }

}
