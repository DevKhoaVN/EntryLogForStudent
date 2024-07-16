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

        public async Task DisplayAllEntryLogs()
        {
            try
            {
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

                if (entryLogs.Any())
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title("[Red]Bảng ra vào[/]").HeavyEdgeBorder().BorderColor(Color.Red);
                            table.AddColumn("[bold yellow]ID học sinh[/]");
                            table.AddColumn("[bold yellow]Tên học sinh[/]");
                            table.AddColumn("[bold yellow]Lớp[/]");
                            table.AddColumn("[bold yellow]Thời gian bản ghi[/]");
                            table.AddColumn("[bold yellow]Trạng thái[/]");
                        
                            
                            foreach (var log in entryLogs)
                            {
                                table.AddRow(
                                    $"[green]{log.StudentId}[/]",
                                    $"[blue]{log.StudentName}[/]",
                                    $"[cyan]{log.StudentClass}[/]",
                                    $"[magenta]{log.LogTime}[/]",
                                    $"[red]{log.Status}[/]"
                                );

                                ctx.UpdateTarget(table);
                                ctx.Refresh();
                             
                            }
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Không có bản ghi ra vào nào trong cơ sở dữ liệu.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị các bản ghi ra vào: {ex.Message}[/]");
            }
        }


        public async Task DisplayEntryLogsByStudentId()
        {
            Console.Write("Nhập ID học sinh: ");
            int studentId;
            while (!int.TryParse(Console.ReadLine(), out studentId))
            {
                Console.WriteLine("ID không đúng định dạng!");
            }

            try
            {
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

                if (entryLogs.Any())
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title($"[Red]Danh sách các bản ghi ra vào cho học sinh có ID {studentId}[/]").HeavyEdgeBorder().BorderColor(Color.Red);
                            table.AddColumn("[bold yellow]ID[/]");
                            table.AddColumn("[bold yellow]Tên học sinh[/]");
                            table.AddColumn("[bold yellow]Lớp[/]");
                            table.AddColumn("[bold yellow]Thời gian bản ghi[/]");
                            table.AddColumn("[bold yellow]Trạng thái[/]");

                            foreach (var log in entryLogs)
                            {
                                table.AddRow(
                                    $"[green]{log.StudentId}[/]",
                                    $"[blue]{log.StudentName}[/]",
                                    $"[cyan]{log.StudentClass}[/]",
                                    $"[magenta]{log.LogTime:yyyy-MM-dd HH:mm:ss}[/]",
                                    $"[red]{log.Status}[/]"
                                );

                                ctx.UpdateTarget(table);
                                ctx.Refresh();
                                
                            }
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Không có bản ghi ra vào nào cho học sinh có ID {studentId} trong cơ sở dữ liệu.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị bản ghi ra vào theo ID học sinh: {ex.Message}[/]");
            }
        }




        public async Task DisplayEntryLogsByTimeRange(DateTime timeStart, DateTime timeEnd)
        {
            timeStart = timeStart.Date;
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            try
            {
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

                if (entryLogs.Any())
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title($"[Red]Danh sách các bản ghi ra vào từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/]").HeavyEdgeBorder().BorderColor(Color.Red);
                            table.AddColumn("[bold yellow]ID[/]");
                            table.AddColumn("[bold yellow]Tên học sinh[/]");
                            table.AddColumn("[bold yellow]Lớp[/]");
                            table.AddColumn("[bold yellow]Thời gian bản ghi[/]");
                            table.AddColumn("[bold yellow]Trạng thái[/]");

                            foreach (var log in entryLogs)
                            {
                                table.AddRow(
                                    $"[green]{log.StudentId}[/]",
                                    $"[blue]{log.StudentName}[/]",
                                    $"[cyan]{log.StudentClass}[/]",
                                    $"[magenta]{log.LogTime:yyyy-MM-dd HH:mm:ss}[/]",
                                    $"[red]{log.Status}[/]"
                                );

                                ctx.UpdateTarget(table);
                                ctx.Refresh();
                                
                            }
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Không có bản ghi ra vào nào trong khoảng thời gian từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị bản ghi ra vào theo thời gian: {ex.Message}[/]");
            }
        }



    }
}