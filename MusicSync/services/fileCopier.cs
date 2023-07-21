﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MusicSync.services
{
    public static class fileCopier
    {
        public static void CopyTracksByIds(string filePath, List<string> trackIds, string destinationFolder)
        {
            if (trackIds == null || trackIds.Count == 0)
            {
                // No track IDs to copy, return early.
                return;
            }

            foreach (string trackId in trackIds)
            {
                string trackFilePath = GetTrackFilePath(filePath, trackId);
                if (!string.IsNullOrEmpty(trackFilePath))
                {
                    string destinationFilePath = Path.Combine(destinationFolder, Path.GetFileName(trackFilePath));

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
            string location = locationNode.NextSibling.InnerText;
            return location;
        }
    }
}
