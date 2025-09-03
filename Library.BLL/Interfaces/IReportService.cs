using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DBO.Reports;

public interface IReportService
{
    // Ən çox icarəyə verilən kitabların siyahısı
    IEnumerable<BookReportDto> GetMostRentedBooks(int top, BookReportFilterDto filter);

    // Ən çox kitab götürən istifadəçilər
    IEnumerable<UserReportDto> GetTopUsers(int top, UserReportFilterDto filter);

    // Gecikmiş kitabların siyahısı
    IEnumerable<OverdueReportDto> GetOverdueBooks(OverdueReportFilterDto filter);

    // Aylıq icarə statistikası
    IEnumerable<MonthlyRentalStatDto> GetMonthlyRentalStats(MonthlyRentalFilterDto filter);
}

