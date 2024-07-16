using EntryManagement.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace EntryManagement.AdminFunction
{
    public class AbsentReportManage
    {
        // Khai báo database
        private readonly EntryLogManagementContext context;

        // khởi tọa database
        public AbsentReportManage(EntryLogManagementContext _context)
        {
            context = _context;
        }

        // hàm hiển thị tất cả báo cáo vắng học
        public async Task DisplayAllReports()
        {
            try
            {
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

                if (allReports.Any())
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title("Bảng báo cáo vắng học");
                            table.AddColumn("ID");
                            table.AddColumn("Tên học sinh");
                            table.AddColumn("Tên phụ huynh");
                            table.AddColumn("Lớp");
                            table.AddColumn("Ngày báo cáo");
                            table.AddColumn("Lý do");

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

                                ctx.UpdateTarget(table);
                                ctx.Refresh();
                            }
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine("Không có báo cáo vắng học nào trong hệ thống.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Lỗi hiện thị tất cả báo cáo vắng học: {ex.Message}");
            }
        }

        public async Task DisplayReportById()
        {
            Console.Write("Nhập ID học sinh: ");
            int id;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.Write("ID không hợp lệ. Vui lòng nhập lại: ");
            }

            try
            {
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

                if (reports.Any())
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title("Bảng báo cáo vắng học theo ID");
                            table.AddColumn("ID học sinh");
                            table.AddColumn("Tên học sinh");
                            table.AddColumn("Tên phụ huynh");
                            table.AddColumn("Lớp");
                            table.AddColumn("Ngày báo cáo");
                            table.AddColumn("Lý do");

                            foreach (var report in reports)
                            {
                                table.AddRow(
                                    $"{report.StudentID}",
                                    $"{report.StudentName}",
                                    $"{report.ParentName}",
                                    $"{report.StudentClass}",
                                    $"{report.ReportDate:yyyy-MM-dd}",
                                    $"{report.ReportReason}"
                                );

                                ctx.UpdateTarget(table);
                                ctx.Refresh();
                               
                            }
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine($"Không tìm thấy báo cáo vắng học với ID {id}.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Lỗi hiển thị báo cáo theo ID: {ex.Message}");
            }
        }

        public async Task DisplayReportsByTime(DateTime timeStart, DateTime timeEnd)
        {
            timeStart = timeStart.Date;
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            try
            {
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

                if (reports.Any())
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title($"Bảng báo cáo vắng học từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}");
                            table.AddColumn("ID học sinh");
                            table.AddColumn("Tên học sinh");
                            table.AddColumn("Tên phụ huynh");
                            table.AddColumn("Lớp");
                            table.AddColumn("Ngày báo cáo");
                            table.AddColumn("Lý do");

                            foreach (var report in reports)
                            {
                                table.AddRow(
                                    $"{report.StudentID}",
                                    $"{report.StudentName}",
                                    $"{report.ParentName}",
                                    $"{report.StudentClass}",
                                    $"{report.ReportDate:yyyy-MM-dd}",
                                    $"{report.ReportReason}"
                                );

                                ctx.UpdateTarget(table);
                                ctx.Refresh();
                               
                            }
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine($"Không có báo cáo vắng học nào trong khoảng thời gian từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Lỗi hiện thị báo cáo vắng học theo thời gian: {ex.Message}");
            }
        }
    }
}
