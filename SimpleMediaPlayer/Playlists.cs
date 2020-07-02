using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMediaPlayer
{
    public class Playlists
    {
        private Dictionary<string, Playlist> _playlists;
        public Playlist CurrentPlaylist { get; private set; }

        public Playlists()
        {
            _playlists = new Dictionary<string, Playlist>();
            CurrentPlaylist = null;
        }

        public void SetCurrentPlaylist(string title)
        {
            if (_playlists.ContainsKey(title) == false)
            {
                _playlists.Add(title, new Playlist());
            }

            CurrentPlaylist = _playlists[title];
        }

        public void AddPlaylist(string title)
        {
            if (_playlists.ContainsKey(title) == false)
            {
                _playlists.Add(title, new Playlist());
            }
        }

        public void RemovePlaylist(string title)
        {
            _playlists.Remove(title);
        }
    }
}
