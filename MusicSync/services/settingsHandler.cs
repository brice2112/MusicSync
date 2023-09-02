using System;
using System.Diagnostics;
using System.Xml;

namespace MusicSync
{
	public class SettingsHandler
	{

        public static string appXmlPath = AppContext.BaseDirectory + @"\appsettings.xml";

        // // // // LOAD METHODS

        public static Dictionary<String, String> InitSettingsLoad()
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            XmlDocument settingsDoc = new XmlDocument();
            settingsDoc = LoadXml(appXmlPath);
            Dictionary<String, String> settingsTable = LoadSettingsValues(settingsDoc);
            return result;
        }

        public static XmlDocument LoadXml(string filePath)
        {
            XmlDocument iTunesXmlDoc = new();
            iTunesXmlDoc.Load(filePath);
            return iTunesXmlDoc;
        }

        private static Dictionary<String, String> LoadSettingsValues(XmlDocument settingsDoc)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            result.Add("iTunesXmlPath", settingsDoc.SelectSingleNode("itunesxmlpath").InnerText);
            result.Add("exportDirectory", settingsDoc.SelectSingleNode("exportdirectory").InnerText);
            result.Add("lastSyncDate", settingsDoc.SelectSingleNode("lastsyncdate").InnerText);
            return result;
        }


        // // // // SAVE METHODS
        public static void SaveSyncDate(XmlDocument settingsDoc)
        {
            settingsDoc = LoadXml(appXmlPath);
            DateTime now = DateTime.Now;
            String syncDate = now.ToString();
            XmlNode dateNode = settingsDoc.DocumentElement.SelectSingleNode("lastsyncdate");
            dateNode.Value = syncDate;
            settingsDoc.Save(appXmlPath);
        }
    }
}


