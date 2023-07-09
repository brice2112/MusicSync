using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MusicSync
{

    public class M3UExporter
    {

        public static void ExportFoldersAndPlaylists(List<msFolder> folders, string exportDirectory)
        {
            ExportFoldersAndPlaylists(folders, exportDirectory, "");
        }

        private static void ExportFoldersAndPlaylists(List<msFolder> folders, string exportDirectory, string parentFolderName)
        {
            foreach (msFolder folder in folders)
            {
                string folderName = Path.Combine(parentFolderName, folder.Name);
                string folderPath = Path.Combine(exportDirectory, folderName);
                Directory.CreateDirectory(folderPath);

                foreach (msPlaylist playlist in folder.Playlists)
                {
                    string playlistPath = Path.Combine(folderPath, $"{playlist.Name}.m3u");
                    string playlistContents = GeneratePlaylistContents(playlist);

                    File.WriteAllText(playlistPath, playlistContents);
                }

                ExportFoldersAndPlaylists(folder.Folders, exportDirectory, folderName);
            }
        }

        private static string GeneratePlaylistContents(msPlaylist playlist)
        {
            // Generate the M3U file contents based on the playlist object
            string locationRootReplacer = "file://localhost/D:/Music/iTunes";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#EXTM3U");
            foreach (msTrack track in playlist.TrackIDs)
            {
                // Add each track to the M3U file
                sb.AppendLine($"#EXTINF: {track.Title} - {track.Artist}");
                sb.AppendLine(track.Location.Replace(locationRootReplacer, @"a:\Music").Replace("%20", " ").Replace('/', '\\'));
                //sb.AppendLine(track.TrackLocation);
            }

            return sb.ToString();
        }
    }
}
