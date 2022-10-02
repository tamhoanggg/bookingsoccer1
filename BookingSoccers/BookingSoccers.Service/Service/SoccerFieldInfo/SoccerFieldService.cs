using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.SoccerFieldInfo
{
    public class SoccerFieldService : ISoccerFieldService
    {
        private readonly ISoccerFieldRepo soccerFieldRepo;

        public SoccerFieldService(ISoccerFieldRepo soccerFieldRepo)
        {
            this.soccerFieldRepo = soccerFieldRepo;
        }

        public async Task<SoccerField> AddANewSoccerField(SoccerField SoccerFieldinfo)
        {
            if (await soccerFieldRepo.GetById(SoccerFieldinfo.FieldName) != null ||
               await soccerFieldRepo.GetById(SoccerFieldinfo.Address) != null)
                return null;
            soccerFieldRepo.Create(SoccerFieldinfo);
            await soccerFieldRepo.SaveAsync();
            return await soccerFieldRepo.GetById(SoccerFieldinfo.FieldName);
        }

        public async Task<SoccerField> RemoveASoccerField(int SoccerFieldId)
        {
            var toDeleteSoccerField = await RetrieveASoccerFieldById(SoccerFieldId);
            if (toDeleteSoccerField == null) return null;

            soccerFieldRepo.Delete(toDeleteSoccerField);
            await soccerFieldRepo.SaveAsync();
            return toDeleteSoccerField;
        }

        public async Task<List<SoccerField>> RetrieveAllSoccerFields()
        {
            var soccerFieldList = await soccerFieldRepo.Get().ToListAsync();
            if (soccerFieldList == null) return null;
            return soccerFieldList;
        }

        public async Task<SoccerField> RetrieveASoccerFieldById(int SoccerFieldId)
        {
            var retrievedSoccerField = await soccerFieldRepo.GetById(SoccerFieldId);
            if (retrievedSoccerField == null) return null;
            return retrievedSoccerField;
        }

        public async Task<SoccerField> UpdateASoccerField(int Id, SoccerField newSoccerFieldInfo)
        {
            var toUpdateSoccerField = await RetrieveASoccerFieldById(Id);
            if (toUpdateSoccerField == null) return null;

            toUpdateSoccerField.CloseHour = newSoccerFieldInfo.CloseHour;
            toUpdateSoccerField.Address = newSoccerFieldInfo.Address;
            toUpdateSoccerField.OpenHour = newSoccerFieldInfo.OpenHour;
            toUpdateSoccerField.Status = newSoccerFieldInfo.Status;
            toUpdateSoccerField.BaseTimeInterval = newSoccerFieldInfo.BaseTimeInterval;
            toUpdateSoccerField.Description = newSoccerFieldInfo.Description;
            toUpdateSoccerField.FieldName = newSoccerFieldInfo.FieldName;
            toUpdateSoccerField.ManagerId = newSoccerFieldInfo.ManagerId;
            toUpdateSoccerField.ReviewScoreSum = newSoccerFieldInfo.ReviewScoreSum;
            toUpdateSoccerField.TotalReviews = newSoccerFieldInfo.TotalReviews;

            soccerFieldRepo.Update(toUpdateSoccerField);
            await soccerFieldRepo.SaveAsync();

            return toUpdateSoccerField;
        }
    }
}
