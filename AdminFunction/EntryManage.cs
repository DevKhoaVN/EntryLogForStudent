using EntryManagement.Models;
using Spectre.Console;

namespace EntryManagement.AdminFunction
{
    internal class EntryManage
    {
        private readonly EntryLogManagementContext context;

        public EntryManage(EntryLogManagementContext _context)
        {
            context = _context;
        }

        // Hiển thị tất cả các bản ghi ra vào
        public void DisplayAllEntryLogs()
        {
            try
            {
                // Truy vấn
                var entryLogs = context.EntryLogs
                    .Select(e => new
                    {
                        StudentName = e.Student.Name,
                        StudentId = e.StudentId,
                        StudentClass = e.Student.Class,
                        LogTime = e.LogTime,
                        Status = e.Status
                    })
                    .OrderByDescending(e => e.LogTime)
                    .ToList();

                // Kiểm tra xem có bản ghi nào không
                if (entryLogs.Any())
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title("[#ffff00]Bảng ra vào[/]").HeavyEdgeBorder();
                    table.AddColumn("ID học sinh");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian bản ghi");
                    table.AddColumn("Trạng thái");

                    // Thêm các hàng vào bảng
                    foreach (var log in entryLogs)
                    {
                        table.AddRow(
                            $"{log.StudentId}",
                            $"{log.StudentName}",
                            $"{log.StudentClass}",
                            $"{log.LogTime:yyyy-MM-dd HH:mm:ss}",
                            $"{log.Status}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Không có bản ghi ra vào nào trong cơ sở dữ liệu.[/]");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị các bản ghi ra vào: {ex.Message}[/]");
                AnsiConsole.WriteLine();
            }
        }


        public void DisplayEntryLogsByStudentId()
        {
            AnsiConsole.Markup("Nhập [green]ID học sinh[/]: ");
            int studentId;

            // Kiểm tra đầu vào ID học sinh
            while (!int.TryParse(Console.ReadLine(), out studentId) || studentId < 0)
            {
                AnsiConsole.MarkupLine("[red]ID không đúng định dạng! Vui lòng nhập lại.[/]");
                AnsiConsole.Markup("Nhập [green]ID học sinh[/]: ");
            }

            try
            {
                // Truy vấn
                var entryLogs = context.EntryLogs
                    .Where(e => e.StudentId == studentId)
                    .Select(e => new
                    {
                        StudentName = e.Student.Name,
                        StudentId = e.StudentId,
                        StudentClass = e.Student.Class,
                        LogTime = e.LogTime,
                        Status = e.Status
                    })
                    .OrderByDescending(e => e.LogTime)
                    .ToList();

                // Kiểm tra xem có bản ghi nào không
                if (entryLogs.Any())
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title($"[#ffff00]Danh sách các bản ghi ra vào cho học sinh có ID {studentId}[/]").HeavyEdgeBorder();
                    table.AddColumn("ID học sinh");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian bản ghi");
                    table.AddColumn("Trạng thái");

                    // Thêm các hàng vào bảng
                    foreach (var log in entryLogs)
                    {
                        table.AddRow(
                            $"{log.StudentId}",
                            $"{log.StudentName}",
                            $"{log.StudentClass}",
                            $"{log.LogTime:yyyy-MM-dd HH:mm:ss}",
                            $"{log.Status}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Không có bản ghi ra vào nào cho học sinh có ID {studentId} trong cơ sở dữ liệu.[/]");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị bản ghi ra vào theo ID học sinh: {ex.Message}[/]");
                AnsiConsole.WriteLine();
            }
        }


        // Hiển thị các bản ghi ra vào trong một khoảng thời gian
        public void DisplayEntryLogsByTimeRange(DateTime timeStart, DateTime timeEnd)
        {
            // Chuyển đổi thời gian bắt đầu và kết thúc thành ngày đầy đủ
            timeStart = timeStart.Date;
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            try
            {
                // Truy vấn
                var entryLogs = context.EntryLogs
                    .Where(e => e.LogTime >= timeStart && e.LogTime <= timeEnd)
                    .Select(e => new
                    {
                        StudentName = e.Student.Name,
                        StudentId = e.StudentId,
                        StudentClass = e.Student.Class,
                        LogTime = e.LogTime,
                        Status = e.Status
                    })
                    .ToList();

                // Kiểm tra xem có bản ghi nào không
                if (entryLogs.Any())
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title($"[yellow]Danh sách các bản ghi ra vào từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/]").HeavyEdgeBorder();
                    table.AddColumn("ID học sinh");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian bản ghi");
                    table.AddColumn("Trạng thái");

                    // Thêm các hàng vào bảng
                    foreach (var log in entryLogs)
                    {
                        table.AddRow(
                            $"{log.StudentId}",
                            $"{log.StudentName}",
                            $"{log.StudentClass}",
                            $"{log.LogTime:yyyy-MM-dd HH:mm:ss}",
                            $"{log.Status}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Không có bản ghi ra vào nào trong khoảng thời gian từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}.[/]");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị bản ghi ra vào theo thời gian: {ex.Message}[/]");
                AnsiConsole.WriteLine();
            }
        }


    }
}
