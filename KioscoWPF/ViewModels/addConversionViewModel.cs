using System.Windows;

namespace Kiosco.WPF.ViewModels
{
    class addConversionViewModel : Base.ViewModelBase
    {
        #region Initialize
        public override void setInitialize(Window sentWindow)
        {
            sentWindow.Close();
        }
        public void setInitialize(Window sentWindow, productosModel sentProducto)
        {
            selectedAbierto.ProductoSacado = sentProducto;
            thisWindow = sentWindow;
        }
        #endregion // Initialize

        #region Variables
        public abiertoProductosModel selectedAbierto { get; } = new abiertoProductosModel() { CantidadSacado = 1, CantidadAgregado = 0, Fecha = Db.returnFecha(), Usuario = Variables.UsuarioLogueado };
        #endregion // Variables


        #region Helpers
        void helperSeleccionarProducto()
        {
            gHelperSelectorView = new Views.helperSelectorView();
            if (gHelperSelectorView.ShowDialog().Value)
            {
                selectedAbierto.ProductoAgregado = gHelperSelectorView.sendProduct;
            }
        }

        void helperGuardar()
        {
            selectedAbierto.ProductoSacado.Stock -= selectedAbierto.CantidadSacado;
            selectedAbierto.ProductoAgregado.Stock += selectedAbierto.CantidadAgregado;
            _ = Variables.Inventario.AbiertoProductos.Add(selectedAbierto);

            _ = Variables.Inventario.SaveChanges();

            thisWindow.DialogResult = true;
        }

        bool checkGuardar => selectedAbierto.ProductoSacado != null && selectedAbierto.ProductoAgregado != null && selectedAbierto.CantidadAgregado > 0 && selectedAbierto.CantidadSacado > 0;
        #endregion // Helpers

        #region Commands
        public Command comSeleccionarProducto => new Command((object parameter) => helperSeleccionarProducto());

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardar);
        #endregion // Commands
    }
}
