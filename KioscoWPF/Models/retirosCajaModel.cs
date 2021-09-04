using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF
{
    public class retirosCajaModel : Base.PropertyChangedBase
    {
        #region Private
        String _Hora; cajaModel _Caja; string _Detalle; bool _Pendiente; motivoRetirosModel _Motivo; fechasModel _Fecha; usuariosModel _UsuarioAutoriza, _UsuarioRetira; proveedoresModel _Proveedor;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int CajaID { get; set; }
        public virtual cajaModel Caja { get => _Caja; set => SetProperty(ref _Caja, value); }

        public String Hora { get => _Hora; set => SetProperty(ref _Hora, Convert.ToDateTime(value).ToString("HH:mm:ss")); }
        public string Detalle { get => _Detalle; set => SetProperty(ref _Detalle, value); }

        public bool Pendiente { get => _Pendiente; set => SetProperty(ref _Pendiente, value); }

        public int MotivoID { get; set; }
        public virtual motivoRetirosModel Motivo { get => _Motivo; set => SetProperty(ref _Motivo, value); }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set => SetProperty(ref _Fecha, value); }

        public int? ProveedorID { get; set; }
        public virtual proveedoresModel Proveedor { get => _Proveedor; set => SetProperty(ref _Proveedor, value); }

        public int UsuarioAutorizaID { get; set; }
        public virtual usuariosModel UsuarioAutoriza { get => _UsuarioAutoriza; set => SetProperty(ref _UsuarioAutoriza, value); }

        public int UsuarioRetiraID { get; set; }
        public virtual usuariosModel UsuarioRetira { get => _UsuarioRetira; set => SetProperty(ref _UsuarioRetira, value); }
        #endregion // Public
    }
}
