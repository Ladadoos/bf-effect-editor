// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class AnimatedMeshSettingsViewModel : ElementSettingsViewModel<AnimatedMesh>
    {
        public string AnimationFile
        {
            get => element.AnimationFilePath;
            set
            {
                element.AnimationFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public string MeshFile
        {
            get => element.MeshFilePath;
            set
            {
                element.MeshFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public AnimatedMeshSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
