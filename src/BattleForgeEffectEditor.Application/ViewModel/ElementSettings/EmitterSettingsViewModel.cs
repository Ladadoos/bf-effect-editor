// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class EmitterSettingsViewModel : ElementSettingsViewModel<Emitter>
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

        public uint ParticleCount
        {
            get => element.ParticleCount;
            set
            {
                element.ParticleCount = value;
                OnPropertyChanged();
            }
        }

        public EmitterSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
