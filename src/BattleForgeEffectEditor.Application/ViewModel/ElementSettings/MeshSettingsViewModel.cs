// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class MeshSettingsViewModel : ElementSettingsViewModel<Mesh>
    {
        public string MeshFile
        {
            get => element.MeshFilePath;
            set
            {
                element.MeshFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public MeshSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
