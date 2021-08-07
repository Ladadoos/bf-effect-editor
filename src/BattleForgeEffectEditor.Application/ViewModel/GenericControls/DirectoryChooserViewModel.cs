// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.Utility;
using System;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel.GenericControls
{
    public class DirectoryChooserViewModel : ObservableObject
    {
        public ICommand OpenOrSetDirectoryCommand => new RelayCommand((_) => OpenOrSetDirectory());

        public ICommand ClearDirectoryCommand => new RelayCommand((_) => ClearDirectory());

        public bool HasDirectorySet => Directory != string.Empty;

        public string Directory => GetDirectory();

        public Action<string> OnDirectorySet;
        public Action OnDirectoryCleared;
        public Func<string> GetDirectory;

        private DialogService dialogService = new DialogService();
        private WindowsService windowsServie = new WindowsService();

        private void OpenOrSetDirectory()
        {
            if (HasDirectorySet)
                windowsServie.TryOpenExplorerOnDirectory(Directory);
            else
            {
                string dirPath = dialogService.OpenDirectoryDialog("Choose directory");
                if (dirPath != string.Empty)
                    SetDirectory(dirPath);
            }
        }

        private void SetDirectory(string dirPath)
        {
            OnDirectorySet?.Invoke(dirPath);
            RaiseAllPropertiesChanged();
        }

        private void ClearDirectory()
        {
            OnDirectoryCleared?.Invoke();
            RaiseAllPropertiesChanged();
        }
    }
}
