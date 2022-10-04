using AutoMapper;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.User;
using Microsoft.EntityFrameworkCore;


namespace BookingSoccers.Service.Service.UserInfo
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly IMapper mapper;

        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            this.userRepo = userRepo;
            this.mapper = mapper;
        }

        public async Task< GeneralResult<User> > AddANewUser(UserCreatePayload userinfo)
        {
            
           var userExistCheck = await userRepo.Get().Where(x =>
                x.UserName == userinfo.UserName ||
                x.Email == userinfo.Email ||
                x.PhoneNumber == userinfo.PhoneNumber).FirstOrDefaultAsync();

            if (userExistCheck != null) return 
                    GeneralResult<User>.Error(403, "User already exists"); 

            var toCreateUser = mapper.Map<User>(userinfo);
            userRepo.Create(toCreateUser);
            await userRepo.SaveAsync();

            return GeneralResult<User>.Success(toCreateUser);
        }

        public async Task< GeneralResult<User> > GetByEmail(string email)
        {
            var User = await userRepo.GetById(email);

            if (User == null) return GeneralResult<User>.Error(
                204, "User not found with Email:" + email); 

            return GeneralResult<User>.Success(User);
        }

        public async Task< GeneralResult<User> > RemoveAUser(int UserId)
        {
            var returnedUser = await userRepo.GetById(UserId);

            if (returnedUser == null) return GeneralResult<User>.Error(
                204, "User not found with Id:" + UserId);

            userRepo.Delete(returnedUser);
            await userRepo.SaveAsync();

            return GeneralResult<User>.Success(returnedUser);
        }

        public async Task< GeneralResult< List<User> > > RetrieveAllUsers()
        {
            var returnedUserList = await userRepo.Get().ToListAsync();

            if (returnedUserList == null) return GeneralResult< List<User>>.Error(
                204, "No users found ");

            return GeneralResult<List<User>>.Success(returnedUserList);
        }

        public async Task<GeneralResult<User>> RetrieveAUserById(int UserId)
        {
            var userById = await userRepo.GetById(UserId);

            if (userById == null) return GeneralResult<User>.Error(
                204, "User not found with Id:" + UserId); 

            return GeneralResult<User>.Success(userById);
        }

        public async Task<GeneralResult<User>> UpdateAUser(int Id, UserUpdatePayload newUserInfo)
        {
            var toUpdateUser = await userRepo.GetById(Id);

            if (toUpdateUser == null) return GeneralResult<User>.Error(
                204, "User not found with Id:" + Id);

            mapper.Map(newUserInfo, toUpdateUser);

            userRepo.Update(toUpdateUser);
            await userRepo.SaveAsync();

            return GeneralResult<User>.Success(toUpdateUser);
        }
    }
}
