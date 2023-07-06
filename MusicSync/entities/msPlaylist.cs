using MusicSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSync
{
    public class msPlaylist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerRef { get; set; }
        public List<msTrack> TrackIDs { get; set; }
    }
}
