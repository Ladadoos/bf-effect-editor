// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class SpecialEffectEditorViewModel : DockWindowViewModel
    {
        public ElementTreeViewModel ElementTree { get; private set; }

        public ElementEditorViewModel ElementEditor { get; private set; }

        public SpecialEffectTopBarViewModel TopBar { get; private set; }

        public bool ShowElementEditor => ElementTree.SelectedItem != null;

        public SpecialEffect SpecialEffect { get; private set; }

        public string FullFilePath { get; set; }

        public SpecialEffectEditorViewModel(SpecialEffect effect, string filePath)
        {
            SpecialEffect = effect;
            FullFilePath = filePath;

            ElementTree = new ElementTreeViewModel(this);
            ElementEditor = new ElementEditorViewModel();
            TopBar = new SpecialEffectTopBarViewModel(this);
        }

        public void Update(ElementTreeItemViewModel treeElement)
        {
            ElementEditor.Update(treeElement);
            RaisePropertyChanged(() => ShowElementEditor);
        }

        public void Reparent(ElementTreeItemViewModel treeItem)
        {
            treeItem.Reparent(ElementTree, this);
        }
    }
}