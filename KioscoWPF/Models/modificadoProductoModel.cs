using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF
{
    public class modificadoProductoModel : Base.PropertyChangedBase
    {
        #region Private
        int _Cantidad; productosModel _Producto; fechasModel _Fecha; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set { if (SetProperty(ref _Cantidad, value)) { OnPropertyChanged(nameof(stockFinal)); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set => SetProperty(ref _Producto, value); }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set => SetProperty(ref _Fecha, value); }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set => SetProperty(ref _Usuario, value); }
        #endregion // Public

        [NotMapped]
        public int stockFinal => Producto != null ? Producto.Stock + Cantidad : 0;

        public void updateItem()
        {
            OnPropertyChanged(nameof(stockFinal));
        }
    }
}
