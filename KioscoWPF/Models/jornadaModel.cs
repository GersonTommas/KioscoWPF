using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF
{
    public class jornadaModel : Base.PropertyChangedBase
    {
        #region Private
        int _HorasTrabajadas, _HorasExtra; String _AñoMes; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int HorasTrabajadas { get => _HorasTrabajadas; set => SetProperty(ref _HorasTrabajadas, value); }
        public int HorasExtra { get => _HorasExtra; set => SetProperty(ref _HorasExtra, value); }

        public String AñoMes { get => _AñoMes; set => SetProperty(ref _AñoMes, Convert.ToDateTime(value).ToString("yyyy/MM")); }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set => SetProperty(ref _Usuario, value); }
        #endregion // Public

        #region Navigation
        #endregion // Navigation
    }
}
