using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaStock.xaml
    /// </summary>
    public partial class VentanaStock : Window
    {
        WBinding WBind;

        public VentanaStock() { InitializeComponent(); WBind = new WBinding(this); DataContext = WBind; }

        class WBinding : ObservableClass
        {
            #region Initialize
            VentanaStock thisWindow;

            public WBinding(VentanaStock tempWindow) { thisWindow = tempWindow; }
            #endregion // Initialize



            #region Item Stock
            string _stockTextoBusqueda = "";
            bool _stockBolInactivos = false;
            bool _stockBolAlarma = false;

            public BindingList<productosModel> stockListProducts { get { return _stockListProducts(); } }


            public string stockTextoBusqueda { get { return _stockTextoBusqueda; } set { if (_stockTextoBusqueda != value) { _stockTextoBusqueda = value; OnPropertyChanged(); OnPropertyChanged(nameof(stockListProducts)); } } }
            public bool stockBolInactivos { get { return _stockBolInactivos; } set { if (_stockBolInactivos != value) { _stockBolInactivos = value; OnPropertyChanged(); OnPropertyChanged(nameof(stockListProducts)); } } }
            public bool stockBolAlarma { get { return _stockBolAlarma; } set { if (_stockBolAlarma != value) { _stockBolAlarma = value; OnPropertyChanged(); OnPropertyChanged(nameof(stockListProducts)); } } }

            BindingList<productosModel> _stockListProducts()
            {
                BindingList<productosModel> tempListProducts = new BindingList<productosModel>(Variables.Inventario.Productos.Local.ToBindingList());

                if (!string.IsNullOrWhiteSpace(stockTextoBusqueda)) { tempListProducts = new BindingList<productosModel>(tempListProducts.Where(x => x.Descripcion.ToLower().Contains(stockTextoBusqueda.ToLower())).ToList()); }
                if (!stockBolAlarma) { tempListProducts = new BindingList<productosModel>(tempListProducts.Where(x => x.Tag.Activo == true).ToList()); }
                if (!stockBolInactivos) { tempListProducts = new BindingList<productosModel>(tempListProducts.Where(x => x.Activo == true).ToList()); }
                return tempListProducts;
            }


            void helperRecalcularStock()
            {
                foreach(var prod in Variables.Inventario.Productos.Local)
                {
                    int ing = 0;
                    int deu = 0;
                    int mod = 0;
                    int ven = 0;
                    int sac = 0;
                    int con = 0;

                    try { ing = prod.IngresoProductosPerProducto.Sum(x => x.Cantidad); } catch { }
                    try { mod = prod.ModificadoProductosPerProducto.Sum(x => x.Cantidad); } catch { }
                    try { ven = prod.VentaProductosPerProducto.Sum(x => x.Cantidad); } catch { }
                    try { sac = prod.SacadoProductosPerProducto.Sum(x => x.Cantidad); } catch { }
                    try { con = prod.ConsumoProductosPerProducto.Sum(x => x.Cantidad); } catch { }

                    prod.Stock = prod.StockInicial + ing + mod - deu - ven - sac - con;
                }
                Variables.Inventario.SaveChanges();

                OnPropertyChanged(nameof(stockListProducts));
            }

            public Command comStockRecalcular { get { return new Command((object parameter) => { helperRecalcularStock(); Variables.messageError.StockRecalcular(); }); } }
            #endregion // Item Stock
        }
    }
}
