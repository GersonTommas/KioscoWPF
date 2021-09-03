using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    /// Interaction logic for VentanaSelectorProductoManual.xaml
    /// </summary>
    public partial class VentanaSelectorProductoManual : Window
    {
        public DBProductosClass sendProduct;

        public VentanaSelectorProductoManual() { InitializeComponent(); (DataContext as ViewModels.VMSelectorProductoManual).setInitialize(this); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMSelectorProductoManual : helperObservableClass
    {
        #region Initialize
        VentanaSelectorProductoManual thisWindow;

        public void setInitialize(VentanaSelectorProductoManual tempWindow)
        {
            thisWindow = tempWindow;
        }
        #endregion // Initialize



        #region Variables
        #endregion // Variables



        #region Commands
        public Command comAbrirProducto => new Command(
            (object parameter) => { if (parameter != null) { abrirAgregarConvertido(parameter as DBProductosClass); } });

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);

        public Command aComSelectorAgregarQuitar => new Command(
            (object parameter) => { thisWindow.sendProduct = parameter as DBProductosClass; if (thisWindow.sendProduct != null) { thisWindow.DialogResult = true; } });

        public Command aComUnSoloProducto => new Command(
            (object parameter) => { thisWindow.sendProduct = parameter as DBProductosClass; if (thisWindow.sendProduct != null) { thisWindow.DialogResult = true; } },
            (object parameter) => bolIsOnlyOne);
        #endregion // Commands
    }
}