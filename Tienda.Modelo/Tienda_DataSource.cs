using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tienda.Modelo
{
    public interface Tienda_DataSource
    {
        IQueryable<Producto> Productos { get; }
        IQueryable<Shop> Tiendas { get; }
        IQueryable<ReporteVenta> ReporteVentas { get; }
        IQueryable<ArticuloVenta> ArticuloVentas { get; }
        IQueryable<ReporteDeuda> ReporteDeudas { get; }
        IQueryable<ArticuloDeuda> ArticuloDeudas { get; }
        IQueryable<ReporteDevolucion> ReporteDevoluciones { get; }
        IQueryable<ArticuloDevolucion> ArticuloDevoluciones { get; }
        IQueryable<ReporteEntrada> ReporteEntradas { get; }
        IQueryable<ArticuloEntrada> ArticuloEntradas { get; }
        IQueryable<ReporteTransferencia> ReporteTransferencias { get; }
        IQueryable<ArticuloTransferencia> ArticuloTransferencias { get; }
        IQueryable<InformeLiquidacion> InformeLiquidaciones { get; }
        IQueryable<Trabajador> Trabajadores { get; }
        IQueryable<Existencia> Existencias { get; }


    }
}
