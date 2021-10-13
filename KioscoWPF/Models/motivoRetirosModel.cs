using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kiosco.WPF
{
    public class motivoRetirosModel : Base.ModelBase
    {
        #region Private
        string _Motivo;
        #endregion // Private

        #region Public
        public string Motivo { get => _Motivo; set { if (SetProperty(ref _Motivo, value)) { OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<retirosCajaModel> Retiros { get; private set; } = new ObservableCollection<retirosCajaModel>();
        #endregion // Navigation

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
