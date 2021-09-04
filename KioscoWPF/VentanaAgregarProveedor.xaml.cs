using System;
using System.Collections.Generic;
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
    /// Interaction logic for VentanaAgregarProveedor.xaml
    /// </summary>
    public partial class VentanaAgregarProveedor : Window
    {
        public VentanaAgregarProveedor() { InitializeComponent(); (DataContext as ViewModels.VMAgregarProveedor).setInitialize(this); }
        public VentanaAgregarProveedor(proveedoresModel sentProveedor) { InitializeComponent(); (DataContext as ViewModels.VMAgregarProveedor).setInitialize(this, sentProveedor); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarProveedor : helperObservableClass
    {
        #region Initialize
        VentanaAgregarProveedor thisWindow;

        public void setInitialize(VentanaAgregarProveedor tempWindow)
        {
            thisWindow = tempWindow;
        }
        public void setInitialize(VentanaAgregarProveedor tempWindow, proveedoresModel tempProveedor)
        {
            thisWindow = tempWindow;
            if (tempProveedor != null)
            {
                _ToEditProveedor = tempProveedor;
                selectedProveedor = new proveedoresModel() { Celular = tempProveedor.Celular, Detalles = tempProveedor.Detalles, Direccion = tempProveedor.Direccion, Nombre = tempProveedor.Nombre, Telefono = tempProveedor.Telefono };
                bolEdit = true;
            }
        }
        #endregion // Initialize


        #region Variables
        proveedoresModel _ToEditProveedor;


        proveedoresModel _selectedProveedor = new proveedoresModel() { };
        public proveedoresModel selectedProveedor { get => _selectedProveedor; set { if (_selectedProveedor != value) { _selectedProveedor = value; OnPropertyChanged(); } } }

        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (_bolEdit != value) { _bolEdit = value; OnPropertyChanged(); OnPropertyChanged(nameof(strBorderTitle)); OnPropertyChanged(nameof(strWindowTitle)); } } }

        
        public string strBorderTitle => bolEdit ? "ID: " + _ToEditProveedor.Id : "Nuevo Proveedor";
        public string strWindowTitle => bolEdit ? "Editar Proveedor" : "Nuevo Proveedor";
        #endregion // Variables


        #region Public
        #endregion // Public


        #region Helper
        void helperGuardar()
        {
            proveedoresModel tempProveedor = null;
            if (selectedProveedor.Detalles != null) { selectedProveedor.Detalles = selectedProveedor.Detalles.Trim(); }
            if (selectedProveedor.Direccion != null) { selectedProveedor.Direccion = selectedProveedor.Direccion.Trim(); }
            if (selectedProveedor.Nombre != null) { selectedProveedor.Nombre = selectedProveedor.Nombre.Trim(); }
            if (selectedProveedor.NumeroDeCliente != null) { selectedProveedor.NumeroDeCliente = selectedProveedor.NumeroDeCliente.Trim(); }

            try { tempProveedor = Variables.Inventario.Proveedores.Local.Single(x => x.Nombre.ToLower() == selectedProveedor.Nombre.ToLower()); } catch { }

            if (bolEdit)
            {
                if (tempProveedor == null || tempProveedor.Id == _ToEditProveedor.Id)
                {
                    _ToEditProveedor.Celular = selectedProveedor.Celular;
                    _ToEditProveedor.Detalles = selectedProveedor.Detalles;
                    _ToEditProveedor.Direccion = selectedProveedor.Direccion;
                    _ToEditProveedor.Nombre = selectedProveedor.Nombre;
                    _ToEditProveedor.NumeroDeCliente = selectedProveedor.NumeroDeCliente;
                    _ToEditProveedor.Telefono = selectedProveedor.Telefono;

                    _ = Variables.Inventario.SaveChanges();
                    thisWindow.Close();
                }
                else
                {
                    Variables.messageError.Existencia();
                }
            }
            else
            {
                if (tempProveedor == null)
                {
                    _ = Variables.Inventario.Proveedores.Add(selectedProveedor);
                    _ = Variables.Inventario.SaveChanges();
                    thisWindow.Close();
                }
                else
                {
                    Variables.messageError.Existencia();
                }
            }
        }

        bool checkGuardar => !string.IsNullOrWhiteSpace(selectedProveedor.Nombre);
        #endregion // Helper


        #region Commands
        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);
        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardar);
        #endregion // Command
    }
}
