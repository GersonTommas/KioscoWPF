using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaPagarDeuda.xaml
    /// </summary>
    public partial class VentanaPagarDeuda : Window
    {
        public VentanaPagarDeuda(deudoresModel sentDeudor)
        { InitializeComponent(); if (sentDeudor == null) { Close(); } (DataContext as ViewModels.VMPagarDeuda).setInitialize(this, sentDeudor); }

    }
}

namespace KioscoWPF.ViewModels
{
    class VMPagarDeuda : helperObservableClass
    {
        #region Initialize
        VentanaPagarDeuda thisWindow;

        public void setInitialize(VentanaPagarDeuda tempWindow, deudoresModel tempDeudor)
        {
            thisWindow = tempWindow; selectedDeudor = tempDeudor;
        }
        #endregion // Initialize



        #region Variables
        readonly cajaModel _selectedNewCaja = new cajaModel() { Fecha = Db.returnFecha(), Hora = Variables.strHora };

        deudoresModel _selectedDeudor;
        public deudoresModel selectedDeudor { get => _selectedDeudor; set { if (_selectedDeudor != value) { _selectedDeudor = value; OnPropertyChanged(); OnPropertyChanged(nameof(_listDeuda)); OnPropertyChanged(nameof(doubleQuedaACuenta)); OnPropertyChanged(nameof(sumFaltante)); OnPropertyChanged(nameof(sumDeuda)); } } }

        Double _doubleEfectivo = 0;
        public Double doubleEfectivo { get => _doubleEfectivo; set { if (_doubleEfectivo != value) { _doubleEfectivo = value; OnPropertyChanged(); OnPropertyChanged(nameof(doubleQuedaACuenta)); OnPropertyChanged(nameof(isVuelto)); } } }

        Double _doubleMercadoPago = 0;
        public Double doubleMercadoPago { get => _doubleMercadoPago; set { if (_doubleMercadoPago != value) { _doubleMercadoPago = value; OnPropertyChanged(); OnPropertyChanged(nameof(doubleQuedaACuenta)); OnPropertyChanged(nameof(isVuelto)); } } }

        public Double doubleQuedaACuenta => selectedDeudor != null ? sumDeuda - (doubleEfectivo + doubleMercadoPago + selectedDeudor.Resto) : 0;

        IEnumerable<ventaProductosModel> _listDeuda => selectedDeudor?.VentaProductosPerDeudor.Where(x => x.BolPagado == false);

        public Double sumDeuda => _listDeuda != null ? _listDeuda.Sum(x => x.TotalFaltante) : 0;
        public Double sumFaltante => selectedDeudor != null ? sumDeuda - selectedDeudor.Resto : 0;

        public bool isVuelto => doubleQuedaACuenta < 0;

        #endregion // Variables


        #region Helpers
        public void helperGuardar()
        {
            _selectedNewCaja.CajaActual = doubleEfectivo; _selectedNewCaja.MercadoPago = doubleMercadoPago;
            Double tempTotalPlata = selectedDeudor.Resto + _selectedNewCaja.CajaActual + _selectedNewCaja.MercadoPago;

            bool tempBolAgregarCajaSola = true;
            bool tempBolACuenta = true;
            if (doubleQuedaACuenta < 0) { tempBolACuenta = false; }

            foreach (ventaProductosModel deuda in _listDeuda)
            {
                bool tempBoolAgregarCaja = false;

                if (tempTotalPlata >= deuda.PrecioFinal)
                {
                    for (int i = 0; i < deuda.CantidadFaltante; i++)
                    {
                        if (tempTotalPlata >= deuda.PrecioFinal)
                        {
                            tempTotalPlata -= deuda.PrecioFinal;
                            i -= 1;
                            deuda.CantidadFaltante -= 1;
                            tempBoolAgregarCaja = true;
                            tempBolAgregarCajaSola = false;

                            deuda.PrecioPagado = deuda.PrecioPagado == 0 ? deuda.PrecioFinal : Math.Round((deuda.PrecioPagado + deuda.PrecioFinal) / 2, 2);
                        }
                    }
                }
                if (tempBoolAgregarCaja) { deuda.CajasPerVentaProducto.Add(_selectedNewCaja); }
            }

            if (tempBolAgregarCajaSola) { Variables.Inventario.Caja.Local.Add(_selectedNewCaja); }

            if (tempBolACuenta) { selectedDeudor.Resto = tempTotalPlata; } else { selectedDeudor.Resto = 0; _selectedNewCaja.Vuelto = tempTotalPlata; }
            Db.contabilizarCaja(_selectedNewCaja);
            _ = Variables.Inventario.SaveChanges();
            thisWindow.DialogResult = true;
        }

        public bool helperCheckPassword()
        {
            return false;
        }
        #endregion // Helpers


        #region Commands
        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);

        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => doubleEfectivo > 0 || doubleMercadoPago > 0);
        #endregion // Commands
    }
}