using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp
{
    public class ElementConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementConfiguration"/> class.
        /// </summary>
        /// <param name="backendFieldType">Type of the backend field.</param>
        /// <param name="elementConfigurator">The element configurator.</param>
        public ElementConfiguration(Type backendFieldType, IElementConfigurator elementConfigurator)
        {
            this.BackendFieldType = backendFieldType;
            this.ElementConfigurator = elementConfigurator;
        }

        /// <summary>
        /// Gets or sets the type of the backend field.
        /// </summary>
        /// <value>
        /// The type of the backend field.
        /// </value>
        public Type BackendFieldType { get; set; }

        /// <summary>
        /// Gets or sets the element configurator.
        /// </summary>
        /// <value>
        /// The element configurator.
        /// </value>
        public IElementConfigurator ElementConfigurator { get; set; }
    }
}