// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;
using System.Collections.Generic;

namespace BattleForgeEffectEditor.Models
{
    public class SpecialEffect : IElement
    {
        public static uint Header = 0xF8AEADE7;

        public BfString Name { get; set; } = new BfString();

        public float Length { get; set; } = 0;

        public float PlayLength { get; set; } = 0;

        public BfString SetupFileName { get; set; } = new BfString();

        public int SetupSourceId { get; set; } = 0;

        public int SetupTargetId { get; set; } = 0;

        public List<IStaticTrack> StaticTracks { get; set; } = new List<IStaticTrack>();

        public List<Track> Tracks { get; set; } = new List<Track>();

        public List<IElement> Children { get; set; } = new List<IElement>();

        public IElement Parent { get => null; set { } }

        public bool IsEnabled { get; set; } = true;

        public SpecialEffect(BfString name, float length, float playLength, BfString setupFileName,
            int setupSourceId, int setupTargetId, List<IStaticTrack> staticTracks,
            List<Track> tracks)
        {
            Name = name;
            Length = length;
            PlayLength = playLength;
            SetupFileName = setupFileName;
            SetupSourceId = setupSourceId;
            SetupTargetId = setupTargetId;

            StaticTracks = staticTracks;
            Tracks = tracks;
        }

        public SpecialEffect() { }
    }
}
