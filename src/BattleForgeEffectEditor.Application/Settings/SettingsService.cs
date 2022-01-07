// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using JsonSettings;

namespace BattleForgeEffectEditor.Application.Settings
{
    public class SettingsService
    {
        private EditorStateSettings EditorStateSettings => JsonSettingsBase.Load<EditorStateSettings>();

        private EditorSettings EditorSettings => JsonSettingsBase.Load<EditorSettings>();

        public string GetResourcesDirectory() => EditorSettings.ResourcesDirectory;

        public void SetResourcesDirectory(string fullDirPath)
        {
            EditorSettings editorSettings = EditorSettings;
            editorSettings.ResourcesDirectory = fullDirPath;
            editorSettings.Save();
        }

        public bool GetAppInDarkTheme() => EditorSettings.AppInDarkTheme;

        public void SetAppDarkTheme(bool enable)
        {
            EditorSettings editorSettings = EditorSettings;
            editorSettings.AppInDarkTheme = enable;
            editorSettings.Save();
        }

        public string GetBackupDirectory() => EditorSettings.BackupDirectory;

        public void SetBackupDirectory(string fullDirPath)
        {
            EditorSettings editorSettings = EditorSettings;
            editorSettings.BackupDirectory = fullDirPath;
            editorSettings.Save();
        }

        public void CleanBackupDirectory()
        {
            string backupDir = GetBackupDirectory();
            if (string.IsNullOrEmpty(backupDir) || !Directory.Exists(backupDir))
                return;

            DirectoryInfo info = new DirectoryInfo(backupDir);
            FileInfo[] files = info.GetFiles().Where(p => p.Extension == ".fxb").OrderBy(p => p.CreationTime).ToArray();

            const int numBackups = 50;
            if (files.Length <= numBackups)
                return;

            int toDeleteFiles = files.Length - numBackups;
            foreach (FileInfo file in files)
            {
                File.Delete(file.FullName);

                toDeleteFiles--;
                if (toDeleteFiles <= 0)
                    break;
            }
        }

        public bool GetFocusMapEditorOnSave() => EditorSettings.FocusMapEditorOnSave;

        public void SetFocusMapEditorOnSave(bool focusOnSave)
        {
            EditorSettings editorSettings = EditorSettings;
            editorSettings.FocusMapEditorOnSave = focusOnSave;
            editorSettings.Save();
        }

        public string GetMapEditorDirectory() => EditorSettings.MapEditorDirectory;

        public void SetMapEditorDirectory(string fullFilePath)
        {
            EditorSettings editorSettings = EditorSettings;
            editorSettings.MapEditorDirectory = fullFilePath;
            editorSettings.Save();
        }

        public List<string> GetRecentPresentFiles()
        {
            EditorStateSettings editorStateSettings = EditorStateSettings;
            List<string> filePaths = new List<string>();
            List<string> toRemove = new List<string>();

            foreach (string filePath in editorStateSettings.RecentOpenedFiles)
            {
                if (File.Exists(filePath))
                    filePaths.Add(filePath);
                else
                    toRemove.Add(filePath);
            }

            if (toRemove.Count > 0)
            {
                foreach (string toRemovePath in toRemove)
                    editorStateSettings.RecentOpenedFiles.Remove(toRemovePath);

                editorStateSettings.Save();
            }

            return filePaths;
        }

        public void AddRecentlyOpenedFile(string filePath)
        {
            EditorStateSettings editorStateSettings = EditorStateSettings;
            List<string> recentFiles = editorStateSettings.RecentOpenedFiles;

            if (recentFiles.Contains(filePath))
                recentFiles.Remove(filePath);

            recentFiles.Insert(0, filePath);

            if (recentFiles.Count > 10)
                recentFiles.RemoveAt(recentFiles.Count - 1);

            editorStateSettings.Save();
        }
    }
}
