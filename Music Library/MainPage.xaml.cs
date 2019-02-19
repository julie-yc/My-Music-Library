using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
//below are added
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Pickers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Music_Library
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //test
        public readonly ObservableCollection<Song> Songs;

        public MainPage()
        {
            Songs = new ObservableCollection<Song>();
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await GetMusicFilesAsync();
            base.OnNavigatedTo(e);
            //first call GetMusicFilesAsync in Mainpage
            //then call OnNavigateTo in the base class (Page)
        }

        private async Task GetMusicFilesAsync()
        {
            QueryOptions options = new QueryOptions();
            options.FolderDepth = FolderDepth.Deep;
            options.FileTypeFilter.Add(".mp3");
            options.FileTypeFilter.Add(".m4a");

            // Get the Music library
            StorageFolder musicFolder = KnownFolders.MusicLibrary;
            var result = musicFolder.CreateFileQueryWithOptions(options);

            IReadOnlyList<StorageFile> musicFiles = await result.GetFilesAsync();
            foreach (StorageFile file in musicFiles)
            {
                // Only files on the local computer are supported. 
                // Files on OneDrive or a network location are excluded.
                if (file.Provider.Id == "computer")
                {
                    Songs.Add(await LoadSong(file));
                }
            }

            ImageGridView.ItemsSource = Songs;
        }

        //???
        private async Task<Song> LoadSong(StorageFile file)
        {
            MusicProperties properties = await file.Properties.GetMusicPropertiesAsync();
            return new Song(file, properties);
        }

        private void Albums_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Artists_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Songs_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Choose_Music_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new Windows.Storage.Pickers.FileOpenPicker();
            //make a collection of all video types you want to support.
            string[] fileTypes = new string[] { ".m4a", ".mp3" };

            //Add your fileTypes to the FileTypeFilter list of filePicker.
            foreach (string fileType in fileTypes)
            {
                filePicker.FileTypeFilter.Add(fileType);
            }

            //Set picker start location to the music library
            filePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;

            //Retrieve file from picker
            StorageFile file = await filePicker.PickSingleFileAsync();
            PlayMusic(file);
        }

        private void PlayMusic(StorageFile file)
        {
            if (!(file is null))
            {
                MediaSource _mediaSource = MediaSource.CreateFromStorageFile(file);
                MediaPlayer _mediaPlayer = new MediaPlayer();
                _mediaPlayer.Source = _mediaSource;
                mediaPlayer.SetMediaPlayer(_mediaPlayer);
                _mediaPlayer.AutoPlay = true;
            }
        }
    }
}
