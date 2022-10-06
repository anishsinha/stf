using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IDSBLoadQueueDomain
    {
        Task<StatusModel> CreateDsbLoadQueue(List<DSBLoadQueueModel> dSBLoadQueue);
        Task<StatusModel> DeleteDsbLoadQueue(List<string> dSBLoadQueue);
    }
}
