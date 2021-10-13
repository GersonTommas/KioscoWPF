using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Kiosco.WPF.ViewModels
{
    class addTagViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, tagsModel sentTag)
        {
            base.setInitialize(sentWindow);

            if (sentTag != null)
            {
                _tagToEdit = sentTag;

                tagSelected = new tagsModel() { Activo = sentTag.Activo, Id = sentTag.Id, Minimo = sentTag.Minimo, Tag = sentTag.Tag };

                bolEdit = true;
            }
        }
        #endregion //Initialize



        #region Variables
        tagsModel _tagToEdit;


        bool _bolEdit = false;
        public bool bolEdit { get => _bolEdit; set { if (_bolEdit != value) { _bolEdit = value; OnPropertyChanged(); OnPropertyChanged(nameof(strWindowTitle)); } } }

        tagsModel _tagSelected = new tagsModel() { Minimo = 0, Activo = true };
        public tagsModel tagSelected { get => _tagSelected; set { if (_tagSelected != value) { _tagSelected = value; OnPropertyChanged(); } } }

        bool _bolMantenerAbierto = false;
        public bool bolMantenerAbierto { get => _bolMantenerAbierto; set { if (_bolMantenerAbierto != value) { _bolMantenerAbierto = value; OnPropertyChanged(); } } }

        int _intTagsCreados = 0;
        public int intTagsCreados { get => _intTagsCreados; set { if (_intTagsCreados != value) { _intTagsCreados = value; OnPropertyChanged(); } } }


        public string strWindowTitle => bolEdit ? "Editar Tag" : "Nuevo Tag";
        public string strBorderTitle => bolEdit ? "ID: " + _tagToEdit.Id : "Nuevo Tag";
        #endregion // Variables



        #region Helpers
        void helperGuardar()
        {
            tagsModel compareTag = null;
            if (!string.IsNullOrWhiteSpace(tagSelected.Tag)) { tagSelected.Tag = tagSelected.Tag.Trim(); }

            try { compareTag = Variables.Inventario.Tags.Single(x => x.Tag.ToLower() == tagSelected.Tag.ToLower() && x.Minimo == tagSelected.Minimo); } catch { }

            if (bolEdit)
            {
                if (compareTag == null || compareTag.Id == tagSelected.Id)
                {
                    _tagToEdit.Activo = _tagSelected.Activo;
                    _tagToEdit.Minimo = _tagSelected.Minimo;
                    _tagToEdit.Tag = _tagSelected.Tag;

                    _ = Variables.Inventario.SaveChanges();

                    thisWindow.DialogResult = true;
                }
                else { Variables.messageError.Existencia(); }
            }
            else if (compareTag == null)
            {
                intTagsCreados++;

                Variables.Inventario.Tags.Local.Add(tagSelected);
                _ = Variables.Inventario.SaveChanges();

                if (!bolMantenerAbierto) { thisWindow.Close(); }

                tagSelected = new tagsModel() { Minimo = 1, Activo = true };

                compareTag = null;
            }
            else { Variables.messageError.Existencia(); }
        }

        bool checkGuardar()
        {
            try { if (tagSelected.Minimo >= 0 && !string.IsNullOrWhiteSpace(tagSelected.Tag) && tagSelected.Tag.Length > 0) { return true; } } catch { }
            return false;
        }
        #endregion // Helpers



        #region Commands
        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => checkGuardar());
        #endregion // Commands
    }
}
