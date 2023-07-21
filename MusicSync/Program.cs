using static System.Net.Mime.MediaTypeNames;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using MusicSync.services;

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
            // Main Properties
            // // // // //
            string xmlPath = @"C:\Users\Admin\Music\iTunes\iTunes Music Library.xml";
            string exportDirectory = @"C:\Users\Admin\Music\Cayin\Playlists";
            XmlDocument xmlDoc = new XmlDocument();
            // // // // //

            //Load Xml
            xmlDoc = LoadXml(xmlPath);

            // Test date
            DateTime testDate = new DateTime(2023, 7, 1);

            // Identify newly added tracks
            List<string> newlyAddedTrackIds = xmlLibraryParser.GetTracksAddedAfterDate(xmlDoc, testDate);

            // TEST  => Copy tracks to a folder
            string folder = "@C:\\Users\\Admin\\Music\\Cayin\\Songs";
            fileCopier.CopyTracksByIds(xmlPath, newlyAddedTrackIds, folder);

            // Test: extract playlist names
            xmlPlaylistParser.ExtractPlaylistNames(xmlDoc);

            // Extract playlists and folders from XML file
            (List<msFolder>, List<msPlaylist>) FoldersAndPlaylist = xmlPlaylistParser.ExtractFoldersAndPlaylists(xmlDoc, xmlPath);

            // Organize folders
            List<msFolder> Folders = FoldersAndPlaylist.Item1;
            List<msPlaylist> Playlists = FoldersAndPlaylist.Item2;
            List<msFolder> OrganizedPlaylists = playlistOrganizer.OrganizeFoldersAndPlaylists(Folders, Playlists);

            // Create folders and M3U playlist in a directory
            M3UExporter.ExportFoldersAndPlaylists(OrganizedPlaylists, exportDirectory);

            //DeserializeXMl();

            Console.WriteLine("Done.");
            Console.ReadLine();


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

        static XmlDocument LoadXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            return xmlDoc;
        }
    }
}
