using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KioscoWPF.ViewModels
{
    class helperModificarStockViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, productosModel sentProducto)
        {
            base.setInitialize(sentWindow);
            selectedStockProducto.Producto = sentProducto;

            _listUsuariosSource.Source = Variables.Inventario.Usuarios.Local.ToObservableCollection(); OnPropertyChanged(nameof(listUsuarios));
            listUsuarios.SortDescriptions.Clear(); listUsuarios.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));

            selectedStockProducto.updateModel();
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
                        int tempVal = selectedStockProducto.Producto.Stock;
                        selectedStockProducto.Producto.Stock = tempVal + selectedStockProducto.Cantidad;
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
        #endregion // Commands
    }
}
