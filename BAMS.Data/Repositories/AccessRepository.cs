using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAMS.Data.Repositories
{
    public class AccessRepository : RepositoryBase<Models.AccessPermission>
    {
        public AccessRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<MenuModel>> GetMenuAccessPermission()
        {
            var data = await GetAll().Where(a => !string.IsNullOrEmpty(a.MenuUrl)).GroupBy(a => new {a.Group, a.MenuUrl,a.MenuOrder})
                .Select(a => new MenuModel()
                {
                    Name = a.Key.Group + "_sidebar",
                    Id = a.Key.MenuUrl.ToLower().Replace("/",""),
                    Url = a.Key.MenuUrl,
                    MenuOrder = a.Key.MenuOrder
                })
                .OrderBy(a => a.MenuOrder)
                .ToListAsync();

            return data;
        }
    }

    public class MenuModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public int MenuOrder { get; set; }
    }
}