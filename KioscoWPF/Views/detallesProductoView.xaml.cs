using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for detallesProductoView.xaml
    /// </summary>
    public partial class detallesProductoView : Window
    {
        public detallesProductoView(productosModel sentProducto)
        {
            InitializeComponent(); (DataContext as ViewModels.detallesProductoViewModel).setInitialize(this, sentProducto);
        }
    }
}
