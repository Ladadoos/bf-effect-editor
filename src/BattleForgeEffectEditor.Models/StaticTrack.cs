// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Enums;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models
{
    public interface IStaticTrack
    {
        StaticTrackType TrackType { get; set; }
    };

    public abstract class StaticTrack : IStaticTrack
    {
        public const uint Header = 0xF857A71C;

        public StaticTrackType TrackType { get; set; }

        public StaticTrack(StaticTrackType trackType)
        {
            TrackType = trackType;
        }
    }

    public class FloatStaticTrack : StaticTrack
    {
        public const uint FloatHeader = 0xF857A7F7;

        public float Data { get; set; }

        public FloatStaticTrack(StaticTrackType trackType, float data) : base(trackType)
        {
            Data = data;
        }
    }

    public class Vector3StaticTrack : StaticTrack
    {
        public const uint Vector3Header = 0xF857A77C;

        public Vector3 Data { get; set; }

        public Vector3StaticTrack(StaticTrackType trackType, Vector3 data) : base(trackType)
        {
            Data = data;
        }
    }

    public class StringStaticTrack : StaticTrack
    {
        public const uint StringHeader = 0xF857A757;

        public BfString Data { get; set; }

        public StringStaticTrack(StaticTrackType trackType, string data) : base(trackType)
        {
            Data = data;
        }
    }

    public class Vector3OtherStaticTrack : StaticTrack
    {
        public const uint Vector3OtherHeader = 0xF857A747;

        public Vector3 Data { get; set; }

        public Vector3OtherStaticTrack(StaticTrackType trackType, Vector3 data) : base(trackType)
        {
            Data = data;
        }
    }
}
