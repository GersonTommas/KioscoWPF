using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KioscoWPF
{
    /// <summary>
    /// Interaction logic for xTestWindow.xaml
    /// </summary>
    public partial class xTestWindow : Window
    {
        public xTestWindow()
        {
            InitializeComponent(); DataContext = new WBinding(this);
        }

        class WBinding : ObservableClass
        {
            readonly xTestWindow thisWindow;
            public WBinding(xTestWindow tempWindow)
            {
                //thisWindow = tempWindow; Variables.Inventario.Tags.Local.CollectionChanged += delegate { listTagsView.Refresh(); };
                listTags = Variables.Inventario.Tags.Local.Where(x => x.Minimo < 5); OnPropertyChanged(nameof(listTags));
                
                listTagsView.Filter = delegate (object item) { return (item as DBTagsClass).Minimo < 5; };
            }

            int _someInt;
            int _otherInt;
            DBTagsClass _selectedTag;
            DBTagsClass _selectedTagView;
            CollectionViewSource collectionViewSource => new CollectionViewSource() { Source = Variables.Inventario.Usuarios.Local.ToObservableCollection() };

            public ICollectionView firstList
            {
                get
                {
                    ICollectionView temp = collectionViewSource.View;
                    temp.SortDescriptions.Clear(); temp.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Descending));
                    return temp;
                }
            }
            public ICollectionView secondList
            {
                get
                {
                    ICollectionView temp = collectionViewSource.View;
                    temp.SortDescriptions.Clear(); temp.SortDescriptions.Add(new SortDescription("Usuario", ListSortDirection.Ascending));
                    return temp;
                }
            }

            ICollectionView _listTagsView = new CollectionViewSource { Source = Variables.Inventario.Tags.Local.ToObservableCollection() }.View;

            public IEnumerable<DBTagsClass> listTags { get; set; }
            public ICollection<DBTagsClass> tempListTags  => Variables.Inventario.Tags.Local.ToObservableCollection();
            public ICollectionView listTagsView => _listTagsView;
            


            public DBTagsClass selectedTag { get => _selectedTag; set { if (_selectedTag != value) { _selectedTag = value; OnPropertyChanged(); } } }
            public DBTagsClass selectedTagView { get => _selectedTagView; set { if (_selectedTagView != value) { _selectedTagView = value; OnPropertyChanged(); } } }
            public int someInt { get => _someInt; set { if (_someInt != value) { _someInt = value; OnPropertyChanged(); } } }
            public int otherInt { get => _otherInt; set { if (_otherInt != value) { _otherInt = value; OnPropertyChanged(); } } }

            public Command comCreate => new Command((object parameter) => { Variables.Inventario.Tags.Local.Add(new DBTagsClass() { Activo = true, Minimo = someInt, Tag = "Test " + someInt }); Variables.Inventario.SaveChanges(); });
            public Command comDelete => new Command((object parameter) => { Variables.Inventario.Tags.Local.Remove(selectedTagView); Variables.Inventario.SaveChanges(); });
            public Command comChangeTag => new Command((object parameter) => { selectedTag.Minimo = someInt; Variables.Inventario.SaveChanges(); });
            public Command comChangeTagView => new Command((object parameter) => { selectedTagView.Minimo = otherInt; Variables.Inventario.SaveChanges(); });
        }
    }
}
