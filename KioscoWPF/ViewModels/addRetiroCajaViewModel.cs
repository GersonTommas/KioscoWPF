using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Kiosco.WPF.ViewModels
{
    class addRetiroCajaViewModel : Base.ViewModelBase
    {

        #region Initialize
        public addRetiroCajaViewModel()
        {
            fechasModel tempFecha = Db.returnFecha();
            String tempHora = Variables.strHora;


            listMotivos.SortDescriptions.Clear(); listMotivos.SortDescriptions.Add(new SortDescription("Motivo", ListSortDirection.Ascending));
            listUsuarios.SortDescriptions.Clear(); listUsuarios.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));
            listAutorizantes.SortDescriptions.Clear(); listAutorizantes.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));


            selectedRetiroCaja = new retirosCajaModel() { Fecha = tempFecha, Hora = tempHora, Caja = new cajaModel() { Fecha = tempFecha, Hora = tempHora }, Motivo = null, Pendiente = false };
        }
        #endregion // Initialize



        #region Variables
        motivoRetirosModel _newMotivo = new motivoRetirosModel();
        public motivoRetirosModel newMotivo { get => _newMotivo; set { if (_newMotivo != value) { _newMotivo = value; OnPropertyChanged(); } } }


        public retirosCajaModel selectedRetiroCaja { get; }



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
                motivoRetirosModel tempMotivo = Variables.Inventario.RetiroMotivos.Local.Single(x => x.Motivo.ToLower() == newMotivo.Motivo.ToLower());
                Variables.messageError.Existencia();
            }
            catch { Variables.Inventario.RetiroMotivos.Local.Add(newMotivo); _ = Variables.Inventario.SaveChanges(); newMotivo = new motivoRetirosModel(); }
        }

        bool checkGuardar => (selectedRetiroCaja.Caja.MercadoPago > 0 || selectedRetiroCaja.Caja.CajaActual > 0) && selectedRetiroCaja.Motivo != null && selectedRetiroCaja.UsuarioAutoriza != null && selectedRetiroCaja.UsuarioRetira != null;
        #endregion // Helpers



        #region Commands
        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(parameter),
            (object parameter) => checkGuardar);

        public Command comAgregarMotivo => new Command(
            (object parameter) => helperAgregarMotivo(),
            (object parameter) => !string.IsNullOrWhiteSpace(newMotivo.Motivo));
        #endregion // Commands
    }
}
