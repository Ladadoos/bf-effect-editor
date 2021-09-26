// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.Utility;
using BattleForgeEffectEditor.Models.Enums;
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

        private string directoryTextBox = "";

        public string DirectoryTextBox
        {
            get
            {
                if (string.IsNullOrEmpty(directoryTextBox))
                    return Directory;
                else
                    return directoryTextBox;
            }

            set
            {
                if (directoryTextBox != value)
                {
                    directoryTextBox = value;
                }
            }
        }

        public string SetOpenButtonText
        {
            get
            {
                if (HasDirectorySet)
                {
                    return "Open";
                }
                else
                {
                    return "Set";
                }
            }
        }


        public Action<string> OnDirectorySet;
        public Action OnDirectoryCleared;
        public Func<string> GetDirectory;

        public DirectoryChooserTypes DirectoryChooserType;

        private DialogService dialogService = new DialogService();
        private WindowsService windowsServie = new WindowsService();

        private void OpenOrSetDirectory()
        {
            DirectoryChooserPathValidatorErrorTypes pathValidatorError;
            if (HasDirectorySet)
                windowsServie.TryOpenExplorerOnDirectory(Directory);
            else
            {
                if (string.IsNullOrEmpty(directoryTextBox))
                {
                    string dirPath = dialogService.OpenDirectoryDialog("Choose directory");
                    if (dirPath != string.Empty)
                    {
                        pathValidatorError = DirectoryChooserPathValidator.ValidatePath(dirPath, DirectoryChooserType);
                        if (pathValidatorError == DirectoryChooserPathValidatorErrorTypes.Ok)
                            SetDirectory(dirPath);
                        else
                        {
                            PathErrorThrow(pathValidatorError);
                        }
                    }   
                }
                else
                {
                    pathValidatorError = DirectoryChooserPathValidator.ValidatePath(directoryTextBox, DirectoryChooserType);
                    if (pathValidatorError == DirectoryChooserPathValidatorErrorTypes.Ok)
                        SetDirectory(directoryTextBox);
                    else
                    {
                        PathErrorThrow(pathValidatorError);
                    }
                }
            }
        }

        private void PathErrorThrow(DirectoryChooserPathValidatorErrorTypes pathValidatorError)
        {
            switch (pathValidatorError)
            {
                case DirectoryChooserPathValidatorErrorTypes.NotSet:
                    dialogService.ShowError("No Path Set", "Not valid path");
                    break;
                case DirectoryChooserPathValidatorErrorTypes.DriveNotExist:
                    dialogService.ShowError("Selected Drive don't exists", "Not valid path");
                    break;
                case DirectoryChooserPathValidatorErrorTypes.PathNotFound:
                    dialogService.ShowError("Selected Path not Found", "Not valid path");
                    break;
                case DirectoryChooserPathValidatorErrorTypes.Not_a_Directory:
                    dialogService.ShowError("Selected Path is not a Directory", "Not valid path");
                    break;
                case DirectoryChooserPathValidatorErrorTypes.NotEmptyResourcesNotFound:
                    dialogService.ShowError("Selected Directory is not empty and no Resources are found", "Not valid path");
                    break;
                case DirectoryChooserPathValidatorErrorTypes.NotEmptyBackupNoBackupsFound:
                    dialogService.ShowError("Selected Directory is not empty and contains no Backups", "Not valid path");
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
            DirectoryTextBox = Directory;
            RaiseAllPropertiesChanged();
        }
    }
}
