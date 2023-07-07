using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MusicSync
{
    public static class xmlParser
    {

        public static XmlDocument LoadXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            return xmlDoc;
        }

        public static List<XmlNode> LoadPlaylists(string filePath)
        {
            XmlNodeList TempPlaylistNodes = LoadXml(filePath).SelectNodes("//dict[key/text()='Playlist ID']");
            List<XmlNode> PlaylistNodes = TempPlaylistNodes.Cast<XmlNode>()
                .Skip(29)
                .ToList();
            return PlaylistNodes;
        }

        public static List<String> ExtractPlaylistNames(string filePath)
        {
            List<string> PlaylistNames = new List<String>();
            List<XmlNode> PlaylistNodes = LoadPlaylists(filePath);
            foreach (XmlNode PlaylistNode in PlaylistNodes)
            {
                XmlNode nameNode = PlaylistNode.SelectSingleNode("key[text()='Name']");
                string PlaylistName = nameNode.NextSibling.InnerText;
                PlaylistNames.Add(PlaylistName);
            }

            return PlaylistNames;
         }

        public static (List<msFolder>, List<msPlaylist>) ExtractFoldersAndPlaylists(string filePath)
        {
            (msFolder, msPlaylist) Output;
            List<msFolder> Folders = new List<msFolder>();
            List<msPlaylist> Playlists = new List<msPlaylist>();

            List<XmlNode> PlaylistNodes = LoadPlaylists(filePath);

            foreach (XmlNode PlaylistNode in PlaylistNodes)
            {
                XmlNode nameNode = PlaylistNode.SelectSingleNode("key[text()='Name']");
                string Name = nameNode.NextSibling.InnerText;
                
                XmlNode IdNode = PlaylistNode.SelectSingleNode("key[text()='Playlist Persistent ID']");
                string Id = IdNode.NextSibling.InnerText;

                string Owner = "0";
                XmlNode OwnerNode = PlaylistNode.SelectSingleNode("key[text()='Parent Persistent ID']");
                if (OwnerNode != null)
                {
                    Owner = OwnerNode.NextSibling.InnerText;
                }

                Boolean isFolder = false;
                XmlNode keyFolder = PlaylistNode.SelectSingleNode("key[text()='Folder']");

                if (keyFolder != null)
                {
                    isFolder = true;
                    msFolder Folder = new msFolder
                    {
                        Id = Id,
                        OwnerRef = Owner,
                        Name = Name,
                    };
                    Folders.Add(Folder);
                    Console.WriteLine($"Folder : {Folder.Name} added");
                }
                else
                {

                    XmlNodeList trackIDsNode = PlaylistNode.SelectNodes("key[text()='Playlist Items']/following-sibling::array/dict/key[text()='Track ID']/following-sibling::integer");

                    if (nameNode != null && trackIDsNode != null)
                    {
                        List<msTrack> tracks = new List<msTrack>();
                        foreach (XmlNode trackIDNode in trackIDsNode)
                        {
                            int trackID = int.Parse(trackIDNode.InnerText);
                            msTrack track = GetTrackInfo(filePath, trackID);
                            if (track != null)
                            {
                                tracks.Add(track);
                            }
                        }

                        msPlaylist Playlist = new msPlaylist
                        {
                            Id = Id,
                            OwnerRef = Owner,
                            Name = Name,
                            TrackIDs = tracks
                        };

                        Playlists.Add(Playlist);
                        Console.WriteLine($"Playlist: {Playlist.Name} added");
                    }
                }
            }


            return (Folders, Playlists);
        }

        private static msTrack GetTrackInfo(string filePath, int trackID)
        {
            XmlNode trackNode = xmlFinder.FindTrackByID(filePath, trackID);
            if (trackNode != null)
            {
                XmlNode titleNode = trackNode.SelectSingleNode("key[text()='Name']");
                XmlNode artistNode = trackNode.SelectSingleNode("key[text()='Artist']");

                if (titleNode != null && artistNode != null)
                {
                    string title = titleNode.NextSibling.InnerText;
                    string artist = artistNode.NextSibling.InnerText;

                    msTrack track = new msTrack
                    {
                        TrackID = trackID,
                        Title = title,
                        Artist = artist
                    };

                    return track;
                }
            }

            return null;
        }
    }
}
