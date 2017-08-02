﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LetsEncrypt.ACME.Simple
{
    public class Target
    {
        public const short DefaultHttpsPort = 443;
        public static readonly IReadOnlyDictionary<string, Plugin> Plugins;

        static Target()
        {
            Plugins = Assembly.GetExecutingAssembly()
                              .GetTypes()
                              .Where(type => type.BaseType == typeof(Plugin))
                              .Select(type => type.GetConstructor(Type.EmptyTypes).Invoke(null))
                              .Cast<Plugin>()
                              .ToDictionary(plugin => plugin.Name);
        }

        public Target()
        {
            HttpsPort = DefaultHttpsPort;
        }

        public int HttpsPort { get; set; }
        public string Host { get; set; }
        public string WebRootPath { get; set; }
        public long SiteId { get; set; }
        public List<string> AlternativeNames { get; set; }
        public string PluginName { get; set; } = "IIS";
        public Plugin Plugin => Plugins[PluginName];

        public override string ToString() => $"{PluginName} {Host} ({WebRootPath})";
    }
}