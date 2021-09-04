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
    /// Interaction logic for VentanaAgregarTag.xaml
    /// </summary>
    public partial class VentanaAgregarTag : Window
    {
        public VentanaAgregarTag() { InitializeComponent(); (DataContext as ViewModels.VMAgregarTag).setInitialize(this); }
        public VentanaAgregarTag(tagsModel sentTag) { InitializeComponent(); (DataContext as ViewModels.VMAgregarTag).setInitialize(this, sentTag); }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMAgregarTag : ObservableClass
    {
        #region Initialize
        VentanaAgregarTag thisWindow;

        public void setInitialize(VentanaAgregarTag tempWindow)
        {
            thisWindow = tempWindow;
        }
        public void setInitialize(VentanaAgregarTag tempWindow, tagsModel tempTag)
        {
            thisWindow = tempWindow;

            if (tempTag != null)
            {
                _tagToEdit = tempTag;

                tagSelected = new tagsModel() { Activo = tempTag.Activo, Id = tempTag.Id, Minimo = tempTag.Minimo, Tag = tempTag.Tag };

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
        void helperGuardarTag()
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

        bool checkTag()
        {
            try { if (tagSelected.Minimo >= 0 && !string.IsNullOrWhiteSpace(tagSelected.Tag) && tagSelected.Tag.Length > 0) { return true; } } catch { }
            return false;
        }
        #endregion // Helpers



        #region Commands
        public Command comNextTarget => new Command((object parameter) => Variables.nextTarget(parameter));

        public Command comGuardarTag => new Command(
            (object parameter) => helperGuardarTag(),
            (object parameter) => checkTag());

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = true);
        #endregion // Commands
    }
}