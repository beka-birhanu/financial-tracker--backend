using Microsoft.EntityFrameworkCore;

namespace Finance.Services.Pagination;

public class PaginationStrategy<T> : IPaginationStrategy<T>
{
  public async Task<PaginationResult<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
  {
    int totalCount = await query.CountAsync();
    List<T> data = await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

    return new PaginationResult<T>(data, totalCount, pageNumber, pageSize);
  }
}

