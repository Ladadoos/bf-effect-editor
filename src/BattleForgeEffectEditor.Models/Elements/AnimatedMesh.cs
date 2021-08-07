// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class AnimatedMesh : Element
    {
        public const uint Header = 0xF8A23E54;

        public BfString MeshFilePath { get; set; } = new BfString();

        public BfString AnimationFilePath { get; set; } = new BfString();

        public AnimatedMesh(BfString meshFilePath, BfString animationFilePath)
        {
            MeshFilePath = meshFilePath;
            AnimationFilePath = animationFilePath;
        }

        public AnimatedMesh() { }
    }
}
