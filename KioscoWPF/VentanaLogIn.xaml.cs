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

namespace Kiosco.WPF
{
    /// <summary>
    /// Interaction logic for VentanaLogIn.xaml
    /// </summary>
    public partial class VentanaLogIn : Window
    {
        public VentanaLogIn() { InitializeComponent(); try { (DataContext as ViewModels.VMLogIn).setInitialize(this); } catch { } }
    }
}

namespace Kiosco.WPF.ViewModels
{
    class VMLogIn : helperObservableClass
    {
        #region Initialize
        VentanaLogIn thisWindow;

        public void setInitialize(VentanaLogIn tempWindow)
        {
            thisWindow = tempWindow;
            _listUsersSource.Source = Variables.Inventario.Usuarios.Local.ToObservableCollection(); OnPropertyChanged(nameof(listUsuarios));
            listUsuarios.SortDescriptions.Clear(); listUsuarios.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));
        }
        #endregion Initialize



        #region Variables
        usuariosModel _selectedUser;
        public usuariosModel selectedUser { get { return _selectedUser; } set { if (_selectedUser != value) { _selectedUser = value; OnPropertyChanged(); } } }

        string _enteredPassword;
        public string enteredPassword { get { return _enteredPassword; } set { if (_enteredPassword != value) { _enteredPassword = value; OnPropertyChanged(); } } }
        #endregion Private


        #region Read Only
        Views.addConteoCajaView vCaja;

        readonly CollectionViewSource _listUsersSource = new CollectionViewSource();
        public ICollectionView listUsuarios => _listUsersSource.View;
        #endregion // Read Only



        #region Public
        #endregion // Public

        #region Helpers
        void logIn(object sender)
        {
            if (selectedUser != null && !string.IsNullOrWhiteSpace(((PasswordBox)sender).Password) && ((PasswordBox)sender).Password == selectedUser.Contraseña)
            {
                Variables.UsuarioLogueado = selectedUser; vCaja = new Views.addConteoCajaView(); vCaja.Show(); thisWindow.Close();
            }
            else { Variables.messageError.LogIn(); if (sender != null) { ((PasswordBox)sender).SelectAll(); } }
        }

        bool checkFormulario => selectedUser != null;
        #endregion // Helpers

        #region Commands
        public Command comIniciar => new Command(
              (object parameter) => logIn(parameter),
              (object parameter) => checkFormulario);
        #endregion // Commands

        public Command comTest => new Command((object parameter) => { xTestWindow tWin = new xTestWindow(); tWin.ShowDialog(); });
    }
}
