using AutoMapper;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.User;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace BookingSoccers.Service.Service.UserInfo
{
    public class UserService : IUserService
    {
        private readonly ISoccerFieldRepo soccerFieldRepo;
        private readonly IBookingRepo bookingRepo;
        private readonly IUserRepo userRepo;
        private readonly IMapper mapper;

        public UserService(IUserRepo userRepo, IMapper mapper,
            IBookingRepo bookingRepo, ISoccerFieldRepo soccerFieldRepo)
        {
            this.userRepo = userRepo;
            this.mapper = mapper;
            this.bookingRepo = bookingRepo;
            this.soccerFieldRepo = soccerFieldRepo;
        }

        public async Task<GeneralResult<User>> AddANewUser(UserCreatePayload userinfo)
        {
            //Check duplicate user name, email or phone number
            var userExistCheck = await userRepo.Get().Where(x =>
                 x.UserName == userinfo.UserName ||
                 x.Email == userinfo.Email ||
                 x.PhoneNumber == userinfo.PhoneNumber).FirstOrDefaultAsync();

            if (userExistCheck != null) return
                    GeneralResult<User>.Error(409, "User already exists");

            //Mapping new user info to new instance of user
            var toCreateUser = mapper.Map<User>(userinfo);

            //and create it
            userRepo.Create(toCreateUser);
            await userRepo.SaveAsync();

            return GeneralResult<User>.Success(toCreateUser);
        }

        public async Task<GeneralResult<User>> GetByEmail(string email)
        {
            //Get a user details by email
            var User = await userRepo.GetById(email);

            if (User == null) return GeneralResult<User>.Error(
                404, "User not found with Email:" + email);

            return GeneralResult<User>.Success(User);
        }

        public async Task<GeneralResult<User>> RemoveAUser(int UserId)
        {
            //Get the requested user by user id and delete it
            var returnedUser = await userRepo.GetById(UserId);

            if (returnedUser == null) return GeneralResult<User>.Error(
                404, "User not found with Id:" + UserId);

            userRepo.Delete(returnedUser);
            await userRepo.SaveAsync();

            return GeneralResult<User>.Success(returnedUser);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrieveUsersList
            (PagingPayload pagingPayload, UserPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<User>(true);

            //list of navi props to include in query
            string? includeList = "role,";

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.RoleId != null)
            {
                newPred = newPred.And(x => x.RoleId == filter.RoleId);
            }

            if (filter.GenderNum != null)
            {
                newPred = newPred.And(x => x.Gender == (GenderEnum)filter.GenderNum);
            }

            //Create a new expression instance
            Expression<Func<User, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if ( !(newPred.Body.ToString().StartsWith("True") &&
                newPred.Body.ToString().EndsWith("True")) )
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedUserList = await userRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList, pred);

            if (returnedUserList.Count() == 0) return 
                    GeneralResult<ObjectListPagingInfo>
                    .Error(404, "No users found ");

            //Get total elements when running the query
            var TotalElement = await userRepo.GetPagingTotalElement(pred);

            //var UserList = mapper.Map<List<UserListInfo>>(returnedUserList);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();
            FinalResult.ObjectList = returnedUserList;

            FinalResult.TotalElement = TotalElement;
            FinalResult.CurrentPage = pagingPayload.PageNum;

            //Calculate total pages based on total element
            var CheckRemain = TotalElement % 20;
            var SumPage = TotalElement / 20;
            if (CheckRemain > 0) FinalResult.TotalPage = SumPage + 1;
            else FinalResult.TotalPage = SumPage;

            return GeneralResult<ObjectListPagingInfo>.Success(FinalResult);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrieveAUserDetails
            (int PageNum, int UserId)
        {
            if(PageNum < 1) return GeneralResult<ObjectListPagingInfo>.Error(
                404, "PageNum must be equal or greater than 1");

            //Get a user details by user Id
            var userDetails = await userRepo.GetById(UserId);

            if (userDetails == null) return GeneralResult<ObjectListPagingInfo>.Error(
                404, "User not found with Id:" + UserId);

            Object listResult = new Object();

            if (userDetails.RoleId == 1) return 
                    GeneralResult<ObjectListPagingInfo>.Error(
                400, "Cannot view other Admin user details");

            //If user is a field manager select properties to show for field manager 
            if (userDetails.RoleId == 2) 
            {
                //Get bookings and payments of a user
                var ReturnedFields = await soccerFieldRepo
                    .GetPaginationFieldList(PageNum, UserId);

                listResult = new 
                { 
                    userDetails.Id, userDetails.UserName,
                    userDetails.FirstName, userDetails.LastName,
                    Gender = userDetails.Gender.ToString(), userDetails.PhoneNumber, 
                    userDetails.Email, FieldList = ReturnedFields
                };
            }

            //If user role is user then select properties to show for user
            if (userDetails.RoleId == 3)
            {
                //Get bookings and payments of a user
                var ReturnedBookings = await bookingRepo
                    .GetBookingsPaginationByUserId(PageNum, UserId);

                    listResult = new
                {
                    userDetails.Id, userDetails.UserName,
                    userDetails.FirstName, userDetails.LastName,
                    Gender = userDetails.Gender.ToString(), userDetails.PhoneNumber,
                    userDetails.Email, BookingList = ReturnedBookings
                };
            }

            var predicate = PredicateBuilder.New<User>(true);

            predicate = predicate.And(x => x.Id == UserId);

            Expression<Func<User, bool>>? pred = predicate;

            //Get total elements when running the query
            var TotalElement = await userRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            FinalResult.ObjectList = listResult;

            FinalResult.TotalElement = TotalElement;
            FinalResult.CurrentPage = PageNum;

            //Calculate total pages based on total element 
            var CheckRemain = TotalElement % 20;
            var SumPage = TotalElement / 20;
            if (CheckRemain > 0) FinalResult.TotalPage = SumPage + 1;
            else FinalResult.TotalPage = SumPage;

            return GeneralResult<ObjectListPagingInfo>.Success(FinalResult);
        }

        public async Task<GeneralResult<BasicUserInfo>> RetrieveAUserForUpdate(string UserName)
        {
            //Get the requested user details for update by User name
            var toUpdateUser = await userRepo.GetByUserName(UserName);

            if (toUpdateUser == null) return GeneralResult<BasicUserInfo>.Error(
                404, "User not found with username:" + UserName);

            var returnedUser = new BasicUserInfo();

            //Mapping returned user to DTO
            mapper.Map(toUpdateUser, returnedUser);

            return GeneralResult<BasicUserInfo>.Success(returnedUser);
        }

        public async Task<GeneralResult<User>> UpdateAUser(int Id, UserUpdatePayload newUserInfo)
        {
            //Get a specific user details for update using Id
            var toUpdateUser = await userRepo.GetById(Id);

            if (toUpdateUser == null) return GeneralResult<User>.Error(
                404, "User not found with Id:" + Id);

            //Mapping new user info to returned user info
            mapper.Map(newUserInfo, toUpdateUser);

            //and update it
            userRepo.Update(toUpdateUser);
            await userRepo.SaveAsync();

            return GeneralResult<User>.Success(toUpdateUser);
        }

        public async Task<GeneralResult<BasicUserInfo>> UpdateUserInfoForUser(int id, UserUpdatePayload newUserInfo)
        {
            //Get requested user details for update
            var toUpdateUser = await userRepo.GetById(id);

            if (toUpdateUser == null) return GeneralResult<BasicUserInfo>.Error(
                404, "User not found with Id:" + id);

            //Mapping new user info to returned user
            mapper.Map(newUserInfo, toUpdateUser);

            //and update
            userRepo.Update(toUpdateUser);
            await userRepo.SaveAsync();

            //Mapping updated user to View DTO
            var UpdatedUser = mapper.Map<BasicUserInfo>(newUserInfo);

            return GeneralResult<BasicUserInfo>.Success(UpdatedUser);
        }

        public async Task<GeneralResult<User>> GetAUserByUserName(string UserName)
        {
            var returnedUser = await userRepo.GetByUserName(UserName);

            if (returnedUser == null) return GeneralResult<User>
                    .Error(404, "No user associated with UserName:" + UserName);

            return GeneralResult<User>.Success(returnedUser);
        }
    }
}
