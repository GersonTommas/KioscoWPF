using System.Windows;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for addVentaView.xaml
    /// </summary>
    public partial class addVentaView : Window
    {
        public addVentaView()
        {
            InitializeComponent(); (DataContext as ViewModels.addVentaViewModel).setInitialize(this);
        }
    }
}
