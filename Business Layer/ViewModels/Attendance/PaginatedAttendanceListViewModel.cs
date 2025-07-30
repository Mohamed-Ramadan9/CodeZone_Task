using System.Collections.Generic;

namespace Business_Layer.ViewModels.Attendance
{
    public class PaginatedAttendanceListViewModel
    {
        public IEnumerable<AttendanceViewModel> AttendanceRecords { get; set; } = new List<AttendanceViewModel>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; } = 0;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
} 