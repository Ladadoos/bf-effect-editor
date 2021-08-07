// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Application.Utility;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class MenuItemViewModel : ObservableObject
    {
        public string Header { get; private set; }

        public ICommand Command { get; private set; }

        public MenuItemViewModel(string header, ICommand command)
        {
            Header = header;
            Command = command;
        }
    }

    public class ElementTreeViewModel : ObservableObject, IDropTarget
    {
        public ObservableCollection<ElementTreeItemViewModel> Elements { get; private set; }

        private ElementTreeItemViewModel selectedItem;
        public ElementTreeItemViewModel SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != null)
                    selectedItem.IsSelected = false;
                value.IsSelected = true;

                selectedItem = value;
                toggleAllPingPong = false;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MenuItemViewModel> AddElementOptions { get; private set; }

        public bool IsNonRootNodeSelected => !SelectedItem?.IsRootNode ?? false;

        public ICommand ExpandAllCommand => new RelayCommand((_) => ExpandAllChildren());

        public ICommand CollapseAllCommand => new RelayCommand((_) => CollapseAllChildren());

        public ICommand DeleteElementCommand => new RelayCommand((_) => DeleteSelectedElement());

        public ICommand CloneElementCommand => new RelayCommand((_) => CloneSelectedElement());

        public ICommand ToggleElementVisibilityCommand => new RelayCommand((_) => ToggleElementVisibility());

        public ICommand ToggleAllExceptSelectedElementCommand => new RelayCommand((_) => ToggleAllExceptSelectedElement());

        private ElementTreeItemViewModel rootElementNode;
        private bool toggleAllPingPong = false;

        private SpecialEffectEditorViewModel effectEditor;

        public ElementTreeViewModel(SpecialEffectEditorViewModel effectEditor)
        {
            this.effectEditor = effectEditor;

            rootElementNode = new ElementTreeItemViewModel(this, null, effectEditor, effectEditor.SpecialEffect);
            Elements = new ObservableCollection<ElementTreeItemViewModel>
            {
                rootElementNode
            };

            if (Elements.Count > 0)
                Elements[0].RecursiveExpand();

            InitializeElementContextMenuItems();
        }

        private void InitializeElementContextMenuItems()
        {
            AddElementOptions = new ObservableCollection<MenuItemViewModel>();

            foreach (Type elementType in AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(assembly => assembly.GetTypes())
                       .Where(type => type.IsSubclassOf(typeof(Element))))
            {
                ICommand relayCommand = new RelayCommand((_) =>
                {
                    AddElement(elementType);
                });
                AddElementOptions.Add(new MenuItemViewModel(elementType.Name, relayCommand));
            }
        }

        public void CloneSelectedElement()
        {
            if (SelectedItem == null || !IsNonRootNodeSelected)
                return;

            IElement clonedElement = SelectedItem.Element.Copy();
            clonedElement.Name += "_New";

            ElementTreeItemViewModel treeItem = new ElementTreeItemViewModel(
                this, SelectedItem.Parent, effectEditor, clonedElement);
            SelectedItem.Parent.Element.Children.Add(clonedElement);
            SelectedItem.Parent.Elements.Add(treeItem);

            SelectedItem = treeItem;
        }

        private void ToggleElementVisibility()
        {
            if (SelectedItem == null)
                return;
            SelectedItem.RecursiveToggleElementEnable();
        }

        private void ToggleAllExceptSelectedElement()
        {
            if (SelectedItem == null)
                return;

            foreach (ElementTreeItemViewModel child in rootElementNode.Elements)
                child.RecursiveSetElementEnable(toggleAllPingPong);

            toggleAllPingPong = !toggleAllPingPong;
            SelectedItem.RecursiveSetElementEnable(true);
        }

        private void ExpandAllChildren()
        {
            if (SelectedItem == null)
                return;
            SelectedItem.RecursiveExpand();
        }

        private void CollapseAllChildren()
        {
            if (SelectedItem == null)
                return;
            SelectedItem.RecursiveCollapse();
        }

        public void DeleteSelectedElement()
        {
            if (SelectedItem == null || !IsNonRootNodeSelected)
                return;

            ElementTreeItemViewModel parent = SelectedItem.Parent;

            parent.Element.Children.Remove(SelectedItem.Element);
            parent.Elements.Remove(SelectedItem);

            ElementTreeItemViewModel newSelection = parent.Elements.LastOrDefault();
            if (newSelection != null)
                SelectedItem = newSelection;
        }

        public void AddElement(Type elementType)
        {
            if (!elementType.IsSubclassOf(typeof(Element)))
                return;

            //TODO Indicate this better in the UI.
            //if ((elementType == typeof(SfpForceField) || elementType == typeof(SfpEmitter))
            //    && SelectedItem.Element.GetType() != typeof(SfpSystem))
            //    return;

            Element element = (Element)Activator.CreateInstance(elementType);
            element.Parent = SelectedItem.Element;
            SelectedItem.Element.Children.Add(element);

            SelectedItem.Elements.Add(new ElementTreeItemViewModel(this, SelectedItem, effectEditor, element));
            SelectedItem.IsExpanded = true;
        }
        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as ElementTreeItemViewModel;
            var targetItem = dropInfo.TargetItem as ElementTreeItemViewModel;

            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            if (!IsDropValid(sourceItem, targetItem))
                dropInfo.Effects = DragDropEffects.None;
            else
                dropInfo.Effects = DragDropEffects.Move;
        }

        private bool IsDropValid(ElementTreeItemViewModel sourceItem, ElementTreeItemViewModel targetItem)
        {
            return !(sourceItem == null ||
                    targetItem == null ||
                    sourceItem == targetItem ||
                    sourceItem.HasTreeItemAsChild(targetItem) ||
                    sourceItem.IsRootNode);
        }

        public void Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as ElementTreeItemViewModel;
            var targetItem = dropInfo.TargetItem as ElementTreeItemViewModel;

            if (!IsDropValid(sourceItem, targetItem))
                return;

            sourceItem.Parent.Elements.Remove(sourceItem);
            sourceItem.Parent.Element.Children.Remove(sourceItem.Element);

            sourceItem.Parent = targetItem;
            sourceItem.Element.Parent = targetItem.Element;

            targetItem.Elements.Add(sourceItem);
            targetItem.Element.Children.Add(sourceItem.Element);

            if (sourceItem.EffectEditor != targetItem.EffectEditor)
                targetItem.EffectEditor.Reparent(sourceItem);

            sourceItem.TreeView.SelectedItem = sourceItem;
        }

        public void HandleKeyshortcut(Key pressedKey)
        {
            if (SelectedItem == null)
                return;

            switch (pressedKey)
            {
                case Key.V:
                    SelectedItem.RecursiveToggleElementEnable();
                    break;
                case Key.C:
                    CloneSelectedElement();
                    break;
                case Key.D:
                    DeleteSelectedElement();
                    break;
                default: break;
            }
        }

        public void Update(ElementTreeItemViewModel selectedItem)
        {
            SelectedItem = selectedItem;
            RaisePropertyChanged(() => IsNonRootNodeSelected);
        }
    }
}
