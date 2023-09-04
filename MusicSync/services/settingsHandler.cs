using System;
using System.Diagnostics;
using System.Xml;

namespace MusicSync
{
	public static class SettingsHandler
	{

        public static string appXmlPath = AppContext.BaseDirectory + @"\appsettings.xml";

        // // // // LOAD METHODS
        public static Dictionary<String, String> InitSettingsLoad()
        {
            XmlDocument settingsDoc = new XmlDocument();
            settingsDoc = LoadXml(appXmlPath);
            Dictionary<String, String> result = LoadSettingsValues(settingsDoc);
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
            result.Add("iTunesXmlPath", settingsDoc.SelectSingleNode("/settings/itunesxmlpath").InnerText);
            result.Add("exportDirectory", settingsDoc.SelectSingleNode("/settings/exportdirectory").InnerText);
            result.Add("lastSyncDate", settingsDoc.SelectSingleNode("/settings/lastsyncdate").InnerText);
            return result;
        }

        // // // // SAVE METHODS
        public static void SaveSyncDate()
        {
            XmlDocument settingsDoc = LoadXml(appXmlPath);
            DateTime now = DateTime.Now;
            String syncDate = now.ToString("yyyyMMdd");
            XmlNode dateNode = settingsDoc.SelectSingleNode("/settings/lastsyncdate");
            dateNode.InnerText = syncDate;
            settingsDoc.Save(appXmlPath);
        }
    }
}