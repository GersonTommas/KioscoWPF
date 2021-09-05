using System.Linq;
using System.Windows.Controls;

namespace KioscoWPF.ViewModels
{
    class addConsumoViewModel : Base.ViewModelBase
    {
        #region Variables
        string _strCodigoFailed;
        int _intCodigoFailedCount = 0;


        string _strCodigo;
        public string strCodigo { get => _strCodigo; set { if (SetProperty(ref _strCodigo, value)) { OnPropertyChanged(); if (selectedConsumo.Producto != null) { selectedConsumo.Producto = null; selectedConsumo.Precio = 0; } } } }


        public consumoProductoModel selectedConsumo { get; } = new consumoProductoModel() { Fecha = Db.returnFecha(), Cantidad = 1 };
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
                            if (gOpenAddProducto(strCodigo))
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
        public Command comIngresoManual => new Command(
                    (object parameter) => { gHelperSelectorView = new Views.helperSelectorView(); if (gHelperSelectorView.ShowDialog().Value) { strCodigo = gHelperSelectorView.sendProduct.Codigo; helperFindProducto(); } });

        public Command comProducto => new Command(
            (object parameter) => helperFindProducto(parameter),
            (object parameter) => !string.IsNullOrWhiteSpace(strCodigo));

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardar);
        #endregion // Commands
    }
}
