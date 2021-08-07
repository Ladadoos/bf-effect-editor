// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.Settings;
using BattleForgeEffectEditor.Application.Utility;
using BattleForgeEffectEditor.Models.DataAccess;
using System;
using System.IO;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class SpecialEffectTopBarViewModel : ObservableObject
    {
        public ICommand SaveCommand => new RelayCommand((_) => Save());

        public ICommand SaveAsCommand => new RelayCommand((_) => SaveAs());

        public ICommand OpenFileExplorerCommand => new RelayCommand((_) => OpenFileExplorer());

        public ICommand UnhideAllElementsCommand => new RelayCommand((_) => UnhideAllElements());

        public bool IsOpenFileExplorerEnabled => File.Exists(effectEditor.FullFilePath);

        private SpecialEffectEditorViewModel effectEditor;

        private SpecialEffectExportService exportService = new SpecialEffectExportService();
        private DialogService dialogService = new DialogService();
        private WindowsService windowsService = new WindowsService();
        private SettingsService settingsService = new SettingsService();

        public SpecialEffectTopBarViewModel(SpecialEffectEditorViewModel effectEditor)
        {
            this.effectEditor = effectEditor;
        }

        private void Save()
        {
            if (effectEditor.FullFilePath == string.Empty)
                SaveAs();
            else
                ExportTo(effectEditor.FullFilePath);
        }

        private void SaveAs()
        {
            string savePath = dialogService.SaveAsDialog("Save special effect as", "Fxb|*.fxb", "fxb");
            ExportTo(savePath);
        }

        public void OpenFileExplorer() => windowsService.TryOpenExplorerOnFile(effectEditor.FullFilePath);

        private void ExportTo(string fullFilePath)
        {
            if (fullFilePath == string.Empty)
                return;

            try
            {
                exportService.ExportEffect(effectEditor.SpecialEffect, fullFilePath);

                effectEditor.FullFilePath = fullFilePath;
                effectEditor.Title = Path.GetFileName(fullFilePath);
                RaisePropertyChanged(() => IsOpenFileExplorerEnabled);

                if (settingsService.GetFocusMapEditorOnSave())
                {
                    windowsService.PlayExclamationSound();
                    windowsService.PullUpAndFocusWindowsOfProcess("pluginbasededitor");
                } else
                    dialogService.ShowOK("Export successful", "Successfully exported fxb.");
            } catch (Exception e)
            {
                dialogService.ShowError("Exporting fxb failed", e.Message);
            }
        }

        private void UnhideAllElements()
        {
            foreach (ElementTreeItemViewModel treeItem in effectEditor.ElementTree.Elements)
                treeItem.RecursiveSetElementEnable(true);

            foreach (TrackListItemViewModel track in effectEditor.ElementEditor.TracksList.Tracks)
                track.IsTrackEnabled = true;
        }
    }
}
