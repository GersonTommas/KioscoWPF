using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace KioscoWPF
{
    public class proveedoresModel : Base.PropertyChangedBase
    {
        #region Private
        int? _Telefono, _Celular; string _Nombre, _Direccion, _NumeroDeCliente, _Detalles; bool _Activo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public string Nombre { get => _Nombre; set => SetProperty(ref _Nombre, value); }
        public string Direccion { get => _Direccion; set => SetProperty(ref _Direccion, value); }
        public string NumeroDeCliente { get => _NumeroDeCliente; set => SetProperty(ref _NumeroDeCliente, value); }
        public string Detalles { get => _Detalles; set => SetProperty(ref _Detalles, value); }
        public int? Telefono { get => _Telefono; set => SetProperty(ref _Telefono, value); }
        public int? Celular { get => _Celular; set => SetProperty(ref _Celular, value); }
        public bool Activo { get => _Activo; set => SetProperty(ref _Activo, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<ingresosModel> IngresosPerProveedor { get; private set; } = new ObservableCollection<ingresosModel>();
        public virtual ICollection<retirosCajaModel> RetirosCajaPerProveedor { get; private set; } = new ObservableCollection<retirosCajaModel>();
        #endregion // Navigation
    }
}
