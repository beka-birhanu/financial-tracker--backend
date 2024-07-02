namespace Finance.Services.FilterSort;

public interface IFilterSortStrategy<T>
{
  public IQueryable<T> ApplyFilter(IQueryable<T> query, string filterValue, string filterFeild);
  public IQueryable<T> ApplyFilter(IQueryable<T> query, int filterValue, string filterFeild);
  public IQueryable<T> ApplySort(IQueryable<T> query, string SortOrder, string sortFeild);
}
