using System;
using System.Collections.Generic;
using System.Text;

namespace Kiosco.WPF
{
    public class jornadaModel : Base.ModelBase
    {
        #region Private
        int _HorasTrabajadas, _HorasExtra; String _AñoMes; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int HorasTrabajadas { get => _HorasTrabajadas; set { if (SetProperty(ref _HorasTrabajadas, value)) { OnPropertyChanged(); } } }
        public int HorasExtra { get => _HorasExtra; set { if (SetProperty(ref _HorasExtra, value)) { OnPropertyChanged(); } } }

        public String AñoMes { get => _AñoMes; set { if (SetProperty(ref _AñoMes, Convert.ToDateTime(value).ToString("yyyy/MM"))) { OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (SetProperty(ref _Usuario, value)) { OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        #endregion // Navigation

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
