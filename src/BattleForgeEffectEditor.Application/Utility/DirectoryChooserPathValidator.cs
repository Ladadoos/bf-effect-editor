using BattleForgeEffectEditor.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattleForgeEffectEditor.Application.Utility
{
    public static class DirectoryChooserPathValidator
    {
        public static DirectoryChooserPathValidatorErrorTypes ValidatePath(string DirectoryPath, DirectoryChooserTypes directoryType)
        {
            if (string.IsNullOrEmpty(DirectoryPath))
                return DirectoryChooserPathValidatorErrorTypes.NotSet;


            if (!DriveExists(in DirectoryPath))
                return DirectoryChooserPathValidatorErrorTypes.DriveNotExist;

            (bool isDirectory, bool Error) IsDirectoryResult = CheckPathIsDirectory(in DirectoryPath);

            if (IsDirectoryResult.Error)
                return DirectoryChooserPathValidatorErrorTypes.PathNotFound;

            if (!IsDirectoryResult.isDirectory)
                return DirectoryChooserPathValidatorErrorTypes.Not_a_Directory;


            if (directoryType == DirectoryChooserTypes.BackupDirectory)
            {
                return ValidateBackupDirectory(in DirectoryPath);
            }
            else if(directoryType == DirectoryChooserTypes.ResourcesDirectory)
            {
                return ValidateResourcesDirectory(in DirectoryPath);
            }

            return DirectoryChooserPathValidatorErrorTypes.NotSet;
        }

        private static DirectoryChooserPathValidatorErrorTypes ValidateBackupDirectory(in string DirectoryPath)
        {
            
            //TODO: Apply other path validation rules

            return DirectoryChooserPathValidatorErrorTypes.Ok;
        }

        private static DirectoryChooserPathValidatorErrorTypes ValidateResourcesDirectory(in string DirectoryPath)
        {

            //TODO: Apply other path validation rules

            return DirectoryChooserPathValidatorErrorTypes.Ok;
        }

        private static (bool isDirectory, bool Error) CheckPathIsDirectory(in string DirectoryPath)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(DirectoryPath);

                if (attr.HasFlag(FileAttributes.Directory))
                    return (true, false);
                else
                    return (false, false);
            }
            catch(FileNotFoundException ex)
            {
                return (false, true);
            }
            catch (Exception)
            {
                //TODO: better handling unknown path Exception
                return (false, true);
            }
            
        }

        private static bool DriveExists(in string DirectoryPath)
        {
            Regex rx = new Regex(@"^((([a-zA-Z]:)|(\))))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = rx.Matches(DirectoryPath.Substring(0,3));

            return matches.Count != 0;
        }
    }
}
