﻿using System.Runtime.Serialization;
using Marvin.Modules.ModulePlugins;

namespace Marvin.Products.Management.Importers
{
    /// <summary>
    /// Config for product importers
    /// </summary>
    [DataContract]
    public abstract class ProductImporterConfig : IPluginConfig
    {
        /// <summary>
        /// Name of the component represented by this entry
        /// </summary>
        public abstract string PluginName { get; }
    }
}