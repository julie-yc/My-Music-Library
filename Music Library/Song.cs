using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Music_Library
{
    public class Song
    {
        public string Artist { get; set; }

        public string Title { get; set; }

        public string Album { get; set; }

        public StorageFile MusicFile { get; }

        public Song(StorageFile musicFile, MusicProperties properties)
        {
            MusicFile = musicFile;
            Artist = properties.Artist;
            Title = properties.Title;
            Album = properties.Album;
        }

    }
}