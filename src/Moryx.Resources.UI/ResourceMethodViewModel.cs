// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using Caliburn.Micro;
using Moryx.Controls;
using Moryx.Serialization;

namespace Moryx.Resources.UI
{
    /// <summary>
    /// View model that represents an editorVisible of a resource
    /// </summary>
    public class ResourceMethodViewModel : PropertyChangedBase
    {
        /// <summary>
        /// Underlying model
        /// </summary>
        public MethodEntry Model { get; private set; }

        /// <summary>
        /// Creates a ViewModel from the given Model
        /// </summary>
        public ResourceMethodViewModel(MethodEntry model)
        {
            UpdateModel(model);
        }

        public void UpdateModel(MethodEntry model)
        {
            Model = model;
            NotifyOfPropertyChange(nameof(DisplayName));
            NotifyOfPropertyChange(nameof(Description));

            Parameters = new EntryViewModel(Model.Parameters);
            NotifyOfPropertyChange(nameof(Parameters));
        }

        /// <summary>
        /// Name of the method
        /// </summary>
        public string DisplayName => Model.DisplayName;

        /// <summary>
        /// Method description
        /// </summary>
        public string Description => Model.Description;

        /// <summary>
        /// Parameters which are necessary for the method call
        /// </summary>
        public EntryViewModel Parameters { get; private set; }
    }
}
