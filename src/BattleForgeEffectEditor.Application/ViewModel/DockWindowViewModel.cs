// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public abstract class DockWindowViewModel : ObservableObject
    {
        public ICommand CloseCommand => new RelayCommand((_) => IsClosed = true);

        private bool isClosed;
        public bool IsClosed
        {
            get => isClosed;
            set
            {
                isClosed = value;
                OnPropertyChanged();
            }
        }

        private bool canClose;
        public bool CanClose
        {
            get => canClose;
            set
            {
                canClose = value;
                OnPropertyChanged();
            }
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public DockWindowViewModel()
        {
            CanClose = true;
            IsClosed = false;
        }
    }
}