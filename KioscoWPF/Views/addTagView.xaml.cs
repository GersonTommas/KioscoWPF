using System.Windows;

namespace Kiosco.WPF.Views
{
    /// <summary>
    /// Interaction logic for addTagView.xaml
    /// </summary>
    public partial class addTagView : Window
    {
        public addTagView() { InitializeComponent(); (DataContext as ViewModels.addTagViewModel).setInitialize(this); }
        public addTagView(tagsModel sentTag) { InitializeComponent(); (DataContext as ViewModels.addTagViewModel).setInitialize(this, sentTag); }
    }
}
