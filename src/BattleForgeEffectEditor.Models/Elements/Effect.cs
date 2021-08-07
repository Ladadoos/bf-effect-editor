// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class Effect : Element
    {
        public const uint Header = 0xF8EFFE37;

        public BfString EffectFilePath { get; set; } = new BfString();

        public uint Embedded { get; set; } = 0; // Actually a Boolean

        public float Length { get; set; } = 0;

        public Effect(BfString effectFilePath, uint embedded, float length)
        {
            EffectFilePath = effectFilePath;
            Embedded = embedded;
            Length = length;
        }

        public Effect() { }
    }
}
