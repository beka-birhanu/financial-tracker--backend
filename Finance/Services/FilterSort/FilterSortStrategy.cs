using Microsoft.EntityFrameworkCore;

namespace Finance.Services.FilterSort;

public class FilterSortStrategy<T> : IFilterSortStrategy<T>
{
  public IQueryable<T> ApplyFilter(IQueryable<T> query, string filterValue, string filterField)
  {
    if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
    {
      try
      {
        filterValue = filterValue.ToLower();
        query = query.Where(e => e != null && EF.Property<string>(e, filterField).ToLower().Contains(filterValue));
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error applying filter: {ex.Message}");
      }
    }

    return query;
  }

  public IQueryable<T> ApplyFilter(IQueryable<T> query, int filterValue, string filterField)
  {
    if (!string.IsNullOrEmpty(filterField))
    {
      try
      {
        var propertyType = typeof(T).GetProperty(filterField)?.PropertyType;

        if (propertyType == typeof(DateTime))
        {
          query = query.Where(e => e != null && EF.Property<DateTime>(e, filterField).Year == filterValue);
        }
        else
        {
          query = query.Where(e => e != null && EF.Property<int>(e, filterField) == filterValue);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error applying filter: {ex.Message}");
      }
    }

    return query;
  }

  public IQueryable<T> ApplySort(IQueryable<T> query, string sortOrder, string sortField)
  {
    if (!string.IsNullOrEmpty(sortField))
    {
      try
      {
        if (sortOrder == "desc")
        {
          query = query.OrderByDescending(e => EF.Property<object>(e!, sortField));
        }
        else
        {
          query = query.OrderBy(e => EF.Property<object>(e!, sortField));
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error applying sort: {ex.Message}");
      }
    }

    return query;
  }
}

