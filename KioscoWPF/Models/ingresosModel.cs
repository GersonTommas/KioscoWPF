using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace KioscoWPF
{
    public class ingresosModel : Base.PropertyChangedBase
    {
        #region Private
        Double _PagadoPesos, _PagadoMP; String _Hora; string _Detalle; proveedoresModel _Proveedor; usuariosModel _Usuario; fechasModel _Fecha;
        #endregion // Private


        #region Public
        public int Id { get; set; }

        public Double PagadoPesos { get => _PagadoPesos; set => SetProperty(ref _PagadoPesos, Math.Round(value, 2)); }
        public Double PagadoMP { get => _PagadoMP; set => SetProperty(ref _PagadoMP, Math.Round(value, 2)); }

        public String Hora { get => _Hora; set => SetProperty(ref _Hora, Convert.ToDateTime(value).ToString("HH:mm:ss")); }
        public string Detalle { get => _Detalle; set => SetProperty(ref _Detalle, value); }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set => SetProperty(ref _Usuario, value); }

        public int ProveedorID { get; set; }
        public virtual proveedoresModel Proveedor { get => _Proveedor; set => SetProperty(ref _Proveedor, value); }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set => SetProperty(ref _Fecha, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<ingresoProductosModel> IngresoProductosPerIngreso { get; private set; } = new ObservableCollection<ingresoProductosModel>();
        #endregion // Navigation


        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(IngresoProductosPerIngreso.Sum(x => x.PrecioTotal), 2);
        [NotMapped]
        public int ingresosCantidadProductosPerIngreso => IngresoProductosPerIngreso.Count();
        #endregion // NotMapped
    }
}
