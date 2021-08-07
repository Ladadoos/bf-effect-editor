// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System;

namespace BattleForgeEffectEditor.Models.Utility
{
    class UnexpectedDataException : Exception
    {
        public UnexpectedDataException()
        {
        }

        public UnexpectedDataException(string message) : base(message)
        {
        }

        public UnexpectedDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
