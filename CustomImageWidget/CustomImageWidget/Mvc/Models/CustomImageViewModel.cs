using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomImageWidget.Mvc.Models
{
    public class CustomImageViewModel
    {
        /// <summary>
        /// Gets or sets the image title.
        /// </summary>
        public string ImageTitle { get; set; }

        /// <summary>
        /// Gets or sets the image alternative text.
        /// </summary>
        public string ImageAlternativeText { get; set; }

        /// <summary>
        /// Gets or sets the selected size image URL.
        /// </summary>
        public string SelectedSizeUrl { get; set; }
    }
}
