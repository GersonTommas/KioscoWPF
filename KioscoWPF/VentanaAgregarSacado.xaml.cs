using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for VentanaAgregarSacado.xaml
    /// </summary>
    public partial class VentanaAgregarSacado : Window
    {
        public VentanaAgregarSacado(usuariosModel sentUsuario)
        {
            InitializeComponent(); try { (DataContext as ViewModels.VMAgregarSacado).setInitialize(this, sentUsuario); } catch { }
        }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarSacado : helperObservableClass
    {
        #region Initialize
        VentanaAgregarSacado thisWindow;

        public void setInitialize(VentanaAgregarSacado tempWindow, usuariosModel tempUsuario)
        {
            thisWindow = tempWindow; selectedUsuario = tempUsuario;

            Db.resetProductoAgregado();
        }
        #endregion // Initialize



        #region Variables
        readonly fechasModel _today = Db.returnFecha();

        usuariosModel _selectedUsuario;
        public usuariosModel selectedUsuario { get => _selectedUsuario; set { if (_selectedUsuario != value) { _selectedUsuario = value; OnPropertyChanged(); } } }

        sacadoProductosModel _selectedSacado;
        public sacadoProductosModel selectedSacado { get => _selectedSacado; set { if (_selectedSacado != value) { _selectedSacado = value; OnPropertyChanged(); } } }


        public ObservableCollection<sacadoProductosModel> listNuevoSacado { get; } = new ObservableCollection<sacadoProductosModel>();
        public int intAgregadosCount => listNuevoSacado.Count();
        public Double doubleTotal => listNuevoSacado.Sum(x => x.PrecioTotal);
        #endregion // Variables



        #region Helpers
        void helperAgregarQuitarProducto(object sentProducto)
        {
            productosModel tempProducto = sentProducto as productosModel;

            if (tempProducto.Agregado)
            {
                try
                {
                    _ = listNuevoSacado.Remove(listNuevoSacado.Single(x => x.Producto == tempProducto));
                    tempProducto.Agregado = false;
                }
                catch { }
            }
            else
            {
                try
                {
                    vHelperCantidad = new VentanaHelperCantidad();
                    if (vHelperCantidad.ShowDialog().Value)
                    {
                        listNuevoSacado.Add(new sacadoProductosModel()
                        { Cantidad = vHelperCantidad.intCantidad, Precio = tempProducto.PrecioActual, Producto = tempProducto, Usuario = selectedUsuario, FechaSacado = _today});
                        tempProducto.Agregado = true;
                    }
                }
                catch { }
            }
            OnPropertyChanged(nameof(doubleTotal)); OnPropertyChanged(nameof(intAgregadosCount));
        }

        void helperQuitarSacado()
        {
            _ = listNuevoSacado.Remove(selectedSacado);
            OnPropertyChanged(nameof(doubleTotal)); OnPropertyChanged(nameof(intAgregadosCount));
        }

        void helperGuardar()
        {
            Variables.Inventario.SacadoProductos.AddRange(listNuevoSacado);
            foreach (sacadoProductosModel sacado in listNuevoSacado)
            {
                sacado.Producto.Stock -= sacado.Cantidad;
            }

            _ = Variables.Inventario.SaveChanges();
            thisWindow.DialogResult = true;
        }
        #endregion // Helpers



        #region Commands
        public Command aComSelectorAgregarQuitar => new Command(
            (object parameter) => helperAgregarQuitarProducto(parameter));

        public Command aComUnSoloProducto => new Command(
            (object parameter) => { if (parameter != null) { helperAgregarQuitarProducto(parameter); } },
            (object parameter) => bolIsOnlyOne);

        public Command comNuevoProducto => new Command((object parameter) => abrirAgregarProducto());

        public Command comQuitarSacado => new Command(
            (object parameter) => helperQuitarSacado(),
            (object parameter) => selectedSacado != null);

        public Command comGuardarIngreso => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => listNuevoSacado.Count > 0 && !listNuevoSacado.Any(x => x.Cantidad < 1));

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);
        #endregion // Commands
    }
}
