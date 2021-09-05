using System.Windows;

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for addRetiroCajaView.xaml
    /// </summary>
    public partial class addRetiroCajaView : Window
    {
        public addRetiroCajaView()
        {
            InitializeComponent(); (DataContext as ViewModels.addRetiroCajaViewModel).setInitialize(this);
        }
    }
}
