using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace SimpleMediaPlayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _mediafiles = new ObservableCollection<Mediafile>
            {
                new Mediafile("Some title", "some path"),
                new Mediafile("Some title 2", " somepath2"),
                new Mediafile("Something other title", ""),
            };

            LbMediafile.ItemsSource = _mediafiles;
            _addingMediafiles = new List<Mediafile>();

            
        }
    }
}
