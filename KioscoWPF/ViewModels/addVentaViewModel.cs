using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace KioscoWPF.ViewModels
{
    class addVentaViewModel : Base.ViewModelBase
    {
        #region Initialize
        public addVentaViewModel() { resetForm(); }

        void resetForm()
        {
            fechasModel tempFecha = Db.returnFecha();
            string tempHora = Variables.strHora;
            selectedVenta = new ventasModel() { Fecha = tempFecha, Hora = tempHora, Usuario = Variables.UsuarioLogueado, Caja = new cajaModel() { Fecha = tempFecha, Hora = tempHora } };
            resetNewProductoVenta();
        }
        void resetNewProductoVenta()
        {
            if (newProductoVenta == null || newProductoVenta.Producto != null)
            {
                newProductoVenta = new ventaProductosModel() { Cantidad = 1 };
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
        public bool bolMantenerVentanaAbierta { get => _bolMantenerVentanaAbierta; set { if (SetProperty(ref _bolMantenerVentanaAbierta, value)) { OnPropertyChanged(); } } }

        bool _ventaFallo = false;
        public bool ventaFallo { get => _ventaFallo; set { if (SetProperty(ref _ventaFallo, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(windowBackground)); } } }


        public string windowBackground => !ventaFallo ? Variables.colorWindowBackgroundOK : Variables.colorWindowBackkgroundNO;
        #endregion // Variables



        #region Helpers
        void helperAgregarQuitarProducto(object sentParameter)
        {
            if (sentParameter is productosModel tempProducto)
            {
                if (tempProducto.Agregado)
                {
                    try
                    {
                        _ = selectedVenta.VentaProductosPerVenta.Remove(selectedVenta.VentaProductosPerVenta.Single(x => x.Producto == tempProducto));
                        tempProducto.Agregado = false;
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        vHelperCantidad = new Views.helperCantidadView();
                        if (vHelperCantidad.ShowDialog().Value)
                        {
                            selectedVenta.VentaProductosPerVenta.Add(new ventaProductosModel()
                            { Cantidad = vHelperCantidad.intCantidad, Precio = tempProducto.PrecioActual, Producto = tempProducto });
                            tempProducto.Agregado = true;
                        }
                    }
                    catch { }
                }
                selectedVenta.updateModel();
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

        ventasModel _selectedVenta;
        public ventasModel selectedVenta { get => _selectedVenta; set { if (_selectedVenta != value) { _selectedVenta = value; OnPropertyChanged(); } } }

        ventaProductosModel _newProductoVenta;
        public ventaProductosModel newProductoVenta { get => _newProductoVenta; set { if (_newProductoVenta != value) { _newProductoVenta = value; OnPropertyChanged(); } } }

        ventaProductosModel _selectedProductoVenta;
        public ventaProductosModel selectedProductoVenta { get => _selectedProductoVenta; set { if (_selectedProductoVenta != value) { _selectedProductoVenta = value; OnPropertyChanged(); } } }

        deudoresModel _selectedDeudor = null;
        public deudoresModel selectedDeudor { get => _selectedDeudor; set { if (_selectedDeudor != value) { _selectedDeudor = value; OnPropertyChanged(); } } }
        #endregion // Variables



        #region Helpers
        void helperAgregarProductoAVenta(object sentParameter)
        {
            try
            {
                ventaProductosModel duplicate = selectedVenta.VentaProductosPerVenta.Single(x => x.Producto == newProductoVenta.Producto);
                duplicate.Cantidad += newProductoVenta.Cantidad;
            }
            catch
            {
                selectedVenta.VentaProductosPerVenta.Add(newProductoVenta);
                newProductoVenta.Producto.Agregado = true;
            }
            newProductoVenta = new ventaProductosModel() { Cantidad = 1 };
            strCodigo = "";
            selectedVenta.updateModel();
        }

        void helperSearchProduct(object sender = null)
        {
            try
            {
                productosModel tempProd = Variables.Inventario.Productos.Local.Single(x => x.Codigo.ToLower() == strCodigo.ToLower());
                newProductoVenta.Producto = tempProd;
                newProductoVenta.Precio = tempProd.PrecioActual;
                if (newProductoVenta.Cantidad < 1) { Variables.nextTarget(sender); }
                _strCodigoFailed = null; _intCodigoFailedCount = 0;
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
                            if (gOpenAddProducto(strCodigo))
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

                foreach (ventaProductosModel prod in selectedVenta.VentaProductosPerVenta) { prod.Producto.Stock -= prod.Cantidad; }
                Db.contabilizarCaja(selectedVenta.Caja);
                Variables.Inventario.Ventas.Local.Add(selectedVenta);

                _ = Variables.Inventario.SaveChanges();

                if (!bolMantenerVentanaAbierta) { thisWindow.Close(); } else { resetForm(); ventaFallo = false; }
            }
            else
            {
                Views.pagarVentaView vPagarVenta = new Views.pagarVentaView(selectedVenta.PrecioTotal);

                if (vPagarVenta.ShowDialog().Value)
                {
                    cajaModel toDitchCaja = new cajaModel() { };

                    selectedVenta.Caja.MercadoPago = toDitchCaja.MercadoPago = vPagarVenta.resultMercadoPago;
                    selectedVenta.Caja.CajaActual = toDitchCaja.CajaActual = vPagarVenta.resultPagadoPesos;
                    selectedVenta.Caja.Vuelto = toDitchCaja.Vuelto = vPagarVenta.resultVuelto;
                    bool tempBoolPagarDeuda = vPagarVenta.resultPagarDeuda;
                    deudoresModel tempDeudor = vPagarVenta.resultDeudor;

                    selectedVenta.Deudor = tempDeudor;


                    selectedVenta.updateModel();
                    Double tempResultadoTotal = toDitchCaja.CajaActual + toDitchCaja.MercadoPago;

                    if (tempDeudor != null) { tempResultadoTotal += tempDeudor.Resto; }

                    if (tempResultadoTotal <= selectedVenta.PrecioTotal)
                    {
                        IOrderedEnumerable<ventaProductosModel> tempProductosVenta = selectedVenta.VentaProductosPerVenta.OrderBy(x => x.PrecioTotal);

                        foreach (ventaProductosModel pv in tempProductosVenta)
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
                        IEnumerable<ventaProductosModel> tempListDeudaPendiente = tempDeudor.VentaProductosPerDeudor.Where(x => x.BolPagado == false);

                        foreach (ventaProductosModel deuda in tempListDeudaPendiente)
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

                    foreach (ventaProductosModel prod in selectedVenta.VentaProductosPerVenta) { prod.Producto.Stock -= prod.Cantidad; }
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
                    (object parameter) => { Views.helperSelectorView hSelector = new Views.helperSelectorView(); if (hSelector.ShowDialog().Value) { strCodigo = hSelector.sendProduct.Codigo; helperSearchProduct(); } });

        public Command comCodigo => new Command((object parameter) => helperSearchProduct(parameter));

        public Command comNuevoProducto => new Command((object parameter) => gOpenAddProducto());

        public Command comAgregarProducto => new Command(
            (object parameter) => helperAgregarProductoAVenta(parameter),
            (object parameter) => newProductoVenta.Producto != null && newProductoVenta.Cantidad > 0);

        public Command comCantidadKeyboardFocus => new Command((object parameter) => { TextBox tb = (TextBox)parameter; tb.SelectAll(); });

        public Command comQuitar => new Command(
                    (object parameter) => { selectedProductoVenta.Producto.Agregado = false; selectedVenta.VentaProductosPerVenta.Remove(selectedProductoVenta); selectedVenta.updateModel(); },
                    (object parameter) => selectedProductoVenta != null);


        public Command comNuevoCliente => new Command((object parameter) => { _ = gOpenAddDeudor(); });

        public Command comGuardarPagoExacto => new Command(
            (object parameter) => helperTryGuardarVenta(true),
            (object parameter) => checkTryGuardarVenta);

        public Command comGuardarVenta => new Command(
            (object parameter) => helperTryGuardarVenta(),
            (object parameter) => checkTryGuardarVenta);
        #endregion // Commands
        #endregion // Venta
    }
}
