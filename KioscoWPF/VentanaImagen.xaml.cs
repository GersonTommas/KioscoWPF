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

namespace Kiosco.WPF
{
    /// <summary>
    /// Interaction logic for VentanaImagen.xaml
    /// </summary>
    public partial class VentanaImagen : Window
    {
        WBinding WBind;

        public VentanaImagen(string imageSource)
        {
            InitializeComponent(); WBind = new WBinding(this, imageSource); DataContext = WBind;
        }

        #region Binding
        class WBinding : ObservableClass
        {
            #region Initialize
            public WBinding(VentanaImagen tempWindow, string tempImageSource)
            {
                thisWindow = tempWindow;
                imgSource = tempImageSource;
            }
            #endregion // Initialize

            #region Private
            VentanaImagen thisWindow;
            private string _imgSource;
            #endregion // Private

            #region Public
            public string imgSource { get { return _imgSource; } set { if (_imgSource != value) { _imgSource = value; OnPropertyChanged(); } } }
            #endregion // Public

                #region Commands
            public Command comCerrar
            {
                get
                {
                    return new Command(
                  (object parameter) => { thisWindow.Close(); },
                  (object parameter) => { return true; });
                }
            }
            #endregion // Commands
        }
        #endregion // Binding
    }
}
