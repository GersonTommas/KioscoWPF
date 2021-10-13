using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Kiosco.WPF
{
    public class ingresoProductosModel : Base.ModelBase
    {
        #region Private
        int _Cantidad; Double _PrecioPagado, _PrecioActual; productosModel _Producto; ingresosModel _Ingreso;
        #endregion // Private


        #region Public
        public int Cantidad { get => _Cantidad; set { if (SetProperty(ref _Cantidad, value)) { OnPropertyChanged(); } } }
        public Double PrecioPagado { get => _PrecioPagado; set { if (SetProperty(ref _PrecioPagado, Math.Round(value, 2))) { OnPropertyChanged(); OnPropertyChanged(nameof(PrecioTotal)); OnPropertyChanged(nameof(PrecioSugerido)); } } }
        public Double PrecioActual { get => _PrecioActual; set { if (SetProperty(ref _PrecioActual, Math.Round(value, 2))) { OnPropertyChanged(); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set { if (SetProperty(ref _Producto, value)) { OnPropertyChanged(); } } }

        public int IngresoID { get; set; }
        public virtual ingresosModel Ingreso { get => _Ingreso; set { if (SetProperty(ref _Ingreso, value)) { OnPropertyChanged(); } } }
        #endregion // Public


        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(Cantidad * PrecioPagado, 2);

        [NotMapped]
        public Double PrecioSugerido => Math.Round(PrecioPagado * 1.3, 2);
        #endregion // NotMapped

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
