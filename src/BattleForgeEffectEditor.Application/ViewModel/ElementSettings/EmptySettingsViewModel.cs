// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class EmptySettingsViewModel : ElementSettingsViewModel<Element>
    {
        public EmptySettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
