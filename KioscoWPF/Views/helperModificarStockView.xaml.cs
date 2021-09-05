using System.Windows;

namespace KioscoWPF.Views
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
