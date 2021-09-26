// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.Resources.Themes;
using BattleForgeEffectEditor.Application.Settings;
using BattleForgeEffectEditor.Application.ViewModel.GenericControls;
using BattleForgeEffectEditor.Models.Enums;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class StartupRecentFileViewModel : ObservableObject
    {
        public ICommand OpenSpecialEffectCommand => new RelayCommand(OpenSpecialEffect);

        

        private string fullFilePath;
        public string FullFilePath => fullFilePath;

        private StartupViewModel startup;

        public StartupRecentFileViewModel(StartupViewModel startup, string fullFilePath)
        {
            this.startup = startup;
            this.fullFilePath = fullFilePath;
        }

        private void OpenSpecialEffect(object _) => startup.LoadSpecialEffect(fullFilePath);
    }

    public class StartupViewModel : DockWindowViewModel
    {
        public bool FocusMapEditorOnSave
        {
            get => settingsService.GetFocusMapEditorOnSave();
            set
            {
                settingsService.SetFocusMapEditorOnSave(value);
                OnPropertyChanged();
            }
        }

        public ICommand SwitchDarkMode => new RelayCommand((_) => SwitchDarkTheme());

        public bool IsDarkMode { get; set; }

        public ObservableCollection<StartupRecentFileViewModel> RecentFiles { get; private set; } =
                new ObservableCollection<StartupRecentFileViewModel>();

        public DirectoryChooserViewModel BackupDirectoryChooser { get; private set; }
        public DirectoryChooserViewModel ResourcesDirectoryChooser { get; private set; }

        private MainWindowViewModel mainWindow;

        private SettingsService settingsService = new SettingsService();

        public StartupViewModel(MainWindowViewModel mainWindow)
        {
            this.mainWindow = mainWindow;

            BackupDirectoryChooser = new DirectoryChooserViewModel()
            {
                OnDirectorySet = (dir) => settingsService.SetBackupDirectory(dir),
                OnDirectoryCleared = () => settingsService.SetBackupDirectory(string.Empty),
                GetDirectory = () => settingsService.GetBackupDirectory(),
                DirectoryChooserType = DirectoryChooserTypes.BackupDirectory
            };

            ResourcesDirectoryChooser = new DirectoryChooserViewModel()
            {
                OnDirectorySet = (dir) => settingsService.SetResourcesDirectory(dir),
                OnDirectoryCleared = () => settingsService.SetResourcesDirectory(string.Empty),
                GetDirectory = () => settingsService.GetResourcesDirectory(),
                DirectoryChooserType = DirectoryChooserTypes.ResourcesDirectory
            };

            RefreshRecentFiles();

            IsDarkMode = settingsService.GetAppDarkTheme();
            SetTheme();
        }

        private void SwitchDarkTheme()
        {
            bool EnableDarkMode = !settingsService.GetAppDarkTheme();
            settingsService.SetAppDarkTheme(EnableDarkMode);
            IsDarkMode = EnableDarkMode;
            SetTheme();
        }

        private void SetTheme()
        {
            if (IsDarkMode)
            {
                ThemeHandler.SetTheme(ThemeHandler.AppTheme.DarkTheme);
            }
            else
            {
                ThemeHandler.SetTheme(ThemeHandler.AppTheme.LightTheme);
            }
        }

        public void RefreshRecentFiles()
        {
            foreach (string recentFilePath in settingsService.GetRecentPresentFiles())
                RecentFiles.Add(new StartupRecentFileViewModel(this, recentFilePath));
        }

        public void LoadSpecialEffect(string fullFilePath) => mainWindow.LoadSpecialEffect(fullFilePath);
    }
}
