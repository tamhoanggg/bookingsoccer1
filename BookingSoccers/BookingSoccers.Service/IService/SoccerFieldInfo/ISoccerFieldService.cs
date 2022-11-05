using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.DTO.PriceMenu;
using BookingSoccers.Service.Models.DTO.SoccerField;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Booking;
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
        Task<GeneralResult<SoccerField>> AddANewSoccerField
            (SoccerFieldCreatePayload SoccerFieldinfo);

        Task<GeneralResult<AddedBookingView>> AddANewBooking
            (BookingCreateForm bookingInfo);

        Task<GeneralResult<PreBookingInfo>> CheckZonesAndCalculatePrice
            (BookingValidateForm createFormInfo); 

        Task<GeneralResult<Object>> GetAFieldDetails(int FieldId);

        Task<GeneralResult<List<SoccerFieldView3>>> GetFieldsForManagerByManagerId
            (int ManagerId);

        Task<GeneralResult<SoccerFieldView2>> GetFieldScheduleOfADateById
            (int FieldId, DateTime date);

        Task<GeneralResult<List<PriceMenuView>>> GetPriceMenusForManagerByFieldId
            (int FieldId);

        Task<GeneralResult<List<FieldRegularCustomerView>>> GetRegularCustomerList
            (int FieldId);

        Task<GeneralResult<SoccerFieldSalesReport>> GetSalesReportForField(int FieldId);

        Task<GeneralResult<SoccerFieldView1>> GetFieldForUserByFieldID(int FieldId);

        Task<GeneralResult<ObjectListPagingInfo>> RetrieveSoccerFieldsListForAdmin
            (PagingPayload pagingPayload, SoccerFieldPredicate filter);

        Task<GeneralResult<ObjectListPagingInfo>> RetrieveSoccerFieldsListForUser
            (PagingPayload pagingPayload, SoccerFieldPredicate filter);

        Task<GeneralResult<SoccerField>> UpdateASoccerField
            (int Id, SoccerFieldUpdatePayload newSoccerFieldInfo);

        Task<GeneralResult<SoccerField>> RemoveASoccerField(int SoccerFieldId);
    }
}
