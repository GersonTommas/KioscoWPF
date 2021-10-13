using System.Linq;
using System.Windows;

namespace Kiosco.WPF.ViewModels
{
    class addProveedorViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, proveedoresModel sentProveedor)
        {
            base.setInitialize(sentWindow);

            if (sentProveedor != null)
            {
                _ToEditProveedor = sentProveedor;
                selectedProveedor = new proveedoresModel() { Celular = sentProveedor.Celular, Detalles = sentProveedor.Detalles, Direccion = sentProveedor.Direccion, Nombre = sentProveedor.Nombre, Telefono = sentProveedor.Telefono };
                bolEdit = true;
            }
        }
        #endregion // Initialize



        #region Variables
        proveedoresModel _ToEditProveedor;


        proveedoresModel _selectedProveedor = new proveedoresModel() { };
        public proveedoresModel selectedProveedor { get => _selectedProveedor; set { if (SetProperty(ref _selectedProveedor, value)) { OnPropertyChanged(); } } }

        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (SetProperty(ref _bolEdit, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(strBorderTitle)); OnPropertyChanged(nameof(strWindowTitle)); } } }


        public string strBorderTitle => bolEdit ? "ID: " + _ToEditProveedor.Id : "Nuevo Proveedor";
        public string strWindowTitle => bolEdit ? "Editar Proveedor" : "Nuevo Proveedor";
        #endregion // Variables



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
        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardar);
        #endregion // Command
    }
}
