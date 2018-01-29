﻿using System.Collections.Generic;
using Marvin.AbstractionLayer;

namespace Marvin.Products.Samples
{
    public class WatchProduct : Product
    {
        public override string Type => nameof(WatchProduct);

        // Watch attributes
        public double Price { get; set; }
        public double Weight { get; set; }

        // References to product
        public ProductPartLink<WatchfaceProduct> Watchface { get; set; } = new ProductPartLink<WatchfaceProduct>();

        public List<NeedlePartLink> Needles { get; set; } = new List<NeedlePartLink>();

        protected override Article Instantiate()
        {
            return new WatchArticle
            {
                Watchface = (WatchfaceArticle)Watchface.Instantiate(),
                Neddles = Needles.Instantiate<NeedleArticle>()
            };
        }
    }
}