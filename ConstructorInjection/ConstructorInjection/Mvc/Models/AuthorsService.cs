using ConstructorInjection.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace ConstructorInjection.Mvc.Models
{
    public class AuthorsService : IAuthorsService
    {
        public AuthorViewModel GetAuthor(DynamicContent item)
        {
            AuthorViewModel author = new AuthorViewModel();
            author.Name = item.GetString("Name");
            author.JobTitle = item.GetString("JobTitle");
            author.Bio = item.GetString("Bio");

            return author;
        }

        public IList<AuthorViewModel> GetAuthors()
        {
            IList<DynamicContent> authors = new List<DynamicContent>();
            authors = PopulateAuthors();

            return authors.Select(author => new AuthorViewModel()
                {
                    Name = author.GetString("Name"),
                    JobTitle = author.GetString("JobTitle"),
                    Bio = author.GetString("Bio")
                })

                .ToList();
        }

        private IList<DynamicContent> PopulateAuthors()
        {
            var manager = DynamicModuleManager.GetManager();
            var authorType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.Authors.Author");
            var authors = manager.GetDataItems(authorType).Where(a => a.Status == ContentLifecycleStatus.Live);

            return authors.ToList();
        }
    }
}
