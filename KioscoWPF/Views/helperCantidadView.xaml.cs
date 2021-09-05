using System.Windows;

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for helperCantidadView.xaml
    /// </summary>
    public partial class helperCantidadView : Window
    {
        public int intCantidad = 1;

        public helperCantidadView()
        {
            InitializeComponent(); (DataContext as ViewModels.helperCantidadViewModel).setInitialize(this);
        }
    }
}
