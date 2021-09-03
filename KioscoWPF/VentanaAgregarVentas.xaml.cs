﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaVentas.xaml
    /// </summary>
    public partial class VentanaAgregarVentas : Window
    {
        public VentanaAgregarVentas() { InitializeComponent(); (DataContext as ViewModels.VMAgregarVentas).setInitialize(this); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarVentas : helperObservableClass
    {
        #region Initialize
        VentanaAgregarVentas thisWindow;

        public void setInitialize(VentanaAgregarVentas tempWindow)
        {
            thisWindow = tempWindow; initilizeClock(); resetForm();
        }

        void resetForm()
        {
            Db.resetProductoAgregado();
            DBFechasClass tempFecha = Db.returnFecha();
            string tempHora = Variables.strHora;
            selectedVenta = new DBVentasClass() { Fecha = tempFecha, Hora = tempHora, Usuario = Variables.UsuarioLogueado, Caja = new DBCajaClass() { Fecha = tempFecha, Hora = tempHora } };
            resetNewProductoVenta();
        }
        void resetNewProductoVenta()
        {
            if (newProductoVenta == null || newProductoVenta.Producto != null)
            {
                newProductoVenta = new DBVentaProductosClass() { Cantidad = 1 };
            }
        }
        #endregion // Initialize



        #region Clock
        readonly System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public void initilizeClock() { Timer.Tick += new EventHandler(timer_Click); Timer.Interval = new TimeSpan(0, 0, 1); Timer.Start(); }
        void timer_Click(object sender, EventArgs e) { strClock = DateTime.Now.ToString("HH:mm"); }
        string _strClock = "00:00";
        public string strClock { get => _strClock; set { if (_strClock != value) { _strClock = value; OnPropertyChanged(nameof(strClock)); } } }
        #endregion // Clock



        #region Variables
        bool _bolMantenerVentanaAbierta = false;
        public bool bolMantenerVentanaAbierta { get => _bolMantenerVentanaAbierta; set { if (_bolMantenerVentanaAbierta != value) { _bolMantenerVentanaAbierta = value; OnPropertyChanged(); } } }

        bool _ventaFallo = false;
        public bool ventaFallo { get => _ventaFallo; set { if (_ventaFallo != value) { _ventaFallo = value; OnPropertyChanged(); OnPropertyChanged(nameof(windowBackground)); } } }


        public string windowBackground => !ventaFallo ? Variables.colorWindowBackgroundOK : Variables.colorWindowBackkgroundNO;
        #endregion // Variables



        #region Helpers
        void helperAgregarQuitarProducto(object sentParameter)
        {
            if (sentParameter is DBProductosClass tempProducto)
            {
                if (tempProducto.Agregado)
                {
                    try
                    {
                        _ = selectedVenta.VentaProductosPerVenta.Remove(selectedVenta.VentaProductosPerVenta.Single(x => x.Producto == tempProducto));
                        tempProducto.Agregado = false;
                    } catch { }
                }
                else
                {
                    try
                    {
                        vHelperCantidad = new VentanaHelperCantidad();
                        if (vHelperCantidad.ShowDialog().Value)
                        {
                            selectedVenta.VentaProductosPerVenta.Add(new DBVentaProductosClass()
                            { Cantidad = vHelperCantidad.intCantidad, Precio = tempProducto.PrecioActual, Producto = tempProducto });
                            tempProducto.Agregado = true;
                        }
                    } catch { }
                }
                selectedVenta.updatePrecios();
                OnPropertyChanged(nameof(windowBackground));
            }
        }
        #endregion // Helpers



        #region Commands
        public Command aComSelectorAgregarQuitar => new Command(
            (object parameter) => helperAgregarQuitarProducto(parameter));

        public Command aComUnSoloProducto => new Command(
            (object parameter) => { if (parameter != null) { helperAgregarQuitarProducto(parameter); } },
            (object parameter) => bolIsOnlyOne);
        #endregion // Commands



        #region Venta
        #region Variables
        string _strCodigoFailed = null;


        string _strCodigo;
        int _intCodigoFailedCount = 0;


        public string strCodigo { get => _strCodigo; set { if (_strCodigo != value) { _strCodigo = value; resetNewProductoVenta(); OnPropertyChanged(); } } }

        DBVentasClass _selectedVenta;
        public DBVentasClass selectedVenta { get => _selectedVenta; set { if (_selectedVenta != value) { _selectedVenta = value; OnPropertyChanged(); } } }

        DBVentaProductosClass _newProductoVenta;
        public DBVentaProductosClass newProductoVenta { get => _newProductoVenta; set { if (_newProductoVenta != value) { _newProductoVenta = value; OnPropertyChanged(); } } }

        DBVentaProductosClass _selectedProductoVenta;
        public DBVentaProductosClass selectedProductoVenta { get => _selectedProductoVenta; set { if (_selectedProductoVenta != value) { _selectedProductoVenta = value; OnPropertyChanged(); } } }

        DBDeudoresClass _selectedDeudor = null;
        public DBDeudoresClass selectedDeudor { get => _selectedDeudor; set { if (_selectedDeudor != value) { _selectedDeudor = value; OnPropertyChanged(); } } }
        #endregion // Variables



        #region Helpers
        void helperAgregarProductoAVenta(object sentParameter)
        {
            try
            {
                DBVentaProductosClass duplicate = selectedVenta.VentaProductosPerVenta.Single(x => x.Producto == newProductoVenta.Producto);
                duplicate.Cantidad += newProductoVenta.Cantidad;
            }
            catch
            {
                selectedVenta.VentaProductosPerVenta.Add(newProductoVenta);
                newProductoVenta.Producto.Agregado = true;
            }
            newProductoVenta = new DBVentaProductosClass() { Cantidad = 1 };
            strCodigo = "";
            selectedVenta.updatePrecios();
        }

        void helperSearchProduct(object sender = null)
        {
            try
            {
                DBProductosClass tempProd = Variables.Inventario.Productos.Local.Single(x => x.Codigo.ToLower() == strCodigo.ToLower());
                newProductoVenta.Producto = tempProd;
                newProductoVenta.Precio = tempProd.PrecioActual;
                if (newProductoVenta.Cantidad < 1) { Variables.nextTarget(sender); } _strCodigoFailed = null; _intCodigoFailedCount = 0;
                OnPropertyChanged(nameof(newProductoVenta));
            }
            catch
            {
                newProductoVenta.Producto = null;
                if (!string.IsNullOrWhiteSpace(_strCodigoFailed) && _strCodigoFailed.ToLower() == _strCodigo.ToLower())
                {
                    if (_intCodigoFailedCount >= 1)
                    {
                        if (Variables.messageError.NewProduct())
                        {
                            if (abrirAgregarProducto(strCodigo))
                            {
                                helperSearchProduct(sender);
                            }
                        }
                    }
                    else
                    {
                        _intCodigoFailedCount += 1;
                        try { (sender as TextBox).SelectAll(); } catch { }
                    }
                }
                else
                {
                    _strCodigoFailed = _strCodigo;
                    _intCodigoFailedCount += 1;
                    try { (sender as TextBox).SelectAll(); } catch { }
                }
            }
        }

        void helperTryGuardarVenta(bool sentIsNoChange = false)
        {
            if (sentIsNoChange)
            {
                selectedVenta.Caja.CajaActual = selectedVenta.PrecioTotal;
                selectedVenta.Caja.MercadoPago = 0;
                selectedVenta.Caja.Vuelto = 0;
                selectedVenta.Deudor = null;

                foreach (DBVentaProductosClass prod in selectedVenta.VentaProductosPerVenta) { prod.Producto.Stock -= prod.Cantidad; }
                Db.contabilizarCaja(selectedVenta.Caja);
                Variables.Inventario.Ventas.Local.Add(selectedVenta);

                _ = Variables.Inventario.SaveChanges();

                if (!bolMantenerVentanaAbierta) { thisWindow.Close(); } else { resetForm(); ventaFallo = false; }
            }
            else
            {
                VentanaPagarVenta vPagarVenta = new VentanaPagarVenta(selectedVenta.PrecioTotal);

                if (vPagarVenta.ShowDialog().Value)
                {
                    DBCajaClass toDitchCaja = new DBCajaClass() { };

                    selectedVenta.Caja.MercadoPago = toDitchCaja.MercadoPago = vPagarVenta.resultMercadoPago;
                    selectedVenta.Caja.CajaActual = toDitchCaja.CajaActual = vPagarVenta.resultPagadoPesos;
                    selectedVenta.Caja.Vuelto = toDitchCaja.Vuelto = vPagarVenta.resultVuelto;
                    bool tempBoolPagarDeuda = vPagarVenta.resultPagarDeuda;
                    DBDeudoresClass tempDeudor = vPagarVenta.resultDeudor;

                    selectedVenta.Deudor = tempDeudor;


                    selectedVenta.updatePrecios();
                    Double tempResultadoTotal = toDitchCaja.CajaActual + toDitchCaja.MercadoPago;

                    if (tempDeudor != null) { tempResultadoTotal += tempDeudor.Resto; }

                    if (tempResultadoTotal <= selectedVenta.PrecioTotal)
                    {
                        IOrderedEnumerable<DBVentaProductosClass> tempProductosVenta = selectedVenta.VentaProductosPerVenta.OrderBy(x => x.PrecioTotal);

                        foreach (DBVentaProductosClass pv in tempProductosVenta)
                        {
                            if (tempResultadoTotal >= pv.Precio)
                            {
                                int tempCantidadFaltante = 0;
                                for (int i = 0; i < pv.Cantidad; i++)
                                {
                                    if (tempResultadoTotal >= pv.Precio)
                                    {
                                        tempResultadoTotal -= pv.Precio;
                                    }
                                    else
                                    {
                                        tempCantidadFaltante++;
                                    }
                                }
                                if (tempCantidadFaltante > 0)
                                {
                                    pv.CantidadDeuda = tempCantidadFaltante; pv.CantidadFaltante = tempCantidadFaltante; pv.Deudor = selectedVenta.Deudor;
                                }
                            }
                            else
                            {
                                pv.CantidadDeuda = pv.Cantidad; pv.CantidadFaltante = pv.Cantidad; pv.Deudor = selectedVenta.Deudor;
                            }
                        }
                    }
                    else { tempResultadoTotal -= selectedVenta.PrecioTotal; }

                    if (tempBoolPagarDeuda && tempResultadoTotal > 0)
                    {
                        IEnumerable<DBVentaProductosClass> tempListDeudaPendiente = tempDeudor.VentaProductosPerDeudor.Where(x => x.BolPagado == false);

                        foreach (DBVentaProductosClass deuda in tempListDeudaPendiente)
                        {
                            bool tempBoolAgregarCaja = false;

                            if (tempResultadoTotal >= deuda.PrecioFinal)
                            {
                                for (int i = 0; i < deuda.CantidadFaltante; i++)
                                {
                                    if (tempResultadoTotal >= deuda.PrecioFinal)
                                    {
                                        tempResultadoTotal -= deuda.PrecioFinal;
                                        i -= 1;
                                        deuda.CantidadFaltante -= 1;
                                        tempBoolAgregarCaja = true;
                                    }
                                }
                            }
                            if (tempBoolAgregarCaja) { deuda.CajasPerVentaProducto.Add(selectedVenta.Caja); }
                        }
                    }

                    if (tempDeudor != null)
                    {
                        if (selectedVenta.Caja.Vuelto <= 0)
                        { tempDeudor.Resto = tempResultadoTotal; selectedVenta.Caja.Vuelto = 0; }
                        else
                        {
                            tempDeudor.Resto = 0;
                        }
                    }

                    foreach (DBVentaProductosClass prod in selectedVenta.VentaProductosPerVenta) { prod.Producto.Stock -= prod.Cantidad; }
                    Db.contabilizarCaja(selectedVenta.Caja);
                    _ = Variables.Inventario.Ventas.Add(selectedVenta);

                    _ = Variables.Inventario.SaveChanges();

                    if (tempDeudor != null) { Variables.globalVentanaPrincipal.updateDeudores(); }

                    if (!bolMantenerVentanaAbierta) { thisWindow.Close(); } else { resetForm(); ventaFallo = false; }
                }
                else { ventaFallo = true; }
            }
        }

        bool checkTryGuardarVenta => selectedVenta != null && selectedVenta.VentaProductosPerVenta.Count > 0;
        #endregion // Helpers



        #region Commands
        public Command comCodigoAgregarProducto => new Command(
            (object parameter) => { helperSearchProduct(parameter); if (newProductoVenta.Producto != null && newProductoVenta.Cantidad > 0) { helperAgregarProductoAVenta(parameter); } },
            (object parameter) => strCodigo != null && strCodigo.Length > 0);

        public Command comPreviousControl => new Command(
            (object parameter) => Variables.nextTarget(parameter, true),
            (object parameter) => newProductoVenta.Cantidad > 0);

        public Command comIngresoManual => new Command(
                    (object parameter) => { vSProductoManual = new VentanaSelectorProductoManual(); if (vSProductoManual.ShowDialog().Value) { strCodigo = vSProductoManual.sendProduct.Codigo; helperSearchProduct(); } });

        public Command comCodigo => new Command((object parameter) => helperSearchProduct(parameter));

        public Command comNuevoProducto => new Command((object parameter) => abrirAgregarProducto());

        public Command comAgregarProducto => new Command(
            (object parameter) => helperAgregarProductoAVenta(parameter),
            (object parameter) => newProductoVenta.Producto != null && newProductoVenta.Cantidad > 0);

        public Command comCantidadKeyboardFocus => new Command((object parameter) => { TextBox tb = (TextBox)parameter; tb.SelectAll(); });

        public Command comQuitar => new Command(
                    (object parameter) => { selectedProductoVenta.Producto.Agregado = false; selectedVenta.VentaProductosPerVenta.Remove(selectedProductoVenta); selectedVenta.updatePrecios(); },
                    (object parameter) => selectedProductoVenta != null);


        public Command comNuevoCliente => new Command((object parameter) => { _ = abrirAgregarDeudor(); });

        public Command comGuardarPagoExacto => new Command(
            (object parameter) => helperTryGuardarVenta(true),
            (object parameter) => checkTryGuardarVenta);

        public Command comGuardarVenta => new Command(
            (object parameter) => helperTryGuardarVenta(),
            (object parameter) => checkTryGuardarVenta);

        public Command comCancelar => new Command((object parameter) => thisWindow.Close());
        #endregion // Commands
        #endregion // Venta
    }
}
