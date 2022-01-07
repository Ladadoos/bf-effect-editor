// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using Microsoft.Win32;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BattleForgeEffectEditor.Application.Utility
{
    class DialogService
    {
        public string SaveAsDialog(string title, string typeFilter, string defaultType)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Title = title,
                Filter = typeFilter,
                AddExtension = true,
                DefaultExt = defaultType
            };
            bool? result = fileDialog.ShowDialog();
            if (result == null || result == false)
                return string.Empty;
            return fileDialog.FileName;
        }

        public string OpenFileDialog(string title, string typeFilter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = title,
                Filter = typeFilter
            };
            bool? result = fileDialog.ShowDialog();
            if (result == null || result == false)
                return string.Empty;
            return fileDialog.FileName;
        }

        public string OpenDirectoryDialog(string title)
        {
            CommonOpenFileDialog folderBrowserDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = title
            };
            CommonFileDialogResult result = folderBrowserDialog.ShowDialog();
            return result == CommonFileDialogResult.Ok ? folderBrowserDialog.FileName : string.Empty;
        }

        public void ShowError(string text, string caption)
        {
            _ = MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowOK(string text, string caption)
        {
            _ = MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool ShowAreYouSure(string text, string caption)
        {
            MessageBoxResult result = MessageBox.Show(caption, text, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }
}
