// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class Trail : Element
    {
        public const uint Header = 0xF878A175;

        public BfString TextureFilePath { get; set; } = new BfString();

        public Trail(BfString textureFilePath)
        {
            TextureFilePath = textureFilePath;
        }

        public Trail() { }
    }
}
