using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Kiosco.WPF.ViewModels
{
    class addDeudorViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, deudoresModel sentDeudor)
        {
            base.setInitialize(sentWindow);

            if (sentDeudor != null)
            {
                _toEditDeudor = sentDeudor;
                _selectedDeudor = new deudoresModel() { Id = sentDeudor.Id, Detalles = sentDeudor.Detalles, Nivel = sentDeudor.Nivel, Nombre = sentDeudor.Nombre, Resto = sentDeudor.Resto, UsuarioID = sentDeudor.UsuarioID, Usuario = sentDeudor.Usuario };
                OnPropertyChanged(nameof(selectedDeudor)); OnPropertyChanged(nameof(strWindowTitle)); OnPropertyChanged(nameof(strGroupTitle));
            }
        }
        #endregion // Initialize


        #region Variables
        deudoresModel _toEditDeudor = null;


        deudoresModel _selectedDeudor = new deudoresModel() { Nivel = 1, Resto = 0, Usuario = Variables.UsuarioLogueado };
        public deudoresModel selectedDeudor => _selectedDeudor;


        public string strWindowTitle => _toEditDeudor != null ? "Editar Cliente" : "Nuevo Cliente";
        public string strGroupTitle => _toEditDeudor != null ? "ID: " + _toEditDeudor.Id : "Nuevo Cliente";
        public List<int> listNiveles => new List<int>() { 1, 2, 3, 4, 5 };
        #endregion // Read Only



        #region Helpers
        void helperGuardarDeudor()
        {
            deudoresModel checkDeudor = null;

            if (!string.IsNullOrWhiteSpace(selectedDeudor.Detalles)) { selectedDeudor.Detalles = selectedDeudor.Detalles.Trim(); }
            try { selectedDeudor.Nombre = selectedDeudor.Nombre.Trim(); } catch { }

            if (_toEditDeudor != null)
            {
                try { checkDeudor = Variables.Inventario.Deudores.Single(x => x.Nombre.ToLower() == selectedDeudor.Nombre.ToLower()); } catch { }
                if (checkDeudor == null || checkDeudor.Id == selectedDeudor.Id)
                {
                    _toEditDeudor.Nombre = selectedDeudor.Nombre; _toEditDeudor.Detalles = selectedDeudor.Detalles;
                    _toEditDeudor.Nivel = selectedDeudor.Nivel; _toEditDeudor.Resto = selectedDeudor.Resto; _toEditDeudor.UsuarioID = selectedDeudor.UsuarioID;

                    _ = Variables.Inventario.SaveChanges();
                    thisWindow.DialogResult = true;
                }
                else { Variables.messageError.Usuario(); }
            }
            else
            {
                try
                {
                    checkDeudor = Variables.Inventario.Deudores.Local.Single(x => x.Nombre.ToLower() == selectedDeudor.Nombre.ToLower());
                    if (checkDeudor != null) { Variables.messageError.Usuario(); }
                    return;
                }
                catch
                {
                    _ = Variables.Inventario.Deudores.Add(selectedDeudor);
                    _ = Variables.Inventario.SaveChanges();
                    thisWindow.DialogResult = true;
                }
            }
        }


        bool helperCheckDeudor => selectedDeudor != null && selectedDeudor.Nivel > 0 && selectedDeudor.Nombre != null && selectedDeudor.Nombre.Length > 0;
        #endregion // Helpers


        #region Commands
        public Command comGuardar => new Command(
            (object parameter) => helperGuardarDeudor(),
            (object parameter) => helperCheckDeudor);
        #endregion // Commands
    }
}
