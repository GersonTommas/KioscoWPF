using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace KioscoWPF.ViewModels
{
    class addIngresoViewModel : Base.ViewModelBase
    {
        #region Initialize
        public addIngresoViewModel()
        {
            try
            {
                Db.resetProductoAgregado();

                listProveedores.SortDescriptions.Clear(); listProveedores.SortDescriptions.Add(new SortDescription("Nombre", ListSortDirection.Ascending));

                listNuevoIngreso.Proveedor = null;
            }
            catch { }
        }
        #endregion // Initialize



        #region Variables
        readonly ObservableCollection<productosModel> _listProductosAgregados = new ObservableCollection<productosModel>();


        int _indexProveedor = 0;
        public int indexProveedor { get => _indexProveedor; set { if (SetProperty(ref _indexProveedor, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(windowBackground)); OnPropertyChanged(nameof(listRetirosPerProveedor)); } } }

        retirosCajaModel _selectedRetiroPerProveedor;
        public retirosCajaModel selectedRetiroPerProveedor { get => _selectedRetiroPerProveedor; set { if (SetProperty(ref _selectedRetiroPerProveedor, value)) { OnPropertyChanged(); } } }

        productosModel _selectedProduct;
        public productosModel selectedProduct { get => _selectedProduct; set { if (SetProperty(ref _selectedProduct, value)) { OnPropertyChanged(); } } }

        ingresoProductosModel _selectedIngreso;
        public ingresoProductosModel selectedIngreso { get => _selectedIngreso; set { if (SetProperty(ref _selectedIngreso, value)) { OnPropertyChanged(); } } }


        public ingresosModel listNuevoIngreso { get; } = new ingresosModel() { Usuario = Variables.UsuarioLogueado };
        public cajaModel ingresoCaja { get; } = new cajaModel() { Fecha = Db.returnFecha(), Hora = Variables.strHora };
        public int intAgregadosCount => _listProductosAgregados.Count();
        public int intTotal => listNuevoIngreso.IngresoProductosPerIngreso.Count();
        public string windowBackground => indexProveedor >= 0 && listNuevoIngreso.IngresoProductosPerIngreso.Count > 0 && !listNuevoIngreso.IngresoProductosPerIngreso.Any(x => x.Cantidad < 1) ? Variables.colorWindowBackgroundOK : Variables.colorWindowBackkgroundNO;


        readonly CollectionViewSource listProveedoresSource = new CollectionViewSource { Source = Variables.Inventario.Proveedores.Local.ToObservableCollection() };
        public ICollectionView listProveedores => listProveedoresSource.View;

        public IEnumerable<retirosCajaModel> listRetirosPerProveedor => listNuevoIngreso.Proveedor?.RetirosCajaPerProveedor.Where(x => x.Pendiente);
        #endregion // Variables



        #region Helpers
        void helperAgregarQuitarProducto(object sentParameter)
        {
            if (sentParameter is productosModel tempProducto && tempProducto != null)
            {
                if (tempProducto.Agregado)
                {
                    try
                    {
                        _ = listNuevoIngreso.IngresoProductosPerIngreso.Remove(listNuevoIngreso.IngresoProductosPerIngreso.Single(x => x.Producto == tempProducto));
                        tempProducto.Agregado = false;
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        gHelperIngresos = new Views.helperIngresoPreciosView(1, tempProducto.PrecioActual, tempProducto.PrecioIngreso);
                        if (gHelperIngresos.ShowDialog().Value)
                        {
                            listNuevoIngreso.IngresoProductosPerIngreso.Add(new ingresoProductosModel()
                            { Cantidad = gHelperIngresos.resultCantidad, PrecioActual = gHelperIngresos.resultPrecioActual, Producto = tempProducto, PrecioPagado = gHelperIngresos.resultPrecioPagado });
                            tempProducto.Agregado = true;
                        }
                    }
                    catch { }
                }
                OnPropertyChanged(nameof(windowBackground));
            }
        }

        void helperQuitarIngreso(ingresoProductosModel sentIngresoProducto)
        {
            try
            {
                if (sentIngresoProducto != null)
                {
                    _ = listNuevoIngreso.IngresoProductosPerIngreso.Remove(sentIngresoProducto);
                    _ = _listProductosAgregados.Remove(sentIngresoProducto.Producto); sentIngresoProducto.Producto.Agregado = false;
                    OnPropertyChanged(nameof(intAgregadosCount)); OnPropertyChanged(nameof(intTotal));
                }
            }
            catch { }
        }

        void helperGuardar()
        {
            _ = Variables.Inventario.Ingresos.Add(listNuevoIngreso);
            foreach (ingresoProductosModel ingreso in listNuevoIngreso.IngresoProductosPerIngreso)
            {
                ingreso.Producto.PrecioActual = ingreso.PrecioActual;
                ingreso.Producto.Stock += ingreso.Cantidad;
            }

            if (selectedRetiroPerProveedor != null) { selectedRetiroPerProveedor.Pendiente = false; }
            if (ingresoCaja.CajaActual > 0 || ingresoCaja.MercadoPago > 0) { Db.contabilizarCaja(ingresoCaja); }

            _ = Variables.Inventario.SaveChanges();
            thisWindow.Close();
        }


        bool checkGuardarIngreso => listNuevoIngreso.Proveedor != null && listNuevoIngreso.IngresoProductosPerIngreso.Count > 0 && !listNuevoIngreso.IngresoProductosPerIngreso.Any(x => x.Cantidad < 1);
        #endregion // Helpers



        #region Commands
        public override void aCommandAgregarQuitarProducto(object sentParameter = null)
        {
            helperAgregarQuitarProducto(sentParameter);
        }
        public override void aCommandUnSoloProducto(object sentParameter = null)
        {
            if (sentParameter != null) { helperAgregarQuitarProducto(sentParameter); }
        }

        public Command comNuevoProveedor => new Command((object parameter) => gOpenAddProveedor());

        public Command comQuitar => new Command(
                    (object parameter) => helperQuitarIngreso(selectedIngreso),
                    (object parameter) => selectedIngreso != null);

        public Command comNuevoProducto => new Command((object parameter) => gOpenAddProducto());

        public Command comGuardarIngreso => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardarIngreso);
        #endregion // Commands
    }
}
