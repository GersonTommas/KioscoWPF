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
    /// Interaction logic for VentanaAgregarConvertido.xaml
    /// </summary>
    public partial class VentanaAgregarConvertido : Window
    {
        public VentanaAgregarConvertido(DBProductosClass sentProducto) { InitializeComponent(); (DataContext as ViewModels.VMAgregarConvertido).setInitialize(this, sentProducto);  }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarConvertido : helperObservableClass
    {
        #region Initialize
        VentanaAgregarConvertido thisWindow;

        public void setInitialize(VentanaAgregarConvertido tempWindow, DBProductosClass sentProducto)
        {
            thisWindow = tempWindow; selectedAbierto.ProductoSacado = sentProducto;
        }
        #endregion // Initialize

        #region Variables
        public DBAbiertoProductosClass selectedAbierto { get; } = new DBAbiertoProductosClass() { CantidadSacado = 1, CantidadAgregado = 0, Fecha = Db.returnFecha(), Usuario = Variables.UsuarioLogueado };
        #endregion // Variables


        #region Helpers
        void helperSeleccionarProducto()
        {
            VentanaSelectorProductoManual vTemp = new VentanaSelectorProductoManual();
            if (vTemp.ShowDialog().Value)
            {
                selectedAbierto.ProductoAgregado = vTemp.sendProduct;
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

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardar);
        #endregion // Commands
    }
}