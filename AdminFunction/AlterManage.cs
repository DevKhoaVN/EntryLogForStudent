using EntryManagement.Models;
using Spectre.Console;
using System;
using System.Linq;

namespace EntryManagement.AdminFunction
{
    internal class AlertManage
    {
        private readonly EntryLogManagementContext context;

        // Constructor khởi tạo context 
        public AlertManage(EntryLogManagementContext _context)
        {
            context = _context;
        }

        // Hiển thị tất cả các cảnh báo
        public void DisplayAllAlerts()
        {
            try
            {
                // Truy vấn
                var alerts = context.Alerts
                                    .Select(a => new
                                    {
                                        StudentId = a.StudentId,
                                        StudentName = a.Student.Name,
                                        StudentClass = a.Student.Class,
                                        AlertTime = a.AlertTime
                                    })
                                    .OrderByDescending(a => a.AlertTime)
                                    .ToList();

                // Kiểm tra xem có cảnh báo nào không
                if (alerts.Any())
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title("[#ffff00]Danh sách các cảnh báo[/]").HeavyEdgeBorder();
                    table.AddColumn("ID");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian cảnh báo");

                    // Thêm các hàng vào bảng
                    foreach (var alert in alerts)
                    {
                        table.AddRow(
                            $"{alert.StudentId}",
                            $"{alert.StudentName}",
                            $"{alert.StudentClass}",
                            $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Không có cảnh báo nào trong cơ sở dữ liệu.[/]");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị các cảnh báo: {ex.Message}[/]");
                AnsiConsole.WriteLine();
            }
        }

        // Tìm và hiển thị các cảnh báo theo ID học sinh
        public void FindAlertsByStudentId()
        {
            AnsiConsole.Markup("Nhập [green]ID học sinh[/]: ");
            int studentId;

            // Kiểm tra đầu vào ID học sinh
            while (!int.TryParse(Console.ReadLine(), out studentId))
            {
                AnsiConsole.MarkupLine("[red]ID không đúng định dạng![/]");
                AnsiConsole.Markup("Nhập lại [green]ID học sinh[/]: ");
            }

            try
            {
                // Truy vấn
                var alerts = context.Alerts
                                    .Where(a => a.StudentId == studentId)
                                    .OrderByDescending(a => a.AlertTime)
                                    .Select(a => new
                                    {
                                        StudentId = a.StudentId,
                                        StudentName = a.Student.Name,
                                        StudentClass = a.Student.Class,
                                        AlertTime = a.AlertTime
                                    })
                                    .ToList();

                // Kiểm tra xem có cảnh báo nào không
                if (alerts.Any())
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title($"[#ffff00]Danh sách cảnh báo cho học sinh có ID {studentId}[/]").HeavyEdgeBorder();
                    table.AddColumn("ID");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian cảnh báo");

                    // Thêm các hàng vào bảng
                    foreach (var alert in alerts)
                    {
                        table.AddRow(
                            $"{alert.StudentId}",
                            $"{alert.StudentName}",
                            $"{alert.StudentClass}",
                            $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Không có cảnh báo nào cho học sinh có ID {studentId} trong cơ sở dữ liệu.[/]");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi tìm cảnh báo theo ID học sinh: {ex.Message}[/]");
                AnsiConsole.WriteLine();
            }
        }


        // Lọc và hiển thị các cảnh báo theo khoảng thời gian
        public void FilterAlertsByTimeRange(DateTime timeStart, DateTime timeEnd)
        {
            // Chuyển đổi thời gian bắt đầu và kết thúc thành ngày đầy đủ
            timeStart = timeStart.Date;
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            try
            {
                // truy vấn
                var alerts = context.Alerts
                                    .Where(a => a.AlertTime >= timeStart && a.AlertTime <= timeEnd)
                                    .Select(a => new
                                    {
                                        StudentId = a.StudentId,
                                        StudentName = a.Student.Name,
                                        StudentClass = a.Student.Class,
                                        AlertTime = a.AlertTime
                                    })
                                    .ToList();

                // Kiểm tra xem có cảnh báo nào không
                if (alerts.Any())
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title($"[#ffff00]Danh sách cảnh báo từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}").HeavyEdgeBorder();
                    table.AddColumn("ID");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian cảnh báo");

                    // Thêm các hàng vào bảng
                    foreach (var alert in alerts)
                    {
                        table.AddRow(
                            $"{alert.StudentId}",
                            $"{alert.StudentName}",
                            $"{alert.StudentClass}",
                            $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                }
                else
                {
                    AnsiConsole.MarkupLine($"Không có cảnh báo nào trong khoảng thời gian từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}.");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"Lỗi khi lọc cảnh báo theo thời gian: {ex.Message}");
            }
        }
    }
}
