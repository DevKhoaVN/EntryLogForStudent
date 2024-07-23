using EntryManagement.Models;
using Spectre.Console;

namespace EntryManagement.AdminFunction
{
    internal class StudentInformationManage
    {
        private readonly EntryLogManagementContext context;

        
        public StudentInformationManage(EntryLogManagementContext _context)
        {
            context = _context;
        }

        public void DisplayStudentsWithParentsInfo()
        {
            try
            {
                // Truy vấn
                var studentsWithParents = context.Students
                    .Select(s => new
                    {
                        StudentId = s.StudentId,
                        StudentName = s.Name,
                        StudentGender = s.Gender,
                        StudentDOB = s.DayOfBirth,
                        StudentClass = s.Class,
                        StudentAddress = s.Address,
                        StudentPhone = s.Phone,
                        ParentName = s.Parent.Name,
                        ParentEmail = s.Parent.Email,
                        ParentPhone = s.Parent.Phone,
                        ParentAddress = s.Parent.Address
                    })
                    .ToList();

                // Kiểm tra có dữ liệu trả về không
                if (studentsWithParents.Any())
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title("[#ffff00]Danh sách học sinh và thông tin phụ huynh[/]").HeavyEdgeBorder();
                    table.AddColumn("ID học sinh");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Giới tính");
                    table.AddColumn("Ngày sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Địa chỉ");
                    table.AddColumn("Số điện thoại");
                    table.AddColumn("Tên phụ huynh");
                    table.AddColumn("Email phụ huynh");
                    table.AddColumn("Số điện thoại phụ huynh");
                    table.AddColumn("Địa chỉ phụ huynh");

                    // Thêm dữ liệu vào hàng
                    foreach (var student in studentsWithParents)
                    {
                        table.AddRow(
                            $"{student.StudentId}",
                            $"{student.StudentName}",
                            $"{student.StudentGender}",
                            $"{student.StudentDOB:yyyy-MM-dd}",
                            $"{student.StudentClass}",
                            $"{student.StudentAddress}",
                            $"{student.StudentPhone}",
                            $"{student.ParentName}",
                            $"{student.ParentEmail}",
                            $"{student.ParentPhone}",
                            $"{student.ParentAddress}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Render(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Không có học sinh nào trong cơ sở dữ liệu.[/]");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi lấy danh sách học sinh và thông tin phụ huynh: {ex.Message}[/]");
                AnsiConsole.WriteLine();
            }
        }


        public void FilterByStudentId()
        {
            AnsiConsole.Markup("Nhập [green]ID học sinh[/]: ");
            int studentId;
            AnsiConsole.WriteLine();

            // Validate ID
            while (!int.TryParse(Console.ReadLine(), out studentId) || studentId <= 0)
            {
                AnsiConsole.Markup("[red]ID không hợp lệ[/]. Vui lòng nhập lại: ");
            }

            try
            {
                // Truy vấn
                var students = context.Students
                    .Where(s => s.StudentId == studentId)
                    .Select(s => new
                    {
                        StudentId = s.StudentId,
                        StudentName = s.Name,
                        StudentGender = s.Gender,
                        StudentDOB = s.DayOfBirth,
                        StudentClass = s.Class,
                        StudentAddress = s.Address,
                        StudentPhone = s.Phone,
                        ParentName = s.Parent.Name,
                        ParentEmail = s.Parent.Email,
                        ParentPhone = s.Parent.Phone,
                        ParentAddress = s.Parent.Address
                    })
                    .FirstOrDefault();

                // Kiểm tra có dữ liệu trả về không
                if (students != null)
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title("[#ffff00]Thông tin học sinh và thông tin bố mẹ[/]");
                    table.AddColumn("Thông tin");
                    table.AddColumn("Chi tiết");

                    table.AddRow("ID học sinh", $"{students.StudentId}");
                    table.AddRow("Tên học sinh", $"{students.StudentName}");
                    table.AddRow("Giới tính", $"{students.StudentGender}");
                    table.AddRow("Ngày sinh", $"{students.StudentDOB:yyyy-MM-dd}");
                    table.AddRow("Lớp", $"{students.StudentClass}");
                    table.AddRow("Địa chỉ", $"{students.StudentAddress}");
                    table.AddRow("Số điện thoại", $"{students.StudentPhone}");
                    table.AddRow("Tên phụ huynh", $"{students.ParentName}");
                    table.AddRow("Email phụ huynh", $"{students.ParentEmail}");
                    table.AddRow("Số điện thoại phụ huynh", $"{students.ParentPhone}");
                    table.AddRow("Địa chỉ phụ huynh", $"{students.ParentAddress}");

                    // Hiển thị bảng
                    AnsiConsole.Write(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Không tìm thấy học sinh với ID {studentId} trong cơ sở dữ liệu[/].");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi lọc theo ID học sinh[/]: {ex.Message}");
                AnsiConsole.WriteLine();
            }
        }


        public void FilterByTimeRange(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                // Khởi tạo đầy đủ ngày
                timeStart = timeStart.Date;
                timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

                // Truy vấn
                var studentsWithReports = context.Students
                    .Where(s => s.JoinDay >= timeStart && s.JoinDay <= timeEnd)
                    .Select(s => new
                    {
                        StudentId = s.StudentId,
                        StudentName = s.Name,
                        StudentGender = s.Gender,
                        StudentDOB = s.DayOfBirth,
                        StudentClass = s.Class,
                        StudentAddress = s.Address,
                        StudentPhone = s.Phone,
                        ParentName = s.Parent.Name,
                        ParentEmail = s.Parent.Email,
                        ParentPhone = s.Parent.Phone,
                        ParentAddress = s.Parent.Address
                    })
                    .ToList();

                // Kiểm tra có dữ liệu trả về không
                if (studentsWithReports.Any())
                {
                    // Tạo bảng và thêm các cột
                    var table = new Table().Expand();
                    table.Title($"[#ffff00]Danh sách học sinh có báo cáo vắng mặt từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/]");
                    table.AddColumn("ID học sinh");
                    table.AddColumn("Tên học sinh");
                    table.AddColumn("Giới tính");
                    table.AddColumn("Ngày sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Địa chỉ");
                    table.AddColumn("Số điện thoại");
                    table.AddColumn("Tên phụ huynh");
                    table.AddColumn("Email phụ huynh");
                    table.AddColumn("Số điện thoại phụ huynh");
                    table.AddColumn("Địa chỉ phụ huynh");

                    foreach (var student in studentsWithReports)
                    {
                        // Thêm dữ liệu vào hàng
                        table.AddRow(
                            $"{student.StudentId}",
                            $"{student.StudentName}",
                            $"{student.StudentGender}",
                            $"{student.StudentDOB:yyyy-MM-dd}",
                            $"{student.StudentClass}",
                            $"{student.StudentAddress}",
                            $"{student.StudentPhone}",
                            $"{student.ParentName}",
                            $"{student.ParentEmail}",
                            $"{student.ParentPhone}",
                            $"{student.ParentAddress}"
                        );
                    }

                    // Hiển thị bảng
                    AnsiConsole.Write(table);
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Không có học sinh nào có báo cáo vắng mặt trong khoảng thời gian từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/].");
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi lọc theo khoảng thời gian: {ex.Message}[/]");
                AnsiConsole.WriteLine();
            }
        }

    }
}
