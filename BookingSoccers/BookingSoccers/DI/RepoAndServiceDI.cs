using BookingSoccers.Repo.IRepository;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Repo.Repository;
using BookingSoccers.Repo.Repository.UserInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Service.UserInfo;
using BookingSoccers.Service.UserInfo;

namespace BookingSoccers.DI
{
    public static class RepoAndServiceDI
    {
        public static void ConfigServiceDI(this IServiceCollection services) 
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<IRoleService, RoleService>();
        }
    }
}
