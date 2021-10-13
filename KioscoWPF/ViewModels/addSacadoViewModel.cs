using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Kiosco.WPF.ViewModels
{
    class addSacadoViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, usuariosModel sentUsuario)
        {
            base.setInitialize(sentWindow);

            selectedUsuario = sentUsuario;

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
                    vHelperCantidad = new Views.helperCantidadView();
                    if (vHelperCantidad.ShowDialog().Value)
                    {
                        listNuevoSacado.Add(new sacadoProductosModel()
                        { Cantidad = vHelperCantidad.intCantidad, Precio = tempProducto.PrecioActual, Producto = tempProducto, Usuario = selectedUsuario, FechaSacado = _today });
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
        public override void aCommandAgregarQuitarProducto(object sentParameter = null)
        {
            helperAgregarQuitarProducto(sentParameter);
        }
        public override void aCommandUnSoloProducto(object sentParameter = null)
        {
            if (sentParameter != null) { helperAgregarQuitarProducto(sentParameter); }
        }

        public Command comNuevoProducto => new Command((object parameter) => gOpenAddProducto());

        public Command comQuitarSacado => new Command(
            (object parameter) => helperQuitarSacado(),
            (object parameter) => selectedSacado != null);

        public Command comGuardarIngreso => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => listNuevoSacado.Count > 0 && !listNuevoSacado.Any(x => x.Cantidad < 1));
        #endregion // Commands
    }
}
