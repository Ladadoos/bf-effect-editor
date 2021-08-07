// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class NodeLinkViewModel : ObservableObject
    {
        public string Parent
        {
            get => nodeLink?.Parent ?? string.Empty;
            set
            {
                nodeLink.Parent = new BfString(value);
                OnPropertyChanged();
            }
        }

        public string Slot
        {
            get => nodeLink?.Slot ?? string.Empty;
            set
            {
                nodeLink.Slot = new BfString(value);
                OnPropertyChanged();
            }
        }

        public string DestinationSlot
        {
            get => nodeLink?.DestinationSlot ?? string.Empty;
            set
            {
                nodeLink.DestinationSlot = new BfString(value);
                OnPropertyChanged();
            }
        }

        public uint World
        {
            get => nodeLink?.World ?? 0;
            set
            {
                nodeLink.World = value;
                OnPropertyChanged();
            }
        }

        public uint Node
        {
            get => nodeLink?.Node ?? 0;
            set
            {
                nodeLink.Node = value;
                OnPropertyChanged();
            }
        }

        public uint Floor
        {
            get => nodeLink?.Floor ?? 0;
            set
            {
                nodeLink.Floor = value;
                OnPropertyChanged();
            }
        }

        public uint Aim
        {
            get => nodeLink?.Aim ?? 0;
            set
            {
                nodeLink.Aim = value;
                OnPropertyChanged();
            }
        }

        public uint Span
        {
            get => nodeLink?.Span ?? 0;
            set
            {
                nodeLink.Span = value;
                OnPropertyChanged();
            }
        }

        public uint Bitfield
        {
            get => nodeLink?.UnknownBitField ?? 0;
            set
            {
                nodeLink.UnknownBitField = value;
                OnPropertyChanged();
            }
        }

        public uint Locator
        {
            get => nodeLink?.Locator ?? 0;
            set
            {
                nodeLink.Locator = value;
                OnPropertyChanged();
            }
        }

        private bool visible;
        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                OnPropertyChanged();
            }
        }

        private NodeLink nodeLink;

        public void Update(IElement element)
        {
            if (element is Element nodeElement)
            {
                nodeLink = nodeElement.NodeLink;
                Visible = true;
                RaiseAllPropertiesChanged();
            } else
            {
                Visible = false;
                nodeLink = null;
            }
        }
    }
}
