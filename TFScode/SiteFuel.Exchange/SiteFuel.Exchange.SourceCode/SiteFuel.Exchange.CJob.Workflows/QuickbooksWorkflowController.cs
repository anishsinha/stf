using SiteFuel.Exchange.CJob.Workflows.WorkflowReducers;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows
{
    public class QuickbooksWorkflowController
    {
        private readonly IEnumerable<IQuickbooksWorkflowReducer> reducers;
        public QuickbooksWorkflowController()
        {
            reducers = (from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.GetInterfaces().Contains(typeof(IQuickbooksWorkflowReducer))
                                 && t.GetConstructor(Type.EmptyTypes) != null
                        select Activator.CreateInstance(t) as IQuickbooksWorkflowReducer).ToList();
        }

        public void StartQuickbooksWorkflow()
        {
            var watch = Stopwatch.StartNew();
            var qbDomain = new QbDomain();
            var workflows = qbDomain.GetNewWorkflowsAndInitialize();
            foreach (var item in workflows)
            {
                try
                {
                    var reducer = reducers.FirstOrDefault(x => x.WorkflowType == item.Type);
                    reducer = Activator.CreateInstance(reducer.GetType()) as IQuickbooksWorkflowReducer;

                    var qbRqs = reducer.Reduce(item, qbDomain);
                    foreach (var rq in qbRqs)
                    {
                        rq.WorkflowId = item.Id;
                    }

                    qbDomain.CreateQbRequestsAndMappings(item, qbRqs);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("QuickbooksWorkflowController", "StartQuickbooksWorkflow",
                        string.Format("Exception : {0} , Workflow ID : {1} , Item Type : {2}", ex.Message, item.Id, item.Type), ex);
                }
            }
            watch.Stop();
            LogManager.Logger.WriteInfo("CJob.Workflows", "StartQuickbooksWorkflow", "End:TotalTime:" + watch.ElapsedMilliseconds);
        }
    }
}
