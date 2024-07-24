using EntryManagement.Models;
using Spectre.Console;
using System;
using System.Globalization;
using System.Linq;

namespace EntryManagement.AdminFunction
{
    public class EntryLaterManage
    {
        private readonly EntryLogManagementContext context;
        private readonly DateTime today = DateTime.Today;
        private DateTime morningStartTime;
        private DateTime morningLateTime;
        private DateTime afternoonStartTime;
        private DateTime afternoonLateTime;

        // Xác định thời gian bắt đầu của buổi sáng và buổi chiều
        public EntryLaterManage(EntryLogManagementContext _context)
        {
            context = _context;

            // Correctly assign to class-level fields
            morningStartTime = new DateTime(today.Year, today.Month, today.Day, 7, 0, 0); // 7:00 AM
            morningLateTime = new DateTime(today.Year, today.Month, today.Day, 7, 30, 0); // 7:30 AM

            afternoonStartTime = new DateTime(today.Year, today.Month, today.Day, 19, 0, 0); // 2:00 PM
            afternoonLateTime = new DateTime(today.Year, today.Month, today.Day, 19, 59, 0); // 2:30 PM
        }

        // 1. hiển thị thời gian đi muộn của học sinh trong ngày
        public void FilterStudentLater()
        {
            try
            {
                // Lọc các bản ghi học sinh đi muộn
                var studentLate = context.EntryLogs.Where(e =>
                    (e.LogTime > morningStartTime && e.LogTime <= morningLateTime) ||
                    (e.LogTime > afternoonStartTime && e.LogTime <= afternoonLateTime)
                ).Select(e => new {
                    ID = e.StudentId,
                    Name = e.Student.Name,
                    Class = e.Student.Class,
                    Status = e.Status,
                    TimeLate = e.LogTime
                }).ToList();

                // Tạo bảng
                var table = new Table();
                table.Border = TableBorder.Rounded;
                table.AddColumn("ID");
                table.AddColumn("Học sinh");
                table.AddColumn("Lớp");
                table.AddColumn("Trạng thái");
                table.AddColumn("Thời gian");

                if (studentLate != null && studentLate.Count > 0) // Ensure list is not empty
                {
                    // Thêm dữ liệu vào bảng
                    foreach (var student in studentLate)
                    {
                        table.AddRow(
                           $"{student.ID}",
                           $"{student.Name}",
                           $"{student.Class}",
                           $"{student.Status}",
                           $"{student.TimeLate}" // Format the time display
                       );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.Markup("[Red]Không có dữ liệu trên hệ thống ![/]");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("[red] Lỗi dữ liệu : " + ex.Message + "[/]");
                AnsiConsole.WriteLine();
            }
        }
        public void FilterByStudentId()
        {
            AnsiConsole.Markup("Nhập[green] ID của học sinh:[/] ");
            Console.WriteLine();
            int id;

            while (!int.TryParse(Console.ReadLine(), out id))
            {
                AnsiConsole.MarkupLine("[red]ID không đúng định dạng![/]");
                AnsiConsole.Markup("Nhập lại [green]ID học sinh[/]: ");
            }


            try
            {
                var studentLog = context.EntryLogs.Where(e =>
                       (((e.LogTime > morningStartTime && e.LogTime <= morningLateTime) ||
                       (e.LogTime > afternoonStartTime && e.LogTime <= afternoonLateTime)) & e.StudentId == id)
                   ).Select(e => new
                   {
                       ID = e.StudentId,
                       Name = e.Student.Name,
                       Class = e.Student.Class,
                       Status = e.Status,
                       TimeLate = e.LogTime
                   }).OrderByDescending(e => e.TimeLate).FirstOrDefault();

                if (studentLog != null)
                {
                    var table = new Table().Expand();
                    table.Title($"Thông tin học sinh {studentLog.Name}");

                    table.AddColumn("ID");
                    table.AddColumn("Học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian");
                    table.AddColumn("Trạng thái");

                    table.AddRow($"{studentLog.ID}",
                                 $"{studentLog.Name}",
                                 $"{studentLog.Class}",
                                 $"{studentLog.Status}",
                                 $"{studentLog.TimeLate}"
                                 );

                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.Markup("[red]Không tìm thấy bản ghi nào cho ID học sinh này.[/]");
                    AnsiConsole.WriteLine();
                }
            }catch(Exception ex)
            {
                AnsiConsole.WriteLine("[red]Lỗi khi lọc dữ liệu theo id[/] :" + ex.InnerException.Message);
                AnsiConsole.WriteLine();
            }
         
        }


        public void FilterByRangeTime()
        {
            try
            {
                // Khởi tạo biến ngày bắt đầu và ngày kết thúc
                DateTime startDate, endDate;

                // Nhập ngày bắt đầu
                AnsiConsole.Markup("Nhập [green]ngày bắt đầu (yyyy/MM/dd)[/]: ");
                while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                {
                    AnsiConsole.MarkupLine("[red]Ngày không hợp lệ.[/] Vui lòng nhập lại (yyyy/MM/dd): ");
                    AnsiConsole.Markup("Nhập [green]ngày bắt đầu (yyyy/MM/dd)[/]: ");
                }

                // Nhập ngày kết thúc
                AnsiConsole.Markup("Nhập [green]ngày kết thúc (yyyy/MM/dd)[/]: ");
                while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                {
                    AnsiConsole.MarkupLine("[red]Ngày không hợp lệ.[/] Vui lòng nhập lại (yyyy/MM/dd): ");
                    AnsiConsole.Markup("Nhập [green]ngày kết thúc (yyyy/MM/dd)[/]: ");
                }

                // Thêm 1 ngày vào ngày kết thúc để bao gồm cả ngày đó
                endDate = endDate.AddDays(1).AddTicks(-1);

                // Truy vấn bản ghi học sinh đi muộn trong khoảng thời gian xác định
                var studentLate = context.EntryLogs
                    .Where(e => (e.LogTime >= startDate && e.LogTime <= endDate) &&
                                ((e.LogTime.TimeOfDay > morningStartTime.TimeOfDay && e.LogTime.TimeOfDay <= morningLateTime.TimeOfDay) ||
                                 (e.LogTime.TimeOfDay > afternoonStartTime.TimeOfDay && e.LogTime.TimeOfDay <= afternoonLateTime.TimeOfDay)))
                    .OrderByDescending(e => e.LogTime)
                    .Select(e => new
                    {
                        ID = e.StudentId,
                        Name = e.Student.Name,
                        Class = e.Student.Class,
                        TimeLate = e.LogTime,
                        Status = e.Status
                    })
                    .ToList();

                // Kiểm tra và hiển thị kết quả trong bảng
                if (studentLate.Any())
                {
                    var table = new Table().Expand();
                    table.Title($"Các bản ghi từ {startDate:yyyy/MM/dd} đến {endDate:yyyy/MM/dd} và trong khoảng giờ quy định");
                    table.AddColumn("ID");
                    table.AddColumn("Học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian");
                    table.AddColumn("Trạng thái");

                    foreach (var studentLog in studentLate)
                    {
                        table.AddRow(
                            $"{studentLog.ID}",
                            $"{studentLog.Name}",
                            $"{studentLog.Class}",
                            $"{studentLog.TimeLate:yyyy-MM-dd HH:mm:ss}",
                            $"{studentLog.Status}"
                        );
                    }

                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Không có bản ghi nào trong khoảng thời gian từ {startDate:yyyy/MM/dd} đến {endDate:yyyy/MM/dd} và trong khoảng giờ quy định.[/]");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi lọc theo khoảng thời gian và giờ:[/] {ex.Message}");
                AnsiConsole.WriteLine();
            }
        }

    }
}
