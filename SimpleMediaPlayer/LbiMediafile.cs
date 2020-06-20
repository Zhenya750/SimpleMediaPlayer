using MimeKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private void BRemoveLbiMediafile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                (LbMediafile.ItemsSource as ObservableCollection<Mediafile>).Remove((Mediafile)btn.DataContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LbiMediafile_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine("mousedown");
            if (sender is ListBoxItem)
            {
                ListBoxItem lbi = sender as ListBoxItem;
                DragDrop.DoDragDrop(lbi, (Mediafile)lbi.DataContext, DragDropEffects.Move);
                lbi.IsSelected = true;
            }
        }

        private void LbiMediafile_Drop(object sender, DragEventArgs e)
        {

        }

        private void LbMediafile_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (sender is ListBox)
            {
                Console.WriteLine("dragenter");

                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);

                if (filenames != null && filenames.Length > 0)
                {
                    _addingMediafiles.Clear();

                    foreach (var filename in filenames)
                    {
                        string mimetype = MimeTypes.GetMimeType(filename);
                        if (mimetype != null)
                        {
                            var finfo = new FileInfo(filename);

                            if (mimetype.StartsWith("audio") ||
                                mimetype.StartsWith("video"))
                            {
                                _addingMediafiles.Add(new Mediafile(finfo.Name, finfo.FullName));
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
                Console.WriteLine("drag enter");
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

        private void LbMediafile_PreviewDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("drop");
            if (sender is ListBox)
            {
                foreach (var mediafile in _addingMediafiles)
                    (LbMediafile.ItemsSource as ObservableCollection<Mediafile>).Insert(_addingMediafilesIndex, mediafile);

                _addingMediafiles.Clear();
                _addingMediafilesIndex = (LbMediafile.ItemsSource as ObservableCollection<Mediafile>).Count;
            }
        }
    }
}