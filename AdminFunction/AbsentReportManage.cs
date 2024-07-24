using EntryManagement.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace EntryManagement.AdminFunction
{
    public class AbsentReportManage
    {
        // Khai báo database
        private readonly EntryLogManagementContext context;

        // Khởi tạo database
        public AbsentReportManage(EntryLogManagementContext _context)
        {
            context = _context;
        }

        // Hàm hiển thị tất cả báo cáo vắng học
        public void DisplayAllReports()
        {
            try
            {

                // truy vấn lấy dữu liệu
                var allReports = context.AbsentReports
                                        .Select(report =>
                                            new
                                            {
                                                StudentID = report.Student.StudentId,
                                                StudentName = report.Student.Name,
                                                ParentName = report.Parent.Name,
                                                StudentClass = report.Student.Class,
                                                ReportDate = report.CreateDay,
                                                ReportReason = report.Reason
                                            })
                                        .OrderByDescending(e => e.ReportDate)
                                        .ToList();

                // kiểm tra xem có dữ liệu trả về hay không
                if (allReports.Any())
                {

                    // Tạo bảng và thêm cột
                    var table = new Table().Expand();
                    table.Title("[#ffff00]Bảng báo cáo vắng học[/]");
                    table.AddColumn("ID");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Tên phụ huynh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Ngày báo cáo");
                    table.AddColumn("Lý do");
                   

                    // thêm data vào hàng
                    foreach (var report in allReports)
                    {
                        table.AddRow(
                            $"{report.StudentID}",
                            $"{report.StudentName}",
                            $"{report.ParentName}",
                            $"{report.StudentClass}",
                            $"{report.ReportDate:yyyy-MM-dd}",
                            $"{report.ReportReason}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("Không có báo cáo vắng học nào trong hệ thống.");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Lỗi hiển thị tất cả báo cáo vắng học: {ex.Message}");
                AnsiConsole.WriteLine();
            }
        }

        public void DisplayReportById()
        {
            AnsiConsole.Markup("Nhập [green]ID học sinh[/]: ");
            int id;

            // Validate ID phải là số nguyên và lớn hơn hoặc bằng 0
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                AnsiConsole.MarkupLine("[red]ID không đúng định dạng![/]");
                AnsiConsole.Markup("Nhập lại [green]ID học sinh[/]: ");
            }



            try
            {
                // truy vấn 
                var reports = context.AbsentReports
                                     .Where(report => report.StudentId == id)
                                     .OrderByDescending(report => report.CreateDay)
                                     .Select(report =>
                                         new
                                         {
                                             StudentID = report.Student.StudentId,
                                             StudentName = report.Student.Name,
                                             ParentName = report.Parent.Name,
                                             StudentClass = report.Student.Class,
                                             ReportDate = report.CreateDay,
                                             ReportReason = report.Reason
                                         })
                          
                                     .ToList();

                // kiểm tra có dữ liệu trả về không
                if (reports.Any())
                {

                    //Tạo bảng và cột
                    var table = new Table().Expand();
                    table.Title("[#ffff00]Bảng báo cáo vắng học theo ID[/]");
                    table.AddColumn("ID học sinh");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Tên phụ huynh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Ngày báo cáo");
                    table.AddColumn("Lý do");

                    foreach (var report in reports)
                    {
                        // Thêm dữ liệu vào hàng
                        table.AddRow(
                            $"{report.StudentID}",
                            $"{report.StudentName}",
                            $"{report.ParentName}",
                            $"{report.StudentClass}",
                            $"{report.ReportDate:yyyy-MM-dd}",
                            $"{report.ReportReason}"
                        );
                    }

                    // in bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine($"Không tìm thấy báo cáo vắng học với ID {id}.");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Lỗi hiển thị báo cáo theo ID: {ex.Message}");
                AnsiConsole.WriteLine();
            }
        }

        public void DisplayReportsByTime(DateTime timeStart, DateTime timeEnd)
        {
            // bắt đầu ngày truyền vào 00:00
            timeStart = timeStart.Date;

            // trước cuối ngày 1 phút
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            try
            {
                // truy vấn
                var reports = context.AbsentReports
                                     .Where(r => r.CreateDay >= timeStart && r.CreateDay <= timeEnd)
                                     .Select(report =>
                                         new
                                         {
                                             StudentID = report.Student.StudentId,
                                             StudentName = report.Student.Name,
                                             ParentName = report.Parent.Name,
                                             StudentClass = report.Student.Class,
                                             ReportDate = report.CreateDay,
                                             ReportReason = report.Reason
                                         })
                                     .ToList();

                // kiểm tra dữ liệu trả về
                if (reports.Any())
                {
                    // tạo bảng và thêm cột
                    var table = new Table().Expand();
                    table.Title($"[#ffff00]Bảng báo cáo vắng học từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/]");
                    table.AddColumn("ID học sinh");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Tên phụ huynh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Ngày báo cáo");
                    table.AddColumn("Lý do");

                    foreach (var report in reports)
                    {
                        // thêm dữ liệu vào cột
                        table.AddRow(
                            $"{report.StudentID}",
                            $"{report.StudentName}",
                            $"{report.ParentName}",
                            $"{report.StudentClass}",
                            $"{report.ReportDate:yyyy-MM-dd}",
                            $"{report.ReportReason}"
                        );
                    }

                    // in bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine($"Không có báo cáo vắng học nào trong khoảng thời gian từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}.");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Lỗi hiển thị báo cáo vắng học theo thời gian: {ex.Message}");
                AnsiConsole.WriteLine();
            }
        }
    }
}
