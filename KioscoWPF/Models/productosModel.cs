using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace KioscoWPF
{
    public class productosModel : Base.ModelBase
    {
        #region Private
        int _StockInicial, _Stock; Double _PrecioActual, _PrecioIngreso; string _Codigo, _Descripcion; fechasModel _FechaModificado; bool _Activo; tagsModel _Tag; medidasModel _Medida;
        bool _Agregado = false;
        #endregion // Private

        #region Public
        public int StockInicial { get => _StockInicial; set { if (SetProperty(ref _StockInicial, value)) { OnPropertyChanged(); } } }
        public int Stock { get => _Stock; set { if (SetProperty(ref _Stock, value)) { OnPropertyChanged(); } } }
        public Double PrecioIngreso { get => _PrecioIngreso; set { if (SetProperty(ref _PrecioIngreso, Math.Round(value, 2))) { OnPropertyChanged(); } } }
        public Double PrecioActual { get => _PrecioActual; set { if (SetProperty(ref _PrecioActual, Math.Round(value, 2))) { OnPropertyChanged(); } } }

        public string Codigo { get => _Codigo; set { if (SetProperty(ref _Codigo, value)) { OnPropertyChanged(); } } }
        public string Descripcion { get => _Descripcion; set { if (SetProperty(ref _Descripcion, value)) { OnPropertyChanged(); } } }

        public bool Activo { get => _Activo; set { if (SetProperty(ref _Activo, value)) { OnPropertyChanged(); } } }

        public int? FechaModificadoID { get; set; }
        public virtual fechasModel FechaModificado { get => _FechaModificado; set { if (SetProperty(ref _FechaModificado, value)) { OnPropertyChanged(); } } }

        public int TagID { get; set; }
        public virtual tagsModel Tag { get => _Tag; set { if (SetProperty(ref _Tag, value)) { OnPropertyChanged(); OnPropertyChanged(nameof(stockVsMinimo)); } } }

        public int MedidaID { get; set; }
        public virtual medidasModel Medida { get => _Medida; set { if (SetProperty(ref _Medida, value)) { OnPropertyChanged(); } } }
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

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
