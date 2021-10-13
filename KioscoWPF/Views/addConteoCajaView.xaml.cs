using System.Windows;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for addConteoCajaView.xaml
    /// </summary>
    public partial class addConteoCajaView : Window
    {
        public addConteoCajaView(bool sentIsSalida = false)
        {
            InitializeComponent(); (DataContext as ViewModels.addConteoCajaViewModel).setInitialize(this, sentIsSalida);
        }
    }
}
