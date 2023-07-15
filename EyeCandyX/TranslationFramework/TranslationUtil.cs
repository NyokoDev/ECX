using System.Linq;
using ColossalFramework.Plugins;
using EyeCandyX;
using ICities;

namespace EyecandyX.TranslationFramework
{
    public static class TranslationUtil
    {
        public static string AssemblyPath => PluginInfo.modPath;

        internal static PluginManager.PluginInfo PluginInfo
        {
            get
            {
                var pluginManager = PluginManager.instance;
                var plugins = pluginManager.GetPluginsInfo();

                foreach (var item in plugins)
                {
                    try
                    {
                        var instances = item.GetInstances<IUserMod>();
                        if (!(instances.FirstOrDefault() is EyecandyXMod))
                            continue;

                        return item;
                    }
                    catch
                    {

                    }
                }

                // If no PluginInfo was found in the loop above, return null
                return null;
            }
        }
    }
}