﻿using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleMediaPlayer
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Mediafile> _mediafiles;
        private List<Mediafile> _addingMediafiles;
        private int _addingMediafilesIndex;
        private Mediafile _currentMediafile;
        private Mediafile _prevMediafile;

        private Mediafile NextMediafile()
        {
            var mediafiles = LbMediafile.ItemsSource as ObservableCollection<Mediafile>;

            if (mediafiles.Contains(_currentMediafile) == false)
                return null;

            var _currentMediafileIndex = mediafiles.IndexOf(_currentMediafile);
            return mediafiles[(_currentMediafileIndex + 1) % mediafiles.Count];
        }

        private Mediafile PreviousMediafile()
        {
            var mediafiles = LbMediafile.ItemsSource as ObservableCollection<Mediafile>;

            if (_prevMediafile != null)
            {
                _currentMediafile = _prevMediafile;
                _prevMediafile = null;
                return _currentMediafile;
            }

            if (mediafiles.Contains(_currentMediafile) == false)
                return null;

            var _currentMediafileIndex = mediafiles.IndexOf(_currentMediafile);
            return mediafiles[(mediafiles.Count + _currentMediafileIndex - 1) % mediafiles.Count];
        }

        private void BRemoveLbiMediafile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var mediafiles = LbMediafile.ItemsSource as ObservableCollection<Mediafile>;
                var mediafileToRemove = (sender as Button).DataContext as Mediafile;
                _prevMediafile = PreviousMediafile();
                mediafiles.Remove(mediafileToRemove);

                if (mediafiles.Count == 0)
                {
                    _currentMediafile = null;
                }
                else if (mediafileToRemove == _currentMediafile)
                {
                    _currentMediafile = _prevMediafile;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LbiMediafile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                ListBoxItem lbi = sender as ListBoxItem;
                DragDrop.DoDragDrop(lbi, (Mediafile)lbi.DataContext, DragDropEffects.Move);
                lbi.IsSelected = true;
            }
        }

        private void LbMediafile_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (sender is ListBox)
            {
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);

                if (filenames != null && filenames.Length > 0)
                {
                    _addingMediafiles.Clear();

                    foreach (var filename in filenames)
                    {
                        string mimetype = MimeTypes.GetMimeType(filename);
                        if (mimetype != null)
                        {
                            var mediaInfo = new FileInfo(filename);

                            if (mimetype.StartsWith("audio") ||
                                mimetype.StartsWith("video"))
                            {
                                _addingMediafiles.Add(new Mediafile(mediaInfo.Name, mediaInfo.FullName));
                            }
                        }
                    }
                }

                _addingMediafilesIndex = (LbMediafile.ItemsSource as ObservableCollection<Mediafile>).Count;
            }
        }

        private void LbiMediafile_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                var entered = e.Data.GetData(typeof(Mediafile)) as Mediafile;
                var target = ((ListBoxItem)sender).DataContext as Mediafile;

                int srcIndex = LbMediafile.Items.IndexOf(entered);
                int tgtIndex = LbMediafile.Items.IndexOf(target);

                if (entered == null || srcIndex == -1)
                {
                    if (_addingMediafiles.Count > 0)
                    {
                        _addingMediafilesIndex = tgtIndex;
                    }
                }
                else
                {
                    (LbMediafile.ItemsSource as ObservableCollection<Mediafile>).Move(srcIndex, tgtIndex);
                }
            }
        }

        private void LbMediafile_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListBox)
            {
                AddMediafiles();
            }
        }

        private void BAddMediafiles_Click(object sender, RoutedEventArgs e)
        {
            _addingMediafilesIndex = (LbMediafile.ItemsSource as ObservableCollection<Mediafile>).Count;

            var openMediafilesDialog = new OpenFileDialog();
            openMediafilesDialog.Multiselect = true;
            openMediafilesDialog.CheckFileExists = true;
            openMediafilesDialog.Filter =
                "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;" +
                "*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;" +
                "*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;" +
                "*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;" +
                "*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;" +
                "*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;" +
                "*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";

            if (openMediafilesDialog.ShowDialog() == true)
            {
                _addingMediafiles = openMediafilesDialog.FileNames
                    .Select(file => new FileInfo(file))
                    .Select(mediaInfo => new Mediafile(mediaInfo.Name, mediaInfo.FullName))
                    .ToList();
            }

            AddMediafiles();
        }

        private void AddMediafiles()
        {
            if (_addingMediafiles == null ||
                _addingMediafiles.Count == 0)
            {
                return;
            }

            var mediafiles = LbMediafile.ItemsSource as ObservableCollection<Mediafile>;

            foreach (var mediafile in _addingMediafiles)
                mediafiles.Insert(_addingMediafilesIndex, mediafile);

            if (_currentMediafile == null)
            {
                // choose the first mediafile on default
                _currentMediafile = mediafiles.First();
            }

            _addingMediafiles.Clear();
            _addingMediafilesIndex = mediafiles.Count;
        }
    }
}