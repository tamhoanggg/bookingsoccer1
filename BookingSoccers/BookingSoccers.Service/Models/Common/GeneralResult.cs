using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Common
{
    public class GeneralResult<TData>
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string Msg { get; set; }
        public TData Data { get; set; }
        public bool IsSuccess { get; set; }

        public static GeneralResult<TData> Success(TData data)
        {
            return new GeneralResult<TData> { Data = data, IsSuccess = true, StatusCode = 200 };
        }

        public static GeneralResult<TData> Error(string errorCode, string msg)
        {
            return new GeneralResult<TData> { ErrorCode = errorCode, Msg = msg, IsSuccess = false };
        }

        public static GeneralResult<TData> Error(int statusCode, string msg)
        {
            return new GeneralResult<TData>
            {
                StatusCode = statusCode,
                ErrorCode = statusCode.ToString(),
                Msg = msg,
                IsSuccess = false
            };
        }

        public static GeneralResult<TData> Error(int statusCode, string errorCode, string msg)
        {
            return new GeneralResult<TData>
            {
                StatusCode = statusCode,
                ErrorCode = errorCode,
                Msg = msg,
                IsSuccess = false
            };
        }
    }
}
