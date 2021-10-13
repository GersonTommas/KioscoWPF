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
    /// Interaction logic for userControlInfoColors.xaml
    /// </summary>
    public partial class userControlInfoColors : UserControl, INotifyPropertyChanged
    {
        public userControlInfoColors()
        {
            InitializeComponent();
        }

        public bool hasAgregado { get => (bool)GetValue(hasAgregadoProperty); set { SetValue(hasAgregadoProperty, value); OnPropChanged(); } }
        public bool hasMiddleColors { get => (bool)GetValue(hasMiddleColorsProperty); set { SetValue(hasMiddleColorsProperty, value); OnPropChanged(); } }
        public bool hasActivoInactivo { get => (bool)GetValue(hasActivoInactivoProperty); set { SetValue(hasActivoInactivoProperty, value); OnPropChanged(); } }
        public bool hasStock { get => (bool)GetValue(hasStockProperty); set { SetValue(hasStockProperty, value); OnPropChanged(); } }
        public bool isActivoFullLine { get => (bool)GetValue(isActivoFullLineProperty); set { SetValue(isActivoFullLineProperty, value); OnPropChanged(); } }
        public bool hasActiveAndStock { get => (bool)GetValue(hasActiveAndStockProperty); set { SetValue(hasActiveAndStockProperty, value); OnPropChanged(); } }


        public static readonly DependencyProperty hasAgregadoProperty = DependencyProperty.Register("hasAgregado", typeof(bool), typeof(userControlInfoColors), new PropertyMetadata(false));
        public static readonly DependencyProperty hasMiddleColorsProperty = DependencyProperty.Register("hasMiddleColors", typeof(bool), typeof(userControlInfoColors), new PropertyMetadata(false));
        public static readonly DependencyProperty hasActivoInactivoProperty = DependencyProperty.Register("hasActivoInactivo", typeof(bool), typeof(userControlInfoColors), new PropertyMetadata(false));
        public static readonly DependencyProperty hasStockProperty = DependencyProperty.Register("hasStock", typeof(bool), typeof(userControlInfoColors), new PropertyMetadata(true));
        public static readonly DependencyProperty isActivoFullLineProperty = DependencyProperty.Register("isActivoFullLine", typeof(bool), typeof(userControlInfoColors), new PropertyMetadata(true));
        public static readonly DependencyProperty hasActiveAndStockProperty = DependencyProperty.Register("hasActiveAndStock", typeof(bool), typeof(userControlInfoColors), new PropertyMetadata(false));


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropChanged([CallerMemberName] string name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
        #endregion // PropertyChanged
    }
}
