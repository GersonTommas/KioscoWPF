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

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaHelperPrecioPrecioCantidad.xaml
    /// </summary>
    public partial class VentanaHelperPrecioPrecioCantidad : Window
    {
        public int resultCantidad = 0;
        public Double resultPrecioActual = 0;
        public Double resultPrecioPagado = 0;

        public VentanaHelperPrecioPrecioCantidad(int sentCantidad = 0, Double sentPrecioActual = 0, Double sentPrecioPagado = 0)
        {
            InitializeComponent(); (DataContext as ViewModels.VMHelperPrecioPrecioCantidad).setInitialize(this, sentCantidad, sentPrecioActual, sentPrecioPagado);
        }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMHelperPrecioPrecioCantidad : helperObservableClass
    {
        #region Initializew
        VentanaHelperPrecioPrecioCantidad thisWindow;

        public void setInitialize(VentanaHelperPrecioPrecioCantidad tempWindow, int tempCantidad, Double tempPrecioActual, Double tempPrecioPagado)
        {
            thisWindow = tempWindow; intCantidad = tempCantidad; doublePrecioVenta = tempPrecioActual; doublePrecioPagado = tempPrecioPagado;
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
            thisWindow.resultCantidad = intCantidad;
            thisWindow.resultPrecioActual = doublePrecioVenta;
            thisWindow.resultPrecioPagado = doublePrecioPagado;

            thisWindow.DialogResult = true;
        }

        bool checkResultado => intCantidad > 0 && doublePrecioPagado >= 0 && doublePrecioVenta >= 0;
        #endregion // Helpers



        #region Commands
        public Command comResultado => new Command(
            (object parameter) => helperResultado(),
            (object parameter) => checkResultado);

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);
        #endregion // Commands
    }
}