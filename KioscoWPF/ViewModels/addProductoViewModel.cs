using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Kiosco.WPF.ViewModels
{
    class addProductoViewModel : Base.ViewModelBase
    {
        #region Initialize
        public addProductoViewModel()
        {
            listTags.SortDescriptions.Clear(); listTags.SortDescriptions.Add(new SortDescription("Tag", ListSortDirection.Ascending));
            listMedidas.SortDescriptions.Clear(); listMedidas.SortDescriptions.Add(new SortDescription("Medida", ListSortDirection.Ascending));
        }
        public void setInitialize(Window sentWindow, string sentCode)
        {
            base.setInitialize(sentWindow);
            if (!string.IsNullOrWhiteSpace(sentCode))
            {
                bolForceNewCode = true;
                productSelected.Codigo = sentCode;
            }
        }
        public void setInitialize(Window sentWindow, productosModel sentProducto)
        {
            base.setInitialize(sentWindow);

            if (sentProducto != null)
            {
                _productToEdit = sentProducto;

                productSelected = new productosModel() { Activo = sentProducto.Activo, Codigo = sentProducto.Codigo, Descripcion = sentProducto.Descripcion, Medida = sentProducto.Medida, MedidaID = sentProducto.MedidaID, PrecioActual = sentProducto.PrecioActual, PrecioIngreso = sentProducto.PrecioIngreso, Tag = sentProducto.Tag, TagID = sentProducto.TagID };

                bolEdit = true;
            }
        }
        #endregion // Initialize



        #region Variables
        productosModel _productToEdit;


        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (SetProperty(ref _bolEdit, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(strWindowTitle)); OnPropertyChanged(nameof(strGroupTitle)); } } }

        bool _forceNewCode = false;
        public bool bolForceNewCode { get => _forceNewCode; set { if (SetProperty(ref _forceNewCode, value)) { OnPropertyChanged(); } } }

        int _intProductosCreados = 0;
        public int intProductosCreados { get => _intProductosCreados; set { if (SetProperty(ref _intProductosCreados, value)) { OnPropertyChanged(); } } }

        int _intStockInicial = 0;
        public int intStockInicial { get => _intStockInicial; set { if (SetProperty(ref _intStockInicial, value)) { OnPropertyChanged(); } } }

        productosModel _productSelected = new productosModel() { Activo = true };
        public productosModel productSelected { get => _productSelected; set { if (SetProperty(ref _productSelected, value)) { OnPropertyChanged(); } } }


        public bool bolMantenerAbierto { get; set; }


        public string strWindowTitle => bolEdit ? "Editar Producto" : "Nuevo Producto";
        public string strGroupTitle => bolEdit ? "Id: " + _productToEdit.Id : "Nuevo Producto";
        public Visibility visMantenerAbierto => _bolEdit || _forceNewCode ? Visibility.Collapsed : Visibility.Visible;


        readonly CollectionViewSource _listTagsSource = new CollectionViewSource { Source = Variables.Inventario.Tags.Local.ToObservableCollection() };
        public ICollectionView listTags => _listTagsSource.View;

        readonly CollectionViewSource _listMedidasSource = new CollectionViewSource { Source = Variables.Inventario.Medidas.Local.ToObservableCollection() };
        public ICollectionView listMedidas => _listMedidasSource.View;
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

        bool checkGuardar()
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

        bool checkCodeDuplicate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(productSelected.Codigo))
                {
                    try { productSelected.Codigo = productSelected.Codigo.Trim(); } catch { }
                    productosModel compareProductCodigo = Variables.Inventario.Productos.Single(x => x.Codigo.ToLower() == productSelected.Codigo.ToLower());
                    return !bolEdit || compareProductCodigo.Id != _productToEdit.Id;
                }
                else { return false; }
            }
            catch { return false; }
        }
        #endregion // Helpers



        #region Commands
        public Command comNuevoTag => new Command((object parameter) => gOpenAddTag());
        public Command comNuevaMedida => new Command((object parameter) => gOpenAddMedida());

        public Command comCodigoExiste => new Command((object parameter) => { if (checkCodeDuplicate()) { Variables.messageError.CodigoExiste(); } else { Variables.nextTarget(parameter); } });

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardar());
        #endregion // Commands
    }
}
