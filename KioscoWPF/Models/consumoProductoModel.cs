using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF
{
    public class consumoProductoModel : Base.PropertyChangedBase
    {
        #region Private
        int _Cantidad; Double _Precio; fechasModel _Fecha; productosModel _Producto;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set => SetProperty(ref _Cantidad, value); }

        public Double Precio { get => _Precio; set => SetProperty(ref _Precio, Math.Round(value, 2)); }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set => SetProperty(ref _Fecha, value); }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set => SetProperty(ref _Producto, value); }
        #endregion // Public
    }
}
