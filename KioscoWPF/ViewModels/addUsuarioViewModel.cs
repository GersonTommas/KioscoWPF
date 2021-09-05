using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KioscoWPF.ViewModels
{
    class addUsuarioViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, usuariosModel sentUsuario)
        {
            base.setInitialize(sentWindow);

            if (sentUsuario != null)
            {
                _toEditUsusario = sentUsuario;
                selectedUsuario = new usuariosModel()
                {
                    Apellido = sentUsuario.Apellido,
                    Contraseña = sentUsuario.Contraseña,
                    Detalle = sentUsuario.Detalle,
                    Activo = sentUsuario.Activo,
                    FechaIngreso = sentUsuario.FechaIngreso,
                    FechaSalida = sentUsuario.FechaSalida,
                    Id = sentUsuario.Id,
                    Nivel = sentUsuario.Nivel,
                    Nombre = sentUsuario.Nombre,
                    Usuario = sentUsuario.Usuario,
                    Resto = sentUsuario.Resto
                };

                bolEdit = true;
            }
        }
        #endregion // Initialize



        #region Variables
        usuariosModel _toEditUsusario = new usuariosModel();


        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (SetProperty(ref _bolEdit, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(strWindowTitle)); OnPropertyChanged(nameof(strBorderTitle)); } } }

        usuariosModel _selectedUsuario = new usuariosModel() { Nivel = 5, Activo = true, FechaIngreso = Variables.strFecha, Resto = 0 };
        public usuariosModel selectedUsuario { get => _selectedUsuario; set { if (SetProperty(ref _selectedUsuario, value)) { OnPropertyChanged(); } } }


        public string strWindowTitle => bolEdit ? "Editar Usuario" : "Nuevo Usuario";
        public string strBorderTitle => bolEdit ? "ID: " + _toEditUsusario.Id : "Nuevo Usuario";
        public List<int> listNiveles => new List<int>() { 5, 4, 3, 2, 1 };
        #endregion // Variables



        #region Helpers
        void helperGuardarUsuario()
        {
            usuariosModel checkUsuario = null;

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
        public Command comGuardar => new Command(
            (object parameter) => helperGuardarUsuario(),
            (object parameter) => checkUsuario());
        #endregion // Commands
    }
}
