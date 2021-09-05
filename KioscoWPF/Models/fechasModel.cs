using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Windows.Data;

namespace KioscoWPF
{
    public class fechasModel : Base.ModelBase
    {
        #region Private
        String _Fecha;
        #endregion // Private

        #region Public
        public String Fecha { get => _Fecha; set { if (SetProperty(ref _Fecha, Convert.ToDateTime(value).ToString("yyyy/MM/dd"))) { OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<cajaModel> CajasPerFecha { get; private set; } = new ObservableCollection<cajaModel>();
        public virtual ICollection<productosModel> ProductosModificadosPerFecha { get; private set; } = new ObservableCollection<productosModel>();
        public virtual ICollection<ingresosModel> IngresosPerFecha { get; private set; } = new ObservableCollection<ingresosModel>();
        public virtual ICollection<ventasModel> VentasPerFecha { get; private set; } = new ObservableCollection<ventasModel>();
        public virtual ICollection<consumoProductoModel> ConsumosProductosPerFecha { get; private set; } = new ObservableCollection<consumoProductoModel>();
        public virtual ICollection<modificadoProductoModel> ModificadosProductosPerFecha { get; private set; } = new ObservableCollection<modificadoProductoModel>();
        public virtual ICollection<abiertoProductosModel> AbiertoProductosPerFecha { get; private set; } = new ObservableCollection<abiertoProductosModel>();
        public virtual ICollection<retirosCajaModel> RetirosPerFecha { get; private set; } = new ObservableCollection<retirosCajaModel>();
        //public virtual ICollection<DBDeudorPagoClass> DeudorPagosPerFecha { get; private set; } = new ObservableCollection<DBDeudorPagoClass>();

        public virtual ICollection<ventaProductosModel> VentaProductosPagadosPerFecha { get; private set; } = new ObservableCollection<ventaProductosModel>();

        [InverseProperty(nameof(sacadoProductosModel.FechaSacado))]
        public virtual ICollection<sacadoProductosModel> SacadoProductosSacadosPerFecha { get; private set; } = new ObservableCollection<sacadoProductosModel>();
        [InverseProperty(nameof(sacadoProductosModel.FechaPagado))]
        public virtual ICollection<sacadoProductosModel> SacadoProductosPagadosPerFecha { get; private set; } = new ObservableCollection<sacadoProductosModel>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        CollectionViewSource ventasPerFechaViewSource;
        [NotMapped]
        public ICollectionView ventasPerFechaView
        {
            get
            {
                if (ventasPerFechaViewSource == null) { ventasPerFechaViewSource = new CollectionViewSource() { Source = VentasPerFecha }; }
                ICollectionView temp = ventasPerFechaViewSource.View;
                temp.SortDescriptions.Clear(); temp.SortDescriptions.Add(new SortDescription("Hora", ListSortDirection.Descending));
                return temp;
            }
        }


        [NotMapped]
        public int TotalCantidadVentasDiario => VentasPerFecha.Count();
        [NotMapped]
        public Double TotalPesosVentasDiario => VentasPerFecha.Sum(x => x.PrecioTotal);

        [NotMapped]
        public int TotalCantidadConsumosDiario => ConsumosProductosPerFecha.Count();

        public void updateTotalVentasDiario()
        {
            OnPropertyChanged(nameof(TotalCantidadVentasDiario));
            OnPropertyChanged(nameof(TotalPesosVentasDiario));
        }
        public void updateTotalConsumosDiario()
        {
            OnPropertyChanged(nameof(TotalCantidadConsumosDiario));
        }

        public void updateThis()
        {
            OnPropertyChanged(nameof(fechasModel));
        }
        #endregion // NotMapped

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
