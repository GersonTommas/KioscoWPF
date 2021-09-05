using System.Windows;

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for addProductoView.xaml
    /// </summary>
    public partial class addProductoView : Window
    {
        public addProductoView() { InitializeComponent(); (DataContext as ViewModels.addProductoViewModel).setInitialize(this); }
        public addProductoView(string sentCodigo) { InitializeComponent(); (DataContext as ViewModels.addProductoViewModel).setInitialize(this, sentCodigo); }
        public addProductoView(productosModel sentProducto) { InitializeComponent(); (DataContext as ViewModels.addProductoViewModel).setInitialize(this, sentProducto); }
    }
}
