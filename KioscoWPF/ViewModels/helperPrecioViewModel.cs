using System;
using System.Windows;

namespace KioscoWPF.ViewModels
{
    class helperPrecioViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, Double sentPrecio)
        {
            base.setInitialize(sentWindow);
            thisWindow = sentWindow; intPrecio = sentPrecio;
        }
        #endregion // Initialize



        #region Variables
        Double _intPrecio = 0;
        public Double intPrecio { get => _intPrecio; set { if (SetProperty(ref _intPrecio, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(windowBackground)); } } }

        public string windowBackground => intPrecio >= 0 ? Variables.colorWindowBackgroundOK : Variables.colorWindowBackkgroundNO;
        #endregion // Variables



        #region Commands
        public Command comResultado => new Command(
            (object parameter) => { (thisWindow as Views.helperPrecioView).resultPrecio = Math.Round(intPrecio, 2); thisWindow.DialogResult = true; },
            (object parameter) => intPrecio >= 0);
        #endregion // Commands
    }
}
