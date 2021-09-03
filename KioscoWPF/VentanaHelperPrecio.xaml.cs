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
    /// Interaction logic for VentanaHelperPrecio.xaml
    /// </summary>
    public partial class VentanaHelperPrecio : Window
    {
        public Double intPrecio;

        public VentanaHelperPrecio() { InitializeComponent(); (DataContext as ViewModels.VMHelperPrecio).setInitialize(this); }
        public VentanaHelperPrecio(int sentPrecio)
        {
            InitializeComponent(); (DataContext as ViewModels.VMHelperPrecio).setInitialize(this, sentPrecio);
        }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMHelperPrecio : helperObservableClass
    {
        #region Initialize
        VentanaHelperPrecio thisWindow;

        public void setInitialize(VentanaHelperPrecio tempWindow)
        {
            thisWindow = tempWindow;
        }
        public void setInitialize(VentanaHelperPrecio tempWindow, int tempPrecio)
        {
            thisWindow = tempWindow; intPrecio = tempPrecio;
        }
        #endregion // Initialize



        #region Variables
        Double _intPrecio = 0;
        public Double intPrecio { get => _intPrecio; set { if (_intPrecio != value) { _intPrecio = value; OnPropertyChanged(); OnPropertyChanged(nameof(windowBackground)); } } }

        public string windowBackground => intPrecio >= 0 ? Variables.colorWindowBackgroundOK : Variables.colorWindowBackkgroundNO;
        #endregion // Variables



        #region Commands
        public Command comResultado => new Command(
            (object parameter) => { thisWindow.intPrecio = Math.Round(intPrecio, 2); thisWindow.DialogResult = true; },
            (object parameter) => intPrecio >= 0);

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);
        #endregion // Commands
    }
}
