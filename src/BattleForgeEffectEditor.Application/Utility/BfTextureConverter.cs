// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Settings;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BattleForgeEffectEditor.Application.Utility
{
    public class BfTextureConverter : IValueConverter
    {
        private static SettingsService settingsService = new SettingsService();

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return "pack://application:,,,/Resources/Editor/NoImage.png";

            if (value != null)
            {
                string textureName = Convert.ToString(value).Trim();
                if (!string.IsNullOrEmpty(textureName))
                {
                    string path = settingsService.GetResourcesDirectory() + "/textures/" + textureName;
                    if (File.Exists(path))
                    {
                        try
                        {
                            return new BitmapImage(new Uri(path));
                        } catch (Exception)
                        {
                            return "pack://application:,,,/Resources/Editor/Corrupted.png";
                        }
                    }
                }
            }

            return "pack://application:,,,/Resources/Editor/NoImage.png";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
