// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models
{
    public class NodeLink
    {
        public const uint Header = 0xF82D712E;

        public BfString Parent { get; set; } = new BfString();

        public BfString Slot { get; set; } = new BfString();

        public BfString DestinationSlot { get; set; } = new BfString();

        public uint World { get; set; } = 0;

        public uint Node { get; set; } = 0;

        public uint Floor { get; set; } = 0;

        public uint Aim { get; set; } = 0;

        public uint Span { get; set; } = 0;

        public uint UnknownBitField { get; set; } = 1;

        public uint Locator { get; set; } = 0;

        public NodeLink(BfString parent, BfString slot, BfString destinationSlot,
            uint world, uint node, uint floor, uint aim, uint span, uint unknown,
            uint locator)
        {
            Parent = parent;
            Slot = slot;
            DestinationSlot = destinationSlot;
            World = world;
            Node = node;
            Floor = floor;
            Aim = aim;
            Span = span;
            UnknownBitField = unknown;
            Locator = locator;
        }

        public NodeLink() { }
    }
}
