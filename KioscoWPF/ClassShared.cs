using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace KioscoWPF
{
    #region Variables
    public class Variables
    {
        public static DBInventarioContext Inventario;
        public static VentanaPrincipal globalVentanaPrincipal;

        public static DBUsuariosClass UsuarioLogueado;
        public static string strFecha => DateTime.Today.ToString(@"yyyy/MM/dd");
        public static string strHora => DateTime.Now.ToString(@"HH:mm:ss");
        public static string varImageLector = "/Resources/Images/Lector01.png";
        public static string varImageLogo = "/Resources/Images/Logo02.png";

        public static string colorWindowBackgroundOK => Properties.Settings.Default.WindowBackgroundOK;
        public static string colorWindowBackkgroundNO => Properties.Settings.Default.WindowBackgroundNO;

        public static bool firstVenta = true;
        public static bool firstIngreso = true;
        public static bool firstDeuda = true;
        public static bool firstConsumo = true;
        public static bool firstSacado = true;
        public static bool firstCaja = true;
        public static bool firstRetiro = true;
        public static bool firstAbiertoProducto = true;

        public class messageError
        {
            #region Confirmación
            public static void Guardado() { /*MessageBox.Show("Item guardado", "Guardar");*/ }

            public static void StockRecalcular() { MessageBox.Show("Stock recalculado.", "Stock"); }

            public static bool NewProduct() { return MessageBox.Show("El producto no existe, desea agregarlo?", "Producto no existente", MessageBoxButton.YesNo) == MessageBoxResult.Yes; }

            public static bool AreYouSure() { return MessageBox.Show("Esta seguro que desea realizar esta modificación?", "Esta seguro?", MessageBoxButton.YesNo) == MessageBoxResult.Yes; }
            #endregion // Confirmación



            #region Errores
            public static void Usuario() { MessageBox.Show("Ya existe un registro con el mismo Usuario.", "Error Usuario"); }

            public static void LogIn() { MessageBox.Show("Usuario o contraseña incorrectos.", "Error Usuario/Contraseña"); }

            public static void Existencia() { MessageBox.Show("Ya existe un Item de éste tipo.", "Error Medida"); }

            public static void FechaErronea() { MessageBox.Show("El recuadro fecha no contiene una fecha válida.", "Error fecha"); }

            public static void CodigoExiste() { MessageBox.Show("Ya existe un producto con éste código.", "Código Duplicado"); }

            public static void CodigoNoExiste() { MessageBox.Show("No existe producto registrado con ese código", "Código Erroneo"); }
            #endregion // Errores
        }

        public static void nextTarget(object sender, bool reverse = false)
        {
            if (sender != null)
            {
                FocusNavigationDirection direction = FocusNavigationDirection.Next; if (reverse) { direction = FocusNavigationDirection.Previous; }

                Control ctrl = (Control)sender;
                _ = ctrl.Focus();

                _ = ctrl.MoveFocus(new TraversalRequest(direction));
            }
        }

        public static void regexNumbers(object sender, TextCompositionEventArgs e)
        {
            if (e != null)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
        }
        public static void regexNegativeNumbers(object sender, TextCompositionEventArgs e)
        {
            if (e != null)
            {
                Regex regex = new Regex("-?[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
        }

        public static void regexDouble(object sender, TextCompositionEventArgs e)
        {/*
            if (e != null)
            {
                Regex regex = new Regex(@"^-?\d(.\d+)?[^.]");
                //var testBol = Regex.Matches(e.Text, @"\.").Count > 1;
                e.Handled = regex.IsMatch(e.Text);
            }
            //e.Handled = double.TryParse(e.Text, out _);
            */
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            e.Handled = !(regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)) && !(e.Text.IndexOf(".") > 0 || e.Text.IndexOf(".") != e.Text.IndexOf(".")));
            
        }
    }
    #endregion // Variables

    #region Helpers
    #region Command
    public class Command : ICommand
    {
        public delegate void ICommandOnExecute(object parameter); public delegate bool ICommandOnCanExecute(object parameter);

        private ICommandOnExecute _execute; private ICommandOnCanExecute _canExecute;

        public Command(ICommandOnExecute onExecuteMethod, ICommandOnCanExecute onCanExecuteMethod = null) { _execute = onExecuteMethod; _canExecute = onCanExecuteMethod; }

        #region ICommand Members
        public event EventHandler CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
        public bool CanExecute(object parameter) { return _canExecute?.Invoke(parameter) ?? true; }
        public void Execute(object parameter = null) { _execute?.Invoke(parameter); }
        #endregion // ICommand Members
    }
    #endregion // Command

    #region Property Changed
    public abstract class ObservableClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
    }

    public abstract class helperObservableClass : ObservableClass
    {
        public VentanaHelperCantidad vHelperCantidad;
        public VentanaHelperPrecio vHelperPrecio;
        public VentanaHelperPrecioPrecioCantidad vHelperPrecioPrecioCantidad;
        public VentanaImagen vImagen;
        public VentanaLogIn vLogIn;
        public VentanaPrincipal vPrincipal;
        public VentanaSelectorProductoManual vSProductoManual;
        public VentanaPagarVenta vPagarVenta;


        public readonly System.Windows.Threading.DispatcherTimer _searchTimer = new System.Windows.Threading.DispatcherTimer();
        public virtual void initilizeSearchTimer() { _searchTimer.Tick += new EventHandler(searchTimer_Click); _searchTimer.Interval = new TimeSpan(0, 0, 0, 0, 150); }
        void searchTimer_Click(object sender, EventArgs e) { searchTimerClick(); _searchTimer.Stop(); }

        public virtual void searchTimerClick()
        {

        }
        public virtual void searchTimerRestart()
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        public bool abrirAgregarConsumo()
        {
            VentanaAgregarConsumo vTemp = new VentanaAgregarConsumo();
            return vTemp.ShowDialog().Value;
        }

        public bool abrirAgregarConvertido(DBProductosClass sentProducto)
        {
            VentanaAgregarConvertido vTemp = new VentanaAgregarConvertido(sentProducto);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirAgregarDeudor()
        {
            VentanaAgregarDeudor vTemp = new VentanaAgregarDeudor();
            return vTemp.ShowDialog().Value;
        }
        public bool abrirAgregarDeudor(DBDeudoresClass sentDeudor)
        {
            VentanaAgregarDeudor vTemp = new VentanaAgregarDeudor(sentDeudor);
            return vTemp.ShowDialog().Value;
        }

        public void abrirAgregarIngresos()
        {
            if (Application.Current.Windows.OfType<VentanaAgregarIngresos>().Any()) { _ = Application.Current.Windows.OfType<VentanaAgregarIngresos>().FirstOrDefault().Focus(); } else { VentanaAgregarIngresos temp = new VentanaAgregarIngresos(); temp.Show(); }
        }

        public bool abrirAgregarMedidas()
        {
            VentanaAgregarMedidas vTemp = new VentanaAgregarMedidas();
            return vTemp.ShowDialog().Value;
        }
        public bool abrirAgregarMedidas(DBMedidasClass sentmedida)
        {
            VentanaAgregarMedidas vTemp = new VentanaAgregarMedidas(sentmedida);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirAgregarProducto()
        {
            VentanaAgregarProducto vTemp = new VentanaAgregarProducto();
            return vTemp.ShowDialog().Value;
        }
        public bool abrirAgregarProducto(string sentString)
        {
            VentanaAgregarProducto vTemp = new VentanaAgregarProducto(sentString);
            return vTemp.ShowDialog().Value;
        }
        public bool abrirAgregarProducto(DBProductosClass sentProducto)
        {
            VentanaAgregarProducto vTemp = new VentanaAgregarProducto(sentProducto);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirAgregarProveedor()
        {
            VentanaAgregarProveedor vTemp = new VentanaAgregarProveedor();
            return vTemp.ShowDialog().Value;
        }
        public bool abrirAgregarProveedor(DBProveedorClass sentProveedor)
        {
            VentanaAgregarProveedor vTemp = new VentanaAgregarProveedor(sentProveedor);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirAgregarRetiroCaja()
        {
            VentanaAgregarRetiroCaja vTemp = new VentanaAgregarRetiroCaja();
            return vTemp.ShowDialog().Value;
        }

        public bool abrirAgregarSacado(DBUsuariosClass sentUsuario)
        {
            VentanaAgregarSacado vTemp = new VentanaAgregarSacado(sentUsuario);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirAgregarTag()
        {
            VentanaAgregarTag vTemp = new VentanaAgregarTag();
            return vTemp.ShowDialog().Value;
        }
        public bool abrirAgregarTag(DBTagsClass sentTag)
        {
            VentanaAgregarTag vTemp = new VentanaAgregarTag(sentTag);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirAgregarUsuario()
        {
            VentanaAgregarUsuario vTemp = new VentanaAgregarUsuario();
            return vTemp.ShowDialog().Value;
        }
        public bool abrirAgregarUsuario(DBUsuariosClass sentUsuario)
        {
            VentanaAgregarUsuario vTemp = new VentanaAgregarUsuario(sentUsuario);
            return vTemp.ShowDialog().Value;
        }

        public void abrirAgregarVentas()
        {
            if (Application.Current.Windows.OfType<VentanaAgregarVentas>().Any()) { Application.Current.Windows.OfType<VentanaAgregarVentas>().FirstOrDefault().Focus(); } else { VentanaAgregarVentas temp = new VentanaAgregarVentas(); temp.Show(); }
        }

        public bool abrirCaja(bool sentBol = false)
        {
            VentanaCaja vTemp = new VentanaCaja(sentBol);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirDetallesProductos(DBProductosClass sentProducto)
        {
            VentanaDetallesProductos vTemp = new VentanaDetallesProductos(sentProducto);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirModificarStockProducto(DBProductosClass sentProducto)
        {
            VentanaModificarStockProducto vTemp = new VentanaModificarStockProducto(sentProducto);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirPagarDeuda(DBDeudoresClass sentDeudor)
        {
            VentanaPagarDeuda vTemp = new VentanaPagarDeuda(sentDeudor);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirPagarSacado(DBUsuariosClass sentUsuario)
        {
            VentanaPagarSacado vTemp = new VentanaPagarSacado(sentUsuario);
            return vTemp.ShowDialog().Value;
        }

        public bool abrirRetirosCaja()
        {
            VentanaRetirosCaja vTemp = new VentanaRetirosCaja();
            return vTemp.ShowDialog().Value;
        }

        public bool abrirStock()
        {
            VentanaStock vTemp = new VentanaStock();
            return vTemp.ShowDialog().Value;
        }

        public bool bolIsOnlyOne { get; set; }

        public virtual Command comPrevTarget => new Command((object parameter) => { Variables.nextTarget(parameter, true); });
        public virtual Command comNextTarget => new Command((object parameter) => { Variables.nextTarget(parameter); });

    }
    #endregion // Property Changed

    #region Focus
    #endregion // Focus


    #region Converters
    [ValueConversion(typeof(Double), typeof(Double))]
    public class doubleInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(Double)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Double)value;
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (targetType != typeof(bool)) { throw new InvalidOperationException("The target must be Bool."); }

            return ((bool)value == true) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (targetType != typeof(bool)) { throw new InvalidOperationException("The target must be Bool."); }

            return ((bool)value == false) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value > 0 ? true : (object)false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return (bool)value ? 1 : 0; }
    }






    [ValueConversion(typeof(DBProductosClass), typeof(bool))]
    public class ProductStockComparerZero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int tempStock = ((DBProductosClass)value).Stock;

                return tempStock == 0;
            } catch { return false; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }


    [ValueConversion(typeof(DBProductosClass), typeof(bool))]
    public class ProductStockComparerMin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tempStock = ((DBProductosClass)value).Stock;
                var tempMinimo = ((DBProductosClass)value).Tag.Minimo;

                return tempStock > 0 && tempStock < tempMinimo;
            }
            catch { return false; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }


    [ValueConversion(typeof(DBProductosClass), typeof(bool))]
    public class ProductStockComparerEqual : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tempStock = ((DBProductosClass)value).Stock;
                var tempMinimo = ((DBProductosClass)value).Tag.Minimo;

                return tempStock == tempMinimo;
            }
            catch { return false; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }


    [ValueConversion(typeof(DBProductosClass), typeof(bool))]
    public class ProductStockComparerMax : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tempStock = ((DBProductosClass)value).Stock;
                var tempMinimo = ((DBProductosClass)value).Tag.Minimo;

                return tempStock > tempMinimo;
            }
            catch { return false; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
#endregion // Converters

    #endregion // Helpers
    class ClassShared
    {
    }

    public static class SendKeys
    {
        public static void Send(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key) { RoutedEvent = Keyboard.KeyDownEvent };
                    InputManager.Current.ProcessInput(e1);
                }
            }
        }
    }

}
