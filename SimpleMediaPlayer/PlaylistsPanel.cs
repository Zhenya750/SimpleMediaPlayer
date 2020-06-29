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
        private void BAddNewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var textBox = cbPlaylists.Template.FindName("tbNewPlaylistTitle", cbPlaylists) as TextBox;
            if (textBox.Text != null && textBox.Text.Length > 0)
            {
                if (cbPlaylists.Items.Contains(textBox.Text) == false)
                {
                    cbPlaylists.Items.Add(textBox.Text);
                    textBox.Text = "";
                }
            }
        }

        private void BRemovePlaylist_Click(object sender, RoutedEventArgs e)
        {
            cbPlaylists.Items.Remove((sender as Button).DataContext);
        }
    }
}
