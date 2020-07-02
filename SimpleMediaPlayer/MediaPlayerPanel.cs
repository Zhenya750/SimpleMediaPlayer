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
        private DoubleAnimation _sTimelineAnimation = new DoubleAnimation();
        private bool _isMediaOpened = false;
        private int _invalidMediafilesCount = 0;
        private bool _previousButtonWasClicked = false;

        private void STimeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbPosition.Text = TimeSpan.FromSeconds(sTimeline.Value).ToString(@"hh\:mm\:ss");
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            _isMediaOpened = true;
            tbDuration.Text = mediaElement.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");

            sTimeline.Value =
                sTimeline.Value / sTimeline.Maximum
                * (sTimeline.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds);

            mediaElement.Position = TimeSpan.FromSeconds(sTimeline.Value);
            StartTimelineAnimation();

            _playlists.CurrentPlaylist.CurrentMediafile.IsValid = true;
            _playlists.CurrentPlaylist.CurrentMediafile.IsPlaying = true;
            _invalidMediafilesCount = 0;
            _previousButtonWasClicked = false;
            sTimeline.IsEnabled = true;
        }

        private void StartTimelineAnimation()
        {
            _sTimelineAnimation.From = sTimeline.Value;
            _sTimelineAnimation.To = sTimeline.Maximum;
            _sTimelineAnimation.Duration = mediaElement.NaturalDuration.TimeSpan - TimeSpan.FromSeconds(sTimeline.Value);

            sTimeline.BeginAnimation(Slider.ValueProperty, _sTimelineAnimation, HandoffBehavior.SnapshotAndReplace);
        }

        private void StopTimelineAnimation(bool saveAnimatedProperty = true)
        {
            var value = sTimeline.Value;
            sTimeline.BeginAnimation(Slider.ValueProperty, null);

            if (saveAnimatedProperty)
            {
                sTimeline.Value = value;
            }
        }

        private void STimeline_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (!_isMediaOpened)
            {
                return;
            }

            StopTimelineAnimation();
            // uncomment if video should pause while moving the timeline thumb
            mediaElement.Pause();
        }

        private void STimeline_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (!_isMediaOpened)
            {
                return;
            }

            mediaElement.Position = TimeSpan.FromSeconds(sTimeline.Value);

            if (mediaElement.Position == mediaElement.NaturalDuration.TimeSpan)
            {
                mediaElement.Play();
            }

            if (bPause.IsEnabled)
            {
                mediaElement.Play();
                StartTimelineAnimation();
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            bool startPlayingImmediately = bPause.IsEnabled;
            Next();
            if (startPlayingImmediately)
            {
                Play();
            }
        }

        private void MediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            _playlists.CurrentPlaylist.CurrentMediafile.IsValid = false;
            _invalidMediafilesCount++;
            if (_invalidMediafilesCount >= (LbMediafile.ItemsSource as ObservableCollection<Mediafile>).Count)
            {
                Stop();
                _invalidMediafilesCount = 0;
                _previousButtonWasClicked = false;
                mediaElement.Source = null;
                return;
            }

            if (_previousButtonWasClicked)
            {
                Previous();
            }
            else
            {
                Next();
            }

            Play();
        }        

        private void BPlay_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }

        private void BPause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        private void BPrevious_Click(object sender, RoutedEventArgs e)
        {
            _previousButtonWasClicked = true;
            bool startPlayingImmediately = bPause.IsEnabled;
            Previous();
            if (startPlayingImmediately)
            {
                Play();
            }
        }

        private void BNext_Click(object sender, RoutedEventArgs e)
        {
            bool startPlayingImmediately = bPause.IsEnabled;
            Next();
            if (startPlayingImmediately)
            {
                Play();
            }
        }

        private void SetPlayingState()
        {
            bPlay.IsEnabled = false;
            bPlay.Visibility = Visibility.Collapsed;
            bPause.IsEnabled = true;
            bPause.Visibility = Visibility.Visible;
        }

        private void SetPausedState()
        {
            bPlay.IsEnabled = true;
            bPlay.Visibility = Visibility.Visible;
            bPause.IsEnabled = false;
            bPause.Visibility = Visibility.Collapsed;
        }

        private void Play()
        {
            if (mediaElement.Source == null)
            {
                if (_playlists.CurrentPlaylist.CurrentMediafile == null)
                {
                    return;
                }

                mediaElement.Source = new Uri(_playlists.CurrentPlaylist.CurrentMediafile.Path);
            }

            mediaElement.Play();
            SetPlayingState();

            if (_isMediaOpened)
            {
                StartTimelineAnimation();
            }
        }

        private void Pause()
        {
            SetPausedState();
            mediaElement.Pause();
            StopTimelineAnimation();
        }

        private void Stop()
        {
            mediaElement.Stop();
            sTimeline.Value = sTimeline.Minimum;
            tbDuration.Text = "00:00:00";
            sTimeline.IsEnabled = false;
            StopTimelineAnimation(false);
            SetPausedState();
        }

        private void Next()
        {
            Stop();
            mediaElement.Close();
            _isMediaOpened = false;
            mediaElement.Source = null;

            if (_playlists.CurrentPlaylist.CurrentMediafile != null)
            {
                _playlists.CurrentPlaylist.CurrentMediafile.IsPlaying = false;
            }

            _playlists.CurrentPlaylist.NextMediafile();
        }

        private void Previous()
        {
            Stop();
            mediaElement.Close();
            _isMediaOpened = false;
            mediaElement.Source = null;

            if (_playlists.CurrentPlaylist.CurrentMediafile != null)
            {
                _playlists.CurrentPlaylist.CurrentMediafile.IsPlaying = false;
            }

            _playlists.CurrentPlaylist.PreviousMediafile();
        }
    }
}
