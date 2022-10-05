using BookingSoccers.Repo.IRepository;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Repo.Repository;
using BookingSoccers.Repo.Repository.BookingInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.UserInfo;
using BookingSoccers.Service.IService;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Service;
using BookingSoccers.Service.Service.BookingInfo;
using BookingSoccers.Service.Service.SoccerFieldInfo;
using BookingSoccers.Service.Service.UserInfo;
using BookingSoccers.Service.UserInfo;

namespace BookingSoccers.DI
{
    public static class RepoAndServiceDI
    {
        public static void ConfigServiceDI(this IServiceCollection services) 
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IAuthenService, AuthenService>();

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IBookingRepo, BookingRepo>();
            services.AddScoped<IBookingService, BookingService>();

            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<ISoccerFieldRepo, SoccerFieldRepo>();
            services.AddScoped<ISoccerFieldService, SoccerFieldService>();

            services.AddScoped<IImageFolderRepo, ImageFolderRepo>();
            services.AddScoped<IImageFolderService, ImageFolderService>();

            services.AddScoped<IPriceMenuRepo, PriceMenuRepo>();
            services.AddScoped<IPriceMenuService, PriceMenuService>();

            services.AddScoped<IPriceItemRepo, PriceItemRepo>();
            services.AddScoped<IPriceItemService, PriceItemService>();

            services.AddScoped<IZoneRepo, ZoneRepo>();
            services.AddScoped<IZoneService, ZoneService>();

            services.AddScoped<IZoneSlotRepo, ZoneSlotRepo>();
            services.AddScoped<IZoneSlotService, ZoneSlotService>();

            services.AddScoped<IZoneTypeRepo, ZoneTypeRepo>();
            services.AddScoped<IZoneTypeService, ZoneTypeService>();
        }
    }
}
