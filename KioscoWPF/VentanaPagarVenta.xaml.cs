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

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaPagarVenta.xaml
    /// </summary>
    public partial class VentanaPagarVenta : Window
    {
        public Double resultPagadoPesos = 0;
        public Double resultMercadoPago = 0;
        public Double resultVuelto = 0;
        public bool resultPagarDeuda = false;
        public DBDeudoresClass resultDeudor = null;

        public VentanaPagarVenta(Double sentTotal)
        {
            InitializeComponent(); try { (DataContext as ViewModels.VMPagarVenta).setInitialize(this, sentTotal); } catch { }
        }
    }
}

namespace KioscoWPF.ViewModels
{
    class VMPagarVenta : helperObservableClass
    {
        #region Initialize
        VentanaPagarVenta thisWindow;

        public void setInitialize(VentanaPagarVenta tempWindow, Double tempTotal)
        {
            thisWindow = tempWindow; doubleTotal = tempTotal;

            _listDeudoresSource.Source = Variables.Inventario.Deudores.Local.ToObservableCollection(); OnPropertyChanged(nameof(listDeudores)); selectedDeudor = null;

            listDeudores.SortDescriptions.Clear(); listDeudores.SortDescriptions.Add(new SortDescription("Nombre", ListSortDirection.Ascending));
        }
        #endregion // Initialize



        #region Variables

        Double _doubleTotal;
        public Double doubleTotal { get => _doubleTotal; set { if (_doubleTotal != value) { _doubleTotal = value; OnPropertyChanged(); } } }

        Double _doublePagadoPesos;
        public Double doublePagadoPesos { get => _doublePagadoPesos; set { if (_doublePagadoPesos != value) { _doublePagadoPesos = value; OnPropertyChanged(); updateEverything(); } } }

        Double _doubleMercadoPago;
        public Double doubleMercadoPago { get => _doubleMercadoPago; set { if (_doubleMercadoPago != value) { _doubleMercadoPago = value; OnPropertyChanged(); updateEverything(); } } }

        bool _bolDeudor;
        public bool bolDeudor { get => _bolDeudor; set { if (_bolDeudor != value) { _bolDeudor = value; OnPropertyChanged(); bolSinVuelto = false; updateEverything(); selectedDeudor = null; } } }

        bool _bolSinVuelto;
        public bool bolSinVuelto { get => _bolSinVuelto; set { if (_bolSinVuelto != value) { _bolSinVuelto = value; OnPropertyChanged(); updateEverything(); } } }

        DBDeudoresClass _selectedDeudor = null;
        public DBDeudoresClass selectedDeudor { get => _selectedDeudor; set { if (_selectedDeudor != value) { _selectedDeudor = value; OnPropertyChanged(); updateEverything(); } } }


        public string windowBackground => doubleVuelto >= 0 || (bolDeudor && selectedDeudor != null) ? Variables.colorWindowBackgroundOK : Variables.colorWindowBackkgroundNO;
        public Double doubleVuelto => !bolSinVuelto ? doublePagadoPesos + doubleMercadoPago - doubleTotal : selectedDeudor != null ? doublePagadoPesos + doubleMercadoPago - (doubleTotal + doubleDeudaTotal) : 0;
        public string strVueltoDeuda => bolSinVuelto && doubleVuelto <= 0 ? "Deuda Restante:" : "Vuelto:";
        public Double doubleDeudaTotal => selectedDeudor != null ? selectedDeudor.doubleDeudaTotal - selectedDeudor.Resto : 0;


        readonly CollectionViewSource _listDeudoresSource = new CollectionViewSource();
        public ICollectionView listDeudores => _listDeudoresSource.View;
        #endregion // Variables



        #region Helpers

        void updateEverything()
        {
            OnPropertyChanged(nameof(doubleVuelto));
            OnPropertyChanged(nameof(strVueltoDeuda));
            OnPropertyChanged(nameof(windowBackground));
        }

        void helperGuardar()
        {
            thisWindow.resultDeudor = bolDeudor ? selectedDeudor : null;
            thisWindow.resultMercadoPago = doubleMercadoPago;
            thisWindow.resultPagadoPesos = doublePagadoPesos;
            thisWindow.resultVuelto = doubleVuelto;
            thisWindow.resultPagarDeuda = bolSinVuelto;
            thisWindow.DialogResult = true;
        }
        #endregion // Helpers



        #region Commands
        public Command comGuardar => new Command(
            (object parameter) => helperGuardar(),
            (object parameter) => doubleVuelto >= -1 || (bolDeudor && selectedDeudor != null));

        public Command comNuevoDeudor => new Command((object parameter) => abrirAgregarDeudor());

        public Command comCancelar => new Command((object parameter) => thisWindow.DialogResult = false);
        #endregion // Commands
    }
}