using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace KioscoWPF
{
    public class motivoRetirosModel : Base.PropertyChangedBase
    {
        #region Private
        string _Motivo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public string Motivo { get => _Motivo; set => SetProperty(ref _Motivo, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<retirosCajaModel> Retiros { get; private set; } = new ObservableCollection<retirosCajaModel>();
        #endregion // Navigation
    }
}
