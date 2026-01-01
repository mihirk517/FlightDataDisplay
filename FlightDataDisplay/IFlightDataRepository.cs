using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenericsBasics.Domain
{
    public interface IFlightDataRepository
    {
        Task<BaggageInfo> GetAllAsync();
    }

}