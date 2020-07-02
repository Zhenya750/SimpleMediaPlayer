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
using System.Windows.Controls.Primitives;

namespace SimpleMediaPlayer
{
    public partial class MainWindow : Window
    {
        private Playlists _playlists;

        private void BAddNewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var textBox = cbPlaylists.Template.FindName("tbNewPlaylistTitle", cbPlaylists) as TextBox;
            if (textBox.Text != null && textBox.Text.Length > 0)
            {
                if (cbPlaylists.Items.Contains(textBox.Text) == false)
                {
                    cbPlaylists.Items.Add(textBox.Text);
                    _playlists.AddPlaylist(textBox.Text);
                    textBox.Text = "";
                }
            }
        }

        private void BRemovePlaylist_Click(object sender, RoutedEventArgs e)
        {
            string playlistTitle = (sender as Button).DataContext as string;
            cbPlaylists.Items.Remove(playlistTitle);
            _playlists.RemovePlaylist(playlistTitle);
        }

        private void CbPlaylists_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var playlistTitle = cbPlaylists.SelectedItem as string;

            if (playlistTitle == null)
            {
                if (cbPlaylists.Items.Count == 0)
                {
                    CreatePlaylist("Default");
                }
                else
                {
                    string anyPlaylist = cbPlaylists.Items[0] as string;
                    _playlists.SetCurrentPlaylist(anyPlaylist);
                    cbPlaylists.SelectedItem = anyPlaylist;
                }
            }
            else
            {
                _playlists.SetCurrentPlaylist(playlistTitle);
                LbMediafile.ItemsSource = _playlists.CurrentPlaylist.Mediafiles;
            }
        }

        private void CreatePlaylist(string title)
        {
            if (cbPlaylists.Items.Contains(title) == false)
            {
                cbPlaylists.Items.Add(title);
                _playlists.SetCurrentPlaylist(title);
                cbPlaylists.SelectedItem = title;
            }
        }
    }
}
