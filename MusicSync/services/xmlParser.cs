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
            string excludedNodeName = "Library";
            List<XmlNode> PlaylistNodes = TempPlaylistNodes.Cast<XmlNode>()
                .Where(node => node.Name != excludedNodeName)
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

        public static List<msPlaylist> ExtractPlaylists(string filePath)
        {
            List<msPlaylist> Playlists = new List<msPlaylist>();
            List<XmlNode> PlaylistNodes = LoadPlaylists(filePath);
            foreach (XmlNode PlaylistNode in PlaylistNodes)
            {
                XmlNode nameNode = PlaylistNode.SelectSingleNode("key[text()='Name']");
                XmlNodeList trackIDsNode = PlaylistNode.SelectNodes("key[text()='Playlist Items']/following-sibling::array/dict/key[text()='Track ID']/following-sibling::integer");

                if (nameNode != null && trackIDsNode != null)
                {
                    string PlaylistName = nameNode.NextSibling.InnerText;

                    List<msTrack> tracks = new List<msTrack>();
                    foreach (XmlNode trackIDNode in trackIDsNode)
                    {
                        int trackID = int.Parse(trackIDNode.InnerText);
                        msTrack track = GetTrackInfo(LoadXml(filePath), trackID);
                        if (track != null)
                        {
                            tracks.Add(track);
                        }
                    }

                    msPlaylist Playlist = new msPlaylist
                    {
                        Name = PlaylistName,
                        TrackIDs = tracks
                    };

                    Playlists.Add(Playlist);
                }
            }

            return Playlists;
        }

        private static msTrack GetTrackInfo(XmlDocument xmlDoc, int trackID)
        {
            XmlNode trackNode = xmlDoc.SelectSingleNode($"//dict[key[text()='Track ID'] and following-sibling::integer[text()='{trackID}']]/following-sibling::dict");
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
