using EntryManagement.Models; // Import các namespace cần thiết
using Spectre.Console;
using System;
using System.Linq;

namespace EntryManagement.ParentFunction
{
    internal class AbsentReportOfParent
    {
        private readonly EntryLogManagementContext context; // DbContext để quản lý dữ liệu
        private readonly int? StudentID; // ID của học sinh cần báo cáo vắng học

        // Constructor để inject DbContext và StudentID
        public AbsentReportOfParent(EntryLogManagementContext context, int? StudentID)
        {
            this.context = context; // Khởi tạo context
            this.StudentID = StudentID; // Khởi tạo StudentID
        }

        // Phương thức gửi báo cáo vắng học
        public void SendReport()
        {
            try
            {
                // Tìm thông tin học sinh và phụ huynh tương ứng
                var student = context.Students
                                     .Where(e => e.StudentId == StudentID)
                                     .Select(e => new { e.StudentId, e.Parent.ParentId })
                                     .FirstOrDefault();

                if (student != null)
                {
                    // Nhập lí do vắng học từ người dùng
                    string reason = AnsiConsole.Ask<string>("Nhập [green]lí do vắng học[/]:");

                    // Tạo đối tượng báo cáo vắng học và lưu vào DbContext
                    AbsentReport report = new AbsentReport()
                    {
                        ParentId = student.ParentId,
                        CreateDay = DateTime.Now,
                        StudentId = student.StudentId,
                        Reason = reason
                    };

                    context.AbsentReports.Add(report); // Thêm báo cáo vào DbContext
                    context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                    AnsiConsole.MarkupLine("[green]Bạn đã lưu thành công![/]"); // Thông báo thành công
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Không tìm thấy học sinh với ID này.[/]"); // Thông báo không tìm thấy học sinh
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi gửi báo cáo vắng học: {ex.Message}[/]"); // Thông báo lỗi nếu có
            }
        }

        // Phương thức hiển thị bảng báo cáo vắng học cho phụ huynh
        public void DisplayReport()
        {
            try
            {
                // Lấy danh sách báo cáo vắng học của học sinh
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
                    // Tạo bảng để hiển thị thông tin báo cáo
                    var table = new Table();
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Tên phụ huynh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Lý do vắng");
                    table.AddColumn("Ngày báo cáo");

                    foreach (var report in multiReport)
                    {
                        // Thêm dòng vào bảng cho từng báo cáo
                        table.AddRow(report.StudentName, report.ParentName, report.StudentClass, report.Reason, report.Date.ToString());
                    }

                    AnsiConsole.Write(table); // Hiển thị bảng ra màn hình
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Không có báo cáo vắng học nào.[/]"); // Thông báo không có báo cáo nào
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi hiển thị bảng báo cáo cho phụ huynh: {ex.Message}[/]"); // Thông báo lỗi nếu có
            }
        }
    }
}
