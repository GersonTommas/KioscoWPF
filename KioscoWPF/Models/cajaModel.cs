using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF
{
    public class cajaModel : Base.PropertyChangedBase
    {
        #region Private
        Double _CajaActual, _MercadoPago, _Vuelto; String _Hora; fechasModel _Fecha; cajaConteosModel _CajaConteoForCaja; ventasModel _VentaForCaja;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public Double CajaActual { get => _CajaActual; set { if (SetProperty(ref _CajaActual, Math.Round(value, 2))) { privUpdateVenta(); } } }
        public Double MercadoPago { get => _MercadoPago; set { if (SetProperty(ref _MercadoPago, Math.Round(value, 2))) { privUpdateVenta(); } } }
        public Double Vuelto { get => _Vuelto; set => SetProperty(ref _Vuelto, Math.Round(value, 2)); }

        public String Hora { get => _Hora; set => SetProperty(ref _Hora, Convert.ToDateTime(value).ToString("HH:mm:ss")); }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set => SetProperty(ref _Fecha, value); }

        public virtual cajaConteosModel CajaConteoForCaja { get => _CajaConteoForCaja; set => SetProperty(ref _CajaConteoForCaja, value); }
        public virtual ventasModel VentaForCaja { get => _VentaForCaja; set => SetProperty(ref _VentaForCaja, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<ventaProductosModel> VentaProductosPerCaja { get; private set; } = new ObservableCollection<ventaProductosModel>();
        public virtual ICollection<retirosCajaModel> RetirosCajaPerCaja { get; private set; } = new ObservableCollection<retirosCajaModel>();
        #endregion // Navigation

        #region Helpers
        void privUpdateVenta()
        {
            if (VentaForCaja != null)
            {
                /*
                OnPropertyChanged(nameof(VentaForCaja.TotalPagado));
                OnPropertyChanged(nameof(VentaForCaja.Vuelto));
                */
                OnPropertyChanged(nameof(doubleEfectivoTotal));
            }
        }
        #endregion // Helpers

        #region NotMapped
        [NotMapped]
        public Double doubleEfectivoTotal => CajaActual - Vuelto;
        #endregion // NotMapped
    }
}
