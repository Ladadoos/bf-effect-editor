// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;
using System.Collections.Generic;

namespace BattleForgeEffectEditor.Application.ViewModel.ElementSettings
{
    public class EffectSettingsViewModel : ElementSettingsViewModel<Effect>
    {
        public string EffectFile
        {
            get => element.EffectFilePath;
            set
            {
                element.EffectFilePath = new BfString(value);
                OnPropertyChanged();
            }
        }

        public IEnumerable<bool> BoolValues => boolValues;
        private readonly IReadOnlyList<bool> boolValues = new List<bool>
        {
            true, false
        };
        public bool Embedded
        {
            get => element.Embedded != 0;
            set
            {
                element.Embedded = value == false ? 0u : 1u;
                OnPropertyChanged();
            }
        }

        public float Length
        {
            get => element.Length;
            set
            {
                element.Length = value;
                OnPropertyChanged();
            }
        }

        public EffectSettingsViewModel(ElementTreeItemViewModel treeElement) : base(treeElement)
        {

        }
    }
}
