using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF
{
    public class productosModel : Base.PropertyChangedBase
    {
        #region Private
        int _StockInicial, _Stock; Double _PrecioActual, _PrecioIngreso; string _Codigo, _Descripcion; fechasModel _FechaModificado; bool _Activo; tagsModel _Tag; medidasModel _Medida;
        bool _Agregado = false;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int StockInicial { get => _StockInicial; set => SetProperty(ref _StockInicial, value); }
        public int Stock { get => _Stock; set { if (SetProperty(ref _Stock, value)) { OnPropertyChanged(nameof(stockVsMinimo)); } } }
        public Double PrecioIngreso { get => _PrecioIngreso; set => SetProperty(ref _PrecioIngreso, Math.Round(value, 2)); }
        public Double PrecioActual { get => _PrecioActual; set => SetProperty(ref _PrecioActual, Math.Round(value, 2)); }

        public string Codigo { get => _Codigo; set => SetProperty(ref _Codigo, value); }
        public string Descripcion { get => _Descripcion; set => SetProperty(ref _Descripcion, value); }

        public bool Activo { get => _Activo; set => SetProperty(ref _Activo, value); }

        public int? FechaModificadoID { get; set; }
        public virtual fechasModel FechaModificado { get => _FechaModificado; set => SetProperty(ref _FechaModificado, value); }

        public int TagID { get; set; }
        public virtual tagsModel Tag { get => _Tag; set { if (SetProperty(ref _Tag, value)) { OnPropertyChanged(nameof(stockVsMinimo)); } } }

        public int MedidaID { get; set; }
        public virtual medidasModel Medida { get => _Medida; set => SetProperty(ref _Medida, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<sacadoProductosModel> SacadoProductosPerProducto { get; private set; } = new ObservableCollection<sacadoProductosModel>();
        public virtual ICollection<ingresoProductosModel> IngresoProductosPerProducto { get; private set; } = new ObservableCollection<ingresoProductosModel>();
        public virtual ICollection<ventaProductosModel> VentaProductosPerProducto { get; private set; } = new ObservableCollection<ventaProductosModel>();
        public virtual ICollection<consumoProductoModel> ConsumoProductosPerProducto { get; private set; } = new ObservableCollection<consumoProductoModel>();
        public virtual ICollection<modificadoProductoModel> ModificadoProductosPerProducto { get; private set; } = new ObservableCollection<modificadoProductoModel>();

        [InverseProperty(nameof(abiertoProductosModel.ProductoSacado))]
        public virtual ICollection<abiertoProductosModel> AbiertoSacadoPerProducto { get; private set; } = new ObservableCollection<abiertoProductosModel>();
        [InverseProperty(nameof(abiertoProductosModel.ProductoAgregado))]
        public virtual ICollection<abiertoProductosModel> AbiertoAgregadoPerProducto { get; private set; } = new ObservableCollection<abiertoProductosModel>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        public int stockVsMinimo => Stock < 1 ? 1 : Stock < Tag.Minimo ? 2 : Stock == Tag.Minimo ? 3 : 4;
        [NotMapped]
        public bool Agregado { get => _Agregado; set { if (_Agregado != value) { _Agregado = value; OnPropertyChanged(); } } }
        #endregion // NotMapped
    }
}
