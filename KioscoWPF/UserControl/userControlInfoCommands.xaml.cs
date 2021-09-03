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

namespace KioscoWPF.userControl
{
    /// <summary>
    /// Interaction logic for userControlInfoCommands.xaml
    /// </summary>
    public partial class userControlInfoCommands : UserControl, INotifyPropertyChanged
    {
        public userControlInfoCommands()
        {
            InitializeComponent();
        }


        public bool hasGuardar { get => (bool)GetValue(hasGuardarProperty); set { SetValue(hasGuardarProperty, value); OnPropChanged(); } }
        public bool hasTag { get => (bool)GetValue(hasTagProperty); set { SetValue(hasTagProperty, value); OnPropChanged(); } }
        public bool hasMedidas { get => (bool)GetValue(hasMedidasProperty); set { SetValue(hasMedidasProperty, value); OnPropChanged(); } }
        public bool hasProductos { get => (bool)GetValue(hasProductosProperty); set { SetValue(hasProductosProperty, value); OnPropChanged(); } }
        public bool hasSeleccionar { get => (bool)GetValue(hasSeleccionarProperty); set { SetValue(hasSeleccionarProperty, value); OnPropChanged(); } }
        public bool hasAbrirProducto { get => (bool)GetValue(hasAbrirProductoProperty); set { SetValue(hasAbrirProductoProperty, value); OnPropChanged(); } }

        public static readonly DependencyProperty hasGuardarProperty = DependencyProperty.Register("hasGuardar", typeof(bool), typeof(userControlInfoCommands), new PropertyMetadata(true));
        public static readonly DependencyProperty hasTagProperty = DependencyProperty.Register("hasTag", typeof(bool), typeof(userControlInfoCommands), new PropertyMetadata(false));
        public static readonly DependencyProperty hasMedidasProperty = DependencyProperty.Register("hasMedidas", typeof(bool), typeof(userControlInfoCommands), new PropertyMetadata(false));
        public static readonly DependencyProperty hasProductosProperty = DependencyProperty.Register("hasProductos", typeof(bool), typeof(userControlInfoCommands), new PropertyMetadata(false));
        public static readonly DependencyProperty hasSeleccionarProperty = DependencyProperty.Register("hasSeleccionar", typeof(bool), typeof(userControlInfoCommands), new PropertyMetadata(false));
        public static readonly DependencyProperty hasAbrirProductoProperty = DependencyProperty.Register("hasAbrirProducto", typeof(bool), typeof(userControlInfoCommands), new PropertyMetadata(false));


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropChanged([CallerMemberName] string name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
        #endregion // PropertyChanged
    }
}
