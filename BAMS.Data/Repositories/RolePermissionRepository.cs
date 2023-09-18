using Microsoft.EntityFrameworkCore;

namespace BAMS.Data.Repositories
{
    public class RolePermissionRepository : RepositoryBase<Models.RolePermission>
    {
        public RolePermissionRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}