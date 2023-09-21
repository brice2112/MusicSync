using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSync
{
    public class msTrack
    {
        public string TrackID { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string AlbumArtist { get; set; }
        public string Album { get; set; }
        public string Location { get; set; }
        public DateTime dateAdded { get; set; }
    }
}
