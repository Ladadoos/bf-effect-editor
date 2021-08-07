// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System.Collections.Generic;
using JsonSettings;

namespace BattleForgeEffectEditor.Application.Settings
{
    public class EditorStateSettings : JsonSettingsBase
    {
        public override Scope Scope => Scope.Application;
        public override string CompanyName => "SkylordsReborn";
        public override string ProductName => "EffectEditor";
        public override string Filename => "EditorStateSettings.json";

        public List<string> RecentOpenedFiles { get; set; } = new List<string>();
    }
}
