using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF
{
    public class tagsModel : Base.PropertyChangedBase
    {
        #region Private
        int _Minimo; string _Tag; bool _Activo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Minimo { get => _Minimo; set { if (SetProperty(ref _Minimo, value)) { OnPropertyChanged(nameof(fullTag)); } } }

        public string Tag { get => _Tag; set { if (SetProperty(ref _Tag, value)) { OnPropertyChanged(nameof(fullTag)); } } }

        public bool Activo { get => _Activo; set => SetProperty(ref _Activo, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<productosModel> ProductosPerTag { get; private set; } = new ObservableCollection<productosModel>();
        #endregion // Navigation

        #region Not Mapped
        [NotMapped]
        public string fullTag => Tag + " " + Minimo.ToString();
        #endregion // Not Mapped
    }
}
