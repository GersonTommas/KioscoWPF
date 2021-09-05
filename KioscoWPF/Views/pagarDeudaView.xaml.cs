using System.Windows;

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for pagarDeudaView.xaml
    /// </summary>
    public partial class pagarDeudaView : Window
    {
        public pagarDeudaView(deudoresModel sentDeudor)
        { InitializeComponent(); if (sentDeudor == null) { Close(); } (DataContext as ViewModels.pagarDeudaViewModel).setInitialize(this, sentDeudor); }
    }
}
