// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class Mesh : Element
    {
        public const uint Header = 0xF83E5400;

        public BfString MeshFilePath { get; set; } = new BfString();

        public Mesh(BfString meshFilePath)
        {
            MeshFilePath = meshFilePath;
        }

        public Mesh() { }
    }
}
