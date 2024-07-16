using EntryManagement.Models;
using Spectre.Console;
using System;
using System.Linq;

namespace EntryManagement.ParentFunction
{
    internal class EntryLogStudentOfParent
    {
        private readonly EntryLogManagementContext context;
        private readonly int? StudentID;

        public EntryLogStudentOfParent(EntryLogManagementContext context , int? StudentID)
        {
            this.context = context;
            this.StudentID = StudentID;
        }

        public void DisplayAllEntryLogs()
        {

            try
            {
                var student = context.Students.Find(StudentID);

                if (student != null)
                {
                    var entryLogs = context.EntryLogs
                                           .Where(e => e.StudentId == StudentID)
                                           .Select(e => new
                                           {
                                               StudentName = e.Student.Name,
                                               StudentClass = e.Student.Class,
                                               LogTime = e.LogTime,
                                               LogStatus = e.Status
                                           })
                                           .OrderByDescending(e => e.LogTime)
                                           .ToList();

                    if (entryLogs.Any())
                    {
                        var table = new Table();
                        table.AddColumn("Học sinh");
                        table.AddColumn("Lớp");
                        table.AddColumn("Thời gian ra vào");
                        table.AddColumn("Trạng thái");

                        foreach (var log in entryLogs)
                        {
                            table.AddRow(log.StudentName, log.StudentClass, log.LogTime.ToString(), log.LogStatus);
                        }

                        AnsiConsole.Write(table);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[yellow]Không có bản ghi ra vào nào trong cơ sở dữ liệu.[/]");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Không tìm thấy học sinh với ID này.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị các bản ghi ra vào: {ex.Message}[/]");
            }
        }

        public void DisplayEntryLogsByTimeRange(DateTime timeStart, DateTime timeEnd)
        {

            try
            {
                var entryLogs = context.EntryLogs
                                       .Where(e => e.LogTime >= timeStart && e.LogTime <= timeEnd && e.StudentId == StudentID)
                                       .Select(e => new
                                       {
                                           StudentName = e.Student.Name,
                                           StudentClass = e.Student.Class,
                                           LogTime = e.LogTime,
                                           LogStatus = e.Status
                                       })
                                       .ToList();

                if (entryLogs.Any())
                {
                    var table = new Table();
                    table.AddColumn("Học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian ra vào");
                    table.AddColumn("Trạng thái");

                    foreach (var log in entryLogs)
                    {
                        table.AddRow(log.StudentName, log.StudentClass, log.LogTime.ToString(), log.LogStatus);
                    }

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Không có bản ghi ra vào nào trong khoảng thời gian từ {timeStart} đến {timeEnd}.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi hiển thị bản ghi ra vào theo thời gian: {ex.Message}[/]");
            }
        }
    }
}
