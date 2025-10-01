using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Services.Pedimentos
{
    public class ErrorService
    {
        public ApiError BuildErrorResponse(Exception ex)
        {
            return ex switch
            {
                IOException ioEx => new ApiError
                {
                    ErrorType = "IOError",
                    Message = $"Error de E/S: {ioEx.Message}",
                    CustomCode = 1001
                },
                UnauthorizedAccessException uaEx => new ApiError
                {
                    ErrorType = "AccessDenied",
                    Message = $"Permiso denegado: {uaEx.Message}",
                    CustomCode = 1002
                },
                _ => new ApiError
                {
                    ErrorType = "GeneralError",
                    Message = $"Error inesperado: {ex.Message}",
                    CustomCode = 1000
                }
            };
        }
    }

}
