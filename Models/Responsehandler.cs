using System;
using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.Models
{
    public class Responsehandler
    {
        public static ApiResponse GetExceptionResponse(Exception ex)
        {

            return new ApiResponse
            {
                Success = false,
                Message = ex.Message,
            };

        }

        public static ApiResponse GetResponse(bool status,string message,object data = null)
        {
            return new ApiResponse { 
                Success = status,
                Message = message,
                Results = data
            };

        }


    }
}
