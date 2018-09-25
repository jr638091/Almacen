using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : Page
    {
        public class infoProd
        {
            public string Nombre { get; set; }
            public string CantBE { get; set; }
            public string CantDef { get; set; }
            public infoProd(string Nombre, string CantBE, string CantDef)
            {
                this.Nombre = Nombre;
                this.CantBE = CantBE;
                this.CantDef = CantDef;
            }
        }

        private string codigo_src;
        private string descrip_src;
        private List<Producto> source_productos;
        public Productos()
        {
            InitializeComponent();
            codigo_src = "";
            descrip_src = "";
            source_productos = new List<Producto>();
        }

        private void Entrada_Button_Click(object sender, RoutedEventArgs e)
        {
            var p = new Entrada();
            this.NavigationService.Navigate(p);
        }

        private void Mis_Tiendas_Click(object sender, RoutedEventArgs e)
        {
            var p = new MisTiendas();
            this.NavigationService.Navigate(p);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                source_productos = db.Productos.Where(p => p.Codigo.Contains(codigo_src) && p.Descripcion.Contains(descrip_src)).ToList();
                dgrid_productos.ItemsSource = source_productos;
            }
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            codigo_src = tbox_codigo_src.Text;
            descrip_src = tbox_descripcion_src.Text;
            dgrid_productos.ItemsSource = source_productos.Where(exis => exis.Codigo.Contains(codigo_src) && exis.Descripcion.Contains(descrip_src)).ToList();
        }

        private void dgrid_productos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgrid_productos.SelectedItem != null && dgrid_productos.SelectedItem is Producto)
            {
                var producto = dgrid_productos.SelectedItem as Producto;
                dgrid_existencia.ItemsSource = new List<object>();
                using (var db = new TiendaDbContext())
                {
                    var tiendas = db.Tiendas.Where(t => t.Productos.Where(p => p.Codigo == producto.Codigo && (p.CantidadBuenEstado > 0 || p.CantidadDefectuoso > 0)).Count() > 0).ToList();
                    List<infoProd> infos = new List<infoProd>();
                    foreach (var item in tiendas)
                    {
                        infos.Add(new infoProd(item.Nombre, item.Productos.First(x => x.Codigo == producto.Codigo).CantidadBuenEstado.ToString(), item.Productos.First(x => x.Codigo == producto.Codigo).CantidadDefectuoso.ToString()));
                    }
                    dgrid_existencia.ItemsSource = infos;

                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MiTiendaPagina());
        }

        private void dgrid_productos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
            var column = e.Column as DataGridBoundColumn;
            if (column != null)
            {
                var bindingPath = (column.Binding as Binding).Path.Path;

                // codigo pastilla

                int index = dgrid_productos.SelectedIndex;
                List<Producto> lista = new List<Producto>();
                for(int i = 0; i<dgrid_productos.Items.Count -1; ++i)
                {
                    if (i == index)
                        continue;
                    lista.Add(dgrid_productos.Items[i] as Producto);
                }

                var resultante = source_productos.Where(p => lista.Find(k => k.Codigo == p.Codigo) == null);
                if (resultante.Count() == 0)
                {
                    MessageBox.Show("No puede usar ese codigo");
                    e.Cancel = true;
                    return;
                }


                // termina codigo pastilla

                string codigo_ant = resultante.First().Codigo;
                
                if (bindingPath == "Codigo")
                {
                    using (var db = new TiendaDbContext())
                    {
                        string codigo_nuevo = (e.EditingElement as TextBox).Text;
                        if (codigo_nuevo == codigo_ant)
                            return;
                        if(db.Productos.Where(p => p.Codigo == codigo_nuevo).Count() >0)
                        {
                            MessageBox.Show("No puede usar ese codigo");
                            e.Cancel = true;
                            return;
                        }
                        foreach (var existencia in db.Existencias.Where(p => p.Codigo == codigo_ant))
                        {
                            existencia.Codigo = codigo_nuevo;
                            db.Entry(existencia).State = EntityState.Modified;
                        }
                        foreach (var existencia in db.ArticuloDeudas.Where(p => p.Codigo == codigo_ant))
                        {
                            existencia.Codigo = codigo_nuevo;
                            db.Entry(existencia).State = EntityState.Modified;
                        }
                        foreach (var existencia in db.ArticuloVentas.Where(p => p.Codigo == codigo_ant))
                        {
                            existencia.Codigo = codigo_nuevo;
                            db.Entry(existencia).State = EntityState.Modified;
                        }
                        foreach (var existencia in db.ArticuloDevoluciones.Where(p => p.Codigo == codigo_ant))
                        {
                            existencia.Codigo = codigo_nuevo;
                            db.Entry(existencia).State = EntityState.Modified;
                        }
                        foreach (var existencia in db.ArticuloTransferencias.Where(p => p.Codigo == codigo_ant))
                        {
                            existencia.Codigo = codigo_nuevo;
                            db.Entry(existencia).State = EntityState.Modified;
                        }
                        foreach (var existencia in db.ArticuloEntradas.Where(p => p.Codigo == codigo_ant))
                        {
                            existencia.Codigo = codigo_nuevo;
                            db.Entry(existencia).State = EntityState.Modified;
                        }
                        var producto = db.Productos.Find(codigo_ant);
                        var nuevo_producto = new Producto { Codigo = codigo_nuevo, Descripcion = producto.Descripcion };
                        db.Productos.Add(nuevo_producto);
                        db.SaveChanges();
                        db.Productos.Remove(producto);
                        db.SaveChanges();
                        source_productos.RemoveAt(source_productos.FindIndex(p => p.Codigo == codigo_ant));
                        source_productos.Add(nuevo_producto);

                    }
                }
                else
                {
                    using (var db = new TiendaDbContext())
                    {
                        string descripcion_nueva = (e.EditingElement as TextBox).Text;
                        var producto = db.Productos.Find(codigo_ant);
                        producto.Descripcion = descripcion_nueva;
                        db.Entry(producto).State = EntityState.Modified;
                        db.SaveChanges();
                        source_productos.Find(p => p.Codigo == codigo_ant).Descripcion = descripcion_nueva;
                    }

                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void dgrid_productos_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (dgrid_productos.SelectedItem == null || (dgrid_productos.SelectedItem as Producto).Codigo == "")
                e.Cancel = true;
        }
    }
}
