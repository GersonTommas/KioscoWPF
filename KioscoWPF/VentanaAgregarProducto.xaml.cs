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
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaAgregarProducto.xaml
    /// </summary>
    public partial class VentanaAgregarProducto : Window
    {
        public VentanaAgregarProducto(productosModel sentProduct) { InitializeComponent(); (DataContext as ViewModels.VMAgregarProducto).setInitialize(this, sentProduct); }
        public VentanaAgregarProducto(string sentCodigo) { InitializeComponent(); (DataContext as ViewModels.VMAgregarProducto).setInitialize(this, sentCodigo); }
        public VentanaAgregarProducto() { InitializeComponent(); (DataContext as ViewModels.VMAgregarProducto).setInitialize(this); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarProducto : helperObservableClass
    {
        #region Initialize
        VentanaAgregarProducto thisWindow;

        public void setInitialize(VentanaAgregarProducto tempWindow)
        {
            thisWindow = tempWindow;
        }
        public void setInitialize(VentanaAgregarProducto tempWindow, string tempCodigo)
        {
            thisWindow = tempWindow;
            if (!string.IsNullOrWhiteSpace(tempCodigo))
            {
                bolForceNewCode = true;
                productSelected.Codigo = tempCodigo;
            }
        }
        public void setInitialize(VentanaAgregarProducto tempWindow, productosModel tempProducto)
        {
            thisWindow = tempWindow;

            _productToEdit = tempProducto;

            productSelected = new productosModel() { Activo = tempProducto.Activo, Codigo = tempProducto.Codigo, Descripcion = tempProducto.Descripcion, Medida = tempProducto.Medida, MedidaID = tempProducto.MedidaID, PrecioActual = tempProducto.PrecioActual, PrecioIngreso = tempProducto.PrecioIngreso, Tag = tempProducto.Tag, TagID = tempProducto.TagID };

            bolEdit = true;
        }
        #endregion Initialize



        #region Variables
        productosModel _productToEdit;


        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (_bolEdit != value) { _bolEdit = value; OnPropertyChanged(); OnPropertyChanged(nameof(strWindowTitle)); OnPropertyChanged(nameof(strGroupTitle)); } } }

        bool _forceNewCode = false;
        public bool bolForceNewCode { get => _forceNewCode; set { if (_forceNewCode != value) { _forceNewCode = value; OnPropertyChanged(); } } }

        int _intProductosCreados = 0;
        public int intProductosCreados { get => _intProductosCreados; set { if (_intProductosCreados != value) { _intProductosCreados = value; OnPropertyChanged(); } } }

        int _intStockInicial = 0;
        public int intStockInicial { get => _intStockInicial; set { if (_intStockInicial != value) { _intStockInicial = value; OnPropertyChanged(); } } }

        productosModel _productSelected = new productosModel() { Activo = true };
        public productosModel productSelected { get => _productSelected; set { if (_productSelected != value) { _productSelected = value; OnPropertyChanged(); } } }


        public bool bolMantenerAbierto { get; set; }


        public string strWindowTitle => bolEdit ? "Editar Producto" : "Nuevo Producto";
        public string strGroupTitle => bolEdit ? "Id: " + _productToEdit.Id : "Nuevo Producto";
        public Visibility visMantenerAbierto => _bolEdit || _forceNewCode ? Visibility.Collapsed : Visibility.Visible;


        readonly CollectionViewSource _listTagsSource = new CollectionViewSource { Source = Variables.Inventario.Tags.Local.ToObservableCollection() };
        public ICollectionView listTags
        {
            get
            {
                ICollectionView temp = _listTagsSource.View;
                temp.SortDescriptions.Clear(); temp.SortDescriptions.Add(new SortDescription("Tag", ListSortDirection.Ascending));
                return temp;
            }
        }

        readonly CollectionViewSource _listMedidasSource = new CollectionViewSource { Source = Variables.Inventario.Medidas.Local.ToObservableCollection() };
        public ICollectionView listMedidas
        {
            get
            {
                ICollectionView temp = _listMedidasSource.View;
                temp.SortDescriptions.Clear(); temp.SortDescriptions.Add(new SortDescription("Medida", ListSortDirection.Ascending));
                return temp;
            }
        }
        #endregion // Variables



        #region Helpers
        void helperGuardar()
        {
            productosModel compareProductCodigo = null;
            productosModel compareProductDescripcion = null;

            if (!string.IsNullOrWhiteSpace(productSelected.Codigo)) { productSelected.Codigo = productSelected.Codigo.Trim(); }
            if (!string.IsNullOrWhiteSpace(productSelected.Descripcion)) { productSelected.Descripcion = productSelected.Descripcion.Trim(); }

            try { compareProductCodigo = Variables.Inventario.Productos.Single(x => x.Codigo.ToLower() == productSelected.Codigo.ToLower()); } catch { }
            try { compareProductDescripcion = Variables.Inventario.Productos.Single(x => x.Descripcion.ToLower() == productSelected.Descripcion.ToLower()); } catch { }

            if (bolEdit)
            {
                if (compareProductCodigo == null || compareProductDescripcion == null || compareProductCodigo.Id == _productToEdit.Id || compareProductDescripcion.Id == _productToEdit.Id)
                {
                    _productToEdit.Activo = productSelected.Activo;
                    _productToEdit.Codigo = productSelected.Codigo;
                    _productToEdit.Descripcion = productSelected.Descripcion;
                    _productToEdit.FechaModificado = productSelected.FechaModificado;
                    _productToEdit.PrecioActual = productSelected.PrecioActual;
                    _productToEdit.TagID = productSelected.Tag.Id;
                    _productToEdit.Tag = productSelected.Tag;
                    _productToEdit.MedidaID = productSelected.Medida.Id;
                    _productToEdit.Medida = productSelected.Medida;

                    _ = Variables.Inventario.SaveChanges();
                    Variables.messageError.Guardado();

                    thisWindow.DialogResult = true;
                }
                else { Variables.messageError.Existencia(); }
            }
            else
            {
                if (compareProductCodigo != null || compareProductDescripcion != null) { Variables.messageError.Existencia(); }
                else
                {
                    intProductosCreados++;
                    productSelected.Stock = intStockInicial; productSelected.StockInicial = intStockInicial;
                    productSelected.TagID = productSelected.Tag.Id; productSelected.Tag = null;
                    productSelected.MedidaID = productSelected.Medida.Id; productSelected.Medida = null;

                    _ = Variables.Inventario.Productos.Add(productSelected);
                    _ = Variables.Inventario.SaveChanges();
                    if (!bolMantenerAbierto) { thisWindow.DialogResult = true; }

                    productSelected = new productosModel() { Activo = true, Stock = 0, StockInicial = 0 };
                    intStockInicial = 0;
                    compareProductCodigo = null;
                    compareProductDescripcion = null;
                }
            }
        }

        bool CheckGuardar()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(productSelected.Codigo) && !string.IsNullOrWhiteSpace(productSelected.Descripcion) && productSelected.Tag != null && productSelected.Medida != null)
                {
                    if (productSelected.Codigo.Length > 0 && productSelected.Descripcion.Length > 0 && productSelected.Medida.Id > 0 && productSelected.PrecioActual > 0)
                    { return true; }
                }
            }
            catch { }
            return false;
        }

        bool helperCheckCodeDuplicate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(productSelected.Codigo))
                {
                    try { productSelected.Codigo = productSelected.Codigo.Trim(); } catch { }
                    productosModel compareProductCodigo = Variables.Inventario.Productos.Single(x => x.Codigo.ToLower() == productSelected.Codigo.ToLower());
                    return bolEdit && compareProductCodigo.Id == _productToEdit.Id ? false : true;
                }
                else { return false; }
            }
            catch { return false; }
        }
        #endregion // Helpers



        #region Commands
        public Command comNextTextBox => new Command((object parameter) => Variables.nextTarget(parameter));

        public Command comNuevoTag => new Command((object parameter) => abrirAgregarTag());
        public Command comNuevaUnidad => new Command((object parameter) => abrirAgregarMedidas());

        public Command comCodigoExiste => new Command((object parameter) => { if (helperCheckCodeDuplicate()) { Variables.messageError.CodigoExiste(); } else { Variables.nextTarget(parameter); } });

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => CheckGuardar());

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = true);
        #endregion // Commands
    }
}
