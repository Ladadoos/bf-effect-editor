// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System;
using System.IO;

namespace BattleForgeEffectEditor.Application.Utility
{
    public static class PathValidator
    {
        public static PathValidationError ValidatePath(string directoryPath)
        {
            directoryPath = directoryPath.Trim();

            if (string.IsNullOrEmpty(directoryPath))
                return PathValidationError.InvalidPath;

            if (!DoesDriveInPathExist(in directoryPath))
                return PathValidationError.DriveDoesNotExist;

            (bool isDirectory, bool error) = IsPathDirectory(in directoryPath);
            if (error)
                return PathValidationError.PathNotFound;
            if (!isDirectory)
                return PathValidationError.NotADirectory;

            return PathValidationError.Ok;
        }

        private static (bool isDirectory, bool error) IsPathDirectory(in string directoryPath)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(directoryPath);
                return attr.HasFlag(FileAttributes.Directory) ? (true, false) : (false, false);
            } catch (FileNotFoundException)
            {
                return (false, true);
            } catch (Exception)
            {
                //TODO: better handling unknown path Exception
                return (false, true);
            }
        }

        private static bool DoesDriveInPathExist(in string directoryPath)
        {
            return Directory.Exists(Path.GetPathRoot(directoryPath));
        }
    }
}
