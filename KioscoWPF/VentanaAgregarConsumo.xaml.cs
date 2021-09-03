using System;
using System.Collections.Generic;
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
    /// Interaction logic for VentanaAgregarConsumo.xaml
    /// </summary>
    public partial class VentanaAgregarConsumo : Window
    {
        public VentanaAgregarConsumo() { InitializeComponent(); (DataContext as ViewModels.VMAgregarConsumo).setInitialize(this); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarConsumo : helperObservableClass
    {
        #region Initialize
        VentanaAgregarConsumo thisWindow;
        VentanaSelectorProductoManual VSProductoManual;

        public void setInitialize(VentanaAgregarConsumo tempWindow)
        {
            thisWindow = tempWindow;
        }
        #endregion // Initialize



        #region Variables
        string _strCodigoFailed;
        int _intCodigoFailedCount = 0;


        string _strCodigo;
        public string strCodigo { get => _strCodigo; set { if (_strCodigo != value) { _strCodigo = value; OnPropertyChanged(); if (selectedConsumo.Producto != null) { selectedConsumo.Producto = null; selectedConsumo.Precio = 0; } } } }


        public DBConsumoProductosClass selectedConsumo { get; } = new DBConsumoProductosClass() { Fecha = Db.returnFecha(), Cantidad = 1 };
        #endregion // Variables



        #region Helpers
        void helperFindProducto(object sender = null)
        {
            try
            {
                selectedConsumo.Producto = Variables.Inventario.Productos.Local.Single(x => x.Codigo.ToLower() == strCodigo.ToLower());
                selectedConsumo.Precio = selectedConsumo.Producto.PrecioActual;
                Variables.nextTarget(sender); _strCodigoFailed = null; _intCodigoFailedCount = 0;
            }
            catch
            {
                selectedConsumo.Producto = null;
                if (!string.IsNullOrWhiteSpace(_strCodigoFailed) && _strCodigoFailed.ToLower() == _strCodigo.ToLower())
                {
                    if (_intCodigoFailedCount >= 1)
                    {
                        if (Variables.messageError.NewProduct())
                        {
                            if (abrirAgregarProducto(strCodigo))
                            {
                                helperFindProducto(sender);
                            }
                        }
                    }
                    else
                    {
                        _intCodigoFailedCount++;
                        if (sender != null) { (sender as TextBox).SelectAll(); }
                    }
                }
                else
                {
                    _strCodigoFailed = _strCodigo;
                    _intCodigoFailedCount++;
                    if (sender != null) { (sender as TextBox).SelectAll(); }
                }
            }
        }

        void helperGuardar()
        {
            if (checkGuardar)
            {
                selectedConsumo.Producto.Stock -= selectedConsumo.Cantidad;
                _ = Variables.Inventario.ConsumosProductos.Add(selectedConsumo);
                _ = Variables.Inventario.SaveChanges();
                selectedConsumo.Fecha.updateTotalConsumosDiario();
                thisWindow.DialogResult = true;
            }
        }

        bool checkGuardar => selectedConsumo != null && selectedConsumo.Producto != null && selectedConsumo.Cantidad > 0;
        #endregion // Helpers



        #region Commands
        public override Command comNextTarget => new Command(
            (object parameter) => Variables.nextTarget(parameter),
            (object parameter) => selectedConsumo.Cantidad > 0);

        public Command comIngresoManual => new Command(
                    (object parameter) => { VSProductoManual = new VentanaSelectorProductoManual(); if (VSProductoManual.ShowDialog().Value) { strCodigo = VSProductoManual.sendProduct.Codigo; helperFindProducto(); } });

        public Command comProducto => new Command(
            (object parameter) => helperFindProducto(parameter),
            (object parameter) => !string.IsNullOrWhiteSpace(strCodigo));

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardar);

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);
        #endregion // Commands
    }
}