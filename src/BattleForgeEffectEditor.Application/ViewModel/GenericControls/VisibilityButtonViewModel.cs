// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using System;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel.GenericControls
{
    public class VisibilityButtonViewModel : ObservableObject
    {
        public ICommand OnClickCommand => new RelayCommand((_) => OnClick());

        private bool visible;
        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }

        private Action<bool> onButtonToggled;

        public VisibilityButtonViewModel(bool visible, Action<bool> onButtonToggled)
        {
            this.visible = visible;
            this.onButtonToggled = onButtonToggled;
        }

        private void OnClick()
        {
            visible = !visible;
            RaisePropertyChanged(() => Visible);
            onButtonToggled?.Invoke(visible);
        }
    }
}
