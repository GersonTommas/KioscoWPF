using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.ObjectModel;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for VentanaPrincipal.xaml
    /// </summary>
    public partial class VentanaPrincipal : Window
    {
        public VentanaPrincipal()
        { InitializeComponent(); Application.Current.MainWindow = this; Variables.globalVentanaPrincipal = this; try { (DataContext as ViewModels.VMVentanaPrincipal).thisWindow = this; } catch { } }

        protected override void OnClosing(CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosing(e);
        }

        public void updateDeudores()
        {
            (DataContext as ViewModels.VMVentanaPrincipal).updateDeudores();
        }
    }
}


namespace KioscoWPF.ViewModels
{


    #region Binding
    public class VMVentanaPrincipal : helperObservableClass
    {
        #region Initialize
        public VentanaPrincipal thisWindow;

        public VMVentanaPrincipal()
        {
            try
            {
                initilizeClock();
                initilizeSearchTimer();

                globalFiltersSorters();
                stockFiltersSorters();
                ventasFiltersSorters();
                ingresosFiltersSorters();
                consumosFiltersSorters();
                deudasFiltersSorters();
                tagsFiltersSorters();
                medidasFiltersSorters();
                retirosFiltersSorters();
                cajaFiltersSorters();
                proveedoresFiltersSorters();
                sacadoFiltersSorters();
            }
            catch { }
        }
        #endregion // Initialize

        #region Clock
        readonly System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        void initilizeClock() { Timer.Tick += new EventHandler(Timer_Click); Timer.Interval = new TimeSpan(0, 0, 1); Timer.Start(); }
        void Timer_Click(object sender, EventArgs e) { strClock = DateTime.Now.ToString("HH:mm:ss"); }
        string _strClock = "00:00:00";
        public string strClock { get => _strClock; set { if (_strClock != value) { _strClock = value; OnPropertyChanged(nameof(strClock)); } } }
        #endregion // Clock


        #region Menu
        bool _menuViewDeudas = true;
        bool _menuViewVentas = true;
        bool _menuViewTags = true;
        bool _menuViewUsuarios = true;
        bool _menuViewMedidas = true;
        bool _menuViewConsumo = true;
        bool _menuViewSacado = false;
        bool _menuViewIngresos = true;
        bool _menuViewCaja = true;



        public bool menuViewDeudas { get => _menuViewDeudas; set { if (_menuViewDeudas != value) { _menuViewDeudas = value; OnPropertyChanged(); } } }
        public bool menuViewVentas { get => _menuViewVentas; set { if (_menuViewVentas != value) { _menuViewVentas = value; OnPropertyChanged(); } } }
        public bool menuViewTags { get => _menuViewTags; set { if (_menuViewTags != value) { _menuViewTags = value; OnPropertyChanged(); } } }
        public bool menuViewUsuarios { get => _menuViewUsuarios; set { if (_menuViewUsuarios != value) { _menuViewUsuarios = value; OnPropertyChanged(); } } }
        public bool menuViewMedidas { get => _menuViewMedidas; set { if (_menuViewMedidas != value) { _menuViewMedidas = value; OnPropertyChanged(); } } }
        public bool menuViewConsumo { get => _menuViewConsumo; set { if (_menuViewConsumo != value) { _menuViewConsumo = value; OnPropertyChanged(); } } }
        public bool menuViewSacado { get => _menuViewSacado; set { if (_menuViewSacado != value) { _menuViewSacado = value; OnPropertyChanged(); } } }
        public bool menuViewIngresos { get => _menuViewIngresos; set { if (_menuViewIngresos != value) { _menuViewIngresos = value; OnPropertyChanged(); } } }
        public bool menuViewCaja { get => _menuViewCaja; set { if (_menuViewCaja != value) { _menuViewCaja = value; OnPropertyChanged(); } } }
        #endregion // Menu


        #region Private
        VentanaCaja vCaja;
        VentanaStock vStock;
        #endregion Private



        #region ReadOnly
        public Visibility visAdmin => Variables.UsuarioLogueado.Nivel < 2 ? Visibility.Visible : Visibility.Collapsed;
        public DBUsuariosClass usuarioActivo => Variables.UsuarioLogueado;
        #endregion // ReadOnly



        #region Commands
        public Command comCerrarSesion => new Command((object parameter) => { vCaja = new VentanaCaja(true); vCaja.Show(); thisWindow.Close(); });

