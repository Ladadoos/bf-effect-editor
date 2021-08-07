// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class DockManagerViewModel
    {
        public ObservableCollection<DockWindowViewModel> Documents { get; private set; } =
            new ObservableCollection<DockWindowViewModel>();

        public ObservableCollection<object> Anchorables { get; private set; } =
            new ObservableCollection<object>();

        public void AddDockWindow(DockWindowViewModel dockWindow)
        {
            dockWindow.PropertyChanged += DockWindowViewModel_PropertyChanged;
            if (!dockWindow.IsClosed)
                Documents.Add(dockWindow);
        }

        public bool HasDockWindowOfType(Type type) => Documents.Any(dock => dock.GetType() == type);

        private void DockWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DockWindowViewModel document = sender as DockWindowViewModel;

            if (e.PropertyName == nameof(DockWindowViewModel.IsClosed))
            {
                if (!document.IsClosed)
                    Documents.Add(document);
                else
                    Documents.Remove(document);
            }
        }
    }
}