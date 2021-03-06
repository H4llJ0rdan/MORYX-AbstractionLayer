// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Products;
using Moryx.Modules;

namespace Moryx.Products.Management.Importers
{
    /// <summary>
    /// Interface for plugins that can import products from file
    /// </summary>
    public interface IProductImporter : IConfiguredPlugin<ProductImporterConfig>
    {
        /// <summary>
        /// Name of the importer
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the parameters of this importer
        /// </summary>
        IImportParameters Parameters { get; }

        /// <summary>
        /// Update parameters based on partial input
        /// </summary>
        IImportParameters Update(IImportParameters currentParameters);

        /// <summary>
        /// Import products using given parameters
        /// </summary>
        IProductType[] Import(IImportParameters parameters);
    }
}
