// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.Settings;
using BattleForgeEffectEditor.Application.Utility;
using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.DataAccess;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public ICommand OpenSpecialEffectCommand => new RelayCommand((_) => OpenSpecialEffect());

        public ICommand ShutdownApplicationCommand => new RelayCommand((_) => ShutdownApplication());

        public ICommand CreateNewSpecialEffectCommand => new RelayCommand((_) => CreateNewSpecialEffect());

        public ICommand CreateStartupCommand => new RelayCommand((_) => CreateStartupWindow());

        public ICommand OpenMapEditorCommand => new RelayCommand((_) => OpenMapEditor());

        public DockManagerViewModel DockManagerViewModel { get; private set; } = new DockManagerViewModel();

        private SpecialEffectImportService importService = new SpecialEffectImportService();
        private DialogService dialogService = new DialogService();
        private WindowsService windowsService = new WindowsService();
        private SettingsService settingsService = new SettingsService();

        public MainWindowViewModel()
        {
            CreateStartupWindow();
            settingsService.CleanBackupDirectory();
        }

        private void CreateStartupWindow()
        {
            if (DockManagerViewModel.HasDockWindowOfType(typeof(StartupViewModel)))
                return;

            DockManagerViewModel.AddDockWindow(new StartupViewModel(this) { Title = "Startup" });
        }

        private void OpenMapEditor()
        {
            string mapEditorPath = settingsService.GetMapEditorDirectory();
            if (mapEditorPath == string.Empty || !File.Exists(mapEditorPath))
            {
                string fullPath = dialogService.OpenFileDialog("Select map editor", "Exe|*.exe");
                if (fullPath == string.Empty)
                    return;

                mapEditorPath = fullPath;
                settingsService.SetMapEditorDirectory(mapEditorPath);
            }

            if (windowsService.PullUpAndFocusWindowsOfProcess("pluginbasededitor"))
                return;

            string currentWorkDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(mapEditorPath));
            Process.Start(Path.GetFileName(mapEditorPath), "--no_file_cache");
            Directory.SetCurrentDirectory(currentWorkDir);
        }

        private void OpenSpecialEffect()
        {
            string fullPath = dialogService.OpenFileDialog("Open special effect file", "Fxb|*.fxb");
            if (fullPath == string.Empty)
                return;

            LoadSpecialEffect(fullPath);
        }

        public void LoadSpecialEffect(string fullFilePath)
        {
            SpecialEffect effect;
            try
            {
                effect = importService.LoadEffect(fullFilePath);
            } catch (Exception e)
            {
                dialogService.ShowError("Importing fxb failed", e.Message);
                return;
            }

            try
            {
                TryCreateBackupOf(fullFilePath);
            } catch (Exception e)
            {
                dialogService.ShowError("Failed to create backup", e.Message);
                return;
            }

            SpecialEffectEditorViewModel effectEditor = new SpecialEffectEditorViewModel(effect, fullFilePath)
            {
                Title = Path.GetFileNameWithoutExtension(fullFilePath)
            };
            DockManagerViewModel.AddDockWindow(effectEditor);
            settingsService.AddRecentlyOpenedFile(fullFilePath);
        }

        private void TryCreateBackupOf(string fullFilePath)
        {
            string backupDir = settingsService.GetBackupDirectory();
            if (backupDir == string.Empty)
                return;

            string fileName = Path.GetFileNameWithoutExtension(fullFilePath) + "-backup-" +
                DateTime.Now.ToString("MM/dd/yyyy-hh-mm-ss") + Path.GetExtension(fullFilePath);
            string destFullFilePath = Path.Combine(backupDir, fileName);

            if (File.Exists(destFullFilePath))
                return;

            File.Copy(fullFilePath, destFullFilePath);
        }

        public void CreateNewSpecialEffect()
        {
            SpecialEffect effect = new SpecialEffect();
            SpecialEffectEditorViewModel effectEditor = new SpecialEffectEditorViewModel(effect, string.Empty)
            {
                Title = "Unsaved" + new Random().Next(100)
            };
            DockManagerViewModel.AddDockWindow(effectEditor);
        }

        private void ShutdownApplication() => Environment.Exit(0);
    }
}
