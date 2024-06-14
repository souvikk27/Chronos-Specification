using Microsoft.EntityFrameworkCore;

namespace Chronos.Specification
{
    public interface IRepositoryOptions<out TContext> where TContext : DbContext
    {
        TContext Context { get; }
    }
}
