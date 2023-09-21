using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MusicSync.services
{
    public static class fileCopier
    {
        /// <summary>
        /// Copy all the tracks listed in the input track list into the destination folder and sort them by artist and album
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="trackIds"></param>
        /// <param name="destinationFolder"></param>
        public static void CopyTracksByIds(string filePath, List<msTrack> tracks, string destinationFolder)
        {
            if (tracks == null || tracks.Count == 0)
            {
                // No track IDs to copy, return early.
                return;
            }

            for (int i = 0; i < tracks.Count; ++i)
                {
                msTrack track = tracks[i];
                Console.Write("\rCopying new tracks...{0}%   ", i);
                Console.WriteLine();

                string trackFilePath = GetTrackFilePath(filePath, track);
                if (!string.IsNullOrEmpty(trackFilePath))
                {
                    string rootDestFilePath = Path.Combine(destinationFolder, Path.GetFileName(trackFilePath));
                    //string destTrackFilePath = $@"{rootDestFilePath}/{track.AlbumArtist}/{track.Album}/{Path.GetFileName(trackFilePath).Replace('\\', '/').Replace("@", "")}";
                    string destTrackFilePath = $@"{rootDestFilePath}/{track.AlbumArtist}/{track.Album}/{Path.GetFileName(trackFilePath)}";

                    try
                    {
                        File.Copy(trackFilePath, destTrackFilePath, true);
                        Console.WriteLine($"Copied track with ID: {track} to {destTrackFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error copying track with ID: {track}. {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Track with ID: {track} not found in the XML data.");
                }
            }
        }

        private static string GetTrackFilePath(string filePath, msTrack track)
        {
            // Implement the method to retrieve the file path for the given track ID
            // using XPath or LINQ to XML, similar to the previous methods.
            string trackId = track.TrackID;
            XmlNode trackNode = xmlFinder.FindTrackByID(filePath, trackId);

            XmlNode locationNode = trackNode.SelectSingleNode("key[text()='Location']");
            string location = locationNode.NextSibling.InnerText.Replace("%20", " ").Replace("file://localhost/","");
            return location;
        }
    }
}
