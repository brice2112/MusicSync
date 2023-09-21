using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;


namespace MusicSync
{
    public static class xmlFinder
    {

        public static XmlNode FindTrackByID(string filePath, string trackID)
        {
            string xmlText = File.ReadAllText(filePath);

            string pattern = $"<key>Persistent ID</key><string>{trackID}</string>";
            Match match = Regex.Match(xmlText, pattern);

            if (match.Success)
            {
                // The track ID was found in the XML text
                int startIndex = match.Index;
                int endIndex = xmlText.IndexOf("</dict>", startIndex) + "</dict>".Length;
                string matchedXml = xmlText.Substring(startIndex, endIndex - startIndex);

                // Wrap the matched XML in a root element
                string wrappedXml = $"<root>{matchedXml}</root>";

                XmlDocument iTunesXmlDoc = new XmlDocument();
                iTunesXmlDoc.LoadXml(wrappedXml);

                // Get the first parsed XML node of the track
                XmlNode trackNode = iTunesXmlDoc.DocumentElement.SelectSingleNode("//dict");
                return trackNode;
            }
            else
            {
                // The track ID was not found in the XML text
                return null;
            }
        }
    }
}
