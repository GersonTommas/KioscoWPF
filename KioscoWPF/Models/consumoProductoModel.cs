using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF
{
    public class consumoProductoModel : Base.ModelBase
    {
        #region Private
        int _Cantidad; Double _Precio; fechasModel _Fecha; productosModel _Producto;
        #endregion // Private

        #region Public
        public int Cantidad { get => _Cantidad; set { if (SetProperty(ref _Cantidad, value)) { OnPropertyChanged(); } } }

        public Double Precio { get => _Precio; set { if (SetProperty(ref _Precio, Math.Round(value, 2))) { OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (SetProperty(ref _Fecha, value)) { OnPropertyChanged(); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set { if (SetProperty(ref _Producto, value)) { OnPropertyChanged(); } } }
        #endregion // Public

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
