// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class Physic : Element
    {
        public const uint Header = 0xF8504859;

        public BfString MeshFilePath { get; set; } = new BfString();

        public Physic(BfString meshFilePath)
        {
            MeshFilePath = meshFilePath;
        }

        public Physic() { }
    }
}
