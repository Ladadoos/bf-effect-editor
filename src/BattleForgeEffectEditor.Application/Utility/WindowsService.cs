// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;

namespace BattleForgeEffectEditor.Application.Utility
{
    public class WindowsService
    {
        private const int SW_RESTORE = 9;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        public bool PullUpAndFocusWindowsOfProcess(string processName)
        {
            bool focussed = false;

            IEnumerable<Process> processes =
                Process.GetProcesses().Where(p => p.ProcessName == processName).ToList();
            foreach (Process process in processes)
            {
                IntPtr handle = process.MainWindowHandle;
                if (IsIconic(handle))
                    ShowWindow(handle, SW_RESTORE);

                SetForegroundWindow(handle);
                focussed = true;
            }

            return focussed;
        }

        public void PlayExclamationSound() => SystemSounds.Exclamation.Play();

        public void TryOpenExplorerOnFile(string fullFilePath)
        {
            if (!File.Exists(fullFilePath))
                return;

            string argument = "/select, \"" + fullFilePath + "\"";
            Process.Start("explorer.exe", argument);
        }

        public void TryOpenExplorerOnDirectory(string fullDirPath)
        {
            if (!Directory.Exists(fullDirPath))
                return;
            Process.Start("explorer.exe", fullDirPath);
        }
    }
}