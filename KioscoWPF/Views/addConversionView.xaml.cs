﻿using System;
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
    /// Interaction logic for AddConversionView.xaml
    /// </summary>
    public partial class addConversionView : Window
    {
        public addConversionView(productosModel sentProducto)
        {
            InitializeComponent(); (DataContext as ViewModels.addConversionViewModel).setInitialize(this, sentProducto);
        }
    }
}
