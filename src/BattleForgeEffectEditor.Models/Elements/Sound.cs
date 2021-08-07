// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Models.Elements
{
    public class Sound : Element
    {
        public const uint Header = 0xF850C5D0;

        public BfString SoundFilePath { get; set; } = new BfString();

        public Sound(BfString soundFilePath)
        {
            SoundFilePath = soundFilePath;
        }

        public Sound() { }
    }
}
