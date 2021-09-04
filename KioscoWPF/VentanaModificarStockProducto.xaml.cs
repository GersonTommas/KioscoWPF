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
    /// Interaction logic for VentanaModificarStockProducto.xaml
    /// </summary>
    public partial class VentanaModificarStockProducto : Window
    {
        public VentanaModificarStockProducto(productosModel sentProducto)
        {
            InitializeComponent(); try { (DataContext as ViewModels.VMModificarStockProducto).setInitialize(this, sentProducto); } catch { }
        }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMModificarStockProducto : helperObservableClass
    {
        #region Initialize
        VentanaModificarStockProducto thisWindow;

        public void setInitialize(VentanaModificarStockProducto tempWindow, productosModel tempProducto)
        {
            thisWindow = tempWindow;
            selectedStockProducto.Producto = tempProducto;

            _listUsuariosSource.Source = Variables.Inventario.Usuarios.Local.ToObservableCollection(); OnPropertyChanged(nameof(listUsuarios));
            listUsuarios.SortDescriptions.Clear(); listUsuarios.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));

            selectedStockProducto.updateItem();
        }
        #endregion // Initialize



        #region Variables
        public modificadoProductoModel selectedStockProducto { get; } = new modificadoProductoModel() { Fecha = Db.returnFecha() };

        readonly CollectionViewSource _listUsuariosSource = new CollectionViewSource();
        public ICollectionView listUsuarios => _listUsuariosSource.View;
        #endregion // Variables



        #region Helpers
        void helperGuardar(object sender)
        {
            if (sender is PasswordBox passBox)
            {
                if (passBox.Password != null)
                {
                    string pass = passBox.Password;
                    if (selectedStockProducto.Usuario.Contraseña == pass)
                    {
                        _ = Variables.Inventario.ModificadoProductos.Add(selectedStockProducto);
                        selectedStockProducto.Producto.Stock += selectedStockProducto.Cantidad;
                        _ = Variables.Inventario.SaveChanges();
                        thisWindow.DialogResult = true;
                    }
                    else { Variables.messageError.LogIn(); }
                }
            }
        }

        bool checkGuardar => selectedStockProducto.Cantidad != 0 && selectedStockProducto.Producto != null && selectedStockProducto.Usuario != null;
        #endregion // Helpers



        #region Commands
        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(parameter),
            (object parameter) => checkGuardar);

        public Command comCancelar => new Command((object parameter) => thisWindow.Close());
        #endregion // Commands
    }
}