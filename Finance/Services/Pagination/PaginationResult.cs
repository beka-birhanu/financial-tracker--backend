namespace Finance.Services.Pagination;

public class PaginationResult<T>
{
  public List<T> Data { get; set; }
  public int TotalCount { get; set; }
  public int PageNumber { get; set; }
  public int PageSize { get; set; }

  public PaginationResult(List<T> data, int totalCount, int pageNumber, int pageSize)
  {
    Data = data;
    TotalCount = totalCount;
    PageNumber = pageNumber;
    PageSize = pageSize;
  }
}
