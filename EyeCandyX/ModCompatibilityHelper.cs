using System;
using System.IO;
using System.Text;
using ColossalFramework.UI;
using UnityEngine;

namespace CompCheck
{

    public class CompatibilityHelper
    {
        private static string steamAppsFolder;

    
        static CompatibilityHelper()
        {
            try
            {
                steamAppsFolder = GetSteamAppsFolder();
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately (e.g., log, display an error message, etc.)
                Console.WriteLine($"Failed to retrieve SteamApps folder: {ex.Message}");
                steamAppsFolder = string.Empty; // Set steamAppsFolder to an empty string to indicate failure
            }
        }

        private static string GetSteamAppsFolder()
        {
            string programFilesFolder = GetProgramFilesFolder();

            // Check if running on macOS
            if (Environment.OSVersion.Platform == PlatformID.MacOSX || Environment.OSVersion.Platform == PlatformID.Unix)
            {
                string homeFolder = GetHomeFolder();
                return PathCombine(homeFolder, "Library/Application Support/Steam/steamapps");
            }
            // Check if running on Windows
            else if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                return PathCombine(programFilesFolder, "Steam", "steamapps");
            }
            else
            {
                throw new Exception("Unsupported operating system");
            }
        }

        private static string GetHomeFolder()
        {
            string homePath = Environment.GetEnvironmentVariable("HOME");
            if (!string.IsNullOrEmpty(homePath))
            {
                return homePath;
            }

            string userProfile = Environment.GetEnvironmentVariable("USERPROFILE");
            if (!string.IsNullOrEmpty(userProfile))
            {
                return userProfile;
            }

            throw new Exception("Unable to determine user home folder");
        }

        private static string GetProgramFilesFolder()
        {
            string programFilesPath = Environment.GetEnvironmentVariable("PROGRAMFILES(X86)");
            if (!string.IsNullOrEmpty(programFilesPath))
            {
                return programFilesPath;
            }

            programFilesPath = Environment.GetEnvironmentVariable("PROGRAMFILES");
            if (!string.IsNullOrEmpty(programFilesPath))
            {
                return programFilesPath;
            }

            throw new Exception("Unable to determine Program Files folder");
        }

        private static string PathCombine(params string[] paths)
        {
            string combinedPath = paths[0];
            for (int i = 1; i < paths.Length; i++)
            {
                combinedPath = Path.Combine(combinedPath, paths[i]);
            }

            return combinedPath;
        }

        private static bool IsModInstalled(string steamId)
        {
            if (string.IsNullOrEmpty(steamAppsFolder))
            {
                throw new Exception("SteamApps folder is not available");
            }

            string modFolderPath = PathCombine(steamAppsFolder, "workshop", "content", "255710", steamId);

            return Directory.Exists(modFolderPath);
        }

        // Mod installation status properties
        public static bool IsLutCreatorInstalled
        {
            get
            {
                try
                {
                    return IsModInstalled("2979442499");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check LutCreator installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsEyecandyXInstalled
        {
            get
            {
                try
                {
                    return IsModInstalled("2980339529");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check EyecandyX installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsPlayItInstalled
        {
            get
            {
                try
                {
                    return IsModInstalled("2741726428");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check PlayIt installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsRelightInstalled
        {
            get
            {
                try
                {
                    return IsModInstalled("1209581656");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check Relight installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsRenderItInstalled
        {
            get
            {
                try
                {
                    return IsModInstalled("1794015399");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check RenderIt installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsSpeedSliderV2Installed
        {
            get
            {
                try
                {
                    return IsModInstalled("406354745");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check SpeedSliderV2 installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsThemeMixer2Installed
        {
            get
            {
                try
                {
                    return IsModInstalled("1899640536");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check ThemeMixer2 installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsThemeMixer2_5Installed
        {
            get
            {
                try
                {
                    return IsModInstalled("2954236385");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check ThemeMixer2_5 installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsUltimateEyecandyInstalled
        {
            get
            {
                try
                {
                    return IsModInstalled("672248733");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check UltimateEyecandy installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }

        public static bool IsUnifiedUIInstalled
        {
            get
            {
                try
                {
                    return IsModInstalled("2966990700");
                }
                catch (Exception ex)
                {
                    // Handle the exception appropriately (e.g., log, display an error message, etc.)
                    Console.WriteLine($"Failed to check UnifiedUI installation: {ex.Message}");
                    return false; // Set to false to indicate failure
                }
            }
        }


        public static void ShowInstalledVisualMods()
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Installed Visual Mods:");

            if (IsLutCreatorInstalled)
            {
                messageBuilder.AppendLine("- LutCreator");
            }

            if (IsEyecandyXInstalled)
            {
                messageBuilder.AppendLine("- EyecandyX");
            }

            if (IsPlayItInstalled)
            {
                messageBuilder.AppendLine("- PlayIt");
            }

            if (IsRelightInstalled)
            {
                messageBuilder.AppendLine("- Relight");
            }

            if (IsRenderItInstalled)
            {
                messageBuilder.AppendLine("- RenderIt");
            }

            if (IsSpeedSliderV2Installed)
            {
                messageBuilder.AppendLine("- SpeedSliderV2");
            }

            if (IsThemeMixer2Installed)
            {
                messageBuilder.AppendLine("- ThemeMixer2");
            }

            if (IsThemeMixer2_5Installed)
            {
                messageBuilder.AppendLine("- ThemeMixer2.5");
            }

            if (IsUltimateEyecandyInstalled)
            {
                messageBuilder.AppendLine("- UltimateEyecandy");
            }

            if (IsUnifiedUIInstalled)
            {
                messageBuilder.AppendLine("- UnifiedUI");
            }

            // Get GPU RAM information
           

          

            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ECX.ecx");
            File.WriteAllText(logFilePath, messageBuilder.ToString());


        }
    }
}

