using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF
{
    public class ingresoProductosModel : Base.PropertyChangedBase
    {
        #region Private
        int _Cantidad; Double _PrecioPagado, _PrecioActual; productosModel _Producto; ingresosModel _Ingreso;
        #endregion // Private


        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set => SetProperty(ref _Cantidad, value); }
        public Double PrecioPagado { get => _PrecioPagado; set { if (SetProperty(ref _PrecioPagado, Math.Round(value, 2))) { OnPropertyChanged(nameof(PrecioTotal)); OnPropertyChanged(nameof(PrecioSugerido)); } } }
        public Double PrecioActual { get => _PrecioActual; set => SetProperty(ref _PrecioActual, Math.Round(value, 2)); }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set => SetProperty(ref _Producto, value); }

        public int IngresoID { get; set; }
        public virtual ingresosModel Ingreso { get => _Ingreso; set => SetProperty(ref _Ingreso, value); }
        #endregion // Public


        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(Cantidad * PrecioPagado, 2);

        [NotMapped]
        public Double PrecioSugerido => Math.Round(PrecioPagado * 1.3, 2);
        #endregion // NotMapped
    }
}
