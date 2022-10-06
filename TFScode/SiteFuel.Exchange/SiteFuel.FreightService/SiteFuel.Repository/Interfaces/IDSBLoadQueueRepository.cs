using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IDSBLoadQueueRepository
    {
        Task<StatusModel> CreateDsbLoadQueue(List<DSBLoadQueueModel> dSBLoadQueue);
        Task<StatusModel> DeleteDsbLoadQueue(List<string> dSBLoadQueue);
    }
}
