// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class StaticDecal : Element
    {
        public const uint Header = 0xF85DECA7;

        public BfString ColorTextureFilePath { get; set; } = new BfString();

        public BfString NormalTextureFilePath { get; set; } = new BfString();

        public StaticDecal(BfString colorTextureFilePath, BfString normalTextureFilePath)
        {
            ColorTextureFilePath = colorTextureFilePath;
            NormalTextureFilePath = normalTextureFilePath;
        }

        public StaticDecal() { }
    }
}
