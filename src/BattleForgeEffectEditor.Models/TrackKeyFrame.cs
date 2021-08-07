// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

namespace BattleForgeEffectEditor.Models
{
    public interface ITrackKeyFrame
    {
        float Frame { get; set; }
    }

    public abstract class TrackKeyFrame : ITrackKeyFrame
    {
        public const uint EndControlPointHeader = 0xF876AC3E;

        public float Frame { get; set; } = 0;
    }

    public class Vector3KeyFrame : TrackKeyFrame
    {
        public const uint EntryHeader = 0xF87E7EC7;
        public const uint StartControlPointHeader = 0xF87E7C95;
        public const uint ControlPointHeader = 0xF87E7EC9;

        public Vector3 Data { get; set; } = Vector3.Zero;

        public Vector3KeyFrame(float frame, Vector3 data)
        {
            Frame = frame;
            Data = data;
        }

        public Vector3KeyFrame() { }
    }

    public class FloatKeyFrame : TrackKeyFrame
    {
        public const uint EntryHeader = 0xF87EF70A;
        public const uint StartControlPointHeader = 0xF87EFC95;
        public const uint ControlPointHeader = 0xF87EF7C9;

        public float Data { get; set; } = 0;

        public FloatKeyFrame(float frame, float data)
        {
            Frame = frame;
            Data = data;
        }

        public FloatKeyFrame() { }
    }
}
