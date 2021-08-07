// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class Emitter : Element
    {
        public const uint Header = 0xF8E31777;

        public BfString TextureFilePath { get; set; } = new BfString();

        public uint ParticleCount { get; set; } = 0;

        public Emitter(BfString textureFilePath, uint particleCount)
        {
            TextureFilePath = textureFilePath;
            ParticleCount = particleCount;
        }

        public Emitter() { }
    }
}
