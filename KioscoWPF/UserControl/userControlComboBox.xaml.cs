using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for userControlComboBox.xaml
    /// </summary>
    public partial class userControlComboBox : UserControl, INotifyPropertyChanged
    {
        public userControlComboBox()
        {
            InitializeComponent();
        }

        public string labelContent { get => (string)GetValue(labelContentProperty); set { SetValue(labelContentProperty, value); OnPropChanged(); } }
        public bool isLabelOn { get => (bool)GetValue(isLabelOnProperty); set { SetValue(isLabelOnProperty, value); OnPropChanged(); } }
        public object itemsSource { get => GetValue(itemsSourceProperty); set { SetValue(itemsSourceProperty, value); OnPropChanged(); } }
        public object selectedItem { get => GetValue(selectedItemProperty); set { SetValue(selectedItemProperty, value); OnPropChanged(); } }


        public static readonly DependencyProperty labelContentProperty = DependencyProperty.Register("labelContent", typeof(string), typeof(userControlComboBox), new PropertyMetadata(""));
        public static readonly DependencyProperty isLabelOnProperty = DependencyProperty.Register("isLabelOn", typeof(bool), typeof(userControlComboBox), new PropertyMetadata(true));
        public static readonly DependencyProperty itemsSourceProperty = DependencyProperty.Register("itemsSource", typeof(object), typeof(userControlComboBox), new PropertyMetadata(null));
        public static readonly DependencyProperty selectedItemProperty = DependencyProperty.Register("selectedItem", typeof(object), typeof(userControlComboBox), new PropertyMetadata(null));


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropChanged([CallerMemberName] string name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
        #endregion // PropertyChanged
    }
}
