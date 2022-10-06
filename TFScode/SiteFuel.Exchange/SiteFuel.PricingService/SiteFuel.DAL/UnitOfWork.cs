using SiteFuel.DataAccess.Entities;
using System;

namespace SiteFuel.DAL
{
	public class PricingUnitOfWork : IDisposable
	{
		private readonly DataContext context = new DataContext();
		private readonly IPricingRepository _priceRepository;

		public PricingUnitOfWork(IPricingRepository priceRepository)
		{
			_priceRepository = priceRepository;
		}

		public void Save()
		{
			context.SaveChanges();
		}
		private bool disposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
			}
			this.disposed = true;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
