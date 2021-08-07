﻿// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class WaterDecalSettingsViewModel : ElementSettingsViewModel<WaterDecal>
    {
        public string TextureFile
        {
            get => element.TextureFilePath;
            set
            {
                element.TextureFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public WaterDecalSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
