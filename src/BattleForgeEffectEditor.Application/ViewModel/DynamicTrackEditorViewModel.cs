// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.Utility;
using BattleForgeEffectEditor.Application.ViewModel.Keyframe;
using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    internal struct TrackDataKey
    {
        public TrackType TrackType;
        public TrackEvaluationType EvaluationType;

        public TrackDataKey(TrackType trackType, TrackEvaluationType evaluationType)
        {
            TrackType = trackType;
            EvaluationType = evaluationType;
        }
    }

    internal interface IKeyframeToViewModelConverter
    {
        IKeyframeViewModel GetEntryViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor);
        IKeyframeViewModel GetControlPointViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor);

        Type GetKeyFrameType();
    }

    internal struct Vector3Converter : IKeyframeToViewModelConverter
    {
        public IKeyframeViewModel GetEntryViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            return new Vector3KeyframeViewModel(TypeUtilities.Cast(keyFrame, GetKeyFrameType()), trackEditor);
        }

        public IKeyframeViewModel GetControlPointViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            return new Vector3KeyframeViewModel(TypeUtilities.Cast(keyFrame, GetKeyFrameType()), trackEditor);
        }

        public Type GetKeyFrameType() => typeof(Vector3KeyFrame);
    }

    internal struct ColorConverter : IKeyframeToViewModelConverter
    {
        public IKeyframeViewModel GetEntryViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            return new ColorKeyframeViewModel(TypeUtilities.Cast(keyFrame, GetKeyFrameType()), trackEditor);
        }

        public IKeyframeViewModel GetControlPointViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            return new Vector3KeyframeViewModel(TypeUtilities.Cast(keyFrame, GetKeyFrameType()), trackEditor);
        }

        public Type GetKeyFrameType() => typeof(Vector3KeyFrame);
    }

    internal struct FloatConverter : IKeyframeToViewModelConverter
    {
        public IKeyframeViewModel GetEntryViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            return new FloatKeyframeViewModel(TypeUtilities.Cast(keyFrame, GetKeyFrameType()), trackEditor);
        }

        public IKeyframeViewModel GetControlPointViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            return new FloatKeyframeViewModel(TypeUtilities.Cast(keyFrame, GetKeyFrameType()), trackEditor);
        }

        public Type GetKeyFrameType() => typeof(FloatKeyFrame);
    }

    internal struct EnumConverter : IKeyframeToViewModelConverter
    {
        private readonly Type enumType;

        public EnumConverter(Type enumType)
        {
            this.enumType = enumType;
        }

        public IKeyframeViewModel GetEntryViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            return new EnumKeyframeViewModel(TypeUtilities.Cast(keyFrame, GetKeyFrameType()), trackEditor, enumType);
        }

        public IKeyframeViewModel GetControlPointViewModel(ITrackKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
        {
            return new FloatKeyframeViewModel(TypeUtilities.Cast(keyFrame, GetKeyFrameType()), trackEditor);
        }

        public Type GetKeyFrameType() => typeof(FloatKeyFrame);
    }

    public class DynamicTrackEditorViewModel : ObservableObject
    {
        public ICommand AddEntryKeyframeCommand => new RelayCommand((_) => AddEntryKeyframe());

        public ICommand AddControlPointCommand => new RelayCommand((_) => AddControlPoint());

        public ICommand SortEntryKeyframeCommand => new RelayCommand((_) => SortEntryKeyframes());

        public ICommand SortControlPointCommand => new RelayCommand((_) => SortControlPoints());

        public string Type => track.TrackType.ToString();

        public float Length
        {
            get => track.Length;
            set
            {
                track.Length = Math.Max(value, 0);
                OnPropertyChanged();
            }
        }

        public IEnumerable<TrackDim> Dims => Enum.GetValues(typeof(TrackDim)).OfType<TrackDim>();
        public TrackDim SelectedDim
        {
            get => track.Dim;
            set
            {
                track.Dim = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TrackMode> Modes => Enum.GetValues(typeof(TrackMode)).OfType<TrackMode>();
        public TrackMode SelectedMode
        {
            get => track.Mode;
            set
            {
                track.Mode = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TrackInterpolationType> Interpolations => Enum.GetValues(typeof(TrackInterpolationType)).OfType<TrackInterpolationType>();
        public TrackInterpolationType SelectedInterpolation
        {
            get => track.InterpolationType;
            set
            {
                track.InterpolationType = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TrackEvaluationType> Evaluations
        {
            get
            {
                if (track.TrackType == TrackType.Translate || track.TrackType == TrackType.Rotate ||
                    track.TrackType == TrackType.Scale || track.TrackType == TrackType.Anim)
                {
                    return Enum.GetValues(typeof(TrackEvaluationType)).OfType<TrackEvaluationType>();
                }

                return new List<TrackEvaluationType>() { TrackEvaluationType.Track };
            }
        }
        public TrackEvaluationType SelectedEvaluation
        {
            get => track.EvaluationType;
            set
            {
                if (EntryFrames.Count > 0 || ControlFrames.Count > 0)
                {
                    if (!dialogService.ShowAreYouSure("Evaluation change",
                        "Changing evaluation type will delete all entry and control point keyframes. Are you sure?"))
                    {
                        OnPropertyChanged();
                        return;
                    }
                }

                track.EvaluationType = value;

                EntryFrames.Clear();
                ControlFrames.Clear();
                track.EntryKeyFrames.Clear();
                track.ControlPointKeyFrames.Clear();

                OnPropertyChanged();
            }
        }

        public ObservableCollection<IKeyframeViewModel> EntryFrames { get; set; } =
            new ObservableCollection<IKeyframeViewModel>();

        public ObservableCollection<IKeyframeViewModel> ControlFrames { get; set; } =
            new ObservableCollection<IKeyframeViewModel>();

        private Track track;

        private DialogService dialogService = new DialogService();

        public DynamicTrackEditorViewModel(Track track)
        {
            this.track = track;

            PopulateEntryFrames();
            PopulateControlPointFrames();
        }

        public void RemoveKeyframe(IKeyframeViewModel keyframe)
        {
            bool isEntry = true;
            if (ControlFrames.Contains(keyframe))
                isEntry = false;

            if (isEntry)
            {
                EntryFrames.Remove(keyframe);
                track.EntryKeyFrames.Remove(keyframe.GetKeyFrame());
            } else
            {
                ControlFrames.Remove(keyframe);
                track.ControlPointKeyFrames.Remove(keyframe.GetKeyFrame());
            }
        }

        public void AddEntryKeyframe()
        {
            IKeyframeToViewModelConverter converter = TrackToDataType[new TrackDataKey(track.TrackType, track.EvaluationType)];
            ITrackKeyFrame keyframe = (ITrackKeyFrame)Activator.CreateInstance(converter.GetKeyFrameType());
            EntryFrames.Add(converter.GetEntryViewModel(keyframe, this));
            track.EntryKeyFrames.Add(keyframe);
        }

        public void AddControlPoint()
        {
            IKeyframeToViewModelConverter converter = TrackToDataType[new TrackDataKey(track.TrackType, track.EvaluationType)];
            ITrackKeyFrame keyframe = (ITrackKeyFrame)Activator.CreateInstance(converter.GetKeyFrameType());
            ControlFrames.Add(converter.GetControlPointViewModel(keyframe, this));
            track.ControlPointKeyFrames.Add(keyframe);
        }

        private Comparison<ITrackKeyFrame> GetKeyframeIndexComparison()
        {
            return (t, t2) =>
            {
                if (t2.Frame > t.Frame)
                    return -1;
                else if (t2.Frame == t.Frame)
                    return 0;
                return 1;
            };
        }

        public void SortEntryKeyframes()
        {
            track.EntryKeyFrames.Sort(GetKeyframeIndexComparison());
            PopulateEntryFrames();
        }

        public void SortControlPoints()
        {
            track.ControlPointKeyFrames.Sort(GetKeyframeIndexComparison());
            PopulateControlPointFrames();
        }

        private void PopulateEntryFrames()
        {
            EntryFrames.Clear();

            IKeyframeToViewModelConverter converter = TrackToDataType[new TrackDataKey(track.TrackType, track.EvaluationType)];
            foreach (ITrackKeyFrame keyframe in track.EntryKeyFrames)
                EntryFrames.Add(converter.GetEntryViewModel(keyframe, this));
        }

        private void PopulateControlPointFrames()
        {
            ControlFrames.Clear();

            IKeyframeToViewModelConverter converter = TrackToDataType[new TrackDataKey(track.TrackType, track.EvaluationType)];
            foreach (ITrackKeyFrame keyframe in track.ControlPointKeyFrames)
                ControlFrames.Add(converter.GetControlPointViewModel(keyframe, this));
        }

        private static readonly Dictionary<TrackDataKey, IKeyframeToViewModelConverter> TrackToDataType = new Dictionary<TrackDataKey, IKeyframeToViewModelConverter>
        {
            { new TrackDataKey(TrackType.Translate, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Translate, TrackEvaluationType.Ramp), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotate, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Rotate, TrackEvaluationType.Ramp), new FloatConverter() },
            { new TrackDataKey(TrackType.Scale, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Scale, TrackEvaluationType.Ramp), new Vector3Converter() },
            { new TrackDataKey(TrackType.Anim, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Anim, TrackEvaluationType.Ramp), new Vector3Converter() },
            { new TrackDataKey(TrackType.Color, TrackEvaluationType.Track), new ColorConverter() },
            { new TrackDataKey(TrackType.Alpha, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Size, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Time, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.BlendMode, TrackEvaluationType.Track), new EnumConverter(typeof(LegacyBlendMode)) },
            { new TrackDataKey(TrackType.Light_Range, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Start_Color, TrackEvaluationType.Track), new ColorConverter() },
            { new TrackDataKey(TrackType.End_Color, TrackEvaluationType.Track), new ColorConverter() },
            { new TrackDataKey(TrackType.Start_Alpha, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.End_Alpha, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Start_Size, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.End_Size, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.StartEnd_Weight, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Random_Luminance, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Random_Size, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Particle_BlendMode, TrackEvaluationType.Track), new EnumConverter(typeof(ParticleBlendMode)) },
            { new TrackDataKey(TrackType.Use_Lighting, TrackEvaluationType.Track), new EnumConverter(typeof(Bool)) },
            { new TrackDataKey(TrackType.Shape, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Direction, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Force_Variance, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Force_Gravity, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Phase_Variance, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Phase_Start, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotation_Speed, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Emitter_Geometry, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Speed_Factor, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Playback_Mode, TrackEvaluationType.Track), new EnumConverter(typeof(PlaybackMode)) },
            { new TrackDataKey(TrackType.Random_Force_Factor, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Particles, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Trail_Length, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotation_Offset, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Radiance, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Shape2, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Shape3, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.AlphaTest, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Particle_Texture_Division, TrackEvaluationType.Track), new EnumConverter(typeof(ParticleTextureDivision)) },
            { new TrackDataKey(TrackType.Texture_Division_U, TrackEvaluationType.Track), new EnumConverter(typeof(TextureDivisionSingleAxis)) },
            { new TrackDataKey(TrackType.Texture_Division_V, TrackEvaluationType.Track), new EnumConverter(typeof(TextureDivisionSingleAxis)) },
            { new TrackDataKey(TrackType.Texture_Display, TrackEvaluationType.Track), new EnumConverter(typeof(TextureDisplay)) },
            { new TrackDataKey(TrackType.Optical_Density, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Start_Emissive_Color, TrackEvaluationType.Track), new ColorConverter() },
            { new TrackDataKey(TrackType.End_Emissive_Color, TrackEvaluationType.Track), new ColorConverter() },
            { new TrackDataKey(TrackType.HDR_Exponent, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Glow_Alpha, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Time_Address, TrackEvaluationType.Track), new EnumConverter(typeof(TimeAddress)) },
            { new TrackDataKey(TrackType.Shadow_Mode, TrackEvaluationType.Track), new EnumConverter(typeof(ShadowMode)) },
            { new TrackDataKey(TrackType.Hardness_Modifier, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Particle_Mode, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Distortion_FallOff, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Offset_Towards_Camera, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Alignment_Axis, TrackEvaluationType.Track), new EnumConverter(typeof(AlignmentAxis)) },
            { new TrackDataKey(TrackType.Pivot_Point, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Twosided, TrackEvaluationType.Track), new EnumConverter(typeof(Bool)) },
            { new TrackDataKey(TrackType.BillBoard_Mode, TrackEvaluationType.Track), new EnumConverter(typeof(BillboardMode)) },
            { new TrackDataKey(TrackType.Distortion_FallOff_Dup_1, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Hardness_Modifier_Dup_1, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Dup_1, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Range, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Power, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Ground_Level, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Mass, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Volume, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Min_Falloff, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Max_Falloff, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Pitch_Min, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Amplitude_Translate, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Frequency_Translate, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Amplitude_Rotate, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Frequency_Rotate, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Min_Falloff_Dup_1, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Max_Falloff_Dup_1, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Size_SfpEmitter_Area, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Alpha_Scale, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Alpha_Offset, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Alpha_Test, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Lighting, TrackEvaluationType.Track), new EnumConverter(typeof(Bool)) },
            { new TrackDataKey(TrackType.Fade_Source, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Fade_Target, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Depth_Sort, TrackEvaluationType.Track), new EnumConverter(typeof(Bool)) },
            { new TrackDataKey(TrackType.Gravity, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Dampening, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Scale_Dup_1, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Opening_Angle_H, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Opening_Angle_V, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Velocity, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Velocity_Variation, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Lifetime, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Lifetime_Variation, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rate, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rate_Variation, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Mass_Dup_1, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Mass_Variation, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Strength, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Size_SfpEmitter_Particle, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Size_Variation, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.RampFloat_1_Input, TrackEvaluationType.Track), new EnumConverter(typeof(RampInput)) },
            { new TrackDataKey(TrackType.RampFloat_2_Input, TrackEvaluationType.Track), new EnumConverter(typeof(RampInput)) },
            { new TrackDataKey(TrackType.RampVector3_1_Input, TrackEvaluationType.Track), new EnumConverter(typeof(RampInput)) },
            { new TrackDataKey(TrackType.RampVector3_2_Input, TrackEvaluationType.Track), new EnumConverter(typeof(RampInput)) },
            { new TrackDataKey(TrackType.RampFloat_1_Output, TrackEvaluationType.Track), new EnumConverter(typeof(RampFloatOutput)) },
            { new TrackDataKey(TrackType.RampFloat_2_Output, TrackEvaluationType.Track), new EnumConverter(typeof(RampFloatOutput)) },
            { new TrackDataKey(TrackType.RampVector3_1_Output, TrackEvaluationType.Track), new EnumConverter(typeof(RampVector3Output)) },
            { new TrackDataKey(TrackType.RampVector3_2_Output, TrackEvaluationType.Track), new EnumConverter(typeof(RampVector3Output)) },
            { new TrackDataKey(TrackType.Float_1_Input_Scale, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Float_2_Input_Scale, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Vector3_1_Input_Scale, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Vector3_2_Input_Scale, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Type, TrackEvaluationType.Track), new EnumConverter(typeof(ForceType)) },
            { new TrackDataKey(TrackType.Resilience, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Sphere_Radius, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Falloff_Radius, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Color_Dup_1, TrackEvaluationType.Track), new ColorConverter() },
            { new TrackDataKey(TrackType.Alpha_Dup_1, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Emissive, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Glow_Alpha_Dup_1, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotation_Initial, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotation_Speed_Min, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotation_Speed_Max, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Distortion_Strength, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Shape_Propabilities, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Force_Range, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Full_Power_Range, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Power, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Jitter_Frequency, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Cross_Jitter_Freq, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Jitter_Power, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Force_Cross_Jitter_Power, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Emitter_Shape, TrackEvaluationType.Track), new EnumConverter(typeof(EmitterShape)) },
            { new TrackDataKey(TrackType.Shape_Thickness, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Shape_Angle_H, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Shape_Angle_V, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Pt1_Orientation, TrackEvaluationType.Track), new EnumConverter(typeof(Pt1Orientation)) },
            { new TrackDataKey(TrackType.Distort_Max_Speed, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Distort_Length, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Pitch_Max, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Kill_Radius, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Particle_Distort_Vec, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Uniform_Force, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Rotation_Speed_Dup_1, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotation_Min_Falloff, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotation_Max_Falloff, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Rotation_Tube_Height, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Shadow_Pass, TrackEvaluationType.Track), new EnumConverter(typeof(Bool)) },
            { new TrackDataKey(TrackType.Emitter_Emitter_Quota, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Emitter_Emitter_Index, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Type, TrackEvaluationType.Track), new EnumConverter(typeof(EmitterType)) },
            { new TrackDataKey(TrackType.Effect_Alpha, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Inherit_Alpha, TrackEvaluationType.Track), new EnumConverter(typeof(Bool)) },
            { new TrackDataKey(TrackType.LOD_Bias, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Group_Collision, TrackEvaluationType.Track), new EnumConverter(typeof(Bool)) },
            { new TrackDataKey(TrackType.Offset_Mode, TrackEvaluationType.Track), new EnumConverter(typeof(OffsetMode)) },
            { new TrackDataKey(TrackType.Trail_Alignment, TrackEvaluationType.Track), new EnumConverter(typeof(TrailAllignment)) },
            { new TrackDataKey(TrackType.Start_Width, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.End_Width, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Start_Fadeout, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Lifetime_Dup_1, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Min_Segment_Length, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Trail_Interpolation, TrackEvaluationType.Track), new EnumConverter(typeof(TrailInterpolation)) },
            { new TrackDataKey(TrackType.RampFloat_1_Input_Dup_1, TrackEvaluationType.Track), new EnumConverter(typeof(RampDupInput)) },
            { new TrackDataKey(TrackType.RampFloat_2_Input_Dup_1, TrackEvaluationType.Track), new EnumConverter(typeof(RampDupInput)) },
            { new TrackDataKey(TrackType.RampVector3_1_Input_Dup_1, TrackEvaluationType.Track), new EnumConverter(typeof(RampDupInput)) },
            { new TrackDataKey(TrackType.RampVector3_2_Input_Dup_1, TrackEvaluationType.Track), new EnumConverter(typeof(RampDupInput)) },
            { new TrackDataKey(TrackType.RampFloat_1_Output_Dup_1, TrackEvaluationType.Track), new EnumConverter(typeof(RampDupFloatOutput)) },
            { new TrackDataKey(TrackType.RampFloat_2_Output_Dup_1, TrackEvaluationType.Track), new EnumConverter(typeof(RampDupFloatOutput)) },
            { new TrackDataKey(TrackType.RampVector3_1_Output_Dup_1, TrackEvaluationType.Track), new EnumConverter(typeof(RampDupVector3Output)) },
            { new TrackDataKey(TrackType.RampVector3_2_Output_Dup_1, TrackEvaluationType.Track), new EnumConverter(typeof(RampDupVector3Output)) },
            { new TrackDataKey(TrackType.Distort_Variation, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Texture_Offset, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.Texture_Zoom, TrackEvaluationType.Track), new Vector3Converter() },
            { new TrackDataKey(TrackType.InheritVelocity, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.DepthWriteThreshold, TrackEvaluationType.Track), new FloatConverter() },
            { new TrackDataKey(TrackType.Torque, TrackEvaluationType.Track), new Vector3Converter() },
        };
    }
}
