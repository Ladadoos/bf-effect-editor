// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Elements;
using System.Collections.ObjectModel;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class ElementTreeItemViewModel : ObservableObject
    {
        public string ElementName => Element.GetType().Name + " (" + Element.Name + ")";

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    if (isSelected)
                        Update();
                    OnPropertyChanged();
                }
            }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HideIconVisibility => !Element.IsEnabled;

        public bool IsElementEnabled
        {
            get => Element.IsEnabled;
            set
            {
                Element.IsEnabled = value;
                OnPropertyChanged();
                RaisePropertyChanged(() => HideIconVisibility);
            }
        }

        public bool IsRootNode => Element.GetType() == typeof(SpecialEffect);

        public ObservableCollection<ElementTreeItemViewModel> Elements { get; set; } =
            new ObservableCollection<ElementTreeItemViewModel>();

        public ElementTreeItemViewModel Parent { get; set; }

        public IElement Element { get; private set; }

        public SpecialEffectEditorViewModel EffectEditor { get; private set; }
        public ElementTreeViewModel TreeView { get; private set; }

        public ElementTreeItemViewModel(ElementTreeViewModel treeView, ElementTreeItemViewModel parent,
            SpecialEffectEditorViewModel effectEditor, IElement element)
        {
            TreeView = treeView;
            Parent = parent;
            EffectEditor = effectEditor;
            Element = element;

            foreach (Element child in element.Children)
                Elements.Add(new ElementTreeItemViewModel(treeView, this, effectEditor, child));
        }

        public void RecursiveExpand()
        {
            IsExpanded = true;
            foreach (ElementTreeItemViewModel child in Elements)
                child.RecursiveExpand();
        }

        public void RecursiveCollapse()
        {
            foreach (ElementTreeItemViewModel child in Elements)
                child.RecursiveCollapse();
            IsExpanded = false;
        }

        public void RecursiveToggleElementEnable()
        {
            if (IsRootNode)
                IsElementEnabled = true;
            else
                IsElementEnabled = !IsElementEnabled;

            foreach (ElementTreeItemViewModel child in Elements)
                child.RecursiveSetElementEnable(IsElementEnabled);
        }

        public void RecursiveSetElementEnable(bool isEnabled)
        {
            if (IsRootNode)
                IsElementEnabled = true;
            else
                IsElementEnabled = isEnabled;

            foreach (ElementTreeItemViewModel child in Elements)
                child.RecursiveSetElementEnable(isEnabled);
        }

        public bool HasTreeItemAsChild(ElementTreeItemViewModel treeItem)
        {
            foreach (ElementTreeItemViewModel child in Elements)
                if (child == treeItem || child.HasTreeItemAsChild(treeItem))
                    return true;
            return false;
        }

        public void Update()
        {
            TreeView.Update(this);
            EffectEditor.Update(this);
        }

        public void UpdateName() => RaisePropertyChanged(() => ElementName);

        public void Reparent(ElementTreeViewModel treeView, SpecialEffectEditorViewModel effectEditor)
        {
            TreeView = treeView;
            EffectEditor = effectEditor;

            foreach (ElementTreeItemViewModel child in Elements)
                child.Reparent(treeView, effectEditor);
        }
    }
}
