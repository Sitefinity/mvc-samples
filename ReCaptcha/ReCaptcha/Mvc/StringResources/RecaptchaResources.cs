using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace ReCaptcha.Mvc.StringResources
{
    /// <summary>
    /// Localizable strings for the Form's field widgets
    /// </summary>
    [ObjectInfo(typeof(RecaptchaResources), ResourceClassId = "RecaptchaResources", Title = "RecaptchaResourcesTitle", Description = "RecaptchaResourcesDescription")]
    public class RecaptchaResources : Resource
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaResources"/> class. 
        /// Initializes new instance of <see cref="RecaptchaResources"/> class with the default <see cref="ResourceDataProvider"/>.
        /// </summary>
        public RecaptchaResources()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaResources"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider.</param>
        public RecaptchaResources(ResourceDataProvider dataProvider)
            : base(dataProvider)
        {
        }
        #endregion

        #region Class Description

        /// <summary>
        /// Gets Title for the reCAPTCHA widget resources class.
        /// </summary>
        [ResourceEntry("RecaptchaResourcesTitle",
            Value = "reCAPTCHA widgets resources",
            Description = "Title for the reCAPTCHA widget resources class.",
            LastModified = "2015/10/01")]
        public string RecaptchaResourcesTitle
        {
            get
            {
                return this["RecaptchaResourcesTitle"];
            }
        }

        /// <summary>
        /// Gets Description for the reCAPTCHA widget resources class.
        /// </summary>
        [ResourceEntry("RecaptchaResourcesDescription",
            Value = "reCAPTCHA widget resources",
            Description = "Description for the reCAPTCHA widget resources class.",
            LastModified = "2015/10/01")]
        public string RecaptchaResourcesDescription
        {
            get
            {
                return this["RecaptchaResourcesDescription"];
            }
        }

        #endregion

        /// <summary>
        /// Gets phrase : What's this?
        /// </summary>
        [ResourceEntry("WhatsThis",
            Value = "What's this?",
            Description = "phrase : What's this?",
            LastModified = "2015/10/01")]
        public string WhatsThis
        {
            get
            {
                return this["WhatsThis"];
            }
        }

        /// <summary>
        /// Gets description for reCapthca widget
        /// </summary>
        [ResourceEntry("WhatsRecaptchaDescription",
            Value = "reCAPTCHA is a free service by Google to protect your website from spam and abuse.",
            Description = "Description for reCapthca widget",
            LastModified = "2015/10/01")]
        public string WhatsRecaptchaDescription
        {
            get
            {
                return this["WhatsRecaptchaDescription"];
            }
        }

        /// <summary>
        /// Gets phrase : Learn more.
        /// </summary>
        [ResourceEntry("LearnMore",
            Value = "Learn more",
            Description = "phrase : Learn more",
            LastModified = "2015/10/01")]
        public string LearnMore
        {
            get
            {
                return this["LearnMore"];
            }
        }

        /// <summary>
        /// Gets phrase : Request unauthenticated users only
        /// </summary>
        [ResourceEntry("RequestUnauthenticatedUsersOnly",
            Value = "Request unauthenticated users only",
            Description = "phrase : Request unauthenticated users only",
            LastModified = "2015/10/01")]
        public string RequestUnauthenticatedUsersOnly
        {
            get
            {
                return this["RequestUnauthenticatedUsersOnly"];
            }
        }

        /// <summary>
        /// Gets word : Site Key
        /// </summary>
        [ResourceEntry("SiteKey",
            Value = "Site Key",
            Description = "word : Site Key",
            LastModified = "2015/10/01")]
        public string SiteKey
        {
            get
            {
                return this["SiteKey"];
            }
        }

        /// <summary>
        /// Gets word : Light
        /// </summary>
        [ResourceEntry("Light",
            Value = "Light",
            Description = "word : Light",
            LastModified = "2015/10/01")]
        public string Light
        {
            get
            {
                return this["Light"];
            }
        }

        /// <summary>
        /// Gets word : Dark
        /// </summary>
        [ResourceEntry("Dark",
            Value = "Dark",
            Description = "word : Dark",
            LastModified = "2015/10/01")]
        public string Dark
        {
            get
            {
                return this["Dark"];
            }
        }

        /// <summary>
        /// Gets word : Theme
        /// </summary>
        [ResourceEntry("Theme",
            Value = "Theme",
            Description = "word : Theme",
            LastModified = "2015/10/01")]
        public string Theme
        {
            get
            {
                return this["Theme"];
            }
        }

        /// <summary>
        /// Gets word : Image
        /// </summary>
        [ResourceEntry("Image",
            Value = "Image",
            Description = "word : Image",
            LastModified = "2015/10/01")]
        public string Image
        {
            get
            {
                return this["Image"];
            }
        }

        /// <summary>
        /// Gets word : Audio
        /// </summary>
        [ResourceEntry("Audio",
            Value = "Audio",
            Description = "word : Audio",
            LastModified = "2015/10/01")]
        public string Audio
        {
            get
            {
                return this["Audio"];
            }
        }

        /// <summary>
        /// Gets word : Type
        /// </summary>
        [ResourceEntry("Type",
            Value = "Type",
            Description = "word : Type",
            LastModified = "2015/10/01")]
        public string Type
        {
            get
            {
                return this["Type"];
            }
        }

        /// <summary>
        /// Gets word : Size
        /// </summary>
        [ResourceEntry("Size",
            Value = "Size",
            Description = "word : Size",
            LastModified = "2015/10/01")]
        public string Size
        {
            get
            {
                return this["Size"];
            }
        }

        /// <summary>
        /// Gets word : Normal
        /// </summary>
        [ResourceEntry("Normal",
            Value = "Normal",
            Description = "word : Normal",
            LastModified = "2015/10/01")]
        public string Normal
        {
            get
            {
                return this["Normal"];
            }
        }

        /// <summary>
        /// Gets word : Compact
        /// </summary>
        [ResourceEntry("Compact",
            Value = "Compact",
            Description = "word : Compact",
            LastModified = "2015/10/01")]
        public string Compact
        {
            get
            {
                return this["Compact"];
            }
        }

        /// <summary>
        /// Gets word : Preview.
        /// </summary>
        [ResourceEntry("Preview",
            Value = "Preview",
            Description = "word : Preview",
            LastModified = "2015/10/01")]
        public string Preview
        {
            get
            {
                return this["Preview"];
            }
        }
    }
}
