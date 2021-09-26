// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using JsonSettings;

namespace BattleForgeEffectEditor.Application.Settings
{
    public class EditorSettings : JsonSettingsBase
    {
        public override Scope Scope => Scope.Application;
        public override string CompanyName => "SkylordsReborn";
        public override string ProductName => "EffectEditor";
        public override string Filename => "EditorSettings.json";

        public string MapEditorDirectory { get; set; } = string.Empty;

        public string BackupDirectory { get; set; } = string.Empty;

        public string ResourcesDirectory { get; set; } = string.Empty;

        public bool FocusMapEditorOnSave { get; set; } = true;

        public bool AppInDarkTheme { get; set; } = false;
    }
}
