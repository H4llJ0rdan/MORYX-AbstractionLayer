// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moryx.AbstractionLayer.Identity;
using Moryx.AbstractionLayer.Recipes;
using Moryx.Workflows;

namespace Moryx.AbstractionLayer.Products
{
    /// <summary>
    /// Merged facade for products and product instances
    /// </summary>
    public interface IProductManagement : IRecipeProvider, IWorkplans
    {
        /// <summary>
        /// Get product types based on a query
        /// </summary>
        IReadOnlyList<IProductType> LoadTypes(ProductQuery query);

        /// <summary>
        /// Load product type by id
        /// </summary>
        IProductType LoadType(long id);

        /// <summary>
        /// Load product type by identity
        /// </summary>
        IProductType LoadType(ProductIdentity identity);

        /// <summary>
        /// Event raised when a product type changed
        /// </summary>
        event EventHandler<IProductType> TypeChanged;

        /// <summary>
        /// Duplicate a product under a new identity
        /// </summary>
        /// <exception cref="IdentityConflictException">Thrown when the new identity causes conflicts</exception>
        IProductType Duplicate(IProductType template, ProductIdentity newIdentity);

        /// <summary>
        /// Save a product type
        /// </summary>
        long SaveType(IProductType modifiedInstance);

        /// <summary>
        /// All importers and their parameters currently configured in the machine
        /// </summary>
        IDictionary<string, IImportParameters> Importers { get; }

        /// <summary>
        /// Import product types for the given parameters with the named importer
        /// </summary>
        IReadOnlyList<IProductType> ImportTypes(string importerName, IImportParameters parameters);

        /// <summary>
        /// Retrieves the current recipe for this product
        /// </summary>
        IReadOnlyList<IProductRecipe> GetRecipes(IProductType productType, RecipeClassification classification);

        /// <summary>
        /// Saves given recipe to the storage
        /// </summary>
        long SaveRecipe(IProductRecipe recipe);

        /// <summary>
        /// Create an product instance of given product
        /// </summary>
        /// <param name="productType">Product to instanciate</param>
        /// <returns>Unsaved instance</returns>
        ProductInstance CreateInstance(IProductType productType);

        /// <summary>
        /// Create an product instance of given product
        /// </summary>
        /// <param name="productType">Product type to instanciate</param>
        /// <param name="save">Flag if new instance should already be saved</param>
        /// <returns>New instance</returns>
        ProductInstance CreateInstance(IProductType productType, bool save);

        /// <summary>
        /// Get an product instance with the given id.
        /// </summary>
        /// <param name="id">The id for the product instance which should be searched for.</param>
        /// <returns>The product instance with the id when it exists.</returns>
        ProductInstance GetInstance(long id);

        /// <summary>
        /// Get a product instance by identity
        /// </summary>
        TInstance GetInstance<TInstance>(IIdentity identity)
            where TInstance : ProductInstance;

        /// <summary>
        /// Get product instance by expression
        /// </summary>
        TInstance GetInstance<TInstance>(Expression<Predicate<TInstance>> selector)
            where TInstance : ProductInstance;

        /// <summary>
        /// Updates the database from the product instance
        /// </summary>
        void SaveInstance(ProductInstance productInstance);

        /// <summary>
        /// Updates the database from the product instance
        /// </summary>
        void SaveInstances(ProductInstance[] productInstances);

        /// <summary>
        /// Get all instances that match a certain 
        /// </summary>
        IEnumerable<TInstance> GetInstances<TInstance>(Expression<Predicate<TInstance>> selector)
            where TInstance : ProductInstance;
    }
}
