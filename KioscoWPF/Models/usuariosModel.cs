using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF
{
    public class usuariosModel : Base.PropertyChangedBase
    {
        #region Private
        int _Nivel; Double _Resto; string _Nombre, _Apellido, _Detalle, _Contraseña, _Usuario; String _Fecha, _FechaSalida; bool _Activo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Nivel { get => _Nivel; set => SetProperty(ref _Nivel, value); }
        public Double Resto { get => _Resto; set => SetProperty(ref _Resto, Math.Round(value, 2)); }

        public string Nombre { get => _Nombre; set => SetProperty(ref _Nombre, value); }
        public string Apellido { get => _Apellido; set => SetProperty(ref _Apellido, value); }
        public string Usuario { get => _Usuario; set => SetProperty(ref _Usuario, value); }
        public string Detalle { get => _Detalle; set => SetProperty(ref _Detalle, value); }
        public string Contraseña { get => _Contraseña; set => SetProperty(ref _Contraseña, value); }

        public String FechaIngreso { get => _Fecha; set => SetProperty(ref _Fecha, Convert.ToDateTime(value).ToString("yyyy/MM/dd")); }
        public String FechaSalida { get => _FechaSalida; set => SetProperty(ref _FechaSalida, Convert.ToDateTime(value).ToString("yyyy/MM/dd")); }

        public bool Activo { get => _Activo; set => SetProperty(ref _Activo, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<cajaConteosModel> CajaConteosPerUsuario { get; private set; } = new ObservableCollection<cajaConteosModel>();
        public virtual ICollection<deudoresModel> DeudoresPerUsuario { get; private set; } = new ObservableCollection<deudoresModel>();
        public virtual ICollection<sacadoProductosModel> SacadoProductosPerUsuario { get; private set; } = new ObservableCollection<sacadoProductosModel>();
        public virtual ICollection<ingresosModel> IngresosPerUsuario { get; private set; } = new ObservableCollection<ingresosModel>();
        public virtual ICollection<ventasModel> VentasPerUsuario { get; private set; } = new ObservableCollection<ventasModel>();
        public virtual ICollection<modificadoProductoModel> ModificadoProductosPerUsuario { get; private set; } = new ObservableCollection<modificadoProductoModel>();
        public virtual ICollection<abiertoProductosModel> AbiertoProductosPerUsuario { get; private set; } = new ObservableCollection<abiertoProductosModel>();

        [InverseProperty(nameof(retirosCajaModel.UsuarioAutoriza))]
        public virtual ICollection<retirosCajaModel> RetirosAutorizaPerUsuario { get; private set; } = new ObservableCollection<retirosCajaModel>();
        [InverseProperty(nameof(retirosCajaModel.UsuarioRetira))]
        public virtual ICollection<retirosCajaModel> RetirosRetiraPerUsuario { get; private set; } = new ObservableCollection<retirosCajaModel>();
        #endregion // Navigation
    }
}
