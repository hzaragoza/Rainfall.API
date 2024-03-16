using Rainfall.Common.Model.Response;
using Rainfall.Model.Rainfall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainfall.Repository.Interface
{
    public interface IRainfallRepository
    {
        Task<httpResponse> GetReadings(GetReadingsParam param);
    }
}
