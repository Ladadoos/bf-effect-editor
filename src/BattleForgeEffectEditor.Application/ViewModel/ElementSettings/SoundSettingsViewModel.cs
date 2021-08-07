// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class SoundSettingsViewModel : ElementSettingsViewModel<Sound>
    {
        public string SoundFile
        {
            get => element.SoundFilePath;
            set
            {
                element.SoundFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public SoundSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
