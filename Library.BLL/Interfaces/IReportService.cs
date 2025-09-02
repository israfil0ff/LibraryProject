using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DBO.Reports;

public interface IReportService
{
    // Ən çox icarəyə verilən kitabların siyahısı
    IEnumerable<BookReportDto> GetMostRentedBooks(int top);

    // Ən çox kitab götürən istifadəçilər
    IEnumerable<UserReportDto> GetTopUsers(int top);

    // Gecikmiş kitabların siyahısı
    IEnumerable<OverdueReportDto> GetOverdueBooks();

    // Aylıq icarə statistikası
    IEnumerable<MonthlyRentalStatDto> GetMonthlyRentalStats(int year);
}

