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
    /// Interaction logic for textBoxCaja.xaml
    /// </summary>
    public partial class textBoxCaja : UserControl
    {
        public textBoxCaja()
        {
            InitializeComponent();
        }

        public string labelContent { get => (string)GetValue(labelContentProperty); set { SetValue(labelContentProperty, value); OnPropChanged(); } }
        public int textboxText { get => (int)GetValue(textboxTextProperty); set { SetValue(textboxTextProperty, value); OnPropChanged(); } }
        public Double textboxPesos { get => (Double)GetValue(textboxPesosProperty); set { SetValue(textboxPesosProperty, value); OnPropChanged(); } }
        public int intCount { get => (int)GetValue(intCountProperty); set { SetValue(intCountProperty, value); OnPropChanged(); } }
        public int inputType { get => (int)GetValue(inputTypeProperty); set { SetValue(inputTypeProperty, value); OnPropChanged(); } }
        public bool isMaster { get => (bool)GetValue(isMasterProperty); set { SetValue(isMasterProperty, value); OnPropChanged(); } }
        public bool selectAll { get => (bool)GetValue(selectAllProperty); set { SetValue(selectAllProperty, value); OnPropChanged(); if (value) { textBox.SelectAll(); selectAll = false; } } }
        public Command enterCommand { get => (Command)GetValue(enterCommandProperty); set { SetValue(enterCommandProperty, value); OnPropChanged(); } }


        public static readonly DependencyProperty labelContentProperty = DependencyProperty.Register("labelContent", typeof(string), typeof(textBoxCaja), new PropertyMetadata(""));
        public static readonly DependencyProperty textboxTextProperty = DependencyProperty.Register("textboxText", typeof(int), typeof(textBoxCaja), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, false, UpdateSourceTrigger.PropertyChanged));
        public static readonly DependencyProperty textboxPesosProperty = DependencyProperty.Register("textboxPesos", typeof(Double), typeof(textBoxCaja), new PropertyMetadata(0.00));
        public static readonly DependencyProperty intCountProperty = DependencyProperty.Register("intCount", typeof(int), typeof(textBoxCaja), new PropertyMetadata(0));
        public static readonly DependencyProperty inputTypeProperty = DependencyProperty.Register("inputType", typeof(int), typeof(textBoxCaja), new PropertyMetadata(0));
        public static readonly DependencyProperty isMasterProperty = DependencyProperty.Register("isMaster", typeof(bool), typeof(textBoxCaja), new PropertyMetadata(false));
        public static readonly DependencyProperty selectAllProperty = DependencyProperty.Register("selectAll", typeof(bool), typeof(textBoxCaja), new PropertyMetadata(false));
        public static readonly DependencyProperty enterCommandProperty = DependencyProperty.Register("enterCommand", typeof(Command), typeof(textBoxCaja), new PropertyMetadata(null));


        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        void initilizeClock() { Timer.Tick += new EventHandler(Timer_Click); Timer.Interval = new TimeSpan(0, 0, 0, 0, 100); }
        void Timer_Click(object sender, EventArgs e) { textBox.SelectAll(); Timer.Stop(); }



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
            if (isMaster) { if (sender != null) { (sender as TextBox).Focus(); (sender as TextBox).SelectAll(); initilizeClock(); Timer.Start(); } }
        }
    }
}
