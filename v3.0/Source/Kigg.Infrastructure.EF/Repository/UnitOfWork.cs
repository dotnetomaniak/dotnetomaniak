using System;

namespace Kigg.Infrastructure.EF.Repository
{
    public class UnitOfWork: DisposableResource, IUnitOfWork
    {
        private readonly DotnetomaniakContext _context;

        public UnitOfWork(DotnetomaniakContext context)
        {
            _context = context;
        }

        private bool _isDisposed;

        public virtual void Commit()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            _context.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}