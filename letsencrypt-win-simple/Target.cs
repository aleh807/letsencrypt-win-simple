﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LetsEncrypt.ACME.Simple
{
    public class Target
    {
        public const short DefaultHttpsPort = 443;
        public static Dictionary<string, Plugin> Plugins = new Dictionary<string, Plugin>();

        static Target()
        {
            foreach (
                var pluginType in
                    (from t in Assembly.GetExecutingAssembly().GetTypes() where t.BaseType == typeof (Plugin) select t))
            {
                AddPlugin(pluginType);
            }
        }

        public Target()
        {
            HttpsPort = DefaultHttpsPort;
        }

        static void AddPlugin(Type type)
        {
            var plugin = type.GetConstructor(new Type[] {}).Invoke(null) as Plugin;
            Plugins.Add(plugin.Name, plugin);
        }

        public string Host { get; set; }
        public int HttpsPort { get; set; }
        public string WebRootPath { get; set; }
        public long SiteId { get; set; }
        public List<string> AlternativeNames { get; set; }
        public string PluginName { get; set; } = "IIS";
        public Plugin Plugin => Plugins[PluginName];

        public override string ToString() => $"{PluginName} {Host} ({WebRootPath})";
    }
}