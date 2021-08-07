// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Utility;
using System;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class SpecialEffectSettingsViewModel : ElementSettingsViewModel<SpecialEffect>
    {
        public float Length
        {
            get => element.Length;
            set
            {
                element.Length = Math.Max(value, 0);
                OnPropertyChanged();
            }
        }

        public float PlayLength
        {
            get => element.PlayLength;
            set
            {
                element.PlayLength = Math.Max(value, 0);
                OnPropertyChanged();
            }
        }

        public int SetupSourceId
        {
            get => element.SetupSourceId;
            set
            {
                element.SetupSourceId = value;
                OnPropertyChanged();
            }
        }

        public int SetupTargetId
        {
            get => element.SetupTargetId;
            set
            {
                element.SetupTargetId = value;
                OnPropertyChanged();
            }
        }

        public string SetupFileName
        {
            get => element.SetupFileName;
            set
            {
                element.SetupFileName = new BfString(value);
                OnPropertyChanged();
            }
        }

        public SpecialEffectSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
