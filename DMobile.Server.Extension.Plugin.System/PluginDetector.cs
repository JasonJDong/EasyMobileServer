using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DMobile.Server.Extension.Plugin.Common;

namespace DMobile.Server.Extension.Plugin.System
{
    public static class PluginDetector
    {
        private const string PLUGIN_SUFFIX = "plugin";
        private const string PLUGIN_DIRECTORY_NAME = "plugins";

        private static IList<string> FindPluginAssemblyLocation()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullPath = assembly.CodeBase.Replace("file:///", "");
            var file = new FileInfo(fullPath);
            DirectoryInfo currentDir = file.Directory;
            var plugins = new List<string>();

            if (currentDir != null)
            {
                string locDir = Path.Combine(currentDir.FullName, PLUGIN_DIRECTORY_NAME);
                var pluginDir = new DirectoryInfo(locDir);

                try
                {
                    FileInfo[] configs = pluginDir.GetFiles(string.Format("*{0}.dll", PLUGIN_SUFFIX));

                    plugins.AddRange(configs.Select(config => config.FullName));
                }
                catch (Exception)
                {
                    return plugins;
                }
            }
            return plugins;
        }

        public static IList<PluginBase> GetPlugins()
        {
            IList<string> pluginsPath = FindPluginAssemblyLocation();
            var plugins = new List<PluginBase>(pluginsPath.Count);
            plugins.AddRange(pluginsPath.Select(LoadPlugin).Where(plugin => plugin != null));
            return plugins;
        }

        private static PluginBase LoadPlugin(string pluginPath)
        {
            if (File.Exists(pluginPath))
            {
                Assembly assembly = Assembly.LoadFile(pluginPath);
                if (assembly != null)
                {
                    Type[] types = assembly.GetExportedTypes();
                    foreach (Type type in types)
                    {
                        if (type.BaseType != null && type.BaseType.Equals(typeof(PluginBase)))
                        {
                            return Activator.CreateInstance(type) as PluginBase;
                        }
                    }
                }
            }
            return null;
        }
    }
}