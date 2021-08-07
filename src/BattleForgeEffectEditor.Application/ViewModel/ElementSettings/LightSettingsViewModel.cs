// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class LightSettingsViewModel : ElementSettingsViewModel<Light>
    {
        public uint Range
        {
            get => element.Range;
            set
            {
                element.Range = value;
                OnPropertyChanged();
            }
        }

        public float Radiance
        {
            get => element.Radiance;
            set
            {
                element.Radiance = value;
                OnPropertyChanged();
            }
        }

        public LightSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
