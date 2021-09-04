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

}
