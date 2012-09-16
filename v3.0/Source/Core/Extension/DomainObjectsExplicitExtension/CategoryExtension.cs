namespace Kigg.Infrastructure.DomainRepositoryExtensions
{
    using System.Security.Permissions;

    using Repository;
    using DomainObjects;

    //[StrongNameIdentityPermission(SecurityAction.Demand, PublicKey = "00240000048000009400000006020000002400005253413100040000010001007f9d35f7398744b708ea57288eb1911f9a46cad961be6baacb27e07d87809a20bf135f61833c121b541676fa95fd373d44ac4404ffae85e5199d0828c00991362b34f93002791f16d901f1714ba3abaa9208f8c41660f57ae0e7732e3655d5d4d9c53521cdb1b0636a78aac7407e194b7bee1a45b229e35559ee6c0a5b11b5b9")]
    public static class CategoryExtension
    {
        public static int GetStoryCount(this ICategory category)
        {
            return IoC.Resolve<IStoryRepository>().CountByCategory(category.Id);            
        }
    }
}
