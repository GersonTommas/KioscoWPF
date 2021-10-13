using System.Windows;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for helperModificarStockView.xaml
    /// </summary>
    public partial class helperModificarStockView : Window
    {
        public helperModificarStockView(productosModel sentProducto)
        {
            InitializeComponent(); try { (DataContext as ViewModels.helperModificarStockViewModel).setInitialize(this, sentProducto); } catch { }
        }
    }
}
