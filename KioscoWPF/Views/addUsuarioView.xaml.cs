using System.Windows;

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for addUsuarioView.xaml
    /// </summary>
    public partial class addUsuarioView : Window
    {
        public addUsuarioView() { InitializeComponent(); (DataContext as ViewModels.addUsuarioViewModel).setInitialize(this); }
        public addUsuarioView(usuariosModel sentUsuario) { InitializeComponent(); (DataContext as ViewModels.addUsuarioViewModel).setInitialize(this, sentUsuario); }
    }
}
