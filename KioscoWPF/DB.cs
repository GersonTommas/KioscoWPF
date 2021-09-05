using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Windows.Data;

namespace KioscoWPF
{
    class Db
    {
        public static cajaModel globalCajaActual;

        public static void loadInventario()
        {
            Variables.Inventario = new DBInventarioContext();
            try { _ = Variables.Inventario.Usuarios.Single(x => x.Id == 1); } catch { _ = Variables.Inventario.Usuarios.Add(new usuariosModel() { Apellido = "admin", Contraseña = "admin", Detalle = "Default", Activo = true, FechaIngreso = Variables.strFecha, Nivel = 1, Nombre = "Admin", Usuario = "Admin", Id = 1 }); _ = Variables.Inventario.SaveChanges(); }

            Variables.Inventario.AbiertoProductos.Load();
            Variables.Inventario.Caja.Load();
            Variables.Inventario.CajaConteos.Load();
            Variables.Inventario.ConsumosProductos.Load();
            Variables.Inventario.Deudores.Load();
            Variables.Inventario.Fechas.Load();
            Variables.Inventario.Ingresos.Load();
            Variables.Inventario.IngresoProductos.Load();
            Variables.Inventario.Medidas.Load();
            Variables.Inventario.ModificadoProductos.Load();
            Variables.Inventario.Productos.Load();
            Variables.Inventario.Proveedores.Load();
            Variables.Inventario.Retiros.Load();
            Variables.Inventario.RetiroMotivos.Load();
            Variables.Inventario.SacadoProductos.Load();
            Variables.Inventario.Tags.Load();
            Variables.Inventario.Usuarios.Load();
            Variables.Inventario.VentaProductos.Load();
            Variables.Inventario.Ventas.Load();

            try { globalCajaActual = Variables.Inventario.Caja.Local.Single(x => x.Id == 1); } catch { Variables.Inventario.Caja.Local.Add(new cajaModel() { CajaActual = 5000, Fecha = returnFecha(), Hora = Variables.strHora }); _ = Variables.Inventario.SaveChanges(); loadInventario(); }
        }

        public static void contabilizarCaja(cajaModel sentCaja, bool sentDescontar = false)
        {
            if (sentDescontar)
            {
                globalCajaActual.CajaActual -= Math.Round(sentCaja.CajaActual, 2);
                globalCajaActual.MercadoPago -= Math.Round(sentCaja.MercadoPago, 2);
            }
            else
            {
                globalCajaActual.CajaActual += Math.Round(sentCaja.CajaActual - sentCaja.Vuelto, 2);
                globalCajaActual.MercadoPago += Math.Round(sentCaja.MercadoPago, 2);
            }
        }

        public static fechasModel returnFecha()
        {
            try { return Variables.Inventario.Fechas.Local.Single(x => x.Fecha == Variables.strFecha); }
            catch { return new fechasModel() { Fecha = Variables.strFecha }; }
        }

        public static void resetProductoAgregado()
        {
            foreach (productosModel prd in Variables.Inventario.Productos.Local.Where(x => x.Agregado))
            {
                prd.Agregado = false;
            }
        }
    }

    #region Database
    public class DBInventarioContext : DbContext
    {
        public DBInventarioContext() : base() { _ = Database.EnsureCreated(); }

        #region Tables
        public DbSet<abiertoProductosModel> AbiertoProductos { get; set; }
        public DbSet<cajaModel> Caja { get; set; }
        public DbSet<cajaConteosModel> CajaConteos { get; set; }
        public DbSet<consumoProductoModel> ConsumosProductos { get; set; }
        public DbSet<deudoresModel> Deudores { get; set; }
        public DbSet<fechasModel> Fechas { get; set; }
        public DbSet<ingresosModel> Ingresos { get; set; }
        public DbSet<ingresoProductosModel> IngresoProductos { get; set; }
        public DbSet<medidasModel> Medidas { get; set; }
        public DbSet<modificadoProductoModel> ModificadoProductos { get; set; }
        public DbSet<productosModel> Productos { get; set; }
        public DbSet<proveedoresModel> Proveedores { get; set; }
        public DbSet<retirosCajaModel> Retiros { get; set; }
        public DbSet<motivoRetirosModel> RetiroMotivos { get; set; }
        public DbSet<sacadoProductosModel> SacadoProductos { get; set; }
        public DbSet<tagsModel> Tags { get; set; }
        public DbSet<usuariosModel> Usuarios { get; set; }
        public DbSet<ventasModel> Ventas { get; set; }
        public DbSet<ventaProductosModel> VentaProductos { get; set; }
        #endregion // Tables

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\IDQ";
            _ = Directory.CreateDirectory(dirPath);

