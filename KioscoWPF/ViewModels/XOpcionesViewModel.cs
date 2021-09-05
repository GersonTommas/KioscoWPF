using System.Windows.Media;

namespace KioscoWPF.ViewModels
{
    class XOpcionesViewModel : Base.ViewModelBase
    {
        #region Initialize
        public XOpcionesViewModel(XOpciones tempWindow)
        {
            thisWindow = tempWindow;
            windowBackground = Properties.Settings.Default.WindowBackground;
            windowBackgroundNO = Properties.Settings.Default.WindowBackgroundNO;
            windowBackgroundOK = Properties.Settings.Default.WindowBackgroundOK;
        }
        #endregion // Initialize

        #region Colors
        string _windowBackground;
        string _windowBackgroundOK;
        string _windowBackgroundNO;
        public Color windowBackgroundItem { get => (Color)ColorConverter.ConvertFromString(Properties.Settings.Default.WindowBackground); }
        public string windowBackground { get => _windowBackground; set { if (_windowBackground != value) { _windowBackground = value; OnPropertyChanged(); } } }
        public string windowBackgroundOK { get => _windowBackgroundOK; set { if (_windowBackgroundOK != value) { _windowBackgroundOK = value; OnPropertyChanged(); } } }
        public string windowBackgroundNO { get => _windowBackgroundNO; set { if (_windowBackgroundNO != value) { _windowBackgroundNO = value; OnPropertyChanged(); } } }
        #endregion // Colors

        #region Commands
        public Command saveOptions => new Command(
            (object parameter) =>
            {
                Properties.Settings.Default.WindowBackground = windowBackground;
                Properties.Settings.Default.WindowBackgroundNO = windowBackgroundNO;
                Properties.Settings.Default.WindowBackgroundOK = windowBackgroundOK;
                Properties.Settings.Default.Save();
            });
        #endregion // Commands
    }
}
