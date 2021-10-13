namespace Kiosco.WPF.ViewModels
{
    class helperCantidadViewModel : Base.ViewModelBase
    {
        #region Variables
        int _intCantidad = 0;
        public int intCantidad { get => _intCantidad; set { if (_intCantidad != value) { _intCantidad = value; OnPropertyChanged(); } } }
        #endregion // Variables

        #region Commands
        public Command comResultado => new Command(
            (object parameter) => { (thisWindow as Views.helperCantidadView).intCantidad = intCantidad; thisWindow.DialogResult = true; },
            (object parameter) => intCantidad > 0);
        #endregion // Commands
    }
}
