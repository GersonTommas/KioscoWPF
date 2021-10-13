using System.Windows;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for addMedidaView.xaml
    /// </summary>
    public partial class addMedidaView : Window
    {
        public addMedidaView() { InitializeComponent(); (DataContext as ViewModels.addMedidaViewModel).setInitialize(this); }
        public addMedidaView(medidasModel sentMedida) { InitializeComponent(); (DataContext as ViewModels.addMedidaViewModel).setInitialize(this, sentMedida); }
    }
}
