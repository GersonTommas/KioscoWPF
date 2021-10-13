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

namespace Kiosco.WPF
{
    #region Variables
    public class Variables
    {
        public static DBInventarioContext Inventario;
        public static VentanaPrincipal globalVentanaPrincipal;

        public static usuariosModel UsuarioLogueado;
        public static fechasModel globalToday;

        public static string strFecha => DateTime.Today.ToString(@"yyyy/MM/dd");
        public static string strHora => DateTime.Now.ToString(@"HH:mm:ss");
        public static string varImageLector = "/Resources/Images/Lector01.png";
        public static string varImageLogo = "/Resources/Images/Logo02.png";

        public static string colorWindowBackgroundOK => Properties.Settings.Default.WindowBackgroundOK;
        public static string colorWindowBackkgroundNO => Properties.Settings.Default.WindowBackgroundNO;

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
        //public VentanaHelperCantidad vHelperCantidad;
        //public VentanaHelperPrecio vHelperPrecio;
        //public VentanaHelperPrecioPrecioCantidad vHelperPrecioPrecioCantidad;
        public VentanaImagen vImagen;
        public VentanaLogIn vLogIn;
        public VentanaPrincipal vPrincipal;
        //public VentanaSelectorProductoManual vSProductoManual;
        //public VentanaPagarVenta vPagarVenta;


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

        public bool gOpenAddConsumo()
        {
            Views.addConsumoView vTemp = new Views.addConsumoView();
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenAddConversion(productosModel sentProducto)
        {
            Views.addConversionView vTemp = new Views.addConversionView(sentProducto);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenAddDeudor()
        {
            Views.addDeudorView vTemp = new Views.addDeudorView();
            return vTemp.ShowDialog().Value;
        }
        public bool gOpenAddDeudor(deudoresModel sentDeudor)
        {
            Views.addDeudorView vTemp = new Views.addDeudorView(sentDeudor);
            return vTemp.ShowDialog().Value;
        }

        public void gOpenAddIngreso()
        {
            if (Application.Current.Windows.OfType<Views.addIngresoView>().Any()) { _ = Application.Current.Windows.OfType<Views.addIngresoView>().FirstOrDefault().Focus(); } else { Views.addIngresoView temp = new Views.addIngresoView(); temp.Show(); }
        }

        public bool gOpenAddMedida()
        {
            Views.addMedidaView vTemp = new Views.addMedidaView();
            return vTemp.ShowDialog().Value;
        }
        public bool gOpenAddMedida(medidasModel sentmedida)
        {
            Views.addMedidaView vTemp = new Views.addMedidaView(sentmedida);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenAddProducto()
        {
            Views.addProductoView vTemp = new Views.addProductoView();
            return vTemp.ShowDialog().Value;
        }
        public bool gOpenAddProducto(string sentString)
        {
            Views.addProductoView vTemp = new Views.addProductoView(sentString);
            return vTemp.ShowDialog().Value;
        }
        public bool gOpenAddProducto(productosModel sentProducto)
        {
            Views.addProductoView vTemp = new Views.addProductoView(sentProducto);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenAddProveedor()
        {
            Views.addProveedorView vTemp = new Views.addProveedorView();
            return vTemp.ShowDialog().Value;
        }
        public bool gOpenAddProveedor(proveedoresModel sentProveedor)
        {
            Views.addProveedorView vTemp = new Views.addProveedorView(sentProveedor);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenAddRetiro()
        {
            Views.addRetiroCajaView vTemp = new Views.addRetiroCajaView();
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenAddSacado(usuariosModel sentUsuario)
        {
            Views.addSacadoView vTemp = new Views.addSacadoView(sentUsuario);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenAddTag()
        {
            Views.addTagView vTemp = new Views.addTagView();
            return vTemp.ShowDialog().Value;
        }
        public bool gOpenAddTag(tagsModel sentTag)
        {
            Views.addTagView vTemp = new Views.addTagView(sentTag);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenAddUsuario()
        {
            Views.addUsuarioView vTemp = new Views.addUsuarioView();
            return vTemp.ShowDialog().Value;
        }
        public bool gOpenAddUsuario(usuariosModel sentUsuario)
        {
            Views.addUsuarioView vTemp = new Views.addUsuarioView(sentUsuario);
            return vTemp.ShowDialog().Value;
        }

        public void gOpenAddVenta()
        {
            if (Application.Current.Windows.OfType<Views.addVentaView>().Any()) { Application.Current.Windows.OfType<Views.addVentaView>().FirstOrDefault().Focus(); } else { Views.addVentaView temp = new Views.addVentaView(); temp.Show(); }
        }

        public bool gOpenAddCaja(bool sentBol = false)
        {
            Views.addConteoCajaView vTemp = new Views.addConteoCajaView(sentBol);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenDetailsProductos(productosModel sentProducto)
        {
            Views.detallesProductoView vTemp = new Views.detallesProductoView(sentProducto);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenStockProducto(productosModel sentProducto)
        {
            Views.helperModificarStockView vTemp = new Views.helperModificarStockView(sentProducto);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenPagarDeuda(deudoresModel sentDeudor)
        {
            Views.pagarDeudaView vTemp = new Views.pagarDeudaView(sentDeudor);
            return vTemp.ShowDialog().Value;
        }

        public bool gOpenPagarSacado(usuariosModel sentUsuario)
        {
            Views.pagarSacadoView vTemp = new Views.pagarSacadoView(sentUsuario);
            return vTemp.ShowDialog().Value;
        }

        public bool bolIsOnlyOne { get; set; }

        public virtual Command comPrevTarget => new Command((object parameter) => { Variables.nextTarget(parameter, true); });
        public virtual Command comNextTarget => new Command((object parameter) => { Variables.nextTarget(parameter); });

    }
    #endregion // Property Changed


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






    [ValueConversion(typeof(productosModel), typeof(bool))]
    public class ProductStockComparerZero : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int tempStock = ((productosModel)value).Stock;

                return tempStock == 0;
            } catch { return false; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }


    [ValueConversion(typeof(productosModel), typeof(bool))]
    public class ProductStockComparerMin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tempStock = ((productosModel)value).Stock;
                var tempMinimo = ((productosModel)value).Tag.Minimo;

                return tempStock > 0 && tempStock < tempMinimo;
            }
            catch { return false; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }


    [ValueConversion(typeof(productosModel), typeof(bool))]
    public class ProductStockComparerEqual : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tempStock = ((productosModel)value).Stock;
                var tempMinimo = ((productosModel)value).Tag.Minimo;

                return tempStock == tempMinimo;
            }
            catch { return false; }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }


    [ValueConversion(typeof(productosModel), typeof(bool))]
    public class ProductStockComparerMax : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tempStock = ((productosModel)value).Stock;
                var tempMinimo = ((productosModel)value).Tag.Minimo;

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

}
