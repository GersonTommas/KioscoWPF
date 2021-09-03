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
    /// Interaction logic for VentanaHelperCantidad.xaml
    /// </summary>
    public partial class VentanaHelperCantidad : Window
    {
        public int intCantidad = 1;

        public VentanaHelperCantidad()
        {
            InitializeComponent(); (DataContext as ViewModels.VMHelperCantidad).setInitialize(this);
        }



        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) { TextBox tb = (TextBox)sender; tb.SelectAll(); }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e) { TextBox tb = (TextBox)sender; tb.SelectAll(); }
        private void TextBoxNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e) { Variables.regexNumbers(sender, e); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMHelperCantidad : helperObservableClass
    {
        #region Initialize
        VentanaHelperCantidad thisWindow;

        public void setInitialize(VentanaHelperCantidad tempWindow) { thisWindow = tempWindow; }
        #endregion // Initialize

        #region Variables
        int _intCantidad = 0;
        public int intCantidad { get => _intCantidad; set { if (_intCantidad != value) { _intCantidad = value; OnPropertyChanged(); } } }
        #endregion // Variables

        #region Commands
        public Command comResultado => new Command(
            (object parameter) => { thisWindow.intCantidad = intCantidad; thisWindow.DialogResult = true; },
            (object parameter) => intCantidad > 0);

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);
        #endregion // Commands
    }
}