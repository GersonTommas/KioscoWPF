using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Kiosco.WPF
{
    public class cajaModel : Base.ModelBase
    {
        #region Private
        Double _CajaActual, _MercadoPago, _Vuelto; String _Hora; fechasModel _Fecha; cajaConteosModel _CajaConteoForCaja; ventasModel _VentaForCaja;
        #endregion // Private

        #region Public
        public Double CajaActual { get => _CajaActual; set { if (SetProperty(ref _CajaActual, Math.Round(value, 2))) { OnPropertyChanged(); privUpdateVenta(); } } }
        public Double MercadoPago { get => _MercadoPago; set { if (SetProperty(ref _MercadoPago, Math.Round(value, 2))) { OnPropertyChanged(); privUpdateVenta(); } } }
        public Double Vuelto { get => _Vuelto; set { if (SetProperty(ref _Vuelto, Math.Round(value, 2))) { OnPropertyChanged(); } } }

        public String Hora { get => _Hora; set { if (SetProperty(ref _Hora, Convert.ToDateTime(value).ToString("HH:mm:ss"))) { OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (SetProperty(ref _Fecha, value)) { OnPropertyChanged(); } } }

        public virtual cajaConteosModel CajaConteoForCaja { get => _CajaConteoForCaja; set { if (SetProperty(ref _CajaConteoForCaja, value)) { OnPropertyChanged(); } } }
        public virtual ventasModel VentaForCaja { get => _VentaForCaja; set { if (SetProperty(ref _VentaForCaja, value)) { OnPropertyChanged(); } } }
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

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
