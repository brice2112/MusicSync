using static System.Net.Mime.MediaTypeNames;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using MusicSync.services;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;

namespace MusicSync
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            // Load main properties from XML settings file
            // // // // //
            Dictionary<String, String> settingsTable = SettingsHandler.InitSettingsLoad();
            // // // // //

            //Load iTunes Xml
            XmlDocument iTunesXmlDoc = new();
            iTunesXmlDoc = SettingsHandler.LoadXml(settingsTable["iTunesXmlPath"]);

            // Get last sync date
            DateTime lastSyncDate = DateTime.Parse(settingsTable["lastSyncDate"]);

            // Identify newly added tracks
            List<string> newlyAddedTrackIds = xmlLibraryParser.GetTracksAddedAfterDate(iTunesXmlDoc, lastSyncDate);

            // TEST  => Copy tracks to a folder
            string folder = "@C:\\Users\\Admin\\Music\\Cayin\\Songs";
            fileCopier.CopyTracksByIds(settingsTable["iTunesXmlPath"], newlyAddedTrackIds, folder);

            // Test: extract playlist names
            xmlPlaylistParser.ExtractPlaylistNames(iTunesXmlDoc);

            // Extract playlists and folders from XML file
            (List<msFolder>, List<msPlaylist>) FoldersAndPlaylist = xmlPlaylistParser.ExtractFoldersAndPlaylists(iTunesXmlDoc, settingsTable["iTunesXmlPath"]);

            // Organize folders
            List<msFolder> Folders = FoldersAndPlaylist.Item1;
            List<msPlaylist> Playlists = FoldersAndPlaylist.Item2;
            List<msFolder> OrganizedPlaylists = playlistOrganizer.OrganizeFoldersAndPlaylists(Folders, Playlists);

            // Create folders and M3U playlist in a directory
            M3UExporter.ExportFoldersAndPlaylists(OrganizedPlaylists, settingsTable["exportDirectory"]);

            // Save settings file
            SettingsHandler.SaveSyncDate();

            //DeserializeXMl();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }


    }
}
