using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using System.Xml.Linq;

namespace MusicSync
{
    public static class xmlLibraryParser
    {
        /// <summary>
        /// Scans and gets all tracks added in iTunes after the last sync date. Returns a list of Track objects
        /// </summary>
        /// <param name="iTunesXmlDoc"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public static List<msTrack> GetTracksAddedAfterDate(string iTunesFilePath, XmlDocument iTunesXmlDoc, DateTime fromDate)
        {
            List<msTrack> tracksAddedAfterDate = new List<msTrack>();

            string xPathExpression = "//key[text()='Tracks']/following-sibling::dict[1]";
            XmlNode trackDictNode = iTunesXmlDoc.SelectSingleNode(xPathExpression);

            if (trackDictNode != null)
            {
                foreach (XmlNode trackNode in trackDictNode.ChildNodes)
                {
                    if (trackNode.Name == "dict")
                    {
                        XmlNode dateAddedNode = trackNode.SelectSingleNode("key[text()='Date Added']/following-sibling::date");
                        if (dateAddedNode != null)
                        {
                            DateTime dateAdded = DateTime.Parse(dateAddedNode.InnerText);
                            if (dateAdded > fromDate)
                            {
                                XmlNode trackTypeNode = trackNode.SelectSingleNode("key[text()='Track Type']/following-sibling::string");
                                if (trackTypeNode != null && trackTypeNode.InnerText == "File")
                                {
                                    string trackID = trackNode.SelectSingleNode("key[text()='Track ID']/following-sibling::string").InnerText;
                                    msTrack track = GetTrackInfo(iTunesFilePath, trackID);
                                    tracksAddedAfterDate.Add(track);
                                }
                            }
                        }
                    }
                }
            }

            return tracksAddedAfterDate;
        }

        /// <summary>
        /// Get track info from track ID in XML file and instanciates a new Track object
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="trackID"></param>
        /// <returns></returns>
        public static msTrack GetTrackInfo(string filePath, string trackID)
        {
            XmlNode trackNode = xmlFinder.FindTrackByID(filePath, trackID);
            if (trackNode != null)
            {
                XmlNode titleNode = trackNode.SelectSingleNode("key[text()='Name']");
                XmlNode artistNode = trackNode.SelectSingleNode("key[text()='Artist']");
                XmlNode albumArtistNode = trackNode.SelectSingleNode("key[text()='Album Artist']");
                XmlNode albumNode = trackNode.SelectSingleNode("key[text()='Album']");
                XmlNode locationNode = trackNode.SelectSingleNode("key[text()='Location']");

                if (titleNode != null && artistNode != null)
                {
                    string title = titleNode.NextSibling.InnerText;
                    string artist = artistNode.NextSibling.InnerText;
                    string albumArtist;
                    try
                    {
                        albumArtist = albumArtistNode.NextSibling.InnerText;
                    }
                    catch (NullReferenceException)
                    {
                        albumArtist = "Compilations";
                    }
                    string album = albumNode.NextSibling.InnerText;
                    string location = locationNode.NextSibling.InnerText;

                    msTrack track = new msTrack
                    {
                        TrackID = trackID,
                        Title = title,
                        Artist = artist,
                        AlbumArtist = albumArtist,
                        Album = album,
                        Location = location
                    };

                    return track;
                }
            }

            return null;
        }

    }
}



