using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConstructorInjection.Mvc.Models
{
    /// <summary>
    /// View model for Authors.
    /// </summary>
    public class AuthorViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        /// <value>
        /// The job title.
        /// </value>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the bio.
        /// </summary>
        /// <value>
        /// The bio.
        /// </value>
        public string Bio { get; set; }

    }
}
