// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System;
using System.Windows;

namespace BattleForgeEffectEditor.Application.Resources.Themes
{
    public enum AppTheme
    {
        LightTheme,
        DarkTheme
    }

    internal class ThemeHandler
    {
        public static AppTheme CurrentTheme { get; set; }

        private static ResourceDictionary ThemeDictionary
        {
            get { return System.Windows.Application.Current.Resources.MergedDictionaries[0]; }
            set { System.Windows.Application.Current.Resources.MergedDictionaries[0] = value; }
        }

        private static void ChangeTheme(Uri uri)
        {
            ThemeDictionary = new ResourceDictionary() { Source = uri };
        }

        public static void SetTheme(AppTheme theme, bool CustomTheme = false, string ThemeName = "", string ThemePath = "")
        {
            string themeName = null;
            CurrentTheme = theme;

            if (!CustomTheme)
            {
                switch (theme)
                {
                    case AppTheme.DarkTheme: themeName = "DarkTheme"; break;
                    case AppTheme.LightTheme: themeName = "LightTheme"; break;
                }
                try
                {
                    if (!string.IsNullOrEmpty(themeName))
                        ChangeTheme(new Uri($"Resources/Themes/{themeName}.xaml", UriKind.Relative));
                } catch (Exception e)
                {
                    Console.WriteLine($"Exception setting theme. Theme {theme}, CustomTheme {CustomTheme}, " +
                        $"ThemeName {ThemeName}, ThemePath {ThemePath}. Exception: {e}");
                }
            } else
            {
                if (string.IsNullOrEmpty(ThemePath))
                    ThemePath = AppDomain.CurrentDomain.BaseDirectory + @"\Themes";
                LoadCustomeTheme(ThemeName, ThemePath);
            }
        }

        private static void LoadCustomeTheme(string themeName, string ThemePath)
        {
            if (!string.IsNullOrEmpty(themeName) && !string.IsNullOrEmpty(ThemePath))
                ChangeTheme(new Uri($@"{ThemePath}\{themeName}", UriKind.Relative));
        }
    }
}
