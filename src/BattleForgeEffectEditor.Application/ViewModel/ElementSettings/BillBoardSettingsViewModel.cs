// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class BillBoardSettingsViewModel : ElementSettingsViewModel<Billboard>
    {
        public string TextureOneFile
        {
            get => element.TextureOneFilePath;
            set
            {
                element.TextureOneFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public string TextureTwoFile
        {
            get => element.TextureTwoFilePath;
            set
            {
                element.TextureTwoFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public BillBoardSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
