using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.SoccerField;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.SoccerFieldInfo
{
    public interface ISoccerFieldService 
    {
        Task<GeneralResult<SoccerField>> AddANewSoccerField(SoccerFieldCreatePayload SoccerFieldinfo);

        Task<GeneralResult<SoccerField>> RetrieveASoccerFieldById(int SoccerFieldId);

        Task<GeneralResult<List<SoccerField>>> RetrieveAllSoccerFields();

        Task<GeneralResult<SoccerField>> UpdateASoccerField(int Id, SoccerFieldUpdatePayload newSoccerFieldInfo);

        Task<GeneralResult<SoccerField>> RemoveASoccerField(int SoccerFieldId);
    }
}
