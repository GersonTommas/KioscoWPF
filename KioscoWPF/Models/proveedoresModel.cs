using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kiosco.WPF
{
    public class proveedoresModel : Base.ModelBase
    {
        #region Private
        int? _Telefono, _Celular; string _Nombre, _Direccion, _NumeroDeCliente, _Detalles; bool _Activo;
        #endregion // Private

        #region Public
        public string Nombre { get => _Nombre; set { if (SetProperty(ref _Nombre, value)) { OnPropertyChanged(); } } }
        public string Direccion { get => _Direccion; set { if (SetProperty(ref _Direccion, value)) { OnPropertyChanged(); } } }
        public string NumeroDeCliente { get => _NumeroDeCliente; set { if (SetProperty(ref _NumeroDeCliente, value)) { OnPropertyChanged(); } } }
        public string Detalles { get => _Detalles; set { if (SetProperty(ref _Detalles, value)) { OnPropertyChanged(); } } }
        public int? Telefono { get => _Telefono; set { if (SetProperty(ref _Telefono, value)) { OnPropertyChanged(); } } }
        public int? Celular { get => _Celular; set { if (SetProperty(ref _Celular, value)) { OnPropertyChanged(); } } }
        public bool Activo { get => _Activo; set { if (SetProperty(ref _Activo, value)) { OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<ingresosModel> IngresosPerProveedor { get; private set; } = new ObservableCollection<ingresosModel>();
        public virtual ICollection<retirosCajaModel> RetirosCajaPerProveedor { get; private set; } = new ObservableCollection<retirosCajaModel>();
        #endregion // Navigation

        public override void updateModel()
        {
            base.updateModel();
        }
    }
}
