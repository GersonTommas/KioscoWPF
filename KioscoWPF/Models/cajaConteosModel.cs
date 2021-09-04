using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF
{
    public class cajaConteosModel : Base.PropertyChangedBase
    {
        #region Private
        Double _Diferencia; string _Detalle; bool _Salida; cajaModel _Caja; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public Double Diferencia { get => _Diferencia; set => SetProperty(ref _Diferencia, Math.Round(value, 2)); }

        public string Detalle { get => _Detalle; set => SetProperty(ref _Detalle, value); }

        public bool Salida { get => _Salida; set => SetProperty(ref _Salida, value); }

        public int CajaID { get; set; }
        public virtual cajaModel Caja { get => _Caja; set => SetProperty(ref _Caja, value); }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set => SetProperty(ref _Usuario, value); }
        #endregion // Public
    }
}
