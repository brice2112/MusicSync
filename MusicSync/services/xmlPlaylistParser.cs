using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MusicSync
{
    public static class xmlPlaylistParser
    {

        /// <summary>
        /// Get the list of playlists as XmlNodes and returns it as a list of strings
        /// </summary>
        /// <param name="iTunesXmlDoc"></param>
        /// <returns></returns>
        public static List<String> ExtractPlaylistNames(XmlDocument iTunesXmlDoc)
        {
            List<string> PlaylistNames = new List<String>();
            List<XmlNode> PlaylistNodes = LoadPlaylists(iTunesXmlDoc);
            foreach (XmlNode PlaylistNode in PlaylistNodes)
            {
                XmlNode nameNode = PlaylistNode.SelectSingleNode("key[text()='Name']");
                string PlaylistName = nameNode.NextSibling.InnerText;
                PlaylistNames.Add(PlaylistName);
            }

            //Exclude 

            return PlaylistNames;
         }

        /// <summary>
        /// Create instances of Folders and Playlists from the iTunes
        /// </summary>
        /// <param name="iTunesXmlDoc"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static (List<msFolder>, List<msPlaylist>) ExtractFoldersAndPlaylists(XmlDocument iTunesXmlDoc, string filePath)
        {
            (msFolder, msPlaylist) Output;
            List<msFolder> Folders = new List<msFolder>();
            List<msPlaylist> Playlists = new List<msPlaylist>();

            // Extract list of playlists as XML nodes from XML doc
            List<XmlNode> PlaylistNodes = LoadPlaylists(iTunesXmlDoc);
             
            // Loops on each node and sorts them in folders and playlists
            foreach (XmlNode PlaylistNode in PlaylistNodes)
            {
                XmlNode nameNode = PlaylistNode.SelectSingleNode("key[text()='Name']");
                string Name = nameNode.NextSibling.InnerText;            
                XmlNode IdNode = PlaylistNode.SelectSingleNode("key[text()='Playlist Persistent ID']");
                string Id = IdNode.NextSibling.InnerText;
                string Owner = "0";
                XmlNode OwnerNode = PlaylistNode.SelectSingleNode("key[text()='Parent Persistent ID']");

                // Case when it's a folder
                if (OwnerNode != null)
                {
                    Owner = OwnerNode.NextSibling.InnerText;
                }

                XmlNode keyFolder = PlaylistNode.SelectSingleNode("key[text()='Folder']");

                // Creates an instance of the Folder object and add it to the final list
                if (keyFolder != null)
                {
                    msFolder Folder = new msFolder
                    {
                        Id = Id,
                        OwnerRef = Owner,
                        Name = Name,
                    };
                    Folders.Add(Folder);
                    Console.WriteLine($"Folder : {Folder.Name} added");
                }

                // Case when it's a playlist
                else
                {
                    XmlNodeList trackIDsNode = PlaylistNode.SelectNodes("key[text()='Playlist Items']/following-sibling::array/dict/key[text()='Track ID']/following-sibling::integer");

                    if (nameNode != null && trackIDsNode != null)
                    {
                        List<msTrack> tracks = new List<msTrack>();
                        foreach (XmlNode trackIDNode in trackIDsNode)
                        {
                            string trackID = trackIDNode.InnerText;
                            msTrack track = xmlLibraryParser.GetTrackInfo(filePath, trackID);
                            if (track != null)
                            {
                                tracks.Add(track);
                            }
                        }

                        // Creates an instance of the Playlist object and add it to the final list
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

        // --- PRIVATE METHODS --- //

        /// <summary>
        /// Load playlists and exclude unwanted playlists (library & collections)
        /// </summary>
        /// <param name="iTunesXmlDoc"></param>
        /// <returns></returns>
        private static List<XmlNode> LoadPlaylists(XmlDocument iTunesXmlDoc)
        {
            // Initialize List of nodes for playlists in XML file
            string CollectionID = SettingsHandler.InitSettingsLoad()["collectionsFolderID"];
            XmlNodeList TempPlaylistNodes = iTunesXmlDoc.SelectNodes("//dict[key/text()='Playlist ID']");

            // Lists to exclude (as specified in settings file)
            //List<XmlNode> listsToExclude = new List<XmlNode>();
            List<string> excludedLists = SettingsHandler.getExcludedLists();
            //foreach (string excludeList in excludedLists)
            //{
            //    string excludeXpath = $"//dict[key='Name' and (string='{excludeList}')]";
            //    XmlNode listToExclude = iTunesXmlDoc.SelectSingleNode(excludeXpath);
            //    listsToExclude.Add(listToExclude);
            //}

            // Construct list of XmlNodes by adding playlists not included in the 'PlaylistsToExclude' lists
            List<XmlNode> PlaylistNodes = new List<XmlNode>();

            foreach (XmlNode node in TempPlaylistNodes)
            {
                XmlNode nameNode = node.SelectSingleNode("key[text()='Name']");
                string Name = nameNode.NextSibling.InnerText;
                if (!excludedLists.Contains(Name))
                {
                    PlaylistNodes.Add(node.Clone());
                }
            }
            return PlaylistNodes;
        }

        /// <summary>
        /// Returns true if a NodeList containes a given Node
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="nodeToCheck"></param>
        /// <returns></returns>
        private static bool ContainsNode(XmlNodeList nodeList, XmlNode nodeToCheck)
        {
            foreach (XmlNode node in nodeList)
            {
                if (node.OuterXml == nodeToCheck.OuterXml)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
