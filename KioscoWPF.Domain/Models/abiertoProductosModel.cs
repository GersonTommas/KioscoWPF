using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace KioscoWPF.Domain
{
    public class abiertoProductosModel : Base.ModelBase
    {
        #region Private
        int _CantidadSacado, _CantidadAgregado; productosModel _ProductoSacado, _ProductoAgregado; fechasModel _Fecha; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int CantidadSacado { get => _CantidadSacado; set { if (SetProperty(ref _CantidadSacado, value)) { OnPropertyChanged(); } } }
        public int CantidadAgregado { get => _CantidadAgregado; set { if (SetProperty(ref _CantidadAgregado, value)) { OnPropertyChanged(); } } }

        public int ProductoSacadoID { get; set; }
        public virtual productosModel ProductoSacado { get => _ProductoSacado; set { if (SetProperty(ref _ProductoSacado, value)) { OnPropertyChanged(); } } }

        public int ProductoAgregadoID { get; set; }
        public virtual productosModel ProductoAgregado { get => _ProductoAgregado; set { if (SetProperty(ref _ProductoAgregado, value)) { OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (SetProperty(ref _Fecha, value)) { OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (SetProperty(ref _Usuario, value)) { OnPropertyChanged(); } } }
        #endregion // Public

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