            _ = optionsBuilder.UseSqlite("Data Source=" + dirPath + "\\Database.db");
            _ = optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder) { builder.Entity<fechasModel>().Property(x => x.Id).ValueGeneratedOnAdd(); }

    }
    #endregion // Database



    #region Base Level
    #region Table Fechas
    /*
    public class fechasModel : ObservableClass
    {
        #region Private
        String _Fecha;
        #endregion // Private

        #region Public
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = Convert.ToDateTime(value).ToString("yyyy/MM/dd"); OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<cajaModel> CajasPerFecha { get; private set; } = new ObservableCollection<cajaModel>();
        public virtual ICollection<productosModel> ProductosModificadosPerFecha { get; private set; } = new ObservableCollection<productosModel>();
        public virtual ICollection<ingresosModel> IngresosPerFecha { get; private set; } = new ObservableCollection<ingresosModel>();
        public virtual ICollection<ventasModel> VentasPerFecha { get; private set; } = new ObservableCollection<ventasModel>();
        public virtual ICollection<consumoProductoModel> ConsumosProductosPerFecha { get; private set; } = new ObservableCollection<consumoProductoModel>();
        public virtual ICollection<modificadoProductoModel> ModificadosProductosPerFecha { get; private set; } = new ObservableCollection<modificadoProductoModel>();
        public virtual ICollection<abiertoProductosModel> AbiertoProductosPerFecha { get; private set; } = new ObservableCollection<abiertoProductosModel>();
        public virtual ICollection<retirosCajaModel> RetirosPerFecha { get; private set; } = new ObservableCollection<retirosCajaModel>();
        //public virtual ICollection<DBDeudorPagoClass> DeudorPagosPerFecha { get; private set; } = new ObservableCollection<DBDeudorPagoClass>();

        public virtual ICollection<ventaProductosModel> VentaProductosPagadosPerFecha { get; private set; } = new ObservableCollection<ventaProductosModel>();

        [InverseProperty(nameof(sacadoProductosModel.FechaSacado))]
        public virtual ICollection<sacadoProductosModel> SacadoProductosSacadosPerFecha { get; private set; } = new ObservableCollection<sacadoProductosModel>();
        [InverseProperty(nameof(sacadoProductosModel.FechaPagado))]
        public virtual ICollection<sacadoProductosModel> SacadoProductosPagadosPerFecha { get; private set; } = new ObservableCollection<sacadoProductosModel>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        CollectionViewSource ventasPerFechaViewSource;
        [NotMapped]
        public ICollectionView ventasPerFechaView
        {
            get
            {
                if (ventasPerFechaViewSource == null) { ventasPerFechaViewSource = new CollectionViewSource() { Source = VentasPerFecha }; }
                ICollectionView temp = ventasPerFechaViewSource.View;
                temp.SortDescriptions.Clear(); temp.SortDescriptions.Add(new SortDescription("Hora", ListSortDirection.Descending));
                return temp;
            }
        }


        [NotMapped]
        public int TotalCantidadVentasDiario => VentasPerFecha.Count();
        [NotMapped]
        public Double TotalPesosVentasDiario => VentasPerFecha.Sum(x => x.PrecioTotal);

        [NotMapped]
        public int TotalCantidadConsumosDiario => ConsumosProductosPerFecha.Count();

        public void updateTotalVentasDiario()
        {
            OnPropertyChanged(nameof(TotalCantidadVentasDiario));
            OnPropertyChanged(nameof(TotalPesosVentasDiario));
        }
        public void updateTotalConsumosDiario()
        {
            OnPropertyChanged(nameof(TotalCantidadConsumosDiario));
        }

        public void updateThis()
        {
            OnPropertyChanged(nameof(fechasModel));
        }
        #endregion // NotMapped
    }
    */
    #endregion // Table Fechas


    #region Table Sueldos
    /*
    public class puestoModel : ObservableClass
    {
        #region Private
        int _HorasMensuales; Double _Sueldo; string _Nombre;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int HorasMensuales { get => _HorasMensuales; set { if (_HorasMensuales != value) { _HorasMensuales = value; OnPropertyChanged(); } } }

        public Double Sueldo { get => _Sueldo; set { value = Math.Round(value, 2); if (_Sueldo != value) { _Sueldo = value; OnPropertyChanged(); } } }

        public string Nombre { get => _Nombre; set { if (_Nombre != value) { _Nombre = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        #endregion // Navigation

        #region NotMapped
        public Double NmSueldoMensual => Math.Round(Sueldo * HorasMensuales, 2);
        #endregion // NotMapped
    }
    */
    #endregion // Table Sueldos



    #region Table Horas Trabajadas
    /*
    public class jornadaModel : ObservableClass
    {
        
        #region Private
        int _HorasTrabajadas, _HorasExtra; String _AñoMes; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int HorasTrabajadas { get => _HorasTrabajadas; set { if (_HorasTrabajadas != value) { _HorasTrabajadas = value; OnPropertyChanged(); } } }
        public int HorasExtra { get => _HorasExtra; set { if (_HorasExtra != value) { _HorasExtra = value; OnPropertyChanged(); } } }

        public String AñoMes { get => _AñoMes; set { if (_AñoMes != value) { _AñoMes = Convert.ToDateTime(value).ToString("yyyy/MM"); OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        #endregion // Navigation
        
    }
    */
    #endregion // Table Horas Trabajadas



    #region Table Usuarios
    /*
    public class usuariosModel : ObservableClass
    {
        
        #region Private
        int _Nivel; Double _Resto; string _Nombre, _Apellido, _Detalle, _Contraseña, _Usuario; String _Fecha, _FechaSalida; bool _Activo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Nivel { get => _Nivel; set { if (_Nivel != value) { _Nivel = value; OnPropertyChanged(); } } }
        public Double Resto { get => _Resto; set { value = Math.Round(value, 2); if (_Resto != value) { _Resto = value; OnPropertyChanged(); } } }

        public string Nombre { get => _Nombre; set { if (_Nombre != value) { _Nombre = value; OnPropertyChanged(); } } }
        public string Apellido { get => _Apellido; set { if (_Apellido != value) { _Apellido = value; OnPropertyChanged(); } } }
        public string Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }
        public string Detalle { get => _Detalle; set { if (_Detalle != value) { _Detalle = value; OnPropertyChanged(); } } }
        public string Contraseña { get => _Contraseña; set { if (_Contraseña != value) { _Contraseña = value; OnPropertyChanged(); } } }

        public String FechaIngreso { get => _Fecha; set { if (_Fecha != value) { _Fecha = Convert.ToDateTime(value).ToString("yyyy/MM/dd"); OnPropertyChanged(); } } }
        public String FechaSalida { get => _FechaSalida; set { if (_FechaSalida != value) { _FechaSalida = value != null ? Convert.ToDateTime(value).ToString("yyyy/MM/dd") : null; OnPropertyChanged(); } } }

        public bool Activo { get => _Activo; set { if (_Activo != value) { _Activo = value; OnPropertyChanged(); } } }
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
    */
    #endregion // Table Usuarios


    #region Table Tags
    /*
    public class tagsModel1 : ObservableClass
    {
        #region Private
        int _Minimo; string _Tag; bool _Activo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Minimo { get => _Minimo; set { if (_Minimo != value) { _Minimo = value; OnPropertyChanged(); OnPropertyChanged(nameof(fullTag)); } } }

        public string Tag { get => _Tag; set { if (_Tag != value) { _Tag = value; OnPropertyChanged(); OnPropertyChanged(nameof(fullTag)); } } }

        public bool Activo { get => _Activo; set { if (_Activo != value) { _Activo = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<DBProductosClass> ProductosPerTag { get; private set; } = new ObservableCollection<DBProductosClass>();
        #endregion // Navigation

        #region Not Mapped
        [NotMapped]
        public string fullTag => Tag + " " + Minimo.ToString();
        #endregion // Not Mapped
    }
    */
    #endregion // Table Tags


    #region Table Medidas
    /*
    public class medidasModel1 : ObservableClass
    {
        #region Private
        int _Tipo; string _Medida; bool _Activo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Tipo { get => _Tipo; set { if (_Tipo != value) { _Tipo = value; OnPropertyChanged(); OnPropertyChanged(nameof(TipoShort)); OnPropertyChanged(nameof(fullMedida)); } } }

        public string Medida { get => _Medida; set { if (_Medida != value) { _Medida = value; OnPropertyChanged(); OnPropertyChanged(nameof(fullMedida)); } } }

        public bool Activo { get => _Activo; set { if (_Activo != value) { _Activo = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<DBProductosClass> ProductosPerMedida { get; private set; } = new ObservableCollection<DBProductosClass>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        public string TipoShort
        {
            get => Tipo switch
            {
                1 => "cm",
                2 => "cc",
                3 => "g",
                4 => "Kg",
                5 => "L",
                6 => "ml",
                7 => "u",
                8 => "Par",
                9 => "w",
                10 => "Kw",
                11 => "v",
                12 => "Talle",
                13 => "mg",
                14 => "cl",
                _ => "",
            };
            set
            {
                Tipo = value switch
                {
                    "cm" => 1,
                    "cc" => 2,
                    "g" => 3,
                    "Kg" => 4,
                    "L" => 5,
                    "ml" => 6,
                    "u" => 7,
                    "Par" => 8,
                    "w" => 9,
                    "Kw" => 10,
                    "v" => 11,
                    "Talle" => 12,
                    "mg" => 13,
                    "cl" => 14,
                    _ => 0,
                }; OnPropertyChanged();
            }
        }

        [NotMapped]
        public string fullMedida => Medida + TipoShort;
        #endregion // NotMapped
    }
    */
    #endregion // Table Medidas
    #endregion // Base Level



    #region Caja
    /*
    public class cajaModel1 : ObservableClass
    {
        #region Private
        Double _CajaActual, _MercadoPago, _Vuelto; String _Hora; fechasModel _Fecha; DBCajaConteosClass _CajaConteoForCaja; DBVentasClass _VentaForCaja;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public Double CajaActual { get => _CajaActual; set { value = Math.Round(value, 2); if (_CajaActual != value) { _CajaActual = value; OnPropertyChanged(); privUpdateVenta(); } } }
        public Double MercadoPago { get => _MercadoPago; set { value = Math.Round(value, 2); if (_MercadoPago != value) { _MercadoPago = value; OnPropertyChanged(); privUpdateVenta(); } } }
        public Double Vuelto { get => _Vuelto; set { value = Math.Round(value, 2); if (_Vuelto != value) { _Vuelto = value; OnPropertyChanged(); } } }

        public String Hora { get => _Hora; set { if (_Hora != value) { _Hora = Convert.ToDateTime(value).ToString("HH:mm:ss"); OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = value; OnPropertyChanged(); } } }

        public virtual DBCajaConteosClass CajaConteoForCaja { get => _CajaConteoForCaja; set { if (_CajaConteoForCaja != value) { _CajaConteoForCaja = value; OnPropertyChanged(); } } }
        public virtual DBVentasClass VentaForCaja { get => _VentaForCaja; set { if (_VentaForCaja != value) { _VentaForCaja = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<DBVentaProductosClass> VentaProductosPerCaja { get; private set; } = new ObservableCollection<DBVentaProductosClass>();
        public virtual ICollection<DBRetirosCaja> RetirosCajaPerCaja { get; private set; } = new ObservableCollection<DBRetirosCaja>();
        #endregion // Navigation

        #region Helpers
        void privUpdateVenta()
        {
            if (VentaForCaja != null)
            {
                /*
                OnPropertyChanged(nameof(VentaForCaja.TotalPagado));
                OnPropertyChanged(nameof(VentaForCaja.Vuelto));
                *//*
                OnPropertyChanged(nameof(doubleEfectivoTotal));
            }
        }
        #endregion // Helpers

        #region NotMapped
        [NotMapped]
        public Double doubleEfectivoTotal => CajaActual - Vuelto;
        #endregion // NotMapped
    }
    */
    #endregion // Caja



    #region Table Conteo Caja
    /*
    public class cajaConteosModel1 : ObservableClass
    {
        #region Private
        Double _Diferencia; string _Detalle; bool _Salida; cajaModel _Caja; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public Double Diferencia { get => _Diferencia; set { value = Math.Round(value, 2); if (_Diferencia != value) { _Diferencia = value; OnPropertyChanged(); } } }

        public string Detalle { get => _Detalle; set { if (_Detalle != value) { _Detalle = value; OnPropertyChanged(); } } }

        public bool Salida { get => _Salida; set { if (_Salida != value) { _Salida = value; OnPropertyChanged(); } } }

        public int CajaID { get; set; }
        public virtual cajaModel Caja { get => _Caja; set { if (_Caja != value) { _Caja = value; OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }
        #endregion // Public
    }
    */
    #endregion // Table Conteo Caja



    #region Table Productos
    /*
    public class productosModel1 : ObservableClass
    {
        #region Private
        int _StockInicial, _Stock; Double _PrecioActual, _PrecioIngreso; string _Codigo, _Descripcion; fechasModel _FechaModificado; bool _Activo; tagsModel _Tag; medidasModel _Medida;
        bool _Agregado = false;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int StockInicial { get => _StockInicial; set { if (_StockInicial != value) { _StockInicial = value; OnPropertyChanged(); } } }
        public int Stock { get => _Stock; set { if (_Stock != value) { _Stock = value; OnPropertyChanged(); OnPropertyChanged(nameof(stockVsMinimo)); } } }
        public Double PrecioIngreso { get => _PrecioIngreso; set { value = Math.Round(value, 2); if (_PrecioIngreso != value) { _PrecioIngreso = value; OnPropertyChanged(); } } }
        public Double PrecioActual { get => _PrecioActual; set { value = Math.Round(value, 2); if (_PrecioActual != value) { _PrecioActual = value; OnPropertyChanged(); } } }

        public string Codigo { get => _Codigo; set { if (_Codigo != value) { _Codigo = value; OnPropertyChanged(); } } }
        public string Descripcion { get => _Descripcion; set { if (_Descripcion != value) { _Descripcion = value; OnPropertyChanged(); } } }

        public bool Activo { get => _Activo; set { if (_Activo != value) { _Activo = value; OnPropertyChanged(); } } }

        public int? FechaModificadoID { get; set; }
        public virtual fechasModel FechaModificado { get => _FechaModificado; set { if (_FechaModificado != value) { _FechaModificado = value; OnPropertyChanged(); } } }

        public int TagID { get; set; }
        public virtual tagsModel Tag { get => _Tag; set { _Tag = value; OnPropertyChanged(); OnPropertyChanged(nameof(stockVsMinimo)); } }

        public int MedidaID { get; set; }
        public virtual medidasModel Medida { get => _Medida; set { _Medida = value; OnPropertyChanged(); } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<sacadoProductosModel> SacadoProductosPerProducto { get; private set; } = new ObservableCollection<sacadoProductosModel>();
        public virtual ICollection<ingresoProductosModel> IngresoProductosPerProducto { get; private set; } = new ObservableCollection<ingresoProductosModel>();
        public virtual ICollection<ventaProductosModel> VentaProductosPerProducto { get; private set; } = new ObservableCollection<ventaProductosModel>();
        public virtual ICollection<consumoProductoModel> ConsumoProductosPerProducto { get; private set; } = new ObservableCollection<consumoProductoModel>();
        public virtual ICollection<modificadoProductoModel> ModificadoProductosPerProducto { get; private set; } = new ObservableCollection<modificadoProductoModel>();

        [InverseProperty(nameof(abiertoProductosModel.ProductoSacado))]
        public virtual ICollection<abiertoProductosModel> AbiertoSacadoPerProducto { get; private set; } = new ObservableCollection<abiertoProductosModel>();
        [InverseProperty(nameof(abiertoProductosModel.ProductoAgregado))]
        public virtual ICollection<abiertoProductosModel> AbiertoAgregadoPerProducto { get; private set; } = new ObservableCollection<abiertoProductosModel>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        public int stockVsMinimo => Stock < 1 ? 1 : Stock < Tag.Minimo ? 2 : Stock == Tag.Minimo ? 3 : 4;
        [NotMapped]
        public bool Agregado { get => _Agregado; set { if (_Agregado != value) { _Agregado = value; OnPropertyChanged(); } } }
        #endregion // NotMapped
    }
    */
    #endregion // Table Productos



    #region Table Deudores
    /*
    public class deudoresModel1 : ObservableClass
    {
        #region Private
        int _Nivel; Double _Resto; string _Nombre, _Detalles; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Nivel { get => _Nivel; set { if (_Nivel != value) { _Nivel = value; OnPropertyChanged(); } } }
        public Double Resto { get => _Resto; set { value = Math.Round(value, 2); if (_Resto != value) { _Resto = value; OnPropertyChanged(); OnPropertyChanged(nameof(doubleFaltanteTotal)); } } }

        public string Nombre { get => _Nombre; set { if (_Nombre != value) { _Nombre = value; OnPropertyChanged(); } } }

        public string Detalles { get => _Detalles; set { if (_Detalles != value) { _Detalles = value; OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<DBVentasClass> VentasPerDeudor { get; private set; } = new ObservableCollection<DBVentasClass>();
        public virtual ICollection<DBVentaProductosClass> VentaProductosPerDeudor { get; private set; } = new ObservableCollection<DBVentaProductosClass>();
        //public virtual ICollection<DBDeudorPagoClass> DeudorPagosPerDeudor { get; private set; } = new ObservableCollection<DBDeudorPagoClass>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        public Double doubleDeudaTotal => Math.Round(VentasPerDeudor.Sum(x => x.DeudaTotalVenta), 2);
        [NotMapped]
        public Double doubleFaltanteTotal => Math.Round(doubleDeudaTotal - Resto, 2);

        [NotMapped]
        CollectionViewSource ventasProductosPendientesPerDeudorSource;
        [NotMapped]
        public ICollectionView ventasProductosPendientesPerDeudorView
        {
            get
            {
                if (ventasProductosPendientesPerDeudorSource == null) { ventasProductosPendientesPerDeudorSource = new CollectionViewSource() { Source = VentaProductosPerDeudor }; }
                ICollectionView temp = ventasProductosPendientesPerDeudorSource.View;
                temp.Filter = delegate (object item) { return (item as DBVentaProductosClass).BolPagado == false; };
                temp.SortDescriptions.Clear(); temp.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));

                return temp;
            }
        }

        public void updateDeudor()
        {
            OnPropertyChanged(nameof(doubleDeudaTotal));
            OnPropertyChanged(nameof(doubleFaltanteTotal));
        }
        #endregion // NotMapped
    }
    */
    #endregion // Table Deudores

    /*
    #region Table DeudorPago
    public class DBDeudorPagoClass : ObservableClass
    {
        #region Private
        DBDeudoresClass _Deudor; DBFechasClass _Fecha;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int FechaID { get; set; }
        public DBFechasClass Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = value; OnPropertyChanged(); } } }

        public int DeudorID { get; set; }
        public DBDeudoresClass Deudor { get => _Deudor; set { if (_Deudor != value) { _Deudor = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        #endregion // Navigation

        #region NotMapped
        #endregion // NotMapped
    }
    #endregion // Table DeudorPago
    */


    #region Table Producto Sacado
    /*
    public class sacadoProductosModel1 : ObservableClass
    {
        #region Private
        int _Cantidad; Double _Precio; productosModel _Producto; fechasModel _FechaSacado, _FechaPagado; usuariosModel _Usuario;
        #endregion // Private


        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set { if (_Cantidad != value) { _Cantidad = value; OnPropertyChanged(); OnPropertyChanged(nameof(PrecioTotal)); } } }
        public Double Precio { get => _Precio; set { value = Math.Round(value, 2); if (_Precio != value) { _Precio = value; OnPropertyChanged(); OnPropertyChanged(nameof(PrecioTotal)); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set { if (_Producto != value) { _Producto = value; OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }

        public int FechaSacadoID { get; set; }
        public virtual fechasModel FechaSacado { get => _FechaSacado; set { if (_FechaSacado != value) { _FechaSacado = value; OnPropertyChanged(); } } }

        public int? FechaPagadoID { get; set; }
        public virtual fechasModel FechaPagado { get => _FechaPagado; set { if (_FechaPagado != value) { _FechaPagado = value; OnPropertyChanged(); OnPropertyChanged(nameof(BolPagado)); } } }
        #endregion // Public


        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(Cantidad * Precio, 2);
        [NotMapped]
        public bool BolPagado => FechaPagado != null;
        #endregion // NotMapped
    }
    */
    #endregion // Table Producto Sacado



    #region Proveedores
    /*
    public class proveedoresModel1 : ObservableClass
    {
        #region Private
        int? _Telefono, _Celular; string _Nombre, _Direccion, _NumeroDeCliente, _Detalles; bool _Activo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public string Nombre { get => _Nombre; set { if (_Nombre != value) { _Nombre = value; OnPropertyChanged(); } } }
        public string Direccion { get => _Direccion; set { if (_Direccion != value) { _Direccion = value; OnPropertyChanged(); } } }
        public string NumeroDeCliente { get => _NumeroDeCliente; set { if (_NumeroDeCliente != value) { _NumeroDeCliente = value; OnPropertyChanged(); } } }
        public string Detalles { get => _Detalles; set { if (_Detalles != value) { _Detalles = value; OnPropertyChanged(); } } }
        public int? Telefono { get => _Telefono; set { if (_Telefono != value) { _Telefono = value; OnPropertyChanged(); } } }
        public int? Celular { get => _Celular; set { if (_Celular != value) { _Celular = value; OnPropertyChanged(); } } }
        public bool Activo { get => _Activo; set { if (_Activo != value) { _Activo = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<DBIngresosClass> IngresosPerProveedor { get; private set; } = new ObservableCollection<DBIngresosClass>();
        public virtual ICollection<DBRetirosCaja> RetirosCajaPerProveedor { get; private set; } = new ObservableCollection<DBRetirosCaja>();
        #endregion // Navigation
    }
    */
    #endregion Proveedores



    #region Table Ingresos
    /*
    public class ingresosModel1 : ObservableClass
    {
        #region Private
        Double _PagadoPesos, _PagadoMP; String _Hora; string _Detalle; proveedoresModel _Proveedor; usuariosModel _Usuario; fechasModel _Fecha;
        #endregion // Private


        #region Public
        public int Id { get; set; }

        public Double PagadoPesos { get => _PagadoPesos; set { value = Math.Round(value, 2); if (_PagadoPesos != value) { _PagadoPesos = value; OnPropertyChanged(); } } }
        public Double PagadoMP { get => _PagadoMP; set { value = Math.Round(value, 2); if (_PagadoMP != value) { _PagadoMP = value; OnPropertyChanged(); } } }

        public String Hora { get => _Hora; set { if (_Hora != value) { _Hora = Convert.ToDateTime(value).ToString("HH:mm:ss"); OnPropertyChanged(); } } }
        public string Detalle { get => _Detalle; set { if (_Detalle != value) { _Detalle = value; OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }

        public int ProveedorID { get; set; }
        public virtual proveedoresModel Proveedor { get => _Proveedor; set { if (_Proveedor != value) { _Proveedor = value; OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<DBIngresoProductosClass> IngresoProductosPerIngreso { get; private set; } = new ObservableCollection<DBIngresoProductosClass>();
        #endregion // Navigation


        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(IngresoProductosPerIngreso.Sum(x => x.PrecioTotal), 2);
        [NotMapped]
        public int ingresosCantidadProductosPerIngreso => IngresoProductosPerIngreso.Count();
        #endregion // NotMapped
    }
    */
    #endregion // Table Ingresos



    #region Table Producto Ingreso
    /*
    public class ingresoProductosModel1 : ObservableClass
    {
        #region Private
        int _Cantidad; Double _PrecioPagado, _PrecioActual; productosModel _Producto; ingresosModel _Ingreso;
        #endregion // Private


        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set { if (_Cantidad != value) { _Cantidad = value; OnPropertyChanged(); } } }
        public Double PrecioPagado { get => _PrecioPagado; set { value = Math.Round(value, 2); if (_PrecioPagado != value) { _PrecioPagado = value; OnPropertyChanged(); OnPropertyChanged(nameof(PrecioTotal)); OnPropertyChanged(nameof(PrecioSugerido)); } } }
        public Double PrecioActual { get => _PrecioActual; set { value = Math.Round(value, 2); if (_PrecioActual != value) { _PrecioActual = value; OnPropertyChanged(); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set { if (_Producto != value) { _Producto = value; OnPropertyChanged(); } } }

        public int IngresoID { get; set; }
        public virtual ingresosModel Ingreso { get => _Ingreso; set { if (_Ingreso != value) { _Ingreso = value; OnPropertyChanged(); } } }
        #endregion // Public


        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(Cantidad * PrecioPagado, 2);

        [NotMapped]
        public Double PrecioSugerido => Math.Round(PrecioPagado * 1.3, 2);
        #endregion // NotMapped
    }
    */
    #endregion // Table Producto Ingreso



    #region Table Ventas
    /*
    public class ventasModel1 : ObservableClass
    {
        #region Private
        cajaModel _Caja; String _Hora; fechasModel _Fecha; usuariosModel _Usuario; deudoresModel _Deudor;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int CajaId { get; set; }
        public virtual cajaModel Caja { get => _Caja; set { if (_Caja != value) { _Caja = value; OnPropertyChanged(); } } }

        public String Hora { get => _Hora; set { if (_Hora != value) { _Hora = Convert.ToDateTime(value).ToString("HH:mm:ss"); OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = value; OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }

        public int? DeudorID { get; set; }
        public virtual deudoresModel Deudor { get => _Deudor; set { if (_Deudor != value) { _Deudor = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<DBVentaProductosClass> VentaProductosPerVenta { get; private set; } = new ObservableCollection<DBVentaProductosClass>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(VentaProductosPerVenta.Sum(x => x.PrecioTotal), 2);
        [NotMapped]
        public int isVentaDeuda => VentaProductosPerVenta.All(x => x.isProductoDeuda == 0) ? 0 : VentaProductosPerVenta.All(x => x.isProductoDeuda == 2) ? 2 : 1;
        [NotMapped]
        public int isVentaPagado => VentaProductosPerVenta.All(x => x.BolPagado) ? 0 : VentaProductosPerVenta.All(x => x.BolPagado == false) ? 2 : 1;
        [NotMapped]
        public Double DeudaTotalVenta => Math.Round(VentaProductosPerVenta.Sum(x => x.TotalFaltante), 2);

        public void updatePrecios()
        {
            OnPropertyChanged(nameof(PrecioTotal));
        }
        #endregion // NotMapped
    }
    */
    #endregion // Table Ventas



    #region Table Producto Ventas
    /*
    public class ventaProductosModel1 : ObservableClass
    {
        #region Private
        int _Cantidad, _CantidadDeuda, _CantidadFaltante; Double _Precio, _PrecioPagado; productosModel _Producto; ventasModel _Venta; deudoresModel _Deudor; fechasModel _FechaPagado;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set { if (_Cantidad != value) { _Cantidad = value; OnPropertyChanged(); OnPropertyChanged(nameof(PrecioTotal)); } } }
        public int CantidadDeuda { get => _CantidadDeuda; set { if (_CantidadDeuda != value) { _CantidadDeuda = value; OnPropertyChanged(); } } }
        public int CantidadFaltante { get => _CantidadFaltante; set { if (_CantidadFaltante != value) { _CantidadFaltante = value; OnPropertyChanged(); } } }

        public Double Precio { get => _Precio; set { value = Math.Round(value, 2); if (_Precio != value) { _Precio = value; OnPropertyChanged(); OnPropertyChanged(nameof(PrecioTotal)); } } }
        public Double PrecioPagado { get => _PrecioPagado; set { value = Math.Round(value, 2); if (_PrecioPagado != value) { _PrecioPagado = value; OnPropertyChanged(); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set { if (_Producto != value) { _Producto = value; OnPropertyChanged(); } } }

        public int VentaID { get; set; }
        public virtual ventasModel Venta { get => _Venta; set { if (_Venta != value) { _Venta = value; OnPropertyChanged(); } } }

        public int? FechaPagadoID { get; set; }
        public virtual fechasModel FechaPagado { get => _FechaPagado; set { if (_FechaPagado != value) { _FechaPagado = value; OnPropertyChanged(); } } }

        public int? DeudorID { get; set; }
        public virtual deudoresModel Deudor { get => _Deudor; set { if (_Deudor != value) { _Deudor = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<cajaModel> CajasPerVentaProducto { get; private set; } = new ObservableCollection<cajaModel>();
        #endregion // Navigation


        #region NotMapped
        [NotMapped]
        public Double PrecioTotal => Math.Round(Cantidad * Precio, 2);

        [NotMapped]
        public int isProductoDeuda => CantidadDeuda == 0 ? 0 : CantidadDeuda < Cantidad ? 1 : 2;
        [NotMapped]
        public int isDeudaPagada => CantidadFaltante == 0 ? 0 : CantidadFaltante != CantidadDeuda ? 1 : 2;
        [NotMapped]
        public bool BolPagado => CantidadFaltante == 0;

        [NotMapped]
        public Double PrecioFinal => Deudor != null
                                        ? !BolPagado
                                            ? Deudor.Nivel switch
                                            {
                                                1 => Math.Round(Precio, 2),
                                                2 => Math.Round(Producto.PrecioActual, 2),
                                                _ => Math.Round(Producto.PrecioActual * 1.05, 2),
                                            }
                                            : Math.Round(PrecioPagado, 2)
                                        : 0;
        [NotMapped]
        public Double TotalFaltante { get { OnPropertyChanged(nameof(PrecioFinal)); return Math.Round(PrecioFinal * CantidadFaltante, 2); } }
        #endregion // NotMapped
    }
    */
    #endregion // Table Producto Ventas



    #region Table Producto Consumos
    /*
    public class consumoProductoModel1 : ObservableClass
    {
        #region Private
        int _Cantidad; Double _Precio; fechasModel _Fecha; productosModel _Producto;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set { if (_Cantidad != value) { _Cantidad = value; OnPropertyChanged(); } } }

        public Double Precio { get => _Precio; set { value = Math.Round(value, 2); if (_Precio != value) { _Precio = value; OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = value; OnPropertyChanged(); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set { if (_Producto != value) { _Producto = value; OnPropertyChanged(); } } }
        #endregion // Public
    }
    */
    #endregion // Table Producto Consumos



    #region Table Producto Modificado
    /*
    public class modificadoProductoModel1 : ObservableClass
    {
        #region Private
        int _Cantidad; productosModel _Producto; fechasModel _Fecha; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Cantidad { get => _Cantidad; set { if (_Cantidad != value) { _Cantidad = value; OnPropertyChanged(); OnPropertyChanged(nameof(stockFinal)); } } }

        public int ProductoID { get; set; }
        public virtual productosModel Producto { get => _Producto; set { if (_Producto != value) { _Producto = value; OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = value; OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }
        #endregion // Public

        [NotMapped]
        public int stockFinal => Producto != null ? Producto.Stock + Cantidad : 0;

        public void updateItem()
        {
            OnPropertyChanged(nameof(stockFinal));
        }
    }
    */
    #endregion // Table Producto Modificado



    #region Table Abierto
    /*
    public class abiertoProductosModel1 : ObservableClass
    {
        #region Private
        int _CantidadSacado, _CantidadAgregado; productosModel _ProductoSacado, _ProductoAgregado; fechasModel _Fecha; usuariosModel _Usuario;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int CantidadSacado { get => _CantidadSacado; set { if (_CantidadSacado != value) { _CantidadSacado = value; OnPropertyChanged(); } } }
        public int CantidadAgregado { get => _CantidadAgregado; set { if (_CantidadAgregado != value) { _CantidadAgregado = value; OnPropertyChanged(); } } }

        public int ProductoSacadoID { get; set; }
        public virtual productosModel ProductoSacado { get => _ProductoSacado; set { if (_ProductoSacado != value) { _ProductoSacado = value; OnPropertyChanged(); } } }

        public int ProductoAgregadoID { get; set; }
        public virtual productosModel ProductoAgregado { get => _ProductoAgregado; set { if (_ProductoAgregado != value) { _ProductoAgregado = value; OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = value; OnPropertyChanged(); } } }

        public int UsuarioID { get; set; }
        public virtual usuariosModel Usuario { get => _Usuario; set { if (_Usuario != value) { _Usuario = value; OnPropertyChanged(); } } }
        #endregion // Public
    }
    */
    #endregion // Table Abierto



    #region Table Retiros Caja Motivo
    /*
    public class motivoRetirosModel1 : ObservableClass
    {
        #region Private
        string _Motivo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public string Motivo { get => _Motivo; set { if (_Motivo != value) { _Motivo = value; OnPropertyChanged(); } } }
        #endregion // Public

        #region Navigation
        public virtual ICollection<DBRetirosCaja> Retiros { get; private set; } = new ObservableCollection<DBRetirosCaja>();
        #endregion // Navigation
    }
    */
    #endregion // Table Retiros Caja Motivo



    #region Table Retiros Caja
    /*
    public class retirosCajaModel1 : ObservableClass
    {
        #region Private
        String _Hora; cajaModel _Caja; string _Detalle; bool _Pendiente; motivoRetirosModel _Motivo; fechasModel _Fecha; usuariosModel _UsuarioAutoriza, _UsuarioRetira; proveedoresModel _Proveedor;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int CajaID { get; set; }
        public virtual cajaModel Caja { get => _Caja; set { if (_Caja != value) { _Caja = value; OnPropertyChanged(); } } }

        public String Hora { get => _Hora; set { if (_Hora != value) { _Hora = Convert.ToDateTime(value).ToString("HH:mm:ss"); OnPropertyChanged(); } } }
        public string Detalle { get => _Detalle; set { if (_Detalle != value) { _Detalle = value; OnPropertyChanged(); } } }

        public bool Pendiente { get => _Pendiente; set { if (_Pendiente != value) { _Pendiente = value; OnPropertyChanged(); } } }

        public int MotivoID { get; set; }
        public virtual motivoRetirosModel Motivo { get => _Motivo; set { if (_Motivo != value) { _Motivo = value; OnPropertyChanged(); } } }

        public int FechaID { get; set; }
        public virtual fechasModel Fecha { get => _Fecha; set { if (_Fecha != value) { _Fecha = value; OnPropertyChanged(); } } }

        public int? ProveedorID { get; set; }
        public virtual proveedoresModel Proveedor { get => _Proveedor; set { if (_Proveedor != value) { _Proveedor = value; OnPropertyChanged(); } } }

        public int UsuarioAutorizaID { get; set; }
        public virtual usuariosModel UsuarioAutoriza { get => _UsuarioAutoriza; set { if (_UsuarioAutoriza != value) { _UsuarioAutoriza = value; OnPropertyChanged(); } } }

        public int UsuarioRetiraID { get; set; }
        public virtual usuariosModel UsuarioRetira { get => _UsuarioRetira; set { if (_UsuarioRetira != value) { _UsuarioRetira = value; OnPropertyChanged(); } } }
        #endregion // Public
    }
    */
    #endregion // Table Retiros Caja
}
