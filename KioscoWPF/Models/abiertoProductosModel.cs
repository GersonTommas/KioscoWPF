using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF
{
    public class abiertoProductosModel : Base.PropertyChangedBase
    {
        #region Private
        int _CantidadSacado, _CantidadAgregado; productosModel _ProductoSacado, _ProductoAgregado; fechasModel _Fecha; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int CantidadSacado { get => _CantidadSacado; set => SetProperty(ref _CantidadSacado, value); }
        public int CantidadAgregado { get => _CantidadAgregado; set => SetProperty(ref _CantidadAgregado, value); }

        public int ProductoSacadoID { get; set; }
        public virtual productosModel ProductoSacado { get => _ProductoSacado; set => SetProperty(ref _ProductoSacado, value); }

        public int ProductoAgregadoID { get; set; }
        public virtual productosModel ProductoAgregado { get => _ProductoAgregado; set => SetProperty(ref _ProductoAgregado, value); }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set => SetProperty(ref _Fecha, value); }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set => SetProperty(ref _Usuario, value); }
        #endregion // Public
    }
}
