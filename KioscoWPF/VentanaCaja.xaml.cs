using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaCaja.xaml
    /// </summary>
    public partial class VentanaCaja : Window
    {
        public VentanaCaja(bool sentBol = false) { InitializeComponent(); try { (DataContext as ViewModels.VMCaja).setInitialize(this, sentBol); } catch { } }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMCaja : helperObservableClass
    {
        #region Initialize
        VentanaCaja thisWindow;

        public void setInitialize(VentanaCaja tempWindow, bool tempBol)
        {
            thisWindow = tempWindow; selectedCaja.Salida = tempBol;
        }
        #endregion // Initialize


        #region Variables
        int _BillUno, _BillDos, _BillCinco, _BillDiez, _BillVeinte, _BillCincuenta, _BillCien, _BillDoscientos, _BillQuinientos, _BillMil, _BillDosMil, _BillCincoMil, _BillDiezMil;

        public int BillUno { get => _BillUno; set { if (_BillUno != value) { _BillUno = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosUno)); helperUpdatePesos(); } } }
        public Double PesosUno => BillUno;
        public int BillDos { get => _BillDos; set { if (_BillDos != value) { _BillDos = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosDos)); helperUpdatePesos(); } } }
        public Double PesosDos => BillDos * 2;
        public int BillCinco { get => _BillCinco; set { if (_BillCinco != value) { _BillCinco = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosCinco)); helperUpdatePesos(); } } }
        public Double PesosCinco => BillCinco * 5;
        public int BillDiez { get => _BillDiez; set { if (_BillDiez != value) { _BillDiez = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosDiez)); helperUpdatePesos(); } } }
        public Double PesosDiez => BillDiez * 10;
        public int BillVeinte { get => _BillVeinte; set { if (_BillVeinte != value) { _BillVeinte = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosVeinte)); helperUpdatePesos(); } } }
        public Double PesosVeinte => BillVeinte * 20;
        public int BillCincuenta { get => _BillCincuenta; set { if (_BillCincuenta != value) { _BillCincuenta = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosCincuenta)); helperUpdatePesos(); } } }
        public Double PesosCincuenta => BillCincuenta * 50;
        public int BillCien { get => _BillCien; set { if (_BillCien != value) { _BillCien = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosCien)); helperUpdatePesos(); } } }
        public Double PesosCien => BillCien * 100;
        public int BillDoscientos { get => _BillDoscientos; set { if (_BillDoscientos != value) { _BillDoscientos = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosDoscientos)); helperUpdatePesos(); } } }
        public Double PesosDoscientos => BillDoscientos * 200;
        public int BillQuinientos { get => _BillQuinientos; set { if (_BillQuinientos != value) { _BillQuinientos = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosQuinientos)); helperUpdatePesos(); } } }
        public Double PesosQuinientos => BillQuinientos * 500;
        public int BillMil { get => _BillMil; set { if (_BillMil != value) { _BillMil = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosMil)); helperUpdatePesos(); } } }
        public Double PesosMil => BillMil * 1000;
        public int BillDosMil { get => _BillDosMil; set { if (_BillDosMil != value) { _BillDosMil = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosDosMil)); helperUpdatePesos(); } } }
        public Double PesosDosMil => BillDosMil * 2000;
        public int BillCincoMil { get => _BillCincoMil; set { if (_BillCincoMil != value) { _BillCincoMil = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosCincoMil)); helperUpdatePesos(); } } }
        public Double PesosCincoMil => BillCincoMil * 5000;
        public int BillDiezMil { get => _BillDiezMil; set { if (_BillDiezMil != value) { _BillDiezMil = value; OnPropertyChanged(); OnPropertyChanged(nameof(PesosDiezMil)); helperUpdatePesos(); } } }
        public Double PesosDiezMil => BillDiezMil * 10000;

        public Double pesosTotal => PesosUno + PesosDos + PesosCinco + PesosDiez + PesosVeinte + PesosCincuenta + PesosCien + PesosDoscientos + PesosQuinientos + PesosMil + PesosDosMil + PesosCincoMil;

        public Double Diferencia => pesosTotal - selectedCaja.Caja.CajaActual;

        public DBCajaConteosClass selectedCaja { get; } = new DBCajaConteosClass() { Caja = new DBCajaClass() { CajaActual = Db.globalCajaActual.CajaActual, Fecha = Db.returnFecha(), Hora = Variables.strHora }, Usuario = Variables.UsuarioLogueado, UsuarioID = Variables.UsuarioLogueado.Id };

        void helperUpdatePesos()
        {
            OnPropertyChanged(nameof(pesosTotal));
            OnPropertyChanged(nameof(Diferencia));
        }
        #endregion // Variables


        #region Helpers
        void helperSaltear()
        {
            if (!selectedCaja.Salida) { vPrincipal = new VentanaPrincipal(); vPrincipal.Show(); }
            else { vLogIn = new VentanaLogIn(); vLogIn.Show(); }

            thisWindow.Close();
        }

        void helperGuardar()
        {
            selectedCaja.Caja.MercadoPago = pesosTotal; selectedCaja.Caja.Vuelto = Diferencia;
            Variables.Inventario.CajaConteos.Local.Add(selectedCaja);
            _ = Variables.Inventario.SaveChanges();

            if (!selectedCaja.Salida) { vPrincipal = new VentanaPrincipal(); vPrincipal.Show(); } else { vLogIn = new VentanaLogIn(); vLogIn.Show(); }

            thisWindow.Close();
        }

        bool checkGuardar => selectedCaja != null && BillCien >= 0 && BillCinco >= 0 && BillCincoMil >= 0 && BillCincuenta >= 0 && BillDiez >= 0 && BillDiezMil >= 0 && BillDos >= 0 && BillDosMil >= 0 && BillMil >= 0 && BillQuinientos >= 0 && BillUno >= 0 && BillVeinte >= 0;

        bool checkLastUser()
        {
            DBCajaConteosClass tempCaja = null;
            try { tempCaja = Variables.Inventario.CajaConteos.Local.Single(x => x.UsuarioID == Variables.UsuarioLogueado.Id && x.Caja.Fecha.Fecha == Variables.strFecha && x.Salida == selectedCaja.Salida); }
            catch { }

            return tempCaja != null || Variables.UsuarioLogueado.Nivel < 3;
        }
        #endregion // Helpers



        #region Commands
        public Command comSaltear => new Command((object parameter) => helperSaltear(), (object parameter) => checkLastUser());

        public Command comGuardar => new Command((object parameter) => helperGuardar(), (object parameter) => checkGuardar);
        #endregion // Commands
    }
}
