using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF.Domain
{
    public class usuariosModel : Base.ModelBase
    {
        #region Private
        int _Nivel; Double _Resto; string _Nombre, _Apellido, _Detalle, _Contraseña, _Usuario; String _Fecha, _FechaSalida; bool _Activo;
        #endregion // Private

        #region Public
        public int Nivel { get => _Nivel; set { if (SetProperty(ref _Nivel, value)) { OnPropertyChanged(); } } }
        public Double Resto { get => _Resto; set { if (SetProperty(ref _Resto, Math.Round(value, 2))) { OnPropertyChanged(); } } }

        public string Nombre { get => _Nombre; set { if (SetProperty(ref _Nombre, value)) { OnPropertyChanged(); } } }
        public string Apellido { get => _Apellido; set { if (SetProperty(ref _Apellido, value)) { OnPropertyChanged(); } } }
        public string Usuario { get => _Usuario; set { if (SetProperty(ref _Usuario, value)) { OnPropertyChanged(); } } }
        public string Detalle { get => _Detalle; set { if (SetProperty(ref _Detalle, value)) { OnPropertyChanged(); } } }
        public string Contraseña { get => _Contraseña; set { if (SetProperty(ref _Contraseña, value)) { OnPropertyChanged(); } } }

        public String FechaIngreso { get => _Fecha; set { if (SetProperty(ref _Fecha, Convert.ToDateTime(value).ToString("yyyy/MM/dd"))) { OnPropertyChanged(); } } }
        public String FechaSalida { get => _FechaSalida; set { if (SetProperty(ref _FechaSalida, Convert.ToDateTime(value).ToString("yyyy/MM/dd"))) { OnPropertyChanged(); } } }

        public bool Activo { get => _Activo; set { if (SetProperty(ref _Activo, value)) { OnPropertyChanged(); } } }
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

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
