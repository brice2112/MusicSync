using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSync
{
    public class msFolder
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerRef { get; set; }
        public List<msFolder> Folders { get; set; }
        public List<msPlaylist> Playlists { get; set; }

        public msFolder()
        {
            Folders = new List<msFolder>();
            Playlists = new List<msPlaylist>();
        }

    }
}
