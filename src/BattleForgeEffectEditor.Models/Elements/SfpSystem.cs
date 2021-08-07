// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class SfpSystem : Element
    {
        public const uint Header = 0xF85F6575;

        public BfString TextureFilePath { get; set; } = new BfString();

        public SfpSystem(BfString textureFilePath)
        {
            TextureFilePath = textureFilePath;
        }

        public SfpSystem() { }
    }
}
