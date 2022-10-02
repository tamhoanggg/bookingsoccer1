using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.SoccerFieldInfo
{
    public interface ISoccerFieldService 
    {
        Task<SoccerField> AddANewSoccerField(SoccerField SoccerFieldinfo);

        Task<SoccerField> RetrieveASoccerFieldById(int SoccerFieldId);

        Task<List<SoccerField>> RetrieveAllSoccerFields();

        Task<SoccerField> UpdateASoccerField(int Id, SoccerField newSoccerFieldInfo);

        Task<SoccerField> RemoveASoccerField(int SoccerFieldId);
    }
}
