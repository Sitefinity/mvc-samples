using ConstructorInjection.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity.DynamicModules.Model;

namespace ConstructorInjection.Mvc.Models
{
    public interface IAuthorsService
    {
        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <returns></returns>
        IList<AuthorViewModel> GetAuthors();

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        AuthorViewModel GetAuthor(DynamicContent item);
    }
}
