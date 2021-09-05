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
    /// Interaction logic for addIngresoView.xaml
    /// </summary>
    public partial class addIngresoView : Window
    {
        public addIngresoView()
        {
            InitializeComponent(); try { (DataContext as ViewModels.addIngresoViewModel).setInitialize(this); } catch { }
        }
    }
}
