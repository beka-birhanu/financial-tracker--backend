namespace Finance.Services.Pagination;

public interface IPaginationStrategy<T>
{
  public Task<PaginationResult<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize);
}
