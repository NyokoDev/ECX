﻿using System;
using ColossalFramework.Plugins;
using UnityEngine;

namespace EyeCandyX
{
    class DebugUtils
    {
        public const string modPrefix = "[Eyecandy X " + EyecandyXMod.version + "] ";

        public static void Message(string message)
        {
            Log(message);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, modPrefix + message);
        }

        public static void Warning(string message)
        {
            Debug.LogWarning(modPrefix + message);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, modPrefix + message);
        }

        public static void Log(string message)
        {
            if (message == m_lastLog)
            {
                m_duplicates++;
            }
            else if (m_duplicates > 0)
            {
                Debug.Log(modPrefix + "(x" + (m_duplicates + 1) + ")");
                Debug.Log(modPrefix + message);
                m_duplicates = 0;
            }
            else
            {
                Debug.Log(modPrefix + message);
            }
            m_lastLog = message;
        }

        public static void LogException(Exception e)
        {
            var message = $"{modPrefix}Unexpected {e.GetType().Name}: {e.Message}\n{e.StackTrace}\n\nInnerException:\n{e.InnerException.Message}";
            Log(message);
        }

        private static string m_lastLog;
        private static int m_duplicates = 0;
    }
}