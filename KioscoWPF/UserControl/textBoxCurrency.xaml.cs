using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kiosco.WPF.userControl
{
    /// <summary>
    /// Interaction logic for textBoxCurrency.xaml
    /// </summary>
    public partial class textBoxCurrency : UserControl
    {
        public textBoxCurrency()
        {
            InitializeComponent();
        }

        public string labelContent { get => (string)GetValue(labelContentProperty); set { SetValue(labelContentProperty, value); OnPropChanged(); } }
        public string textboxText { get => (string)GetValue(textboxTextProperty); set { SetValue(textboxTextProperty, value); OnPropChanged(); } }
        public bool isHeaderTop { get => (bool)GetValue(isHeaderTopProperty); set { SetValue(isHeaderTopProperty, value); OnPropChanged(); } }
        public bool isCurrency { get => (bool)GetValue(isCurrencyProperty); set { SetValue(isCurrencyProperty, value); OnPropChanged(); } }
        public TextAlignment textAlignment { get => (TextAlignment)GetValue(textAlignmentProperty); set { SetValue(textAlignmentProperty, value); OnPropChanged(); } }
        public int inputType { get => (int)GetValue(inputTypeProperty); set { SetValue(inputTypeProperty, value); OnPropChanged(); } }
        public bool isMaster { get => (bool)GetValue(isMasterProperty); set { SetValue(isMasterProperty, value); OnPropChanged(); } }
        public bool selectAll { get => (bool)GetValue(selectAllProperty); set { SetValue(selectAllProperty, value); OnPropChanged(); if (value) { textBox.SelectAll(); selectAll = false; } } }
        public bool isLabelOn { get => (bool)GetValue(isLabelOnProperty); set { SetValue(isLabelOnProperty, value); OnPropChanged(); } }
        public bool isReadOnly { get => (bool)GetValue(isReadOnlyProperty); set { SetValue(isReadOnlyProperty, value); OnPropChanged(); } }
        public Command enterCommand { get => (Command)GetValue(enterCommandProperty); set { SetValue(enterCommandProperty, value); OnPropChanged(); } }
        public int textWidth { get => (int)GetValue(textWithdProperty); set { SetValue(textWithdProperty, value); OnPropChanged(); } }


        public static readonly DependencyProperty labelContentProperty = DependencyProperty.Register("labelContent", typeof(string), typeof(textBoxCurrency), new PropertyMetadata(""));
        public static readonly DependencyProperty textboxTextProperty = DependencyProperty.Register("textboxText", typeof(string), typeof(textBoxCurrency), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, false, UpdateSourceTrigger.PropertyChanged));
        public static readonly DependencyProperty isHeaderTopProperty = DependencyProperty.Register("isHeaderTop", typeof(bool), typeof(textBoxCurrency), new PropertyMetadata(false));
        public static readonly DependencyProperty isCurrencyProperty = DependencyProperty.Register("isCurrency", typeof(bool), typeof(textBoxCurrency), new PropertyMetadata(true));
        public static readonly DependencyProperty textAlignmentProperty = DependencyProperty.Register("textAlignment", typeof(TextAlignment), typeof(textBoxCurrency), new PropertyMetadata(TextAlignment.Right));
        public static readonly DependencyProperty inputTypeProperty = DependencyProperty.Register("inputType", typeof(int), typeof(textBoxCurrency), new PropertyMetadata(0));
        public static readonly DependencyProperty isMasterProperty = DependencyProperty.Register("isMaster", typeof(bool), typeof(textBoxCurrency), new PropertyMetadata(false));
        public static readonly DependencyProperty selectAllProperty = DependencyProperty.Register("selectAll", typeof(bool), typeof(textBoxCurrency), new PropertyMetadata(false));
        public static readonly DependencyProperty isLabelOnProperty = DependencyProperty.Register("isLabelOn", typeof(bool), typeof(textBoxCurrency), new PropertyMetadata(true));
        public static readonly DependencyProperty isReadOnlyProperty = DependencyProperty.Register("isReadOnly", typeof(bool), typeof(textBoxCurrency), new PropertyMetadata(false));
        public static readonly DependencyProperty enterCommandProperty = DependencyProperty.Register("enterCommand", typeof(Command), typeof(textBoxCurrency), new PropertyMetadata(null));
        public static readonly DependencyProperty textWithdProperty = DependencyProperty.Register("textWidth", typeof(int), typeof(textBoxCurrency), new PropertyMetadata(80));


        readonly System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        void initilizeClock() { Timer.Tick += new EventHandler(timer_Click); Timer.Interval = new TimeSpan(0, 0, 0, 0, 100); }
        void timer_Click(object sender, EventArgs e) { textBox.SelectAll(); Timer.Stop(); }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropChanged([CallerMemberName] string name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        void textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender; tb.SelectAll();
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (inputType == 1)
            {
                Variables.regexNumbers(sender, e);
            }
            else if (inputType == 2)
            {
                Variables.regexDouble(sender, e);
            }
            else if (inputType == 3)
            {
                Variables.regexNegativeNumbers(sender, e);
            }
        }

        void textBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (isMaster) { if (sender != null) { _ = (sender as TextBox).Focus(); (sender as TextBox).SelectAll(); initilizeClock(); Timer.Start(); } }
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    class textBoxBoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (targetType != typeof(bool)) { throw new InvalidOperationException("The target must be Bool."); }

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    class textBoxInverseBoolToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (targetType != typeof(bool)) { throw new InvalidOperationException("The target must be Bool."); }

            return (bool)value == false ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}
