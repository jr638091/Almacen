using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tienda.Modelo;

namespace WpfAplicacion
{
    public class entrada
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadBuenEstado { get; set; }
        public int CantidadDefectuoso { get; set; }
        public int CantidadTotal { get { return CantidadDefectuoso + CantidadBuenEstado; } }
        public entrada(string Codigo, string Descripcion)
        {
            this.Codigo = Codigo;
            this.Descripcion = Descripcion;
            CantidadBuenEstado = 0;
            CantidadDefectuoso = 0;
        }

        public ArticuloEntrada genera_articulo_entrada()
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
                return PrecioBuenEstado * CantidadBuenEstado + PrecioDefectuoso * CantidadDefectuoso;
            }
        }

        public int CantidadExistenteBE { get; set; }
        public int CantidadExistenteDefec { get; set; }
        public int CantidadTotalExistente { get { return CantidadExistenteBE + CantidadExistenteDefec; } }

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

        public ArticuloVenta genera_articulo(objeto_venta articulo, int reporte_id)
        {
            var art = new ArticuloVenta
            {
                Codigo = articulo.Codigo,
                ReporteVentaId = reporte_id,
                CantidadBuenEstado = articulo.CantidadBuenEstado,
                CantidadDefectuoso = articulo.CantidadDefectuoso,
                Precio = articulo.PrecioBuenEstado,
                PrecioDefectuoso = articulo.PrecioDefectuoso,

            };
            using (var db = new TiendaDbContext())
            {
                db.ArticuloVentas.Add(art);
                db.SaveChanges();
            }
            return art;
        }

        public ReporteVenta generar_reporte(int tienda_id, int trabajador_id, ICollection<objeto_venta> Articulos)
        {
            var reporte = new ReporteVenta { ShopId = tienda_id, TrabajadorId = trabajador_id, Fecha = DateTime.Now };

            using (var db = new TiendaDbContext())
            {
                db.ReporteVentas.Add(reporte);
                db.SaveChanges();
                foreach (var art in Articulos)
                {
                    var art_venta = art.genera_articulo(art, reporte.ReporteVentaId);
                    reporte.Articulos.Add(art_venta);
                }
                db.SaveChanges();
            }

            return reporte;
        }
    }

}
