﻿// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

namespace BattleForgeEffectEditor.Models.Enums
{
    public enum TrackType : int
    {
        Unknown = -1,
        Translate = 0,
        Rotate = 1,
        Scale = 2,
        Anim = 3,
        Color = 4,
        Alpha = 5,
        Size = 6,
        Time = 7,
        BlendMode = 8,
        Light_Range = 9,
        Start_Color = 10,
        End_Color = 11,
        Start_Alpha = 12,
        End_Alpha = 13,
        Start_Radiance = 14,
        End_Radiance = 15,
        Start_Size = 16,
        End_Size = 17,
        StartEnd_Weight = 18,
        Random_Color = 19,
        Random_Luminance = 20,
        Random_Size = 21,
        Particle_BlendMode = 22,
        Use_Lighting = 23, //Use particle color as albedo for passive lighting.
        Shape = 24,
        Force = 25,
        Force_Direction = 26,
        Force_Variance = 27,
        Force_Gravity = 28,
        Phase_Variance = 29,
        Phase_Start = 30,
        Rotation_Speed = 31,
        Emitter_Geometry = 32,
        Speed_Factor = 33,
        Playback_Mode = 34,
        Random_Force_Factor = 35,
        Particles = 36,
        Trail_Length = 37,
        Use_Radiance = 38,
        Rotation_Offset = 39,
        Radiance = 40,
        Shape2 = 41,
        Shape3 = 42,
        AlphaTest = 43,
        Synchrony = 44,
        Radius = 45,
        Radius_Weight = 46,
        Phase = 47,
        Frequency = 48,
        Height_Influence = 49,
        Sine_Size = 50,
        Particle_Texture_Division = 51,
        Texture_Division_U = 52,
        Texture_Division_V = 53,
        Texture_Display = 54,
        Optical_Density = 55, //When optical density is > 0, the alpha channel\nis multiplied with an opacity valuecalculated\nfrom density and particle size.
        Start_Emissive_Color = 56, //The emissive color is multiplied with radiance and added to the particle color.
        End_Emissive_Color = 57, //The emissive color is multiplied with radiance and added to the particle color.
        HDR_Exponent = 58,
        Glow_Alpha = 59,
        Time_Address = 60,
        Shadow_Mode = 61,
        Hardness_Modifier = 62,
        Particle_Mode = 63,
        Distortion_FallOff = 64,
        Offset_Towards_Camera = 65,
        Alignment_Axis = 66,
        Pivot_Point = 67,
        Twosided = 68,
        BillBoard_Mode = 69,
        Distortion_FallOff_Dup_1 = 70,
        Hardness_Modifier_Dup_1 = 71,
        Force_Dup_1 = 72,
        Range = 73,
        Power = 74,
        Ground_Level = 75,
        Mass = 76,
        Volume = 77,
        Min_Falloff = 78,
        Max_Falloff = 79,
        Pitch_Min = 80,
        Amplitude_Translate = 81,
        Frequency_Translate = 82,
        Amplitude_Rotate = 83,
        Frequency_Rotate = 84,
        Min_Falloff_Dup_1 = 85,
        Max_Falloff_Dup_1 = 86,
        Add_Translate = 87, //Add. Translate
        Add_Rotate = 88, //Add. Rotate
        Size_SfpEmitter_Area = 89, //CFxSize_Dup_1
        Alpha_Scale = 90,
        Alpha_Offset = 91,
        Alpha_Test = 92,
        Lighting = 93,
        Fade_Source = 94,
        Fade_Target = 95,
        Depth_Sort = 96,
        Gravity = 97,
        Dampening = 98,
        Scale_Dup_1 = 99,
        Opening_Angle_H = 100,
        Opening_Angle_V = 101,
        Velocity = 102,
        Velocity_Variation = 103,
        Lifetime = 104,
        Lifetime_Variation = 105,
        Rate = 106,
        Rate_Variation = 107,
        Mass_Dup_1 = 108,
        Mass_Variation = 109,
        Strength = 110,
        Size_SfpEmitter_Particle = 111, //CFxSize_Dup_2
        Size_Variation = 112,
        RampFloat_1_Input = 113,
        RampFloat_2_Input = 114,
        RampVector3_1_Input = 115,
        RampVector3_2_Input = 116,
        RampFloat_1_Output = 117,
        RampFloat_2_Output = 118,
        RampVector3_1_Output = 119,
        RampVector3_2_Output = 120,
        Float_1_Input_Scale = 121,
        Float_2_Input_Scale = 122,
        Vector3_1_Input_Scale = 123,
        Vector3_2_Input_Scale = 124,
        Force_Type = 125,
        Resilience = 126,
        Sphere_Radius = 127,
        Falloff_Radius = 128,
        Color_Dup_1 = 129,
        Alpha_Dup_1 = 130,
        Emissive = 131,
        Glow_Alpha_Dup_1 = 132,
        Rotation_Initial = 133,
        Rotation_Speed_Min = 134,
        Rotation_Speed_Max = 135,
        Distortion_Strength = 136,
        Shape_Propabilities = 137,
        Force_Range = 138,
        Force_Full_Power_Range = 139,
        Force_Power = 140,
        Force_Jitter_Frequency = 141,
        Force_Cross_Jitter_Freq = 142,
        Force_Jitter_Power = 143,
        Force_Cross_Jitter_Power = 144,
        Emitter_Shape = 145,
        Shape_Thickness = 146,
        Shape_Angle_H = 147,
        Shape_Angle_V = 148,
        Pt1_Orientation = 149,
        Distort_Max_Speed = 150,
        Distort_Length = 151,
        Pitch_Max = 152,
        Kill_Radius = 153,
        Particle_Distort_Vec = 154,
        Uniform_Force = 155,
        Rotation_Speed_Dup_1 = 156,
        Rotation_Min_Falloff = 157,
        Rotation_Max_Falloff = 158,
        Rotation_Tube_Height = 159,
        Shadow_Pass = 160,
        Emitter_Emitter_Quota = 161,
        Emitter_Emitter_Index = 162,
        Type = 163,
        Effect_Alpha = 164,
        Inherit_Alpha = 165,
        LOD_Bias = 166,
        Group_Collision = 167,
        Offset_Mode = 168,
        Trail_Alignment = 169,
        Start_Width = 170,
        End_Width = 171,
        Start_Fadeout = 172,
        Lifetime_Dup_1 = 173,
        Min_Segment_Length = 174,
        Trail_Interpolation = 175,
        RampFloat_1_Input_Dup_1 = 176,
        RampFloat_2_Input_Dup_1 = 177,
        RampVector3_1_Input_Dup_1 = 178,
        RampVector3_2_Input_Dup_1 = 179,
        RampFloat_1_Output_Dup_1 = 180,
        RampFloat_2_Output_Dup_1 = 181,
        RampVector3_1_Output_Dup_1 = 182,
        RampVector3_2_Output_Dup_1 = 183,
        Distort_Variation = 184,
        Texture_Offset = 185,
        Texture_Zoom = 186,
        InheritVelocity = 187,
        DepthWriteThreshold = 188,
        Torque = 189
    }
}
