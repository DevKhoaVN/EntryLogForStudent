using EntryManagement.Models;
using Spectre.Console;
using System;
using System.Linq;

namespace EntryManagement.ParentFunction
{
    internal class AbsentReportOfParent
    {
        private readonly EntryLogManagementContext context;
        private readonly int? StudentID;

        public AbsentReportOfParent(EntryLogManagementContext context , int? StudentID)
        {
            this.context = context;
            this.StudentID = StudentID;

        }

        public void SendReport()
        {
            

            try
            {
                var student = context.Students
                                     .Where(e => e.StudentId == StudentID)
                                     .Select(e => new { e.StudentId, e.Parent.ParentId })
                                     .FirstOrDefault();

                if (student != null)
                {
                    string reason = AnsiConsole.Ask<string>("Nhập [green]lí do vắng học[/]:");

                    AbsentReport report = new AbsentReport()
                    {
                        ParentId = student.ParentId,
                        CreateDay = DateTime.Now,
                        StudentId = student.StudentId,
                        Reason = reason
                    };

                    context.AbsentReports.Add(report);
                    context.SaveChanges();

                    AnsiConsole.MarkupLine("[green]Bạn đã lưu thành công![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Không tìm thấy học sinh với ID này.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi gửi báo cáo vắng học: {ex.Message}[/]");
            }
        }

        public void DisplayReport()
        {
           

            try
            {
                var multiReport = context.AbsentReports
                                         .Where(e => e.StudentId == StudentID)
                                         .Select(e => new
                                         {
                                             StudentName = e.Student.Name,
                                             ParentName = e.Parent.Name,
                                             StudentClass = e.Student.Class,
                                             Reason = e.Reason,
                                             Date = e.CreateDay
                                         })
                                         .OrderByDescending(e => e.Date)
                                         .ToList();

                if (multiReport.Any())
                {
                    var table = new Table();
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Tên phụ huynh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Lý do vắng");
                    table.AddColumn("Ngày báo cáo");

                    foreach (var report in multiReport)
                    {
                        table.AddRow(report.StudentName, report.ParentName, report.StudentClass, report.Reason, report.Date.ToString());
                    }

                    AnsiConsole.Write(table);
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Không có báo cáo vắng học nào.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi hiển thị bảng báo cáo cho phụ huynh: {ex.Message}[/]");
            }
        }
    }
}
