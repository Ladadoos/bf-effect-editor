// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

namespace BattleForgeEffectEditor.Models.Elements
{
    public class Light : Element
    {
        public const uint Header = 0xF8716470;

        public uint Range { get; set; } = 0;

        public float Radiance { get; set; } = 0;

        public Light(uint range, float radiance)
        {
            Range = range;
            Radiance = radiance;
        }

        public Light() { }
    }
}
