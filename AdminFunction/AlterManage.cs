using EntryManagement.Models;
using Spectre.Console;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EntryManagement.AdminFunction
{
    internal class AlertManage
    {
        private readonly EntryLogManagementContext context;

        public AlertManage(EntryLogManagementContext _context)
        {
            context = _context;
        }

        public void DisplayAllAlerts()
        {
            try
            {
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

                if (alerts.Any())
                {
                    var table = new Table().Expand();
                    table.Title("Danh sách các cảnh báo").HeavyEdgeBorder().BorderColor(Color.Red);
                    table.AddColumn("[bold yellow]ID[/]");
                    table.AddColumn("[bold yellow]Tên học sinh[/]");
                    table.AddColumn("[bold yellow]Lớp[/]");
                    table.AddColumn("[bold yellow]Thời gian cảnh báo[/]");

                    foreach (var alert in alerts)
                    {
                        table.AddRow(
                            $"{alert.StudentId}",
                            $"{alert.StudentName}",
                            $"{alert.StudentClass}",
                            $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                        );
                    }

                    AnsiConsole.Render(table);
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Không có cảnh báo nào trong cơ sở dữ liệu.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị các cảnh báo: {ex.Message}[/]");
            }
        }

        public void FindAlertsByStudentId()
        {
            Console.Write("Nhập ID học sinh: ");
            int studentId;
            while (!int.TryParse(Console.ReadLine(), out studentId))
            {
                Console.WriteLine("ID không đúng định dạng!");
                Console.Write("Nhập lại ID học sinh: ");
            }

            try
            {
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

                if (alerts.Any())
                {
                    var table = new Table().Expand();
                    table.Title($"Danh sách cảnh báo cho học sinh có ID {studentId}").HeavyEdgeBorder().BorderColor(Color.Red);
                    table.AddColumn("[bold yellow]ID[/]");
                    table.AddColumn("[bold yellow]Tên học sinh[/]");
                    table.AddColumn("[bold yellow]Lớp[/]");
                    table.AddColumn("[bold yellow]Thời gian cảnh báo[/]");

                    foreach (var alert in alerts)
                    {
                        table.AddRow(
                            $"{alert.StudentId}",
                            $"{alert.StudentName}",
                            $"{alert.StudentClass}",
                            $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                        );
                    }

                    AnsiConsole.Render(table);
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Không có cảnh báo nào cho học sinh có ID {studentId} trong cơ sở dữ liệu.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi tìm cảnh báo theo ID học sinh: {ex.Message}[/]");
            }
        }

        public void FilterAlertsByTimeRange(DateTime timeStart, DateTime timeEnd)
        {
            timeStart = timeStart.Date;
            timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

            try
            {
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

                if (alerts.Any())
                {
                    var table = new Table().Expand();
                    table.Title($"Danh sách cảnh báo từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}").HeavyEdgeBorder().BorderColor(Color.Red);
                    table.AddColumn("[bold yellow]ID[/]");
                    table.AddColumn("[bold yellow]Tên học sinh[/]");
                    table.AddColumn("[bold yellow]Lớp[/]");
                    table.AddColumn("[bold yellow]Thời gian cảnh báo[/]");

                    foreach (var alert in alerts)
                    {
                        table.AddRow(
                            $"{alert.StudentId}",
                            $"{alert.StudentName}",
                            $"{alert.StudentClass}",
                            $"{alert.AlertTime:yyyy-MM-dd HH:mm:ss}"
                        );
                    }

                    AnsiConsole.Render(table);
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Không có cảnh báo nào trong khoảng thời gian từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi lọc cảnh báo theo thời gian: {ex.Message}[/]");
            }
        }
    }
}
