using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Tienda.Modelo
{
    public class Producto
    {
        [Key]
        public virtual string Codigo { get; set; }
        public virtual string Descripcion { get; set; }
    }
    public class Trabajador
    {
        public virtual int TrabajadorId { get; set; }
        public virtual string Nombre { get; set; }
        public virtual int ShopId { get; set; }
        public virtual Shop Tienda { get; set; }

    }
    public class Existencia
    {
        public virtual int ExistenciaId { get; set; }
        public virtual int ShopId { get; set; }
        public virtual Shop Tienda { get; set; }
        public virtual string Codigo { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
        public virtual int CantidadTotal
        {
            get
            {
                return CantidadBuenEstado + CantidadDefectuoso;
            }
        }

        public virtual double PrecioBuenEstado { get; set; }
        public virtual double PrecioDefectuoso { get; set; }
    }
    public class Shop
    {
        public virtual int ShopId { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Direccion { get; set; }
        public virtual int Telefono { get; set; }
        public virtual Trabajador Encargado { get; set; }
        public virtual int TrabajadorId { get; set; }
        public virtual ICollection<Trabajador> Trabajadores { get; set; }
        public virtual ICollection<Existencia> Productos { get; set; }
    }
    public class ReporteVenta
    {
        public virtual int ReporteVentaId { get; set; }
        public virtual int ShopId { get; set; }
        public virtual Shop Tienda { get; set; }
        public virtual ICollection<ArticuloVenta> Articulos { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual int TrabajadorId { get; set; }
        public virtual Trabajador Trabajador { get; set; }
        public virtual double CostoTotal
        {
            get
            {
                double temp = 0;
                foreach (var art in Articulos)
                {
                    temp += art.Costo;
                }
                return temp;
            }
        }
        public virtual double Pagado { get; set; }
        public virtual int CantidadTotal
        {
            get
            {
                int temp = 0;
                foreach (var art in Articulos)
                {
                    temp += art.CantidadTotal;
                }
                return temp;
            }
        }

        public string EscribirReporte()
        {
            string reporte = this.Fecha.ToShortDateString() + " ";
            
            reporte += "Productos vendidos: \n";

            foreach (var item in Articulos)
            {
                reporte += "    " + item.Codigo + ": Buen Estado: " + item.CantidadBuenEstado.ToString() + " Defectuosos: " + item.CantidadDefectuoso.ToString() + " Total: " + item.CantidadTotal.ToString() + "\n";
            }

            return reporte;
        }

        public ReporteVenta()
        {
        }
    }
    public class ArticuloVenta
    {
        public virtual int ArticuloVentaId { get; set; }
        public virtual int ReporteVentaId { get; set; }
        public virtual ReporteVenta ReporteVenta { get; set; }
        public virtual string Codigo { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
        public virtual int CantidadTotal { get { return CantidadBuenEstado + CantidadDefectuoso; } }
        public virtual double Precio { get; set; }
        public virtual double PrecioDefectuoso { get; set; }
        public virtual double Costo
        {
            get
            {
                return Precio * CantidadBuenEstado + PrecioDefectuoso * CantidadDefectuoso;
            }
        }
    }
    public class ReporteDeuda
    {
        public virtual int ReporteDeudaId { get; set; }
        public virtual int ShopId { get; set; }
        public virtual Shop Tienda { get; set; }
        public virtual ICollection<ArticuloDeuda> Articulos { get; set; }
        public virtual double CostoTotal
        {
            get
            {
                double temp = 0;
                foreach (var art in Articulos)
                {
                    temp += art.Costo;
                }
                return temp;
            }
        }
        public virtual int CantidadTotal
        {
            get
            {
                int temp = 0;
                foreach (var art in Articulos)
                {
                    temp += art.CantidadTotal;
                }
                return temp;
            }
        }
        public virtual double Pagado { get; set; }
        public virtual DateTime Fecha { get; set; }

        public string EscribirReporte()
        {
            string reporte = "";

            reporte += "Productos de deuda: \n";

            foreach (var item in Articulos)
            {
                reporte += "    " + item.Codigo + ": Buen Estado: " + item.CantidadBuenEstado.ToString() + " Defectuosos: " + item.CantidadDefectuoso.ToString() + " Total: " + item.CantidadTotal.ToString() + "\n";
            }

            return reporte;
        }
    }
    public class ArticuloDeuda
    {
        public virtual int ArticuloDeudaId { get; set; }
        public virtual int ReporteDeudaId { get; set; }
        public virtual ReporteDeuda ReporteDeuda { get; set; }
        public virtual string Codigo { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
        public virtual int CantidadTotal { get { return CantidadBuenEstado + CantidadDefectuoso; } }
        public virtual double Precio { get; set; }
        public virtual double PrecioDefectuoso { get; set; }
        public virtual double Costo
        {
            get
            {
                return Precio * CantidadBuenEstado + PrecioDefectuoso * CantidadDefectuoso;
            }
        }
    }
    public class ReporteDevolucion
    {
        public virtual int ReporteDevolucionId { get; set; }
        public virtual int ShopId { get; set; }
        public virtual Shop Tienda { get; set; }
        public virtual ICollection<ArticuloDevolucion> Articulos { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual int CantidadTotal
        {
            get
            {
                int temp = 0;
                foreach (var art in Articulos)
                {
                    temp += art.CantidadTotal;
                }
                return temp;
            }
        }
        public string EscribirReporte()
        {
            string reporte = "";

            reporte += "Productos devueltos: \n";

            foreach (var item in Articulos)
            {
                reporte += "    " + item.Codigo + ": Buen Estado: " + item.CantidadBuenEstado.ToString() + " Defectuosos: " + item.CantidadDefectuoso.ToString() + " Total: " + item.CantidadTotal.ToString() + "\n";
            }

            return reporte;
        }
    }
    public class ArticuloDevolucion
    {
        public virtual int ArticuloDevolucionId { get; set; }
        public virtual int ReporteDevolucionId { get; set; }
        public virtual ReporteDevolucion ReporteDevolucion { get; set; }
        public virtual string Codigo { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
        public virtual int CantidadTotal
        {
            get
            {
                return CantidadBuenEstado + CantidadDefectuoso;
            }
        }


    }
    public class ReporteEntrada
    {
        public virtual int ReporteEntradaId { get; set; }
        public virtual ICollection<ArticuloEntrada> Articulos { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual int CantidadTotal
        {
            get
            {
                int temp = 0;
                foreach (var art in Articulos)
                {
                    temp += art.CantidadTotal;
                }
                return temp;
            }
        }

        public string EscribirReporte()
        {
            string reporte = "";

            reporte += this.Fecha.ToShortDateString() + " Entrada de los productos: \n";

            foreach (var item in Articulos)
            {
                reporte += "    " + item.Codigo + ": Buen Estado: " + item.CantidadBuenEstado.ToString() + " Defectuosos: " + item.CantidadDefectuoso.ToString() + " Total: " + item.CantidadTotal.ToString() + "\n";
            }

            return reporte;
        }
    }
    public class ArticuloEntrada
    {
        public virtual int ArticuloEntradaId { get; set; }
        public virtual int ReporteEntradaId { get; set; }
        public virtual ReporteEntrada ReporteEntrada { get; set; }
        public virtual string Codigo { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
        public virtual int CantidadTotal { get { return CantidadBuenEstado + CantidadDefectuoso; } }
    }
    public class ReporteTransferencia
    {
        public virtual int ReporteTransferenciaId { get; set; }
        public virtual int ShopId { get; set; }
        public virtual Shop Tienda { get; set; }
        public virtual ICollection<ArticuloTransferencia> Articulos { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual int CantidadTotal
        {
            get
            {
                int temp = 0;
                foreach (var art in Articulos)
                {
                    temp += art.CantidadTotal;
                }
                return temp;
            }
        }

        public string EscribirReporte()
        {
            string reporte = "";

            reporte += this.Fecha.ToShortDateString() + " Se transfieren productos a la tienda " + this.Tienda.Nombre +" \n";

            foreach (var item in Articulos)
            {
                reporte += "    " + item.Codigo + ": Buen Estado: " + item.CantidadBuenEstado.ToString() + " Defectuosos: " + item.CantidadDefectuoso.ToString() + " Total: " + item.CantidadTotal.ToString() + "\n";
            }

            return reporte;
        }

    }
    public class ArticuloTransferencia
    {
        public virtual int ArticuloTransferenciaId { get; set; }
        public virtual int ReporteTransferenciaId { get; set; }
        public virtual ReporteTransferencia ReporteTransferencia { get; set; }
        public virtual string Codigo { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual int CantidadBuenEstado { get; set; }
        public virtual int CantidadDefectuoso { get; set; }
        public virtual int CantidadTotal { get { return CantidadBuenEstado + CantidadDefectuoso; } }

    }
    public class InformeLiquidacion
    {

        public virtual int InformeLiquidacionId { get; set; }
        public virtual int ShopId { get; set; }
        public virtual Shop Tienda { get; set; }
        public virtual int ReporteVentaId { get; set; }
        public virtual ReporteVenta ReporteVenta { get; set; }
        public virtual int ReporteDeudaId { get; set; }
        public virtual ReporteDeuda ReporteDeuda { get; set; }
        public virtual int ReporteDevolucionId { get; set; }
        public virtual ReporteDevolucion ReporteDevolucion { get; set; }
        public virtual DateTime Fecha { get; set; }

        public string EscribirReporte()
        {
            string reporte = this.Fecha.ToShortDateString() + " " + "Informe de Liquidacion (Id" + this.InformeLiquidacionId.ToString()+"):\n";
            reporte += "    " + "Articulos Vendidos:\n";
            foreach(var item in this.ReporteVenta.Articulos)
            {
                reporte += "        " + item.Codigo + ": Buen Estado: " + item.CantidadBuenEstado.ToString() + " Defectuosos: " + item.CantidadDefectuoso.ToString() + " Total: " + item.CantidadTotal.ToString() + "\n";
                
            }
            reporte += "    Articulos Devueltos:\n";
            foreach (var item in this.ReporteDevolucion.Articulos)
            {
                reporte += "        " + item.Codigo + ": Buen Estado: " + item.CantidadBuenEstado.ToString() + " Defectuosos: " + item.CantidadDefectuoso.ToString() + " Total: " + item.CantidadTotal.ToString() + "\n";
            }
            reporte += "    Articulos vendidos pero con deuda: \n";
            foreach (var item in this.ReporteDeuda.Articulos)
            {
                reporte += "        " + item.Codigo + ": Buen Estado: " + item.CantidadBuenEstado.ToString() + " Defectuosos: " + item.CantidadDefectuoso.ToString() + " Total: " + item.CantidadTotal.ToString() + "\n";

            }
            return reporte;
        }
    }

}
