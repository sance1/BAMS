using BAMS.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Z.EntityFramework.Plus;

namespace BAMS.Data.Repositories
{
    public class PageTextRepository : ExtendedRepository<PageText>
    {
        public PageTextRepository(
            DbContext dbContext): base(dbContext) 
        {

        }

        public string GetPortalText(string key, string language, string defaultLanguage)
        {
            var list = dbSet.FromCache().ToList();
            var text =
                list.Where(t => t.Key == key)
                    .OrderBy(t => t.LanguageCode != language)
                    .ThenBy(t => t.LanguageCode != defaultLanguage)
                    .FirstOrDefault();

            return text != null ? text.Text : "";

        }

    }
}
