using Microsoft.EntityFrameworkCore;
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
using System.ComponentModel;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaPagarSacado.xaml
    /// </summary>
    public partial class VentanaPagarSacado : Window
    {
        public VentanaPagarSacado(usuariosModel sentUsuario) { InitializeComponent(); if (sentUsuario == null) { Close(); } try { (DataContext as ViewModels.VMPagarSacado).setInitialize(this, sentUsuario); } catch { } }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMPagarSacado : helperObservableClass
    {
        #region Initialize
        VentanaPagarSacado thisWindow;

        public void setInitialize(VentanaPagarSacado tempWindow, usuariosModel tempUsuario)
        {
            thisWindow = tempWindow; selectedUsuario = tempUsuario;

            _listAdminsSource.Source = Variables.Inventario.Usuarios.Local.ToObservableCollection(); OnPropertyChanged(nameof(listAdmins));

            listAdmins.Filter += delegate (object item) { return (item as usuariosModel).Nivel == 1; };
            listAdmins.SortDescriptions.Clear(); listAdmins.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));
        }
        #endregion // Initialize



        #region Variables
        usuariosModel _selectedUsuario;
        public usuariosModel selectedUsuario { get => _selectedUsuario; set { if (_selectedUsuario != value) { _selectedUsuario = value; OnPropertyChanged(); } } }

        usuariosModel _selectedAdmin;
        public usuariosModel selectedAdmin { get => _selectedAdmin; set { if (_selectedAdmin != value) { _selectedAdmin = value; OnPropertyChanged(); } } }

        cajaModel _selectedCaja;
        public cajaModel selectedCaja { get => _selectedCaja; set { if (_selectedCaja != value) { _selectedCaja = value; OnPropertyChanged(); } } }

        int _intAPagar;
        public int intAPagar { get => _intAPagar; set { if (_intAPagar != value) { _intAPagar = value; OnPropertyChanged(); } } }

        string _adminContraseña;
        public string adminContraseña { get => _adminContraseña; set { if (_adminContraseña != value) { _adminContraseña = value; OnPropertyChanged(); } } }


        readonly CollectionViewSource _listAdminsSource = new CollectionViewSource();
        public ICollectionView listAdmins => _listAdminsSource.View;

        IEnumerable<sacadoProductosModel> _listSacado => selectedUsuario.SacadoProductosPerUsuario.Where(x => x.BolPagado == false);



        public Double sumDeuda => _listSacado != null ? _listSacado.Sum(x => x.PrecioTotal) : 0;
        public Double sumFaltante => selectedUsuario != null ? sumDeuda - selectedUsuario.Resto : 0;
        #endregion // Variables


        #region Helpers
        public void helperGuardar()
        {
            if (selectedAdmin != null && selectedAdmin.Contraseña == adminContraseña)
            {
                Double tempTotalPlata = selectedUsuario.Resto + intAPagar;

                if (_listSacado != null)
                {
                    foreach (sacadoProductosModel sac in _listSacado)
                    {
                        if (tempTotalPlata > sac.PrecioTotal)
                        {
                            sac.FechaPagado = Db.returnFecha();
                            tempTotalPlata -= sac.PrecioTotal;
                        }
                    }
                }

                selectedUsuario.Resto = tempTotalPlata;
                _ = Variables.Inventario.SaveChanges();
                thisWindow.DialogResult = true;
            }
            else { Variables.messageError.LogIn(); }
        }

        public bool helperCheckPassword => selectedAdmin != null && adminContraseña != null && intAPagar > 0 && selectedAdmin.Contraseña == adminContraseña;
        #endregion // Helpers


        #region Commands
        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => helperCheckPassword);
        #endregion // Commands
    }
}
