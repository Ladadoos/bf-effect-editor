// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class Billboard : Element
    {
        public const uint Header = 0xF88177BD;

        public BfString TextureOneFilePath { get; set; } = new BfString();

        public BfString TextureTwoFilePath { get; set; } = new BfString();

        public Billboard(BfString textureOneFilePath, BfString textureTwoFilePath)
        {
            TextureOneFilePath = textureOneFilePath;
            TextureTwoFilePath = textureTwoFilePath;
        }

        public Billboard() { }
    }
}
