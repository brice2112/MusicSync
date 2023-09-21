using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection.PortableExecutable;


namespace MusicSync
{
    public static class xmlFinder
    {

        public static XmlNode FindTrackByID(string filePath, string trackID)
        {
            XmlNode trackNode = null;

            // init file reader
            XmlReaderSettings settings = new XmlReaderSettings(); // to be optimized
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(filePath, settings);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "key")
                {
                    string keyName = reader.ReadElementContentAsString();

                    if (keyName == "Persistent ID" && reader.NodeType == XmlNodeType.Element && reader.Name == "string")
                    {
                        string persistentId = reader.ReadElementContentAsString();
                        if (persistentId == trackID)
                        {
                            // The desired Persistent ID is found, now let's find the parent <dict> element
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "dict")
                                {
                                    trackNode = ReadDictAsXmlNode(reader); // Load the <dict> as an XmlNode
                                    break; // Exit the inner loop
                                }
                            }
                            break; // Exit the outer loop
                        }
                    }
                }
            }

            return trackNode;
        }


        //if (found == true)
        //        {
        //            // The track ID was found in the XML text
        //            int startIndex = match.Index;
        //            int endIndex = xmlText.IndexOf("</dict>", startIndex) + "</dict>".Length;
        //            string matchedXml = xmlText.Substring(startIndex, endIndex - startIndex);

        //            // Wrap the matched XML in a root element
        //            string wrappedXml = $"<root>{matchedXml}</root>";

        //            XmlDocument iTunesXmlDoc = new XmlDocument();
        //            iTunesXmlDoc.LoadXml(wrappedXml);

        //            // Get the first parsed XML node of the track
        //            XmlNode trackNode = iTunesXmlDoc.DocumentElement.SelectSingleNode("//dict");
        //            return trackNode;
        //        }
        //        else
        //        {
        //            // The track ID was not found in the XML text
        //            return null;
        //        }
        //    }
        //}

        // Function to read the <dict> as an XmlNode
        private static XmlNode ReadDictAsXmlNode(XmlReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(reader.ReadOuterXml()); // Load the <dict> and its content as XML
            return xmlDoc.DocumentElement;
        }

    }
}