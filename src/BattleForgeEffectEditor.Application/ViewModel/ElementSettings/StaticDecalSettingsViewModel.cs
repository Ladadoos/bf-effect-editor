// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class StaticDecalSettingsViewModel : ElementSettingsViewModel<StaticDecal>
    {
        public string ColorTextureFile
        {
            get => element.ColorTextureFilePath;
            set
            {
                element.ColorTextureFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public string NormalTextureFile
        {
            get => element.NormalTextureFilePath;
            set
            {
                element.NormalTextureFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public StaticDecalSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
