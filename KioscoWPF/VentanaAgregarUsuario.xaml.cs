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
using System.Linq;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaAgregarUsuario.xaml
    /// </summary>
    public partial class VentanaAgregarUsuario : Window
    {
        public VentanaAgregarUsuario() { InitializeComponent(); (DataContext as ViewModels.VMAgregarUsuario).setInitialize(this); }
        public VentanaAgregarUsuario(DBUsuariosClass sentUsuario) { InitializeComponent(); (DataContext as ViewModels.VMAgregarUsuario).setInitialize(this, sentUsuario); }
    }
}

namespace KioscoWPF.ViewModels
{

    class VMAgregarUsuario : helperObservableClass
    {
        #region Initialize
        VentanaAgregarUsuario thisWindow;


        public void setInitialize(VentanaAgregarUsuario tempWindow)
        {
            thisWindow = tempWindow;
        }
        public void setInitialize(VentanaAgregarUsuario tempWindow, DBUsuariosClass tempUsuario)
        {
            thisWindow = tempWindow;
            if (tempUsuario != null)
            {
                _toEditUsusario = tempUsuario;
                selectedUsuario = new DBUsuariosClass()
                {
                    Apellido = tempUsuario.Apellido,
                    Contraseña = tempUsuario.Contraseña,
                    Detalle = tempUsuario.Detalle,
                    Activo = tempUsuario.Activo,
                    FechaIngreso = tempUsuario.FechaIngreso,
                    FechaSalida = tempUsuario.FechaSalida,
                    Id = tempUsuario.Id,
                    Nivel = tempUsuario.Nivel,
                    Nombre = tempUsuario.Nombre,
                    Usuario = tempUsuario.Usuario,
                    Resto = tempUsuario.Resto
                };

                bolEdit = true;
            }
        }
        #endregion // Initialize



        #region Variables
        DBUsuariosClass _toEditUsusario = new DBUsuariosClass();


        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (_bolEdit != value) { _bolEdit = value; OnPropertyChanged(); OnPropertyChanged(nameof(strWindowTitle)); OnPropertyChanged(nameof(strBorderTitle)); } } }

        DBUsuariosClass _selectedUsuario = new DBUsuariosClass() { Nivel = 5, Activo = true, FechaIngreso = Variables.strFecha, Resto = 0 };
        public DBUsuariosClass selectedUsuario { get => _selectedUsuario; set { if (_selectedUsuario != value) { _selectedUsuario = value; OnPropertyChanged(); } } }


        public string strWindowTitle => bolEdit ? "Editar Usuario" : "Nuevo Usuario";
        public string strBorderTitle => bolEdit ? "ID: " + _toEditUsusario.Id : "Nuevo Usuario";
        public List<int> listNiveles => new List<int>() { 5, 4, 3, 2, 1 };
        #endregion // Variables



        #region Helpers
        void helperGuardarUsuario()
        {
            DBUsuariosClass checkUsuario = null;

            try { selectedUsuario.Apellido = selectedUsuario.Apellido.Trim(); } catch { }
            try { selectedUsuario.Detalle = selectedUsuario.Detalle.Trim(); } catch { }
            try { selectedUsuario.Nombre = selectedUsuario.Nombre.Trim(); } catch { }
            try { selectedUsuario.Usuario = selectedUsuario.Usuario.Trim(); } catch { }

            try { DateTime hola = Convert.ToDateTime(selectedUsuario.FechaIngreso); }
            catch { Variables.messageError.FechaErronea(); return; }

            if (bolEdit)
            {
                try { checkUsuario = Variables.Inventario.Usuarios.Single(x => x.Usuario.ToLower() == selectedUsuario.Usuario.ToLower()); } catch { }

                if (checkUsuario == null || checkUsuario.Id == selectedUsuario.Id)
                {
                    _toEditUsusario.Activo = selectedUsuario.Activo; _toEditUsusario.Apellido = selectedUsuario.Apellido; _toEditUsusario.Contraseña = selectedUsuario.Contraseña;
                    _toEditUsusario.Detalle = selectedUsuario.Detalle; _toEditUsusario.FechaIngreso = selectedUsuario.FechaIngreso; _toEditUsusario.FechaSalida = selectedUsuario.FechaSalida;
                    _toEditUsusario.Nivel = selectedUsuario.Nivel; _toEditUsusario.Nombre = selectedUsuario.Nombre; _toEditUsusario.Resto = selectedUsuario.Resto; _toEditUsusario.Usuario = selectedUsuario.Usuario;

                    _ = Variables.Inventario.SaveChanges();
                    thisWindow.DialogResult = true;
                }
                else { Variables.messageError.Usuario(); }
            }
            else
            {
                try
                {
                    checkUsuario = Variables.Inventario.Usuarios.Single(x => x.Usuario.ToLower() == selectedUsuario.Usuario.ToLower());
                    if (checkUsuario != null) { Variables.messageError.Usuario(); }
                    return;
                }
                catch
                {
                    Variables.Inventario.Usuarios.Local.Add(selectedUsuario);
                    _ = Variables.Inventario.SaveChanges();
                    thisWindow.DialogResult = true;
                }
            }

        }

        bool checkUsuario()
        {
            try
            {
                if (selectedUsuario.Nombre != null && selectedUsuario.Apellido != null && selectedUsuario.FechaIngreso != null && selectedUsuario.Usuario != null && selectedUsuario.Contraseña != null)
                {
                    if (selectedUsuario.Nombre.Length > 0 && selectedUsuario.Apellido.Length > 0 && selectedUsuario.Nivel > 0 && selectedUsuario.Nivel < 6 && selectedUsuario.Usuario.Length > 4 && selectedUsuario.Contraseña.Length > 7)
                    { return true; }
                }
            }
            catch { return false; }

            return false;
        }
        #endregion // Helpers



        #region Commands
        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);

        public Command comGuardar => new Command(
            (object parameter) => helperGuardarUsuario(),
            (object parameter) => checkUsuario());
        #endregion // Commands
    }
}