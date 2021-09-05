using System.Windows;

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for addSacadoView.xaml
    /// </summary>
    public partial class addSacadoView : Window
    {
        public addSacadoView(usuariosModel sentUsuario)
        {
            InitializeComponent(); (DataContext as ViewModels.addSacadoViewModel).setInitialize(this, sentUsuario);
        }
    }
}
