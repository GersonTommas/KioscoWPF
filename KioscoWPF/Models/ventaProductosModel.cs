using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF
{
    public class ventaProductosModel : Base.PropertyChangedBase
    {
        #region Private
        int _Cantidad, _CantidadDeuda, _CantidadFaltante; Double _Precio, _PrecioPagado; productosModel _Producto; ventasModel _Venta; deudoresModel _Deudor; fechasModel _FechaPagado;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set { if (SetProperty(ref _Cantidad, value)) { OnPropertyChanged(nameof(PrecioTotal)); } } }
        public int CantidadDeuda { get => _CantidadDeuda; set => SetProperty(ref _CantidadDeuda, value); }
        public int CantidadFaltante { get => _CantidadFaltante; set => SetProperty(ref _CantidadFaltante, value); }

        public Double Precio { get => _Precio; set { if (SetProperty(ref _Precio, Math.Round(value, 2))) { OnPropertyChanged(nameof(PrecioTotal)); } } }
        public Double PrecioPagado { get => _PrecioPagado; set => SetProperty(ref _PrecioPagado, Math.Round(value, 2)); }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set => SetProperty(ref _Producto, value); }

        public int VentaID { get; set; }
        public virtual ventasModel Venta { get => _Venta; set => SetProperty(ref _Venta, value); }

        public int? FechaPagadoID { get; set; }
        public virtual fechasModel FechaPagado { get => _FechaPagado; set => SetProperty(ref _FechaPagado, value); }

        public int? DeudorID { get; set; }
        public virtual deudoresModel Deudor { get => _Deudor; set => SetProperty(ref _Deudor, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<cajaModel> CajasPerVentaProducto { get; private set; } = new ObservableCollection<cajaModel>();
        #endregion // Navigation


        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(Cantidad * Precio, 2);

        [NotMapped]
        public int isProductoDeuda => CantidadDeuda == 0 ? 0 : CantidadDeuda < Cantidad ? 1 : 2;
        [NotMapped]
        public int isDeudaPagada => CantidadFaltante == 0 ? 0 : CantidadFaltante != CantidadDeuda ? 1 : 2;
        [NotMapped]
        public bool BolPagado => CantidadFaltante == 0;

        [NotMapped]
        public Double PrecioFinal => Deudor != null
                                        ? !BolPagado
                                            ? Deudor.Nivel switch
                                            {
                                                1 => Math.Round(Precio, 2),
                                                2 => Math.Round(Producto.PrecioActual, 2),
                                                _ => Math.Round(Producto.PrecioActual * 1.05, 2),
                                            }
                                            : Math.Round(PrecioPagado, 2)
                                        : 0;
        [NotMapped]
        public Double TotalFaltante { get { OnPropertyChanged(nameof(PrecioFinal)); return Math.Round(PrecioFinal * CantidadFaltante, 2); } }
        #endregion // NotMapped
    }
}
