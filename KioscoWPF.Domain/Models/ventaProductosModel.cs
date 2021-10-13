using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF.Domain
{
    public class ventaProductosModel : Base.ModelBase
    {
        #region Private
        int _Cantidad, _CantidadDeuda, _CantidadFaltante; Double _Precio, _PrecioPagado; productosModel _Producto; ventasModel _Venta; deudoresModel _Deudor; fechasModel _FechaPagado;
        #endregion // Private

        #region Public
        public int Cantidad { get => _Cantidad; set { if (SetProperty(ref _Cantidad, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(PrecioTotal)); } } }
        public int CantidadDeuda { get => _CantidadDeuda; set { if (SetProperty(ref _CantidadDeuda, value)) { OnPropertyChanged(); } } }
        public int CantidadFaltante { get => _CantidadFaltante; set { if (SetProperty(ref _CantidadFaltante, value)) { OnPropertyChanged(); } } }

        public Double Precio { get => _Precio; set { if (SetProperty(ref _Precio, Math.Round(value, 2))) { OnPropertyChanged(); OnPropertyChanged(nameof(PrecioTotal)); } } }
        public Double PrecioPagado { get => _PrecioPagado; set { if (SetProperty(ref _PrecioPagado, Math.Round(value, 2))) { OnPropertyChanged(); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set { if (SetProperty(ref _Producto, value)) { OnPropertyChanged(); } } }

        public int VentaID { get; set; }
        public virtual ventasModel Venta { get => _Venta; set { if (SetProperty(ref _Venta, value)) { OnPropertyChanged(); } } }

        public int? FechaPagadoID { get; set; }
        public virtual fechasModel FechaPagado { get => _FechaPagado; set { if (SetProperty(ref _FechaPagado, value)) { OnPropertyChanged(); } } }

        public int? DeudorID { get; set; }
        public virtual deudoresModel Deudor { get => _Deudor; set { if (SetProperty(ref _Deudor, value)) { OnPropertyChanged(); } } }
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

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
