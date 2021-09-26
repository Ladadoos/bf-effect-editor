using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattleForgeEffectEditor.Application.Resources.Themes
{
    class ThemeHandler
    {
        public enum AppTheme
        {
            LightTheme,
            DarkTheme
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theme">Theme Enum</param>
        /// <param name="CustomTheme">Load custom theme</param>
        /// <param name="ThemeName">Name of theme</param>
        /// <param name="ThemePath">PathDirectory from settings</param>
        public static void SetTheme(AppTheme theme, bool CustomTheme = false, string ThemeName = "", string ThemePath = "")
        {
            string themeName = null;
            CurrentTheme = theme;

            if(!CustomTheme)
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
                }
                catch { }
            }
            else
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
