using System;
using System.Windows;

namespace Kiosco.WPF.ViewModels
{
    class helperIngresoPreciosViewModel : Base.ViewModelBase
    {
        #region Initializew
        public void setInitialize(Window sentWindow, int sentCantidad, Double sentPrecioActual, Double sentPrecioPagado)
        {
            base.setInitialize(sentWindow);
            intCantidad = sentCantidad; doublePrecioVenta = sentPrecioActual; doublePrecioPagado = sentPrecioPagado;
        }
        #endregion // Initialize



        #region Variables
        int _intCantidad = 0;
        public int intCantidad { get => _intCantidad; set { if (_intCantidad != value) { _intCantidad = value; OnPropertyChanged(); OnPropertyChanged(nameof(doubleTotalPagado)); } } }

        Double _doublePrecioVenta = 0;
        public Double doublePrecioVenta { get => _doublePrecioVenta; set { if (_doublePrecioVenta != value) { _doublePrecioVenta = value; OnPropertyChanged(); } } }

        Double _doublePrecioPagado = 0;
        public Double doublePrecioPagado { get => _doublePrecioPagado; set { if (_doublePrecioPagado != value) { _doublePrecioPagado = value; OnPropertyChanged(); OnPropertyChanged(nameof(doubleTotalPagado)); OnPropertyChanged(nameof(doublePrecioSugerido)); } } }


        public Double doubleTotalPagado => Math.Round(doublePrecioPagado * intCantidad, 2);
        public Double doublePrecioSugerido => Math.Round(doublePrecioPagado * 1.3, 2);
        #endregion // Variables



        #region Helpers
        void helperResultado()
        {
            (thisWindow as Views.helperIngresoPreciosView).resultCantidad = intCantidad;
            (thisWindow as Views.helperIngresoPreciosView).resultPrecioActual = doublePrecioVenta;
            (thisWindow as Views.helperIngresoPreciosView).resultPrecioPagado = doublePrecioPagado;

            thisWindow.DialogResult = true;
        }

        bool checkResultado => intCantidad > 0 && doublePrecioPagado >= 0 && doublePrecioVenta >= 0;
        #endregion // Helpers



        #region Commands
        public Command comResultado => new Command(
            (object parameter) => helperResultado(),
            (object parameter) => checkResultado);
        #endregion // Commands
    }
}
