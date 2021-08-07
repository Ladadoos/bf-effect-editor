// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

namespace BattleForgeEffectEditor.Models.Enums
{
    public enum StaticTrackType : int
    {
        Unknown = 0,
        SoundFilename = 1,
        SoundDistanceMin = 2,
        SoundDistanceMax = 3,
        SoundLoop = 4, //Static = 0, Loop = 1, PlayOnce = 2
        SoundVolume = 5,
        FXVersion = 6,
        SoundVolume_Dup_1 = 7,
        Node_Start = 8,
        Node_End = 9,
        Max_Sfp_count = 10,
        Priority = 11, //EyeCandy = 0, Nice = 1, Important = 2, Critical = 3
        Offset_Rotation = 12,
        Effect_Type = 13, //Dummy = 0, Presentable = 1
        Manual_BCube = 14, //Off = 0, On = 1
        BCube_Size = 15,
        Fade_on_start = 16, //Disable = 0, Enabled = 1
        Fade_on_kill = 17, //Disable = 0, Enabled = 1
        Loop_after = 18,
        Manual_sort_order = 19 //Off = 0, On = 1
    }

    public enum Priority : int
    {
        EyeCandy = 0,
        Nice = 1,
        Important = 2,
        Critical = 3
    }

    public enum SoundLoop : int
    {
        Static = 0,
        Loop = 1,
        PlayOnce = 2
    }

    public enum EffecType : int
    {
        Dummy = 0,
        Presentable = 1
    }

    public enum ManualBCube : int
    {
        Off = 0,
        On = 1
    }

    public enum FadeOnStart : int
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum FadeOnKill : int
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum ManualSortOrder : int
    {
        Off = 1,
        On = 1
    }
}
