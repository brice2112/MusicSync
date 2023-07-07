using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MusicSync
{
    public static class playlistController
    {
        public static List<msFolder> OrganizeFoldersAndPlaylists(List<msFolder> folders, List<msPlaylist> playlists)
        {
            List<msFolder> rootFolders = new List<msFolder>();
            Dictionary<string, msFolder> folderDictionary = folders.ToDictionary(f => f.Id);

            foreach (msFolder folder in folders)
            {
                if (folder.OwnerRef == "0") // Root folder (no parent)
                {
                    rootFolders.Add(folder);
                }
                else
                {
                    if (folderDictionary.TryGetValue(folder.OwnerRef, out msFolder parentFolder))
                    {
                        parentFolder.Folders.Add(folder);
                    }
                }
            }

            foreach (msPlaylist playlist in playlists)
            {
                if (playlist.Id == "0") // Root playlist (no parent folder)
                {
                    msFolder rootFolder = rootFolders.FirstOrDefault(f => f.Id == playlist.OwnerRef);
                    rootFolder?.Playlists.Add(playlist);
                }
                else
                {
                    if (folderDictionary.TryGetValue(playlist.Id, out msFolder parentFolder))
                    {
                        msFolder targetFolder = FindFolderByID(parentFolder, playlist.OwnerRef);
                        targetFolder?.Playlists.Add(playlist);
                    }
                }
            }

            return rootFolders;
        }

        private static msFolder FindFolderByID(msFolder folder, string folderID)
        {
            if (folder.Id == folderID)
            {
                return folder;
            }

            foreach (msFolder subFolder in folder.Folders)
            {
                msFolder targetFolder = FindFolderByID(subFolder, folderID);
                if (targetFolder != null)
                {
                    return targetFolder;
                }
            }

            return null;
        }
    }
}
