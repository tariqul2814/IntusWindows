using IntusWindowsInterview.Model.Data;
using IntusWindowsInterview.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace IntusWindowsInterview.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository<Order> Orders { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Orders = new GenericRepository<Order>(_context);
        }

        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        // Garbage Collector
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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
