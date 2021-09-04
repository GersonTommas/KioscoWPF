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
    /// Interaction logic for VentanaAgregarDeudor.xaml
    /// </summary>
    public partial class VentanaAgregarDeudor : Window
    {
        public VentanaAgregarDeudor(deudoresModel sentDeudor) { InitializeComponent(); (DataContext as ViewModels.VMAgregarDeudor).setInitialize(this, sentDeudor); }
        public VentanaAgregarDeudor() { InitializeComponent(); (DataContext as ViewModels.VMAgregarDeudor).setInitialize(this); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarDeudor : helperObservableClass
    {
        #region Initialize
        VentanaAgregarDeudor thisWindow;

        public void setInitialize(VentanaAgregarDeudor tempWindow)
        {
            thisWindow = tempWindow;
        }
        public void setInitialize(VentanaAgregarDeudor tempWindow, deudoresModel tempDeudor)
        {
            thisWindow = tempWindow;

            if (tempDeudor != null)
            {
                _toEditDeudor = tempDeudor;
                _selectedDeudor = new deudoresModel() { Id = tempDeudor.Id, Detalles = tempDeudor.Detalles, Nivel = tempDeudor.Nivel, Nombre = tempDeudor.Nombre, Resto = tempDeudor.Resto, UsuarioID = tempDeudor.UsuarioID, Usuario = tempDeudor.Usuario }; OnPropertyChanged(nameof(selectedDeudor));
                OnPropertyChanged(nameof(strWindowTitle)); OnPropertyChanged(nameof(strGroupTitle));
            }
            else
            {
                setInitialize(tempWindow);
            }
        }
        #endregion // Initialize


        #region Private
        deudoresModel _toEditDeudor = null;
        #endregion // Private


        #region Public
        deudoresModel _selectedDeudor = new deudoresModel() { Nivel = 1, Resto = 0, Usuario = Variables.UsuarioLogueado };
        public deudoresModel selectedDeudor => _selectedDeudor;
        #endregion // Public


        #region Read Only
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

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);
        #endregion // Commands
    }
}