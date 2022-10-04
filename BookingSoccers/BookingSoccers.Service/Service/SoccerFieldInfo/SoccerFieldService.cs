using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.SoccerField;
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
        private readonly IMapper mapper;

        public SoccerFieldService(ISoccerFieldRepo soccerFieldRepo, IMapper mapper)
        {
            this.soccerFieldRepo = soccerFieldRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<SoccerField>> AddANewSoccerField(
            SoccerFieldCreatePayload SoccerFieldinfo)
        {
            var soccerFieldExistCheck = soccerFieldRepo.Get().Where(x =>
            x.FieldName == SoccerFieldinfo.FieldName ||
            x.Address == SoccerFieldinfo.Address);

            if (soccerFieldExistCheck != null) return
                GeneralResult<SoccerField>.Error(
                    403, "Soccer field name or address already exists");

            var toCreateSoccerField = mapper.Map<SoccerField>(SoccerFieldinfo);

            soccerFieldRepo.Create(toCreateSoccerField);
            await soccerFieldRepo.SaveAsync();

            return GeneralResult<SoccerField>.Success(toCreateSoccerField);
        }

        public async Task<GeneralResult<SoccerField>> RemoveASoccerField(int SoccerFieldId)
        {
            var toDeleteSoccerField = await soccerFieldRepo.GetById(SoccerFieldId);

            if (toDeleteSoccerField == null) return GeneralResult<SoccerField>.Error(
                204, "No soccer field found with Id:" + SoccerFieldId);

            soccerFieldRepo.Delete(toDeleteSoccerField);
            await soccerFieldRepo.SaveAsync();

            return GeneralResult<SoccerField>.Success(toDeleteSoccerField);
        }

        public async Task<GeneralResult<List<SoccerField>>> RetrieveAllSoccerFields()
        {
            var soccerFieldList = await soccerFieldRepo.Get().ToListAsync();

            if (soccerFieldList == null) return GeneralResult<List<SoccerField>>.Error(
                204, "No soccer fields found");

            return GeneralResult<List<SoccerField>>.Success(soccerFieldList); 
        }

        public async Task<GeneralResult<SoccerField>> RetrieveASoccerFieldById(int SoccerFieldId)
        {
            var retrievedSoccerField = await soccerFieldRepo.GetById(SoccerFieldId);

            if (retrievedSoccerField == null) return GeneralResult<SoccerField>.Error(
                204, "No soccer field found with Id:"+ SoccerFieldId);

            return GeneralResult<SoccerField>.Success(retrievedSoccerField);
        }

        public async Task<GeneralResult<SoccerField>> UpdateASoccerField(int Id,
            SoccerFieldUpdatePayload newSoccerFieldInfo)
        {
            var toUpdateSoccerField = await soccerFieldRepo.GetById(Id);

            if (toUpdateSoccerField == null) return GeneralResult<SoccerField>.Error(
                204, "No soccer field found with Id:" + Id);

            mapper.Map(newSoccerFieldInfo, toUpdateSoccerField);

            soccerFieldRepo.Update(toUpdateSoccerField);
            await soccerFieldRepo.SaveAsync();

            return GeneralResult<SoccerField>.Success(toUpdateSoccerField);
        }
    }
}
