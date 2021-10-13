using System;
using System.Windows;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for pagarVentaView.xaml
    /// </summary>
    public partial class pagarVentaView : Window
    {
        public Double resultPagadoPesos = 0;
        public Double resultMercadoPago = 0;
        public Double resultVuelto = 0;
        public bool resultPagarDeuda = false;
        public deudoresModel resultDeudor = null;

        public pagarVentaView(Double sentTotal)
        {
            InitializeComponent(); try { (DataContext as ViewModels.pagarVentaViewModel).setInitialize(this, sentTotal); } catch { }
        }
    }
}
