using System;
using System.Windows;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for helperIngresoPreciosView.xaml
    /// </summary>
    public partial class helperIngresoPreciosView : Window
    {
        public int resultCantidad = 0;
        public Double resultPrecioActual = 0;
        public Double resultPrecioPagado = 0;

        public helperIngresoPreciosView(int sentCantidad = 0, Double sentPrecioActual = 0, Double sentPrecioPagado = 0)
        {
            InitializeComponent(); (DataContext as ViewModels.helperIngresoPreciosViewModel).setInitialize(this, sentCantidad, sentPrecioActual, sentPrecioPagado);
        }
    }
}
