// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

namespace BattleForgeEffectEditor.Models
{
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Zero => new Vector3 { X = 0, Y = 0, Z = 0 };

        public override string ToString()
        {
            return "Vector3(x=" + X + ",y=" + Y + ",z=" + Z + ")";
        }
    }
}
