// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

namespace BattleForgeEffectEditor.Models.Enums
{
    public enum TextureDivisionSingleAxis : int
    {
        One = 0,
        Two = 1,
        Four = 2,
        Eight = 3,
        Sixteen = 4
    }

    public enum ParticleTextureDivision : int
    {
        OneByOne = 0,
        TwoByTwo = 1,
        FourByFour = 2,
        EightByEight = 3,
        SixteenBySixteen = 4
    }

    public enum Bool : int
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum PlaybackMode : int
    {
        Foreward = 0,
        Backward = 1,
        PingPong = 2,
        Sinus = 3,
        Parabola = 4,
        Curve = 5,
        FadeIn = 6,
        FadeOut = 7,
        SmoothFadeIn = 8,
        SmoothFadeOout = 9
    }

    public enum TextureDisplay : int
    {
        NotPremultiplied = 0,
        DefaultPremultiplied = 1,
    }

    public enum TimeAddress : int
    {
        Repeat = 0,
        Clamp = 1,
        ClampToTransparency = 2
    }

    public enum ShadowMode : int
    {
        None = 0,
        Cast = 1,
        Receive = 2,
        CastAndReceive = 3
    }

    public enum AlignmentAxis : int
    {
        Camera = 0,
        LockWorldYAxis = 1,
        LockObjectXAxis = 2,
        LockObjectYAxis = 3,
        LockParentYAxis = 4,
        LockParentZAxis = 5,
        Arbitrary = 6
    }

    public enum BillboardMode : int
    {
        Default = 0,
        Soft = 1,
        Distortion = 2
    }

    public enum RampInput : int
    {
        None = 0,
        Lifetime = 1,
        Speed = 2,
        Age = 3,
        Rotation = 4,
        RotationSpeed = 5
    }

    public enum RampFloatOutput : int
    {
        None = 0,
        Alpha = 1,
        GlowAlpha = 2,
        Size = 3,
        Rotation = 4,
        Distortion = 5,
        LODBias = 6,
        Shape = 7
    }

    public enum RampVector3Output : int
    {
        None = 0,
        Color = 1,
        Emissive = 2
    }

    public enum ForceType : int
    {
        Newton = 0,
        CollisionSphere = 1,
        CollisionSphereInverse = 2,
        Turbulence = 3,
        Uniform = 4,
        Rotation = 5
    }

    public enum EmitterShape : int
    {
        Box = 0,
        SolidSphere = 1,
        HollowSphere = 2,
        Ring = 3
    }

    public enum Pt1Orientation : int
    {
        CameraAxisRotation = 0,
        Distort = 1,
        DistortUnitform = 2,
        DistortDirection = 3,
        DistortToPoint = 4,
        OrientVelocity = 5,
        OrientDirection = 6,
        OrientToPoint = 7
    }

    public enum EmitterType : int
    {
        BaseEmitter = 0,
        EmitterTemplate = 1
    }

    public enum OffsetMode : int
    {
        OCRelative = 0,
        AbsoluteObject = 1,
        AbsoluteCamera = 2
    }

    public enum TrailAllignment : int
    {
        Camera = 0,
        Emitted = 1
    }

    public enum TrailInterpolation : int
    {
        None = 0,
        TwoTimes = 1,
        FourTimes = 2
    }

    public enum RampDupInput : int
    {
        None = 0,
        Lifetime = 1
    }

    public enum RampDupFloatOutput : int
    {
        None = 0,
        Alpha = 1,
        Width = 2
    }

    public enum RampDupVector3Output : int
    {
        None = 0,
        Color = 1
    }

    public enum ParticleBlendMode : int
    {
        Disable = 0,
        Add = 1,
        AddSoft = 2,
        Blend = 3,
        Max = 4,
        Min = 5,
        Multiply = 6,
        Subtract = 7
    }

    //Very likely very similar to ParticleBlendMode
    public enum LegacyBlendMode : int
    {
        Unknown0 = 770,
        Unknown1 = 771,
        Unknown3 = 773,
        Unknown4 = 774,
        Unknown5 = 776,
        Unknown6 = 779,
        Unknown7 = 780,
        Unknown8 = 782,
        Unknown9 = 783
    }
}
