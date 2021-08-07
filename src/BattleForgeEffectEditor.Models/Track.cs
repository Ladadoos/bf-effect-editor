// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Enums;
using System.Collections.Generic;

namespace BattleForgeEffectEditor.Models
{
    public class Track
    {
        public const uint Header = 0xF876AC30;
        public const uint StartTracks = 0xF8575767;

        public TrackType TrackType { get; set; }

        public float Length { get; set; }

        public TrackDim Dim { get; set; }

        public TrackMode Mode { get; set; }

        public TrackInterpolationType InterpolationType { get; set; }

        public TrackEvaluationType EvaluationType { get; set; }

        public List<ITrackKeyFrame> EntryKeyFrames { get; set; }

        public List<ITrackKeyFrame> ControlPointKeyFrames { get; set; }

        public bool IsEnabled { get; set; } = true;

        public Track(TrackType trackType, float length, TrackDim trackDim, TrackMode trackMode,
            TrackInterpolationType interpolationType, TrackEvaluationType evaluationType,
            List<ITrackKeyFrame> entryKeyFrames, List<ITrackKeyFrame> controlPointKeyFrames)
        {
            TrackType = trackType;
            Length = length;
            Dim = trackDim;
            Mode = trackMode;
            InterpolationType = interpolationType;
            EvaluationType = evaluationType;
            EntryKeyFrames = entryKeyFrames;
            ControlPointKeyFrames = controlPointKeyFrames;
        }

        public Track(TrackType trackType) : this(trackType, 0, TrackDim.TimeElapsed,
            TrackMode.Clamp, TrackInterpolationType.Linear, TrackEvaluationType.Track,
            new List<ITrackKeyFrame>(), new List<ITrackKeyFrame>())
        {

        }
    }
}
