using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMediaPlayer
{
    public class Mediafile
    {
        private string _title;
        private string _path;

        public string Title
        {
            get { return _title; }
            private set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Path
        {
            get { return _path; }
            private set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }

        public Mediafile(string title, string path)
        {
            Title = title;
            Path = path;
        }

        private event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return String.Format($"Title {Title}, Path {Path}");
        }
    }
}
