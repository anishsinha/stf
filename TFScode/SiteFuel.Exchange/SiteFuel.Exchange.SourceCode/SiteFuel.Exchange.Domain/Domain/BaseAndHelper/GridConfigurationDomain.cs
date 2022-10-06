using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.DataAccess.Entities;
using System.Data.Entity;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace SiteFuel.Exchange.Domain
{
    public class GridConfigurationDomain : BaseDomain
    {
        public GridConfigurationDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public GridConfigurationDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> SaveUserGridConfiguration(UserGridConfigurationViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var jsonMessage = new JavaScriptSerializer().Serialize(viewModel.Setting);
                        UserGridConfiguration userGrid; 
                        userGrid = await Context.DataContext.UserGridConfigurations.FirstOrDefaultAsync(t => t.UserId == viewModel.UserId 
                                    && t.GridId == (int)viewModel.GridId);
                        if (userGrid == null)
                        {
                            userGrid = new UserGridConfiguration();
                            userGrid.GridId = (int)viewModel.GridId;
                            userGrid.UserId = viewModel.UserId;
                            userGrid.Setting = jsonMessage;
                            userGrid.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.UserGridConfigurations.Add(userGrid);
                        }
                        else
                        {
                            userGrid.Setting = jsonMessage;
                            userGrid.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(userGrid).State = EntityState.Modified;
                        }

                        await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("GridConfigurationDomain", "SaveUserGridConfiguration", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("GridConfigurationDomain", "SaveUserGridConfiguration", ex.Message, ex);
            }

            return response;
        }

        public async Task<UserGridConfigurationViewModel> GetUserGridConfigurationAsync(int userId, GridName gridId)
        {
            UserGridConfigurationViewModel response = new UserGridConfigurationViewModel();

            try
            {
                var userGrid = await Context.DataContext.UserGridConfigurations.Where(t => t.UserId == userId && t.GridId == (int)gridId)
                                                                               .OrderByDescending(t => t.Id)
                                                                               .Select(t => new { t.UserId, t.GridId, t.Setting })
                                                                               .FirstOrDefaultAsync();
                if (userGrid != null)
                {
                    response.GridId = gridId;
                    response.UserId = userId;
                    response.Setting = JsonConvert.DeserializeObject<List<GridSetting>>(userGrid.Setting);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("GridConfigurationDomain", "GetUserGridConfigurationAsync", ex.Message, ex);
            }

            return response;
        }
    }
}
