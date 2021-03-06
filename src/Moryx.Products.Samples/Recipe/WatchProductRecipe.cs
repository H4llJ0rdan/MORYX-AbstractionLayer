// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.ComponentModel;
using Moryx.AbstractionLayer;
using Moryx.AbstractionLayer.Products;
using Moryx.AbstractionLayer.Recipes;

namespace Moryx.Products.Samples.Recipe
{
    public class WatchProductRecipe : WorkplanRecipe, IProductRecipe
    {
        public WatchProductRecipe()
        {
        }

        public WatchProductRecipe(WatchProductRecipe source) : base(source)
        {
            Product = source.Product;
        }

        /// <inheritdoc />
        public IProductType Product { get; set; }

        /// <inheritdoc />
        public virtual IProductType Target => Product;

        [EditorBrowsable]
        [DisplayName("Cores Installed")]
        public int CoresInstalled { get; set; }

        [EditorBrowsable]
        [DisplayName("Case Parameters")]
        public CaseDescription Case { get; set; }

        /// <inheritdoc />
        public override IRecipe Clone()
        {
            return new WatchProductRecipe(this);
        }

        /// <summary>
        /// Create a <see cref="ProductionProcess"/> for this recipe
        /// </summary>
        public override IProcess CreateProcess() =>
            new ProductionProcess { Recipe = this };
    }

    public class CaseDescription
    {
        [EditorBrowsable]
        [DisplayName("Color Code")]
        [Description("Numeric code of the color")]
        public int CaseColorCode { get; set; }

        [EditorBrowsable]
        [DisplayName("Material")]
        public int CaseMaterial { get; set; }
    }
}
