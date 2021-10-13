using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF.Domain
{
    public class tagsModel : Base.ModelBase
    {
        #region Private
        int _Minimo; string _Tag; bool _Activo;
        #endregion // Private

        #region Public
        public int Minimo { get => _Minimo; set { if (SetProperty(ref _Minimo, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(fullTag)); } } }

        public string Tag { get => _Tag; set { if (SetProperty(ref _Tag, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(fullTag)); } } }

        public bool Activo { get => _Activo; set { if (SetProperty(ref _Activo, value)) { OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<productosModel> ProductosPerTag { get; private set; } = new ObservableCollection<productosModel>();
        #endregion // Navigation

        #region Not Mapped
        [NotMapped]
        public string fullTag => Tag + " " + Minimo.ToString();
        #endregion // Not Mapped

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
