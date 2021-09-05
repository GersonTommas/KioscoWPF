using System.Linq;
using System.Windows;

namespace KioscoWPF.ViewModels
{
    class addMedidaViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, medidasModel sentMedida)
        {
            base.setInitialize(sentWindow);

            if (sentMedida != null)
            {
                _editMedida = sentMedida;

                selectedMedida.Activo = sentMedida.Activo; selectedMedida.Id = sentMedida.Id; selectedMedida.Medida = sentMedida.Medida; selectedMedida.Tipo = sentMedida.Tipo;
                bolEdit = true;
            }
        }
        #endregion // Initialize



        #region Variables
        medidasModel _editMedida = null;


        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (SetProperty(ref _bolEdit, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(strWindowTitle)); OnPropertyChanged(nameof(strGroupTitle)); } } }


        public medidasModel selectedMedida { get; } = new medidasModel() { Activo = true };
        public string strWindowTitle => bolEdit ? "Editar Medida" : "Nueva Medida";
        public string strGroupTitle => bolEdit ? "Id: " + _editMedida.Id : "Nueva Medida";
        #endregion // Variables



        #region Helpers
        void helperGuardar(object sentParameter)
        {
            if (!checkGuardar)
            {
                Variables.nextTarget(sentParameter);
                return;
            }

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

        bool checkGuardar => !string.IsNullOrWhiteSpace(selectedMedida.Medida) && selectedMedida.Medida.Length > 0 && selectedMedida.Tipo > 0;
        #endregion // Helpers



        #region Commands
        public Command comGuardar => new Command((object parameter) => helperGuardar(parameter));
        #endregion // Commands
    }
}
