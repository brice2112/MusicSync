using static System.Net.Mime.MediaTypeNames;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using MusicSync.services;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

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

            // Load iTunes Xml
            XmlDocument iTunesXmlDoc = new();
            iTunesXmlDoc = SettingsHandler.LoadXml(settingsTable["iTunesXmlPath"]);

            // Get iTUnes XML file path and last sync date
            string iTunesFilePath = settingsTable["iTunesXmlPath"];
            DateTime lastSyncDate = DateTime.ParseExact(settingsTable["lastSyncDate"], "yyyyMMdd", CultureInfo.InvariantCulture);

            // Identify newly added tracks
            List<msTrack> newlyAddedTrackIds = xmlLibraryParser.GetTracksAddedAfterDate(iTunesFilePath, iTunesXmlDoc, lastSyncDate);

            // Copy tracks to a folder
            Console.WriteLine("Copy tracks");
            fileCopier.CopyTracksByIds(settingsTable["iTunesXmlPath"], newlyAddedTrackIds, settingsTable["exportDirectory"]+"/Songs");

            // Extract playlist names
            xmlPlaylistParser.ExtractPlaylistNames(iTunesXmlDoc);

            // Extract playlists and folders from XML file
            (List<msFolder>, List<msPlaylist>) FoldersAndPlaylist = xmlPlaylistParser.ExtractFoldersAndPlaylists(iTunesXmlDoc, iTunesFilePath);

            // Organize folders
            List<msFolder> Folders = FoldersAndPlaylist.Item1;
            List<msPlaylist> Playlists = FoldersAndPlaylist.Item2;
            List<msFolder> OrganizedPlaylists = playlistOrganizer.OrganizeFoldersAndPlaylists(Folders, Playlists);

            // Create folders and M3U playlist in a directory
            M3UExporter.ExportFoldersAndPlaylists(OrganizedPlaylists, settingsTable["exportDirectory"]);

            // Save settings file
            SettingsHandler.SaveSyncDate();

            // Finishing process
            Console.WriteLine("Done.");
            Console.ReadLine();
        }


    }
}
