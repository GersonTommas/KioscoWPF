using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for VentanaDetallesProductos.xaml
    /// </summary>
    public partial class VentanaDetallesProductos : Window
    {
        public VentanaDetallesProductos(productosModel sentProducto)
        {
            InitializeComponent(); try { (DataContext as ViewModels.VMDetallesProductos).setInitialize(this, sentProducto); } catch { }
        }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMDetallesProductos : helperObservableClass
    {
        #region Initialize
        VentanaDetallesProductos thisWindow;

        public void setInitialize(VentanaDetallesProductos tempWindow, productosModel sentProducto)
        {
            thisWindow = tempWindow; _selectedProducto = sentProducto;
            OnPropertyChanged(nameof(selectedProducto));
        }
        #endregion // Initialize

        #region Variables
        productosModel _selectedProducto;
        public productosModel selectedProducto => _selectedProducto;
        #endregion // Variables

        #region Lists
        #endregion // Lists

        #region Commands
        public Command comCancelar => new Command((object parameter) => thisWindow.Close());
        #endregion // Commands
    }
}