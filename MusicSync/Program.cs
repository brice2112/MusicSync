using static System.Net.Mime.MediaTypeNames;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace MusicSync
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string XmlPath = @"C:\Users\Admin\Music\iTunes\iTunes Music Library.xml";

            // Init: Get playlist names
            xmlParser.ExtractPlaylistNames(XmlPath);

            // Read XML using XMLDocument
            (List<msFolder>, List<msPlaylist>) FoldersAndPlaylist = xmlParser.ExtractFoldersAndPlaylists(XmlPath);

            // Organize folders
            List<msFolder> Folders = FoldersAndPlaylist.Item1;
            List<msPlaylist> Playlists = FoldersAndPlaylist.Item2;
            playlistController.OrganizeFoldersAndPlaylists(Folders, Playlists);

            //DeserializeXMl();

            Console.ReadLine();
        }

        //static void ReadXml(string filePath)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(filePath);
        //    XmlElement Root = doc.DocumentElement;
        //    if (Root != null)
        //    {
        //        foreach (XmlNode node in Root.ChildNodes)
        //        {
        //            String Header = node.InnerText;
        //            String Value = "";
        //            if (node.NextSibling != null)
        //            {
        //                Value = node.NextSibling.InnerText;
        //            }

        //            Console.WriteLine($"{Header} - {Value}");
        //        }
        //    }
        //}

        //static void DeserializeXMl()
        //{
        //    XmlReader xmlDoc = XmlReader.Create(@"C:\Users\Admin\source\repos\MusicSync\MusicSync\test.xml");
        //    XmlSerializer xmlSer = new XmlSerializer(typeof(String));
        //    String deser = (string)xmlSer.Deserialize(xmlDoc);
        //    Console.WriteLine(deser );
        //}
    }
}
