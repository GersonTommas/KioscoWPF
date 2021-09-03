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
    /// Interaction logic for VentanaAgregarRetiroCaja.xaml
    /// </summary>
    public partial class VentanaAgregarRetiroCaja : Window
    {
        public VentanaAgregarRetiroCaja()
        {
            InitializeComponent(); (DataContext as ViewModels.VMAgregarRetiroCaja).setInitialize(this);
        }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarRetiroCaja : helperObservableClass
    {
        #region Initialize
        VentanaAgregarRetiroCaja thisWindow;

        public void setInitialize(VentanaAgregarRetiroCaja tempWindow)
        {
            thisWindow = tempWindow;

            DBFechasClass tempFecha = Db.returnFecha();
            String tempHora = Variables.strHora;


            listMotivos.SortDescriptions.Clear(); listMotivos.SortDescriptions.Add(new SortDescription("Motivo", ListSortDirection.Ascending));
            listUsuarios.SortDescriptions.Clear(); listUsuarios.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));
            listAutorizantes.SortDescriptions.Clear(); listAutorizantes.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));

            selectedRetiroCaja.Fecha = tempFecha; selectedRetiroCaja.Hora = tempHora; selectedRetiroCaja.Caja = new DBCajaClass() { Fecha = tempFecha, Hora = tempHora };
            selectedRetiroCaja.Motivo = null;
        }
        #endregion // Initialize



        #region Variables
        DBMotivosRetirosCaja _newMotivo = new DBMotivosRetirosCaja();
        public DBMotivosRetirosCaja newMotivo { get => _newMotivo; set { if (_newMotivo != value) { _newMotivo = value; OnPropertyChanged(); } } }


        public DBRetirosCaja selectedRetiroCaja { get; } = new DBRetirosCaja() { Pendiente = false };



        readonly CollectionViewSource _listMotivosSource = new CollectionViewSource { Source = Variables.Inventario.RetiroMotivos.Local.ToObservableCollection() };
        public ICollectionView listMotivos => _listMotivosSource.View;

        readonly CollectionViewSource _listUsuariosSource = new CollectionViewSource { Source = Variables.Inventario.Usuarios.Local.ToObservableCollection() };
        public ICollectionView listUsuarios => _listUsuariosSource.View;

        readonly CollectionViewSource _listAutorizantesSource = new CollectionViewSource { Source = Variables.Inventario.Usuarios.Local.ToObservableCollection() };
        public ICollectionView listAutorizantes => _listAutorizantesSource.View;
        #endregion // Variables



        #region Helpers
        void helperGuardar(object sentPass)
        {
            if (sentPass != null)
            {
                PasswordBox passBox = sentPass as PasswordBox;

                if (passBox.Password != null)
                {
                    string tempPass = passBox.Password;

                    if (selectedRetiroCaja.UsuarioAutoriza.Contraseña == tempPass)
                    {
                        if (selectedRetiroCaja.Proveedor != null) { selectedRetiroCaja.Pendiente = true; }
                        Variables.Inventario.Retiros.Local.Add(selectedRetiroCaja);
                        Db.contabilizarCaja(selectedRetiroCaja.Caja, true);
                        _ = Variables.Inventario.SaveChanges();
                        thisWindow.Close();
                    }
                    else { Variables.messageError.LogIn(); }
                }
            }
        }

        void helperAgregarMotivo()
        {
            if (newMotivo.Motivo != null) { newMotivo.Motivo = newMotivo.Motivo.Trim(); }
            try
            {
                DBMotivosRetirosCaja tempMotivo = Variables.Inventario.RetiroMotivos.Local.Single(x => x.Motivo.ToLower() == newMotivo.Motivo.ToLower());
                Variables.messageError.Existencia();
            }
            catch { Variables.Inventario.RetiroMotivos.Local.Add(newMotivo); _ = Variables.Inventario.SaveChanges(); newMotivo = new DBMotivosRetirosCaja(); }
        }

        bool checkGuardar => (selectedRetiroCaja.Caja.MercadoPago > 0 || selectedRetiroCaja.Caja.CajaActual > 0) && selectedRetiroCaja.Motivo != null && selectedRetiroCaja.UsuarioAutoriza != null && selectedRetiroCaja.UsuarioRetira != null;
        #endregion // Helpers



        #region Commands
        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(parameter),
            (object parameter) => checkGuardar);

        public Command comAgregarMotivo => new Command(
            (object parameter) => helperAgregarMotivo(),
            (object parameter) => !string.IsNullOrWhiteSpace(newMotivo.Motivo));
        #endregion // Commands
    }
}
