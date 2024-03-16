using Rainfall.Model.Rainfall;
using Rainfall.Model.Rainfall.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Service.Interface.Rainfall
{
    public interface IRainfallService
    {
        Task<rainfallReadingResponse> GetReadings(GetReadingsParam param);
    }
}
