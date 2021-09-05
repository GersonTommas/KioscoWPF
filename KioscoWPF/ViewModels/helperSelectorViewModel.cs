using System.Windows;

namespace KioscoWPF.ViewModels
{
    class helperSelectorViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window tempWindow, bool tempIsAbrirProducto = false)
        {
            thisWindow = tempWindow; isAbrirProducto = !tempIsAbrirProducto; OnPropertyChanged(nameof(isAbrirProducto));
        }
        #endregion // Initialize



        #region Varibles
        public bool isAbrirProducto { get; set; } = true;
        #endregion // Variables



        #region Helpers
        void sendProductAndClose(object sentParameter)
        {
            (thisWindow as Views.helperSelectorView).sendProduct = sentParameter as productosModel;
            if ((thisWindow as Views.helperSelectorView).sendProduct != null) { thisWindow.DialogResult = true; }
        }
        #endregion // Helpers



        #region Commands
        public override void aCommandAgregarQuitarProducto(object sentParameter) { sendProductAndClose(sentParameter); }
        public override void aCommandUnSoloProducto(object sentParameter) { sendProductAndClose(sentParameter); }

        public Command comAbrirProducto => new Command(
            (object parameter) => { if (parameter != null) { _ = gOpenAddConversion(parameter as productosModel); } },
            (object parameter) => isAbrirProducto);
        #endregion // Commands
    }
}
