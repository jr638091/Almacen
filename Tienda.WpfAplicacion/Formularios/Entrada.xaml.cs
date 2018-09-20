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
using System.Data.Entity;

namespace WpfAplicacion
{
    /// <summary>
    /// Lógica de interacción para Entrada.xaml
    /// </summary>
    /// 
    
    
    public partial class Entrada : Page, IFormulario
    {
        private List<Producto> source_productos;
        private List<entrada> source_entrada;
        private string codigo_src;
        private string descrip_src;
        public Entrada()
        {
            InitializeComponent();
            codigo_src = "";
            descrip_src = "";

        }

        private void CancelEntry_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btn_adicionar_Click(object sender, RoutedEventArgs e)
        {
            var nuevo_producto = new Producto { Codigo = tbox_codigo.Text, Descripcion = tbox_descripcion.Text };
            using (var db = new TiendaDbContext())
            {
                if (db.Productos.Find(nuevo_producto.Codigo) != null)
                {
                    MessageBox.Show("Ya existe un producto con ese codigo");
                    return;
                }
                double precioBuenEstado;
                double precioDefectuoso;
                if (!double.TryParse(tbox_precioDefectuoso.Text, out precioDefectuoso) || !double.TryParse(tbox_precioBuenEstado.Text, out precioBuenEstado))
                {
                    MessageBox.Show("Rectifique los precios");
                    return;
                }
                db.Productos.Add(nuevo_producto);
                db.SaveChanges();
                source_productos.Add(nuevo_producto);

                int index = dgrid_productos.SelectedIndex;

                dgrid_productos.ItemsSource = null;
                dgrid_productos.ItemsSource = source_productos.Where(s=> s.Codigo.ToLower().Contains(codigo_src) && s.Descripcion.ToLower().Contains(descrip_src)).ToList();
                dgrid_productos.SelectedIndex = index;

                var tiendas = db.Tiendas.ToList();
                foreach (var tienda in tiendas)
                {
                    var exist = new Existencia
                    {
                        CantidadBuenEstado = 0,
                        CantidadDefectuoso = 0,
                        Producto = nuevo_producto,
                        Tienda = tienda,
                        PrecioBuenEstado = precioBuenEstado,
                        PrecioDefectuoso = precioDefectuoso

                    };
                    tienda.Productos.Add(exist);
                    db.Existencias.Add(exist);
                }
                db.SaveChanges();
            }
            tbox_codigo.Text = "";
            tbox_descripcion.Text = "";
            tbox_precioBuenEstado.Text = "";
            tbox_precioDefectuoso.Text = "";
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                source_productos = db.Productos.ToList();
                dgrid_productos.ItemsSource = source_productos;
            }
        }

        private void btn_Agregar_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_productos.SelectedItem != null && dgrid_productos.SelectedItem is Producto)
            {
                var prod = dgrid_productos.SelectedItem as Producto;
                var ent = new entrada(prod.Codigo, prod.Descripcion);
                source_entrada.Add(ent);
                source_productos.RemoveAt(source_productos.FindIndex(p => p.Codigo == prod.Codigo));

                dgrid_entrada.ItemsSource = null;
                dgrid_entrada.ItemsSource = source_entrada;
                dgrid_productos.ItemsSource = source_productos.Where(exis => exis.Codigo.Contains(codigo_src) && exis.Descripcion.Contains(descrip_src)).ToList();

            }
            else
            {
                MessageBox.Show("objeto no seleccionado");
            }
        }

        private void btn_Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid_entrada.SelectedItem != null && dgrid_entrada.SelectedItem is entrada)
            {
                var prod = (dgrid_entrada.SelectedItem as entrada).genera_producto();

                source_productos.Add(prod);
                source_entrada.RemoveAt(source_entrada.FindIndex(p => p.Codigo == prod.Codigo));

                dgrid_entrada.ItemsSource = null;
                dgrid_entrada.ItemsSource = source_entrada;
                dgrid_productos.ItemsSource = source_productos.Where(s => s.Codigo.ToLower().Contains(codigo_src) && s.Descripcion.ToLower().Contains(descrip_src)).ToList();


            }
            else
            {
                MessageBox.Show("Objeto no seleccionado");
            }
        }

        private void btn_cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {

            if (dgrid_entrada.ItemsSource != null)
            {
                using (var db = new TiendaDbContext())
                {
                    var tienda = db.Tiendas.First();
                    var articulo_entrada = new List<ArticuloEntrada>();
                    foreach (var item in dgrid_entrada.ItemsSource)
                    {
                        articulo_entrada.Add((item as entrada).genera_articulo_entrada());
                    }
                    foreach (var articulo in articulo_entrada)
                    {
                        var producto = tienda.Productos.Where(p => p.Codigo == articulo.Codigo).First();
                        producto.CantidadBuenEstado += articulo.CantidadBuenEstado;
                        producto.CantidadDefectuoso += articulo.CantidadDefectuoso;
                        db.Entry(producto).State = EntityState.Modified;
                    }
                    db.Entry(tienda).State = EntityState.Modified;
                    articulo_entrada.ForEach(p => db.ArticuloEntradas.Add(p));
                    var reporte = new ReporteEntrada { Fecha = DateTime.Now, Articulos = articulo_entrada };
                    db.ReporteEntradas.Add(reporte);

                    db.SaveChanges();
                }
            }
            this.NavigationService.GoBack();
        }

        private void dgrid_entrada_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int valor;
            if (!int.TryParse((e.EditingElement as TextBox).Text, out valor))
            {
                e.Cancel = true;
                return;
            }
            if (valor < 0)
                valor = 0;
            var objeto = e.Row.Item as entrada;
            var column = ((e.Column as DataGridBoundColumn).Binding as Binding).Path.Path;
            var obj = source_entrada.Find(r => r.Codigo == objeto.Codigo);
            if (column == "CantidadBuenEstado")
                obj.CantidadBuenEstado = valor;
            else
                obj.CantidadDefectuoso = valor;
            Metodos_Auxiliares.refresh(dgrid_entrada, source_entrada);
        }

        private void dgrid_entrada_Loaded(object sender, RoutedEventArgs e)
        {
            source_entrada = new List<entrada>();
            dgrid_entrada.ItemsSource = source_entrada;
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            codigo_src = tbox_codigo_src.Text.ToLower();
            descrip_src = tbox_descripcion_src.Text.ToLower();
            dgrid_productos.ItemsSource = source_productos.Where(s => s.Codigo.ToLower().Contains(codigo_src) && s.Descripcion.ToLower().Contains(descrip_src)).ToList();
        }
    }
}