        public Command comOpciones => new Command((object parameter) => { XOpciones xOpciones = new XOpciones(); _ = xOpciones.ShowDialog(); });
        #endregion // Commands



        #region Global
        void globalFiltersSorters()
        {
            globalListUsuarios.SortDescriptions.Clear(); globalListUsuarios.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));
        }

        readonly CollectionViewSource globalListUsuariosSource = new CollectionViewSource() { Source = Variables.Inventario.Usuarios.Local.ToObservableCollection() };
        public ICollectionView globalListUsuarios => globalListUsuariosSource.View;

        public DBCajaClass CajaActual => Db.globalCajaActual;
        #endregion // Global



        #region Stock
        void stockFiltersSorters()
        {
            stockListProductosView.SortDescriptions.Clear(); stockListProductosView.SortDescriptions.Add(new SortDescription("Descripcion", ListSortDirection.Ascending));

            stockListProductosView.Filter += delegate (object item)
            {
                if (item == null) { return false; }
                else
                {
                    DBProductosClass tempItem = item as DBProductosClass;
                    return !string.IsNullOrWhiteSpace(stockStrSearch)
                        ? stockBolInactivos != null
                            ? stockBolCodigoDescripcion ? tempItem.Activo == stockBolInactivos && tempItem.Descripcion.ToLower().Contains(stockStrSearch.ToLower()) : tempItem.Activo == stockBolInactivos && tempItem.Codigo.ToLower().Contains(stockStrSearch.ToLower())
                            : stockBolCodigoDescripcion ? tempItem.Descripcion.ToLower().Contains(stockStrSearch.ToLower()) : tempItem.Descripcion.ToLower().Contains(stockStrSearch.ToLower())
                        : stockBolInactivos == null || tempItem.Activo == stockBolInactivos;
                }
            };
        }

        public override void searchTimerClick()
        {
            stockListProductosView.Refresh();
            OnPropertyChanged(nameof(stockIntProductosTotal));
        }


        DBProductosClass _stockSelectedProduct;
        public DBProductosClass stockSelectedProducto { get => _stockSelectedProduct; set { if (_stockSelectedProduct != value) { _stockSelectedProduct = value; OnPropertyChanged(); } } }

        string _stockStrSearch = "";
        public string stockStrSearch { get => _stockStrSearch; set { if (_stockStrSearch != value) { _stockStrSearch = value; OnPropertyChanged(); searchTimerRestart(); } } }

        bool _stockBolCodigoDescripcion = true;
        public bool stockBolCodigoDescripcion { get => _stockBolCodigoDescripcion; set { if (_stockBolCodigoDescripcion != value) { _stockBolCodigoDescripcion = value; OnPropertyChanged(); OnPropertyChanged(nameof(stockStrCodigoDescripcion)); stockStrSearch = ""; } } }

        bool? _stockBolInactivos = true;
        public bool? stockBolInactivos { get => _stockBolInactivos; set { if (_stockBolInactivos != value) { _stockBolInactivos = value; OnPropertyChanged(); searchTimerRestart(); OnPropertyChanged(nameof(stockStrInactivos)); } } }


        public string stockStrCodigoDescripcion => stockBolCodigoDescripcion ? "Descripción" : "Código";
        public string stockStrInactivos => stockBolInactivos == null ? "Todo" : stockBolInactivos == false ? "Solo Inactivos" : "Solo Activos";
        public int stockIntProductosTotal => stockListProductosSource.View.Cast<object>().Count();


        readonly CollectionViewSource stockListProductosSource = new CollectionViewSource() { Source = Variables.Inventario.Productos.Local.ToObservableCollection() };
        public ICollectionView stockListProductosView => stockListProductosSource.View;


        public Command comStockDetalleProducto => new Command(
            (object parameter) => abrirDetallesProductos(stockSelectedProducto),
            (object parameter) => stockSelectedProducto != null);

        public Command comStockCodigoDescripcion => new Command((object parameter) => { stockStrSearch = ""; stockBolCodigoDescripcion = !stockBolCodigoDescripcion; });

        public Command comStockEditarProducto => new Command(
            (object parameter) => abrirAgregarProducto(stockSelectedProducto),
            (object parameter) => stockSelectedProducto != null);

        public Command comStockDesactivarProducto => new Command(
            (object parameter) => { stockSelectedProducto.Activo = !stockSelectedProducto.Activo; _ = Variables.Inventario.SaveChanges(); stockListProductosView.Refresh(); },
            (object parameter) => stockSelectedProducto != null);

        public Command comStockNuevoProducto => new Command((object parameter) => abrirAgregarProducto());

        public Command comStockModificarStock => new Command(
            (object parameter) => abrirModificarStockProducto(stockSelectedProducto),
            (object parameter) => stockSelectedProducto != null);

        public Command comStockAbrirProducto => new Command(
            (object parameter) => abrirAgregarConvertido(stockSelectedProducto),
            (object parameter) => stockSelectedProducto != null);

        public Command comStock => new Command((object parameter) => { vStock = new VentanaStock(); _ = vStock.ShowDialog(); });
        #endregion // Stock



        #region Ventas
        void ventasFiltersSorters()
        {
            ventasListFechas.SortDescriptions.Clear();
            ventasListFechas.Filter += delegate (object item) { DBFechasClass tempFecha = item as DBFechasClass; return tempFecha.Fecha == Variables.strFecha || tempFecha.VentasPerFecha.Count() > 0; };
            ventasListFechas.SortDescriptions.Add(new SortDescription("Fecha", ListSortDirection.Descending));
        }

        DBFechasClass _ventasSelectedFecha;
        public DBFechasClass ventasSelectedFecha { get => _ventasSelectedFecha; set { if (_ventasSelectedFecha != value) { _ventasSelectedFecha = value; OnPropertyChanged(); OnPropertyChanged(nameof(intVentasDiariaTotal)); OnPropertyChanged(nameof(doubleVentasDiariasPesos)); } } }

        DBVentasClass _ventasSelectedVenta;
        public DBVentasClass ventasSelectedVenta { get => _ventasSelectedVenta; set { if (_ventasSelectedVenta != value) { _ventasSelectedVenta = value; OnPropertyChanged(); OnPropertyChanged(nameof(ventaVisDeudor)); } } }


        public Visibility ventaVisDeudor => ventasSelectedVenta?.Deudor != null ? Visibility.Visible : Visibility.Collapsed;
        public int intVentasDiariaTotal => ventasSelectedFecha != null ? ventasSelectedFecha.VentasPerFecha.Count() : 0;
        public Double doubleVentasDiariasPesos => ventasSelectedFecha != null ? ventasSelectedFecha.CajasPerFecha.Where(x => x.VentaForCaja != null).Sum(x => x.doubleEfectivoTotal) : 0;


        readonly CollectionViewSource ventasListFechasSource = new CollectionViewSource() { Source = Variables.Inventario.Fechas.Local.ToObservableCollection() };
        public ICollectionView ventasListFechas => ventasListFechasSource.View;


        public Command comVenta => new Command((object parameter) => { abrirAgregarVentas(); });
        #endregion // Ventas



        #region Ingresos
        void ingresosFiltersSorters()
        {
            ingresosListFechas.SortDescriptions.Clear();
            ingresosListFechas.Filter += delegate (object item) { DBFechasClass tempFecha = item as DBFechasClass; return tempFecha.Fecha == Variables.strFecha ? true : tempFecha.IngresosPerFecha.Count() > 0; };
            ingresosListFechas.SortDescriptions.Add(new SortDescription("Fecha", ListSortDirection.Descending));
        }

        DBFechasClass _ingresosSelectedFecha;
        public DBFechasClass ingresosSelectedFecha { get { return _ingresosSelectedFecha; } set { if (_ingresosSelectedFecha != value) { _ingresosSelectedFecha = value; OnPropertyChanged(); } } }

        DBIngresosClass _ingresosSelectedIngreso;
        public DBIngresosClass ingresosSelectedIngreso { get { return _ingresosSelectedIngreso; } set { if (_ingresosSelectedIngreso != value) { _ingresosSelectedIngreso = value; OnPropertyChanged(); } } }


        public int intIngresosIngresoTotal => ingresosSelectedIngreso != null ? ingresosSelectedIngreso.IngresoProductosPerIngreso.Sum(x => x.Cantidad) : 0;


        readonly CollectionViewSource ingresosListFechasSource = new CollectionViewSource() { Source = Variables.Inventario.Fechas.Local.ToObservableCollection() };
        public ICollectionView ingresosListFechas => ingresosListFechasSource.View;


        public Command comIngreso => new Command((object parameter) => abrirAgregarIngresos());
        #endregion // Ingresos



        #region Sacado
        void sacadoFiltersSorters()
        {
            listSacado.Filter = delegate (object item) { return (item as DBSacadoProductosClass).Usuario == sacadoSelectedUsuario; };
        }

        DBUsuariosClass _sacadoSelectedUsuario;
        public DBUsuariosClass sacadoSelectedUsuario { get => _sacadoSelectedUsuario; set { if (_sacadoSelectedUsuario != value) { _sacadoSelectedUsuario = value; OnPropertyChanged(); listSacado.Refresh(); OnPropertyChanged(nameof(TotalSacado)); } } }


        readonly CollectionViewSource _sacadoListSacadoSource = new CollectionViewSource { Source = Variables.Inventario.SacadoProductos.Local.ToObservableCollection() };
        public ICollectionView listSacado => _sacadoListSacadoSource.View;
        public Double TotalSacado => listSacado != null ? listSacado.Cast<DBSacadoProductosClass>().Sum(x => x.PrecioTotal) : 0;


        public Command comAgregarSacado => new Command(
            (object parameter) => abrirAgregarSacado(sacadoSelectedUsuario),
            (object parameter) => sacadoSelectedUsuario != null);
        #endregion // Sacado



        #region Consumos
        void consumosFiltersSorters()
        {
            consumosListFechas.SortDescriptions.Clear();
            consumosListFechas.Filter += delegate (object item) { DBFechasClass tempFecha = item as DBFechasClass; return tempFecha.Fecha == Variables.strFecha ? true : tempFecha.ConsumosProductosPerFecha.Count() > 0; };
            consumosListFechas.SortDescriptions.Add(new SortDescription("Fecha", ListSortDirection.Descending));
        }


        DBFechasClass _consumosSelectedFecha;
        public DBFechasClass consumosSelectedFecha { get => _consumosSelectedFecha; set { if (_consumosSelectedFecha != value) { _consumosSelectedFecha = value; OnPropertyChanged(); } } }

        readonly CollectionViewSource consumosListFechasSource = new CollectionViewSource() { Source = Variables.Inventario.Fechas.Local.ToObservableCollection() };
        public ICollectionView consumosListFechas => consumosListFechasSource.View;


        public Command comConsumir => new Command((object parameter) => abrirAgregarConsumo());
        #endregion // Consumos



        #region Deudas
        void deudasFiltersSorters()
        {

            deudasListDeudores.SortDescriptions.Clear(); deudasListDeudores.SortDescriptions.Add(new SortDescription("Nombre", ListSortDirection.Ascending));


            deudasListFechas.Filter += delegate (object item) { return (item as DBFechasClass).Fecha == Variables.strFecha || deudasSelectedDeudor != null && (item as DBFechasClass).VentasPerFecha.Any(x => x.Deudor == deudasSelectedDeudor); };
            deudasListFechas.SortDescriptions.Clear(); deudasListFechas.SortDescriptions.Add(new SortDescription("Fecha", ListSortDirection.Descending));


            deudasListVentasPerFecha.Filter += delegate (object item) { return _deudasSelectedDeudor != null && _deudasSelectedFecha != null && (item as DBVentasClass).Deudor == _deudasSelectedDeudor && (item as DBVentasClass).Fecha == _deudasSelectedFecha; };


            deudasListPagosPerDeudor.Filter += delegate (object item) { return deudasSelectedDeudor != null && (item as DBCajaClass).VentaProductosPerCaja.Any(x => x.Deudor == deudasSelectedDeudor); };
            deudasListPagosPerDeudor.SortDescriptions.Clear(); deudasListPagosPerDeudor.SortDescriptions.Add(new SortDescription("Fecha.Fecha", ListSortDirection.Descending));
        }

        public void updateDeudores()
        {
            if (deudasSelectedDeudor != null) { deudasSelectedDeudor.updateDeudor(); }
        }

        DBDeudoresClass _deudasSelectedDeudor;
        public DBDeudoresClass deudasSelectedDeudor { get => _deudasSelectedDeudor; set { if (_deudasSelectedDeudor != value) { _deudasSelectedDeudor = value; OnPropertyChanged(); deudasListFechas.Refresh(); deudasListVentasPerFecha.Refresh(); OnPropertyChanged(nameof(deudasListVentasPerFecha)); deudasListPagosPerDeudor.Refresh(); OnPropertyChanged(nameof(deudasListPagosPerDeudor)); } } }

        DBFechasClass _deudasSelectedFecha;
        public DBFechasClass deudasSelectedFecha { get => _deudasSelectedFecha; set { if (_deudasSelectedFecha != value) { _deudasSelectedFecha = value; OnPropertyChanged(); deudasListVentasPerFecha.Refresh(); } } }

        DBVentasClass _deudasSelectedVenta;
        public DBVentasClass deudasSelectedVenta { get => _deudasSelectedVenta; set { if (_deudasSelectedVenta != value) { _deudasSelectedVenta = value; OnPropertyChanged(); } } }


        readonly CollectionViewSource _deudasListDeudoresSource = new CollectionViewSource() { Source = Variables.Inventario.Deudores.Local.ToObservableCollection() };
        public ICollectionView deudasListDeudores => _deudasListDeudoresSource.View;

        readonly CollectionViewSource _deudasListFechasSource = new CollectionViewSource { Source = Variables.Inventario.Fechas.Local.ToObservableCollection() };
        public ICollectionView deudasListFechas => _deudasListFechasSource.View;

        readonly CollectionViewSource _deudasListVentasPerFechaSource = new CollectionViewSource() { Source = Variables.Inventario.Ventas.Local.ToObservableCollection() };
        public ICollectionView deudasListVentasPerFecha => _deudasListVentasPerFechaSource.View;


        readonly CollectionViewSource _deudasListPagosSource = new CollectionViewSource() { Source = Variables.Inventario.Caja.Local.ToObservableCollection() };
        public ICollectionView deudasListPagosPerDeudor => _deudasListPagosSource.View;


        public Command comEditarDeudor => new Command(
            (object parameter) => abrirAgregarDeudor(deudasSelectedDeudor),
            (object parameter) => deudasSelectedDeudor != null);

        public Command comNuevoDeudor => new Command((object parameter) => abrirAgregarDeudor());

        public Command comDeduasPagar => new Command(
            (object parameter) => { VentanaPagarDeuda vPagarDeuda = new VentanaPagarDeuda(deudasSelectedDeudor); _ = vPagarDeuda.ShowDialog(); deudasListDeudores.Refresh(); OnPropertyChanged(nameof(deudasSelectedDeudor)); },
            (object parameter) => deudasSelectedDeudor != null);
        #endregion // Deudas



        #region Usuarios
        DBUsuariosClass _usuariosSelectedUsuario;
        public DBUsuariosClass usuariosSelectedUsuario { get => _usuariosSelectedUsuario; set { if (_usuariosSelectedUsuario != value) { _usuariosSelectedUsuario = value; OnPropertyChanged(); } } }


        public Command comUsuariosNuevoUsuario => new Command((object parameter) => abrirAgregarUsuario());

        public Command comUsuariosEditarUsuario => new Command(
            (object parameter) => abrirAgregarUsuario(usuariosSelectedUsuario),
            (object parameter) => usuariosSelectedUsuario != null);

        public Command comUsuariosDesactivarUsuario
        {
            get
            {
                return new Command(
                (object parameter) => { usuariosSelectedUsuario.Activo = !usuariosSelectedUsuario.Activo; _ = Variables.Inventario.SaveChanges(); },
                (object parameter) => usuariosSelectedUsuario != null);
            }
        }
        #endregion // Usuarios



        #region Tags
        void tagsFiltersSorters()
        {
            tagsListTags.SortDescriptions.Clear(); tagsListTags.SortDescriptions.Add(new SortDescription("fullTag", ListSortDirection.Ascending));
        }


        DBTagsClass _tagsSelectedTag;
        public DBTagsClass tagsSelectedTag { get => _tagsSelectedTag; set { if (_tagsSelectedTag != value) { _tagsSelectedTag = value; OnPropertyChanged(); } } }


        public int tagsIntTotalTags => tagsListTags.Cast<object>().Count();


        readonly CollectionViewSource _tagsListTagsSource = new CollectionViewSource() { Source = Variables.Inventario.Tags.Local.ToObservableCollection() };
        public ICollectionView tagsListTags => _tagsListTagsSource.View;


        public Command comTagsNuevoTag => new Command((object parameter) => abrirAgregarTag());

        public Command comTagsEditarTag => new Command(
            (object parameter) => { _ = abrirAgregarTag(tagsSelectedTag); OnPropertyChanged(nameof(stockListProductosView)); },
            (object parameter) => tagsSelectedTag != null);

        public Command comTagsAlarmaTag => new Command(
            (object parameter) => { tagsSelectedTag.Activo = !tagsSelectedTag.Activo; _ = Variables.Inventario.SaveChanges(); },
            (object parameter) => tagsSelectedTag != null);
        #endregion // Tags



        #region Medidas
        void medidasFiltersSorters()
        {
            medidasListMedidas.SortDescriptions.Clear();
            medidasListMedidas.GroupDescriptions.Clear(); medidasListMedidas.GroupDescriptions.Add(new PropertyGroupDescription("TipoShort"));
            medidasListMedidas.SortDescriptions.Add(new SortDescription("Medida", ListSortDirection.Ascending));
        }


        DBMedidasClass _medidasSelectedMedida;
        public DBMedidasClass medidasSelectedMedida { get => _medidasSelectedMedida; set { if (_medidasSelectedMedida != value) { _medidasSelectedMedida = value; OnPropertyChanged(); } } }


        public int medidasIntTotalMedidas => medidasListMedidas.Cast<object>().Count();


        readonly CollectionViewSource _medidasListMedidasSource = new CollectionViewSource() { Source = Variables.Inventario.Medidas.Local.ToObservableCollection() };
        public ICollectionView medidasListMedidas => _medidasListMedidasSource.View;


        public Command comMedidasNuevaMedida => new Command((object parameter) => { _ = abrirAgregarMedidas(); OnPropertyChanged(nameof(medidasIntTotalMedidas)); });

        public Command comMedidasEditarMedida => new Command(
            (object parameter) => { _ = abrirAgregarMedidas(medidasSelectedMedida); OnPropertyChanged(nameof(medidasIntTotalMedidas)); },
            (object parameter) => medidasSelectedMedida != null);

        public Command comMedidasCambiarEstado => new Command(
            (object parameter) => { medidasSelectedMedida.Activo = !medidasSelectedMedida.Activo; _ = Variables.Inventario.SaveChanges(); },
            (object parameter) => medidasSelectedMedida != null);
        #endregion // Medidas



        #region Retiros
        void retirosFiltersSorters()
        {
            retirosListFechas.Filter = delegate (object item) { return (item as DBFechasClass).Fecha == Variables.strFecha || (item as DBFechasClass).RetirosPerFecha.Count > 0; };
            retirosListFechas.SortDescriptions.Clear(); retirosListFechas.SortDescriptions.Add(new SortDescription("Fecha", ListSortDirection.Descending));

            retirosListRetirosPerFecha.Filter = delegate (object item) { return retirosSelectedFecha != null && (item as DBRetirosCaja).Fecha == retirosSelectedFecha; };
            retirosListRetirosPerFecha.SortDescriptions.Clear(); retirosListRetirosPerFecha.SortDescriptions.Add(new SortDescription("Hora", ListSortDirection.Descending));

            retirosListALLRetiros.SortDescriptions.Clear();
            retirosListALLRetiros.SortDescriptions.Add(new SortDescription("Fecha.Fecha", ListSortDirection.Descending));
            retirosListALLRetiros.SortDescriptions.Add(new SortDescription("Hora", ListSortDirection.Descending));
        }

        DBFechasClass _retirosSelectedFecha;
        public DBFechasClass retirosSelectedFecha { get => _retirosSelectedFecha; set { if (_retirosSelectedFecha != value) { _retirosSelectedFecha = value; OnPropertyChanged(); retirosListRetirosPerFecha.Refresh(); } } }

        readonly CollectionViewSource _retirosListFechasSource = new CollectionViewSource { Source = Variables.Inventario.Fechas.Local.ToObservableCollection() };
        public ICollectionView retirosListFechas => _retirosListFechasSource.View;

        readonly CollectionViewSource _retirosListRetirosPerFechaSource = new CollectionViewSource { Source = Variables.Inventario.Retiros.Local.ToObservableCollection() };
        public ICollectionView retirosListRetirosPerFecha => _retirosListRetirosPerFechaSource.View;

        readonly CollectionViewSource _retirosListALLRetirosSource = new CollectionViewSource { Source = Variables.Inventario.Retiros.Local.ToObservableCollection() };
        public ICollectionView retirosListALLRetiros => _retirosListALLRetirosSource.View;


        public Command comRetiro => new Command((object paramter) => abrirAgregarRetiroCaja());
        #endregion // Retiros



        #region Caja
        void cajaFiltersSorters()
        {
            cajaListFechas.Filter = delegate (object item) { return (item as DBFechasClass).Fecha == Variables.strFecha || (item as DBFechasClass).CajasPerFecha.Any(x => x.CajaConteoForCaja != null); };
            cajaListFechas.SortDescriptions.Clear();
            cajaListFechas.SortDescriptions.Add(new SortDescription("Fecha", ListSortDirection.Descending));

            cajaListCajasPerFecha.Filter = delegate (object item) { return cajaSelectedFecha != null && (item as DBCajaConteosClass).Caja.Fecha == cajaSelectedFecha; };
            cajaListCajasPerFecha.SortDescriptions.Clear(); cajaListCajasPerFecha.SortDescriptions.Add(new SortDescription("Caja.Hora", ListSortDirection.Descending));


            cajaListALLUsuarios.SortDescriptions.Clear();
            cajaListALLUsuarios.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));

            cajaListFechas.MoveCurrentToFirst();
        }


        DBUsuariosClass _cajaSelectedUsuario;
        public DBUsuariosClass cajaSelectedUsuario { get => _cajaSelectedUsuario; set { if (_cajaSelectedUsuario != value) { _cajaSelectedUsuario = value; OnPropertyChanged(); } } }

        DBFechasClass _cajaSelectedFecha;
        public DBFechasClass cajaSelectedFecha { get => _cajaSelectedFecha; set { if (_cajaSelectedFecha != value) { _cajaSelectedFecha = value; OnPropertyChanged(); cajaListCajasPerFecha.Refresh(); } } }


        readonly CollectionViewSource _cajaListFechasSource = new CollectionViewSource { Source = Variables.Inventario.Fechas.Local.ToObservableCollection() };
        public ICollectionView cajaListFechas => _cajaListFechasSource.View;

        readonly CollectionViewSource _cajaListCajasPerFecha = new CollectionViewSource { Source = Variables.Inventario.CajaConteos.Local.ToObservableCollection() };
        public ICollectionView cajaListCajasPerFecha => _cajaListCajasPerFecha.View;

        readonly CollectionViewSource _cajaListALLUsuariosSource = new CollectionViewSource { Source = Variables.Inventario.Usuarios.Local.ToObservableCollection() };
        public ICollectionView cajaListALLUsuarios => _cajaListALLUsuariosSource.View;
        #endregion // Caja



        #region Proveedores
        void proveedoresFiltersSorters()
        {
            proveedoresListProveedores.SortDescriptions.Clear(); proveedoresListProveedores.SortDescriptions.Add(new SortDescription("Nombre", ListSortDirection.Ascending));
        }


        DBProveedorClass _proveedoresSelectedProveedor;
        public DBProveedorClass proveedoresSelectedProveedor { get => _proveedoresSelectedProveedor; set { if (_proveedoresSelectedProveedor != value) { _proveedoresSelectedProveedor = value; OnPropertyChanged(); } } }


        readonly CollectionViewSource _proveedoresListProveedoresSource = new CollectionViewSource() { Source = Variables.Inventario.Proveedores.Local.ToObservableCollection() };
        public ICollectionView proveedoresListProveedores => _proveedoresListProveedoresSource.View;


        public Command comProveedoresNuevoProveedor => new Command((object parameter) => abrirAgregarProveedor());

        public Command comProveedoresEditarProveedor => new Command(
            (object parameter) => abrirAgregarProveedor(proveedoresSelectedProveedor),
            (object parameter) => proveedoresSelectedProveedor != null);

        public Command comProveedoresDesactivarProveedor => new Command((object parameter) => { proveedoresSelectedProveedor.Activo = !proveedoresSelectedProveedor.Activo; _ = Variables.Inventario.SaveChanges(); });
        #endregion // Proveedores
    }
    #endregion // Binding
}