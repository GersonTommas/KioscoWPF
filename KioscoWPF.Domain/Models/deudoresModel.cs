using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace KioscoWPF.Domain
{
    public class deudoresModel : Base.ModelBase
    {
        #region Private
        int _Nivel; Double _Resto; string _Nombre, _Detalles; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Nivel { get => _Nivel; set { if (SetProperty(ref _Nivel, value)) { OnPropertyChanged(); } } }
        public Double Resto { get => _Resto; set { if (SetProperty(ref _Resto, Math.Round(value, 2))) { OnPropertyChanged(); OnPropertyChanged(nameof(doubleFaltanteTotal)); } } }

        public string Nombre { get => _Nombre; set { if (SetProperty(ref _Nombre, value)) { OnPropertyChanged(); } } }

        public string Detalles { get => _Detalles; set { if (SetProperty(ref _Detalles, value)) { OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (SetProperty(ref _Usuario, value)) { OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<ventasModel> VentasPerDeudor { get; private set; } = new ObservableCollection<ventasModel>();
        public virtual ICollection<ventaProductosModel> VentaProductosPerDeudor { get; private set; } = new ObservableCollection<ventaProductosModel>();
        //public virtual ICollection<DBDeudorPagoClass> DeudorPagosPerDeudor { get; private set; } = new ObservableCollection<DBDeudorPagoClass>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        public Double doubleDeudaTotal => Math.Round(VentasPerDeudor.Sum(x => x.DeudaTotalVenta), 2);
        [NotMapped]
        public Double doubleFaltanteTotal => Math.Round(doubleDeudaTotal - Resto, 2);

        [NotMapped]
        CollectionViewSource ventasProductosPendientesPerDeudorSource;
        [NotMapped]
        public ICollectionView ventasProductosPendientesPerDeudorView
        {
            get
            {
                if (ventasProductosPendientesPerDeudorSource == null) { ventasProductosPendientesPerDeudorSource = new CollectionViewSource() { Source = VentaProductosPerDeudor }; }
                ICollectionView temp = ventasProductosPendientesPerDeudorSource.View;
                temp.Filter = delegate (object item) { return (item as ventaProductosModel).BolPagado == false; };
                temp.SortDescriptions.Clear(); temp.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                return temp;
            }
        }

        public override void updateModel()
        {
            OnPropertyChanged(nameof(doubleDeudaTotal));
            OnPropertyChanged(nameof(doubleFaltanteTotal));
        }
        #endregion // NotMapped
    }
}
