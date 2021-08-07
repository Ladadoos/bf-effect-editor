// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.View
{
    /// <summary>
    /// Interaction logic for ElementTreeView.xaml
    /// </summary>
    public partial class ElementTreeView : UserControl
    {
        public ElementTreeView()
        {
            InitializeComponent();
        }

        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TreeView))
                return;

            var treeView = (TreeView)sender;
            var dataContext = (ElementTreeViewModel)treeView.DataContext;
            dataContext.HandleKeyshortcut(e.Key);
        }
    }
}
