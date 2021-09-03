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
        public VentanaAgregarSacado(DBUsuariosClass sentUsuario)
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

        public void setInitialize(VentanaAgregarSacado tempWindow, DBUsuariosClass tempUsuario)
        {
            thisWindow = tempWindow; selectedUsuario = tempUsuario;

            Db.resetProductoAgregado();
        }
        #endregion // Initialize



        #region Variables
        readonly DBFechasClass _today = Db.returnFecha();

        DBUsuariosClass _selectedUsuario;
        public DBUsuariosClass selectedUsuario { get => _selectedUsuario; set { if (_selectedUsuario != value) { _selectedUsuario = value; OnPropertyChanged(); } } }

        DBSacadoProductosClass _selectedSacado;
        public DBSacadoProductosClass selectedSacado { get => _selectedSacado; set { if (_selectedSacado != value) { _selectedSacado = value; OnPropertyChanged(); } } }


        public ObservableCollection<DBSacadoProductosClass> listNuevoSacado { get; } = new ObservableCollection<DBSacadoProductosClass>();
        public int intAgregadosCount => listNuevoSacado.Count();
        public Double doubleTotal => listNuevoSacado.Sum(x => x.PrecioTotal);
        #endregion // Variables



        #region Helpers
        void helperAgregarQuitarProducto(object sentProducto)
        {
            DBProductosClass tempProducto = sentProducto as DBProductosClass;

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
                        listNuevoSacado.Add(new DBSacadoProductosClass()
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
            foreach (DBSacadoProductosClass sacado in listNuevoSacado)
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
