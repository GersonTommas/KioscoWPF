using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF.Domain
{
    public class puestoModel : Base.ModelBase
    {
        #region Private
        int _HorasMensuales; Double _Sueldo; string _Nombre;
        #endregion // Private

        #region Public
        public int HorasMensuales { get => _HorasMensuales; set { if (SetProperty(ref _HorasMensuales, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(NmSueldoMensual)); } } }

        public Double Sueldo { get => _Sueldo; set { if (SetProperty(ref _Sueldo, Math.Round(value, 2))) { OnPropertyChanged(); OnPropertyChanged(nameof(NmSueldoMensual)); } } }

        public string Nombre { get => _Nombre; set { if (SetProperty(ref _Nombre, value)) { OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        #endregion // Navigation

        #region NotMapped
        public Double NmSueldoMensual => Math.Round(Sueldo * HorasMensuales, 2);
        #endregion // NotMapped

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
