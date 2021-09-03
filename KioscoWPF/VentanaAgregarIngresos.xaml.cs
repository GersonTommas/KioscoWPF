using System;
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
using System.Collections.ObjectModel;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaIngresos.xaml
    /// </summary>
    public partial class VentanaAgregarIngresos : Window
    {
        public VentanaAgregarIngresos() { InitializeComponent(); try { (DataContext as ViewModels.VMAgregarIngresos).setInitialize(this); } catch { } }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarIngresos : helperObservableClass
    {
        #region Initialize
        VentanaAgregarIngresos thisWindow;

        VMAgregarIngresos()
        {
            Db.resetProductoAgregado();

            ingresoCaja.Fecha = listNuevoIngreso.Fecha = Db.returnFecha();
            ingresoCaja.Hora = listNuevoIngreso.Hora = Variables.strHora;


            listProveedores.SortDescriptions.Clear(); listProveedores.SortDescriptions.Add(new SortDescription("Nombre", ListSortDirection.Ascending));
            
        }

        public void setInitialize(VentanaAgregarIngresos tempWindow)
        {
            thisWindow = tempWindow;
            listNuevoIngreso.Proveedor = null;
        }
        #endregion // Initialize



        #region Variables
        VentanaHelperPrecioPrecioCantidad vHelperPrecioPercioCantidad;
        readonly ObservableCollection<DBProductosClass> _listProductosAgregados = new ObservableCollection<DBProductosClass>();


        int _indexProveedor = 0;
        public int indexProveedor { get => _indexProveedor; set { if (_indexProveedor != value) { _indexProveedor = value; OnPropertyChanged(); OnPropertyChanged(nameof(windowBackground)); OnPropertyChanged(nameof(listRetirosPerProveedor)); } } }

        DBRetirosCaja _selectedRetiroPerProveedor;
        public DBRetirosCaja selectedRetiroPerProveedor { get => _selectedRetiroPerProveedor; set { if (_selectedRetiroPerProveedor != value) { _selectedRetiroPerProveedor = value; OnPropertyChanged(); } } }

        DBProductosClass _selectedProduct;
        public DBProductosClass selectedProduct { get => _selectedProduct; set { if (_selectedProduct != value) { _selectedProduct = value; OnPropertyChanged(); } } }

        DBIngresoProductosClass _selectedIngreso;
        public DBIngresoProductosClass selectedIngreso { get => _selectedIngreso; set { if (_selectedIngreso != value) { _selectedIngreso = value; OnPropertyChanged(); } } }


        public DBIngresosClass listNuevoIngreso { get; } = new DBIngresosClass() { Usuario = Variables.UsuarioLogueado };
        public DBCajaClass ingresoCaja { get; } = new DBCajaClass();
        public int intAgregadosCount => _listProductosAgregados.Count();
        public int intTotal => listNuevoIngreso.IngresoProductosPerIngreso.Count();
        public string windowBackground => indexProveedor >= 0 && listNuevoIngreso.IngresoProductosPerIngreso.Count > 0 && !listNuevoIngreso.IngresoProductosPerIngreso.Any(x => x.Cantidad < 1) ? Variables.colorWindowBackgroundOK : Variables.colorWindowBackkgroundNO;


        readonly CollectionViewSource listProveedoresSource = new CollectionViewSource() { Source = Variables.Inventario.Proveedores.Local.ToObservableCollection() };
        public ICollectionView listProveedores => listProveedoresSource.View;

        public IEnumerable<DBRetirosCaja> listRetirosPerProveedor => listNuevoIngreso.Proveedor?.RetirosCajaPerProveedor.Where(x => x.Pendiente);
        #endregion // Variables



        #region Helpers
        void helperAgregarQuitarProducto(object sentParameter)
        {
            if (sentParameter is DBProductosClass tempProducto && tempProducto != null)
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
                        vHelperPrecioPercioCantidad = new VentanaHelperPrecioPrecioCantidad(1, tempProducto.PrecioActual, tempProducto.PrecioIngreso);
                        if (vHelperPrecioPercioCantidad.ShowDialog().Value)
                        {
                            listNuevoIngreso.IngresoProductosPerIngreso.Add(new DBIngresoProductosClass()
                            { Cantidad = vHelperPrecioPercioCantidad.resultCantidad, PrecioActual = vHelperPrecioPercioCantidad.resultPrecioActual, Producto = tempProducto, PrecioPagado = vHelperPrecioPercioCantidad.resultPrecioPagado });
                            tempProducto.Agregado = true;
                        }
                    }
                    catch { }
                }
                OnPropertyChanged(nameof(windowBackground));
            }
        }


        void helperAgregarProducto()
        {
            DBProductosClass prd = selectedProduct;
            if (!_listProductosAgregados.Contains(prd))
            {
                prd.Agregado = true;
                _listProductosAgregados.Add(prd);

                listNuevoIngreso.IngresoProductosPerIngreso.Add(new DBIngresoProductosClass() { Cantidad = 0, PrecioActual = prd.PrecioActual, Producto = prd });
                OnPropertyChanged(nameof(intAgregadosCount)); OnPropertyChanged(nameof(intTotal));
            }
        }

        void helperQuitarProducto(DBProductosClass prd)
        {
            try
            {
                if (_listProductosAgregados.Contains(prd))
                {
                    _ = listNuevoIngreso.IngresoProductosPerIngreso.Remove(listNuevoIngreso.IngresoProductosPerIngreso.Single(x => x.Producto == prd));
                    _ = _listProductosAgregados.Remove(prd); prd.Agregado = false;
                    OnPropertyChanged(nameof(intAgregadosCount)); OnPropertyChanged(nameof(intTotal));
                }

            }
            catch { }
        }

        void helperQuitarIngreso(DBIngresoProductosClass sentIngresoProducto)
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
            foreach (DBIngresoProductosClass ingreso in listNuevoIngreso.IngresoProductosPerIngreso)
            {
                ingreso.Producto.PrecioActual = ingreso.PrecioActual;
                ingreso.Producto.Stock += ingreso.Cantidad;
            }

            if (selectedRetiroPerProveedor != null) { selectedRetiroPerProveedor.Pendiente = false; }
            if (ingresoCaja.CajaActual > 0 || ingresoCaja.MercadoPago > 0) { Db.contabilizarCaja(ingresoCaja); }

            _ = Variables.Inventario.SaveChanges();
            thisWindow.Close();
        }


        bool checkAgregarProducto => selectedProduct != null && selectedProduct.Agregado == false;
        bool checkQuitarProducto => selectedProduct != null && selectedProduct.Agregado;
        bool checkGuardarIngreso => listNuevoIngreso.Proveedor != null && listNuevoIngreso.IngresoProductosPerIngreso.Count > 0 && !listNuevoIngreso.IngresoProductosPerIngreso.Any(x => x.Cantidad < 1);
        #endregion // Helpers



        #region Commands
        public Command aComSelectorAgregarQuitar => new Command(
            (object parameter) => helperAgregarQuitarProducto(parameter));

        public Command aComUnSoloProducto => new Command(
            (object parameter) => { if (parameter != null) { helperAgregarQuitarProducto(parameter); } },
            (object parameter) => bolIsOnlyOne);

        public Command comNuevoProveedor => new Command((object parameter) => abrirAgregarProveedor());

        public Command comAgregar => new Command(
            (object parameter) => helperAgregarProducto(),
            (object parameter) => checkAgregarProducto);

        public Command comQuitarProducto => new Command(
            (object parameter) => helperQuitarProducto(selectedProduct),
            (object parameter) => checkQuitarProducto);


        public Command comQuitar => new Command(
                    (object parameter) => helperQuitarIngreso(selectedIngreso),
                    (object parameter) => selectedIngreso != null);

        public Command comNuevoProducto => new Command((object parameter) => abrirAgregarProducto());

        public Command comGuardarIngreso => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardarIngreso);

        public Command comCancel => new Command((object parameter) => thisWindow.Close());

        public Command comSelectorAgregarQuitar => new Command(
            (object parameter) => helperAgregarQuitarProducto(parameter),
            (object parameter) => selectedProduct != null);
        #endregion // Commands
    }
}