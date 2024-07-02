namespace Finance.Services.Pagination;

public interface IPaginationStrategy<T>
{
  public Task<List<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize);
}
