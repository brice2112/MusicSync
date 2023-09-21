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

            foreach (string trackId in tracks)
            {
                string trackFilePath = GetTrackFilePath(filePath, trackId);
                if (!string.IsNullOrEmpty(trackFilePath))
                {
                    string rootDestFilePath = Path.Combine(destinationFolder, Path.GetFileName(trackFilePath)).Replace('\\', '/').Replace("@","");
                    string DestFilePath = rootDestFilePath + 

                    try
                    {
                        File.Copy(trackFilePath, destinationFilePath, true);
                        Console.WriteLine($"Copied track with ID: {trackId} to {destinationFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error copying track with ID: {trackId}. {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Track with ID: {trackId} not found in the XML data.");
                }
            }
        }

        private static string GetTrackFilePath(string filePath, string trackId)
        {
            // Implement the method to retrieve the file path for the given track ID
            // using XPath or LINQ to XML, similar to the previous methods.
            int intTrackId = int.Parse(trackId);
            XmlNode trackNode = xmlFinder.FindTrackByID(filePath, intTrackId);

            XmlNode locationNode = trackNode.SelectSingleNode("key[text()='Location']");
            string location = locationNode.NextSibling.InnerText.Replace("%20", " ").Replace("file://localhost/","");
            return location;
        }
    }
}
