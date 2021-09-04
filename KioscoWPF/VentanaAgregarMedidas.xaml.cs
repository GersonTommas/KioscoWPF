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
    /// Interaction logic for VentanaUnidades.xaml
    /// </summary>
    public partial class VentanaAgregarMedidas : Window
    {
        public VentanaAgregarMedidas(medidasModel sentMedida) { InitializeComponent(); (DataContext as ViewModels.VMAgregarMedidas).setInitialize(this, sentMedida); }
        public VentanaAgregarMedidas() { InitializeComponent(); (DataContext as ViewModels.VMAgregarMedidas).setInitialize(this); }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) { TextBox tb = (TextBox)sender; tb.SelectAll(); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarMedidas : helperObservableClass
    {
        #region Initialize
        VentanaAgregarMedidas thisWindow;

        public void setInitialize(VentanaAgregarMedidas tempWindow)
        {
            thisWindow = tempWindow;
        }
        public void setInitialize(VentanaAgregarMedidas tempWindow, medidasModel tempMedida)
        {
            thisWindow = tempWindow;

            if (tempMedida != null)
            {
                _editMedida = tempMedida;

                selectedMedida.Activo = tempMedida.Activo; selectedMedida.Id = tempMedida.Id; selectedMedida.Medida = tempMedida.Medida; selectedMedida.Tipo = tempMedida.Tipo;
                bolEdit = true;
            }
        }
        #endregion // Initialize



        #region Variables
        medidasModel _editMedida = null;


        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (_bolEdit != value) { _bolEdit = value; OnPropertyChanged(); OnPropertyChanged(nameof(strWindowTitle)); OnPropertyChanged(nameof(strGroupTitle)); } } }


        public medidasModel selectedMedida { get; } = new medidasModel() { Activo = true };
        public string strWindowTitle => bolEdit ? "Editar Medida" : "Nueva Medida";
        public string strGroupTitle => bolEdit ? "Id: " + _editMedida.Id : "Nueva Medida";
        #endregion // Variables



        #region Helpers
        void helperGuardarMedida()
        {
            medidasModel compareMedida = null;
            if (!string.IsNullOrWhiteSpace(selectedMedida.Medida)) { selectedMedida.Medida = selectedMedida.Medida.Trim(); }

            try { compareMedida = Variables.Inventario.Medidas.Single(x => x.Medida.ToLower() == selectedMedida.Medida.ToLower() && x.Tipo == selectedMedida.Tipo); } catch { }

            if (bolEdit)
            {
                if (compareMedida == null || compareMedida.Id == _editMedida.Id)
                {
                    _editMedida.Activo = selectedMedida.Activo; _editMedida.Medida = selectedMedida.Medida;
                    _ = Variables.Inventario.SaveChanges();
                    Variables.messageError.Guardado();

                    thisWindow.DialogResult = true;
                }
                else { Variables.messageError.Existencia(); }
            }
            else if (compareMedida == null)
            {
                _ = Variables.Inventario.Medidas.Add(selectedMedida);
                _ = Variables.Inventario.SaveChanges();
                Variables.messageError.Guardado();

                thisWindow.DialogResult = true;
            }
            else { Variables.messageError.Existencia(); }
        }

        bool helperCheckMedida => !string.IsNullOrWhiteSpace(selectedMedida.Medida) && selectedMedida.Medida.Length > 0 && selectedMedida.Tipo > 0;
        #endregion // Helpers



        #region Commands
        public Command comNextControl => new Command((object parameter) => { if (!string.IsNullOrWhiteSpace(selectedMedida.Medida)) { Variables.nextTarget(parameter); } });

        public Command comCancelar => new Command((object parameter) => thisWindow.Close());

        public Command comGuardar => new Command(
            (object parameter) => helperGuardarMedida(),
            (object parameter) => helperCheckMedida);
        #endregion // Commands
    }
}
