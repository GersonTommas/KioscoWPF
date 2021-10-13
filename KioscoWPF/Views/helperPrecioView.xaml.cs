using System;
using System.Windows;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for helperPrecioView.xaml
    /// </summary>
    public partial class helperPrecioView : Window
    {
        public Double resultPrecio;

        public helperPrecioView(Double sentPrecio = 0)
        {
            InitializeComponent(); resultPrecio = sentPrecio; (DataContext as ViewModels.helperPrecioViewModel).setInitialize(this, sentPrecio);
        }
    }
}
