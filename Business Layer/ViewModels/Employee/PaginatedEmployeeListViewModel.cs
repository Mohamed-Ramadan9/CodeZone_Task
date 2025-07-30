using System.Collections.Generic;

namespace Business_Layer.ViewModels.Employee
{
    public class PaginatedEmployeeListViewModel
    {
        public IEnumerable<EmployeeListViewModel> Employees { get; set; } = new List<EmployeeListViewModel>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int StartPage => Math.Max(1, CurrentPage - 2);
        public int EndPage => Math.Min(TotalPages, CurrentPage + 2);
    }
} 