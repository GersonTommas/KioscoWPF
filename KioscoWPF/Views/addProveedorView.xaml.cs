using System.Windows;

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for addProveedorView.xaml
    /// </summary>
    public partial class addProveedorView : Window
    {
        public addProveedorView() { InitializeComponent(); (DataContext as ViewModels.addProveedorViewModel).setInitialize(this); }
        public addProveedorView(proveedoresModel sentProveedor) { InitializeComponent(); (DataContext as ViewModels.addProveedorViewModel).setInitialize(this, sentProveedor); }
    }
}
