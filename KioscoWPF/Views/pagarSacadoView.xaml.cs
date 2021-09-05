using System.Windows;

namespace KioscoWPF.Views
{
    /// <summary>
    /// Interaction logic for pagarSacadoView.xaml
    /// </summary>
    public partial class pagarSacadoView : Window
    {
        public pagarSacadoView(usuariosModel sentUsuario) { InitializeComponent(); if (sentUsuario == null) { Close(); } try { (DataContext as ViewModels.pagarSacadoViewModel).setInitialize(this, sentUsuario); } catch { } }
    }
}
