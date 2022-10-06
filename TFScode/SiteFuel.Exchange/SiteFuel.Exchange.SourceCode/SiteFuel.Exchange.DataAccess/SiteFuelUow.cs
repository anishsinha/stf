using SiteFuel.Exchange.DataAccess.Entities;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess
{
	public class SiteFuelUow : IDisposable
	{
		private SiteFuelDataContext Context { get; set; }

		public SiteFuelUow(string connectionString)
		{
			Context = new SiteFuelDataContext(connectionString);
		}

		public SiteFuelDataContext DataContext
		{
			get
			{
				return Context;
			}
		}

		public void Commit()
		{
			try
			{
				Context.SaveChanges();
			}
			catch (DbEntityValidationException ex)
			{
				var str = ex.EntityValidationErrors
			.Select(dbEntityValidationResult => dbEntityValidationResult.DbValidationErrorsToString(dbEntityValidationResult.ValidationErrors))
			.Aggregate(string.Empty, (current, next) => string.Format("{0}{1}{2}", current, Environment.NewLine, next));
				Logger.LogManager.Logger.WriteException("SiteFuelUow", "Commit", str, ex);
				throw;
			}
		}



		public async Task CommitAsync()
		{
			try
			{
				await Context.SaveChangesAsync();
			}
			catch (DbEntityValidationException ex)
			{
				var str = ex.EntityValidationErrors
			.Select(dbEntityValidationResult => dbEntityValidationResult.DbValidationErrorsToString(dbEntityValidationResult.ValidationErrors))
			.Aggregate(string.Empty, (current, next) => string.Format("{0}{1}{2}", current, Environment.NewLine, next));
				Logger.LogManager.Logger.WriteException("SiteFuelUow", "Commit", str, ex);
				throw;
			}
		}

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Context != null)
				{
					Context.Dispose();
				}
			}
		}

		#endregion
	}
}
