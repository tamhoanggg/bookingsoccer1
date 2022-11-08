using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.SoccerFieldInfo
{
    public interface ISoccerFieldRepo : IBaseRepository<SoccerField>
    {
        Task<SoccerField> GetSoccerFieldByFieldId(int Id);

        Task<SoccerField> GetFieldBookingScheduleOfADateByFieldId(int FieldId, DateTime date);

        Task<List<SoccerField>> GetFieldsForManagerByManagerId(int ManagerId);

        Task<SoccerField> GetFieldByFieldName(string FieldName);

        Task<SoccerField> GetFieldDetails(int FieldId);

        Task<List<SoccerField>> GetPaginationFieldList(int PageNum, int ManagerId);

    }
}
