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

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for AddConsumoView.xaml
    /// </summary>
    public partial class addConsumoView : Window
    {
        public addConsumoView()
        {
            InitializeComponent(); (DataContext as ViewModels.addConsumoViewModel).setInitialize(this);
        }
    }
}
