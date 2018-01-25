﻿using System.Collections.Generic;
using Marvin.Model;

namespace Marvin.Products.Samples.Model
{
    /// <summary>
    /// The public API of the ProductProperties repository.
    /// </summary>
    public interface IAnalogWatchProductPropertiesEntityRepository : IRepository<AnalogWatchProductPropertiesEntity>
    {
		/// <summary>
        /// Get all ProductPropertiess from the database
        /// </summary>
		/// <param name="deleted">Include deleted entities in result</param>
		/// <returns>A collection of entities. The result may be empty but not null.</returns>
        ICollection<AnalogWatchProductPropertiesEntity> GetAll(bool deleted);
        /// <summary>
        /// Creates instance with all not nullable properties prefilled
        /// </summary>
        AnalogWatchProductPropertiesEntity Create(string name); 
    }
}
