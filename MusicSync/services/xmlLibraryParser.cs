﻿using System;
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

        public static List<string> GetTracksAddedAfterDate(string xmlFilePath, DateTime fromDate)
        {
            List<string> tracksAddedAfterDate = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            string xPathExpression = "//key[text()='Tracks']/following-sibling::dict[1]";
            XmlNode trackDictNode = xmlDoc.SelectSingleNode(xPathExpression);

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
                                    // Here, you can access the <key> element of each track.
                                    tracksAddedAfterDate.Add(trackNode.PreviousSibling.InnerText);
                                }
                            }
                        }
                    }
                }
            }

            return tracksAddedAfterDate;
        }

        public static List<string> GetKeysForTracksAddedAfterDateLinq(string xmlFilePath, DateTime fromDate)
        {
            List<string> keysForTracksAddedAfterDate = new List<string>();

            XDocument xmlDoc = XDocument.Load(xmlFilePath);

            var trackKeys = xmlDoc.Descendants("key")
                .Where(key => key.Value == "Date Added")
                .SelectMany(key => key.ElementsAfterSelf("date"))
                .Where(date => DateTime.Parse(date.Value) > fromDate)
                .Select(date => date.PreviousNode)
                .OfType<XElement>()
                .Select(key => key.Value);

            keysForTracksAddedAfterDate.AddRange(trackKeys);

            return keysForTracksAddedAfterDate;
        }


    }
}


