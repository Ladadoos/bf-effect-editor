using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleForgeEffectEditor.Models.Enums
{
    public enum DirectoryChooserPathValidatorErrorTypes
    {
        Ok,
        DriveNotExist,
        PathNotFound,
        Not_a_Directory,
        NotEmptyResourcesNotFound,
        NotEmptyBackupNoBackupsFound,
        NotSet,
    }
}
