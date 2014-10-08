using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DevMagazine.Core.Mvc.Helpers
{
    /// <summary>
    /// Provisions Html Helper extensions for working with Mvc Razor views
    /// </summary>
    public static class GenericHtmlHelperExtensions
    {
        #region Public methods

        /// <summary>
        /// Renders text if a condition is true
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="text">The text, which is going to be rendered if a given condition is met</param>
        /// <param name="condition">The condition being evaluated</param>
        /// <returns>The text if the condition is tru and Empty string if false</returns>
        public static string RenderTextIfTrue(this HtmlHelper helper, string text, bool condition = false)
        {
            // checks if the text is empty or null
            if (String.IsNullOrEmpty(text) || !condition)
                return String.Empty;

            // returns the text if the condition is true
            return text;
        }



        /// <summary>
        /// Evaluates a condition and renders various text basd on it
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="textIfTrue">The text, which is going to be rendered if the condition is met</param>
        /// <param name="textIfFalse">The text, which is going to be rendered if the condition is not met</param>
        /// <param name="condition">The condition being evaluated</param>
        /// <returns>One text if the condition is true and nother if condition is false</returns>
        public static string RenderTextOrText(this HtmlHelper helper, string textIfTrue, string textIfFalse, bool condition = false)
        {
            string result = (condition) ? textIfTrue : textIfFalse;

            return result;
        }

        #endregion
    }
}
