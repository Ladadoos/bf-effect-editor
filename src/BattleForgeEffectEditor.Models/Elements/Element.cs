// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;
using System.Collections.Generic;

namespace BattleForgeEffectEditor.Models.Elements
{
    public interface IElement
    {
        BfString Name { get; set; }

        List<IStaticTrack> StaticTracks { get; set; }

        List<Track> Tracks { get; set; }

        List<IElement> Children { get; set; }

        IElement Parent { get; set; }

        bool IsEnabled { get; set; }
    }

    public abstract class Element : IElement
    {
        public const uint StartElementChildrenHeader = 0xF876E2D0;
        public const uint EndElementChildrenHeader = 0xF8E2DE2D;
        public const uint StartElementHeader = 0xF8E7EAA7;
        public const uint EndElementHeader = 0xF8E75E2D;

        public NodeLink NodeLink { get; set; } = new NodeLink();

        public BfString Name { get; set; } = new BfString();

        public List<IStaticTrack> StaticTracks { get; set; } = new List<IStaticTrack>();

        public List<Track> Tracks { get; set; } = new List<Track>();

        public List<IElement> Children { get; set; } = new List<IElement>();

        public IElement Parent { get; set; }

        public bool IsEnabled { get; set; } = true;

        public void Set(BfString name, NodeLink nodeLink, List<IStaticTrack> staticTracks,
            List<Track> tracks)
        {
            Name = name;
            NodeLink = nodeLink;
            StaticTracks = staticTracks;
            Tracks = tracks;
            Children = new List<IElement>();
        }
    }
}
