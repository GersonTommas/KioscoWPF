using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); Db.loadInventario(); DataContext = new WBinding(this);
            try { _ = Variables.Inventario.Fechas.Local.Single(x => x.Fecha == Variables.strFecha); } catch { Variables.Inventario.Fechas.Local.Add(new DBFechasClass() { Fecha = Variables.strFecha }); _ = Variables.Inventario.SaveChanges(); }
            if (Variables.Inventario.Fechas.Local.Any(x => x.Fecha == Variables.strFecha))
            {
                DBFechasClass tempFecha = Variables.Inventario.Fechas.Local.Single(x => x.Fecha == Variables.strFecha);
                if (tempFecha.VentasPerFecha.Count() > 0) { Variables.firstVenta = false; }
                if (tempFecha.CajasPerFecha.Count() > 0) { Variables.firstCaja = false; }
                if (tempFecha.AbiertoProductosPerFecha.Count() > 0) { Variables.firstAbiertoProducto = false; }
                if (tempFecha.IngresosPerFecha.Count() > 0) { Variables.firstIngreso = false; }
                if (tempFecha.RetirosPerFecha.Count() > 0) { Variables.firstRetiro = false; }
            }
        }

        #region Binding
        class WBinding : ObservableClass
        {
            #region Initialize
            public MainWindow thisWindow;

            public WBinding(MainWindow tempWindow)
            {
                thisWindow = tempWindow;
                SplashScreen splash = new SplashScreen(@"Resources\Images\Logo01.png");
                splash.Show(false);
                splash.Close(TimeSpan.FromSeconds(1));
                helperUpdateFinished();
            }
            #endregion Initialize



            #region Private
            VentanaLogIn vLogIn;
            #endregion Private



            #region Read Only
            #endregion // Read Only



            #region Public
            #endregion // Public



            #region Helpers
            public void helperUpdateFinished() { vLogIn = new VentanaLogIn(); vLogIn.Show(); thisWindow.Close(); }
            #endregion // Helpers



            #region Commands
            #endregion // Commands
        }
        #endregion // Binding
    }
}
