using Microsoft.EntityFrameworkCore;

namespace Finance.Services.Pagination;

public class PaginationStrategy<T> : IPaginationStrategy<T>
{
  public async Task<List<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
  {
    return await query.Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();
  }
}

