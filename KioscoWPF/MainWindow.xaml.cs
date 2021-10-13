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

namespace Kiosco.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); Db.loadInventario(); DataContext = new WBinding(this);
            try { _ = Variables.Inventario.Fechas.Local.Single(x => x.Fecha == Variables.strFecha); } catch { Variables.Inventario.Fechas.Local.Add(new fechasModel() { Fecha = Variables.strFecha }); _ = Variables.Inventario.SaveChanges(); }

            try { Variables.globalToday = Variables.Inventario.Fechas.Local.First(x => x.Fecha == Variables.strFecha); } catch { }
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



            #region Helpers
            public void helperUpdateFinished() { vLogIn = new VentanaLogIn(); vLogIn.Show(); thisWindow.Close(); }
            #endregion // Helpers
        }
        #endregion // Binding
    }
}
