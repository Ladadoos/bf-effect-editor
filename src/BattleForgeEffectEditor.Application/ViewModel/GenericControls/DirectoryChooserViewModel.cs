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

        public bool HasDirectorySet => !string.IsNullOrEmpty(Directory);

        public string Directory => GetDirectory();

        public Action<string> OnDirectorySet;
        public Action OnDirectoryCleared;
        public Func<string> GetDirectory;

        private DialogService dialogService = new DialogService();
        private WindowsService windowsServie = new WindowsService();

        public string SetOpenButtonText => HasDirectorySet ? "Open" : "Set";

        private void OpenOrSetDirectory()
        {
            if (HasDirectorySet)
                windowsServie.TryOpenExplorerOnDirectory(Directory);
            else
            {
                string path = Directory;
                if (string.IsNullOrEmpty(path))
                {
                    string dirPath = dialogService.OpenDirectoryDialog("Choose directory");
                    if (!string.IsNullOrEmpty(dirPath))
                        path = dirPath;
                }

                PathValidationError validationErr = PathValidator.ValidatePath(path);
                if (validationErr == PathValidationError.Ok)
                    SetDirectory(path);
                else
                    PathErrorThrow(validationErr);
            }
        }

        private void PathErrorThrow(PathValidationError pathValidatorError)
        {
            switch (pathValidatorError)
            {
                case PathValidationError.InvalidPath:
                    dialogService.ShowError("No Path is set.", "Invalid path");
                    break;
                case PathValidationError.DriveDoesNotExist:
                    dialogService.ShowError("Selected Drive does not exist.", "Invalid path");
                    break;
                case PathValidationError.PathNotFound:
                    dialogService.ShowError("Selected Path not found.", "Path not found");
                    break;
                case PathValidationError.NotADirectory:
                    dialogService.ShowError("Selected Path is not a Directory.", "Invalid path");
                    break;
                default:
                    break;
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
