using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF
{
    public class puestoModel : Base.PropertyChangedBase
    {
        #region Private
        int _HorasMensuales; Double _Sueldo; string _Nombre;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int HorasMensuales { get => _HorasMensuales; set { if (SetProperty(ref _HorasMensuales, value)) { OnPropertyChanged(nameof(NmSueldoMensual)); } } }

        public Double Sueldo { get => _Sueldo; set { if (SetProperty(ref _Sueldo, Math.Round(value, 2))) { OnPropertyChanged(nameof(NmSueldoMensual)); } } }

        public string Nombre { get => _Nombre; set => SetProperty(ref _Nombre, value); }
        #endregion // Public

        #region Navigation
        #endregion // Navigation

        #region NotMapped
        public Double NmSueldoMensual => Math.Round(Sueldo * HorasMensuales, 2);
        #endregion // NotMapped
    }
}
