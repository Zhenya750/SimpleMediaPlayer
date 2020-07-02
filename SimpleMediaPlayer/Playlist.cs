using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMediaPlayer
{
    public class Playlist
    {
        public ObservableCollection<Mediafile> Mediafiles { get; private set; }
        public Mediafile CurrentMediafile { get; set; }
        private int _currentMediafileIndexWhenRemove;

        public Playlist()
        {
            Mediafiles = new ObservableCollection<Mediafile>();
            CurrentMediafile = null;
            _currentMediafileIndexWhenRemove = -1;
        }

        public Mediafile NextMediafile()
        {
            if (_currentMediafileIndexWhenRemove > -1 &&
                _currentMediafileIndexWhenRemove < Mediafiles.Count)
            {
                CurrentMediafile = Mediafiles[_currentMediafileIndexWhenRemove];
                _currentMediafileIndexWhenRemove = -1;
                return CurrentMediafile;
            }

            if (_currentMediafileIndexWhenRemove >= Mediafiles.Count)
            {
                _currentMediafileIndexWhenRemove = -1;
                return CurrentMediafile = Mediafiles.FirstOrDefault();
            }

            var _currentMediafileIndex = Mediafiles.IndexOf(CurrentMediafile);
            return _currentMediafileIndex > -1 ?
                CurrentMediafile = Mediafiles[(_currentMediafileIndex + 1) % Mediafiles.Count] :
                CurrentMediafile = null;
        }

        public Mediafile PreviousMediafile()
        {
            if (_currentMediafileIndexWhenRemove > -1 &&
                _currentMediafileIndexWhenRemove < Mediafiles.Count)
            {
                CurrentMediafile = Mediafiles[(Mediafiles.Count + _currentMediafileIndexWhenRemove - 1) % Mediafiles.Count];
                _currentMediafileIndexWhenRemove = -1;
                return CurrentMediafile;
            }

            if (_currentMediafileIndexWhenRemove >= Mediafiles.Count)
            {
                _currentMediafileIndexWhenRemove = -1;
                return CurrentMediafile = Mediafiles.LastOrDefault();
            }

            var _currentMediafileIndex = Mediafiles.IndexOf(CurrentMediafile);
            return _currentMediafileIndex > -1 ?
                CurrentMediafile = Mediafiles[(Mediafiles.Count + _currentMediafileIndex - 1) % Mediafiles.Count] :
                CurrentMediafile = null;
        }

        public void AddMediafiles(ICollection<Mediafile> mediafiles, int index = -1)
        {
            if (mediafiles == null || mediafiles.Count == 0)
            {
                return;
            }

            if (index < 0 || index >= Mediafiles.Count)
            {
                index = Math.Max(0, Mediafiles.Count);
            }

            if (index >= 0)
            {
                foreach (var mediafile in mediafiles)
                {
                    Mediafiles.Insert(index, mediafile);
                }
            }

            if (Mediafiles.Count == mediafiles.Count)
            {
                CurrentMediafile = Mediafiles.First();
            }
        }

        public void RemoveMediafile(Mediafile mediafile)
        {
            if (mediafile == CurrentMediafile)
            {
                _currentMediafileIndexWhenRemove = Mediafiles.IndexOf(CurrentMediafile);
            }

            Mediafiles.Remove(mediafile);
        }

    }
}
