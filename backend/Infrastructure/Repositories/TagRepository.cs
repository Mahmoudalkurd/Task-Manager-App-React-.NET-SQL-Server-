using backend.Domain.Entities;
using backend.Infrastructure.Data;

namespace backend.Infrastructure.Repositories;

public interface ITagRepository : IRepository<Tag>
{
}

public class TagRepository : Repository<Tag>, ITagRepository
{
    public TagRepository(AppDbContext ctx) : base(ctx)
    {
    }
}
