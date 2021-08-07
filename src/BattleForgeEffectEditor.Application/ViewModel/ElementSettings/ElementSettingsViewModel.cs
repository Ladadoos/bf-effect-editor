// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public interface IElementSettings
    {
    }

    public class ElementSettingsViewModel<T> : ObservableObject, IElementSettings where T : IElement
    {
        public string Name
        {
            get => element.Name.ToString();
            set
            {
                element.Name = new BfString(value);
                OnPropertyChanged();
                treeElement.UpdateName();
            }
        }

        protected T element;

        private ElementTreeItemViewModel treeElement;

        public ElementSettingsViewModel(ElementTreeItemViewModel treeElement)
        {
            this.element = (T)treeElement.Element;
            this.treeElement = treeElement;
        }
    }
}
