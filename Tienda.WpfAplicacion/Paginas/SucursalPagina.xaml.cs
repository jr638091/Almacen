using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
using OfficeExcel = Microsoft.Office.Interop.Excel;
using winForm = System.Windows.Forms;

namespace WpfAplicacion
{
    /// <summary>
    /// Lógica de interacción para SucursalPagina.xaml
    /// </summary>
    public partial class SucursalPagina : Page
    {
        int tienda;
        public SucursalPagina()
        {
            InitializeComponent();
        }

        public SucursalPagina(int shopId)
        {
            
            InitializeComponent();
            tienda = shopId;
            using (var db = new TiendaDbContext())
            {
                var tienda = db.Tiendas.First(x => x.ShopId == shopId);
                Tienda_Encargado.Text = tienda.Encargado.Nombre;
                top.Title = tienda.Nombre;
                Tienda_Nombre.Text = tienda.Nombre;
                Tienda_Direccion.Text = tienda.Direccion;
            
                Tienda_Telefono.Text = tienda.Telefono.ToString();
            }
        }

        private void btn_Informe_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LiquidacionPagina(tienda));
        }

        private void btn_entrada_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaginaSucursalEntrda(tienda));
        }

        private void Edit_btn_Click(object sender, MouseButtonEventArgs e)
        {
            Tienda_Direccion.IsEnabled = true;
            Tienda_Encargado.IsEnabled = true;
            Tienda_Nombre.IsEnabled = true;
            Tienda_Telefono.IsEnabled = true;
            Edit_btn.Visibility = Visibility.Hidden;
            Accept_btn.Visibility = Visibility.Visible;
            Canel_btn.Visibility = Visibility.Visible;
        }

        private void Accept_btn_click(object sender, MouseButtonEventArgs e)
        {
            using (var db = new TiendaDbContext())
            {
                var t = db.Tiendas.First(x => x.ShopId == tienda);
                t.Nombre = Tienda_Nombre.Text;
                t.Telefono = int.Parse(Tienda_Telefono.Text);
                t.Direccion = Tienda_Direccion.Text;
                var c = db.Trabajadores.First(x => x.ShopId == t.ShopId);
                c.Nombre = Tienda_Encargado.Text;
                db.SaveChanges();
                top.Title = t.Nombre;
            }
            Tienda_Direccion.IsEnabled = false;
            Tienda_Encargado.IsEnabled = false;
            Tienda_Nombre.IsEnabled = false;
            Tienda_Telefono.IsEnabled = false;
            Edit_btn.Visibility = Visibility.Visible;
            Accept_btn.Visibility = Visibility.Hidden;
            Canel_btn.Visibility = Visibility.Hidden;
        }

        private void Cancel_btn_click(object sender, MouseButtonEventArgs e)
        {
            Tienda_Direccion.IsEnabled = false;
            Tienda_Encargado.IsEnabled = false;
            Tienda_Nombre.IsEnabled = false;
            Tienda_Telefono.IsEnabled = false;
            Edit_btn.Visibility = Visibility.Visible;
            Accept_btn.Visibility = Visibility.Hidden;
            Canel_btn.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Formularios.DeudasLiquidacionPagina(tienda));
        }

        class art_temp
        {
            public string Codigo { get; set; }
            public string Decripcion { get; set; }
            public int CantidadBuenEstado { get; set; }
            public int CantidadDefectuoso { get; set; }
            public int CantidadTotal { get; set; }
            public double PrecioBuenEstado { get; set; }
            public double PrecioDefectuoso { get; set; }
            public art_temp(string cod, string des, int cbe, int cme, int ct, double pbe, double pme)
            {
                Codigo = cod;
                Decripcion = des;
                CantidadBuenEstado = cbe;
                CantidadDefectuoso = cme;
                CantidadTotal = ct;
                PrecioBuenEstado = pbe;
                PrecioDefectuoso = pme;
            }
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using(var db = new TiendaDbContext())
            {
                List<art_temp> t = new List<art_temp>();
                foreach (var item in db.Tiendas.Find(tienda).Productos.Where(x => x.CantidadTotal > 0))
                {
                    t.Add(new art_temp(item.Codigo, item.Producto.Descripcion, item.CantidadBuenEstado, item.CantidadDefectuoso, item.CantidadTotal, item.PrecioBuenEstado, item.PrecioDefectuoso));
                }
                dg_prods.ItemsSource = t;
            }
        }

        private void btn_GenExcel_Click(object sender, RoutedEventArgs e)
        {
            var dialogo = new winForm.FolderBrowserDialog();
            if(dialogo.ShowDialog() != winForm.DialogResult.OK)
            {
                return;
            }
            string path = dialogo.SelectedPath;
            

            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            
            string tienda_nombre;
            using (var db = new TiendaDbContext())
            {
                table.TableName = "Precios";
                table.Columns.Add("Codigo");
                table.Columns.Add("Descripcion");
                table.Columns.Add("Precio Buen Estado");
                table.Columns.Add("Precio Defectuoso");

                var existencias = db.Tiendas.Find(tienda).Productos.Where(p => p.CantidadTotal > 0);
                

                foreach(var item in existencias)
                {
                    table.Rows.Add(item.Codigo, item.Producto.Descripcion, item.PrecioBuenEstado, item.PrecioDefectuoso);
                }
                tienda_nombre = existencias.First().Tienda.Nombre;
                
                path += "\\precios(" + tienda_nombre +").xlsx";
                MessageBox.Show(path);
            }
            System.Reflection.Missing d = System.Reflection.Missing.Value;
            int inHeaderLength = 3, inColumn = 0, inRow = 0;

            OfficeExcel.Application excel_app = new OfficeExcel.Application();
            OfficeExcel.Workbook excel_work_book = excel_app.Workbooks.Add(1);

            OfficeExcel.Worksheet excel_sheet = excel_work_book.Sheets.Add(After: excel_work_book.Sheets[excel_work_book.Sheets.Count], Count: 1);
            excel_sheet.Name = table.TableName;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                excel_sheet.Cells[inHeaderLength + 1, i + 1] = table.Columns[i].ColumnName.ToUpper();
            }

            for (int m = 0; m < table.Rows.Count; m++)
            {
                for (int n = 0; n < table.Columns.Count; n++)
                {
                    inColumn = n + 1;
                    inRow = inHeaderLength + 2 + m;
                    excel_sheet.Cells[inRow, inColumn] = table.Rows[m].ItemArray[n].ToString();
                    if (m % 2 == 0)
                        excel_sheet.Range["A" + inRow.ToString(), "D" + inRow.ToString()].Interior.Color = ColorTranslator.FromHtml("#FCE4D6");
                }

            }
            excel_sheet.Range["C" + (inHeaderLength + 1).ToString(), "D1" + (inHeaderLength + 1).ToString()].EntireColumn.NumberFormat = "0.00";

            var cell_header = excel_sheet.Range["A1", "D" + inHeaderLength.ToString()];
            cell_header.Merge(false);
            cell_header.Interior.Color = System.Drawing.Color.White;
            cell_header.Font.Color = System.Drawing.Color.Gray;
            cell_header.HorizontalAlignment = OfficeExcel.XlHAlign.xlHAlignCenter;
            cell_header.VerticalAlignment = OfficeExcel.XlVAlign.xlVAlignCenter;
            cell_header.Font.Size = 26;
            excel_sheet.Cells[1, 1] = tienda_nombre;

            cell_header = excel_sheet.Range["A" + (inHeaderLength + 1).ToString(), "D" + (inHeaderLength + 1).ToString()];
            cell_header.Font.Bold = true;
            cell_header.Font.Color = System.Drawing.Color.White;
            cell_header.Interior.Color = System.Drawing.ColorTranslator.FromHtml("#ED7D31");

            
            // excel_sheet.Range(from, to)
            // EntireColumn es para referirse a la columna
            // NumberFormat = "0.00"
            // NumberFormat = "MM/DD/YYYY" for dates

            excel_sheet.Columns.AutoFit();

            excel_app.DisplayAlerts = false;
            OfficeExcel.Worksheet lastWS = (OfficeExcel.Worksheet)excel_work_book.Worksheets[1];
            lastWS.Delete();
            excel_app.DisplayAlerts = true;

            (excel_work_book.Sheets[1] as OfficeExcel._Worksheet).Activate();

            excel_work_book.SaveAs(Filename: path, ReadOnlyRecommended: false, AccessMode: OfficeExcel.XlSaveAsAccessMode.xlNoChange);
            excel_work_book.Close();

            MessageBox.Show("Se ha generado la tabla de precios");
        }

        private void eliminar_btn_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Estas Seguro","Eliminar Tienda", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using(var db = new TiendaDbContext())
                {
                    var t = db.Tiendas.Find(tienda);
                    t.eliminado = true;
                    t.Encargado.eliminado = true;
                    db.SaveChanges();
                }
                NavigationService.GoBack();
            }
        }
    }
}
