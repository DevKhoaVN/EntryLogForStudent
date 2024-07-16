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

        public async Task DisplayStudentsWithParentsInfo()
        {
            try
            {
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

                if (studentsWithParents.Any())
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title("[Red]Danh sách học sinh và thông tin bố mẹ[/]");
                            table.AddColumn("[bold yellow]ID học sinh[/]");
                            table.AddColumn("[bold yellow]Tên học sinh[/]");
                            table.AddColumn("[bold yellow]Giới tính[/]");
                            table.AddColumn("[bold yellow]Ngày sinh[/]");
                            table.AddColumn("[bold yellow]Lớp[/]");
                            table.AddColumn("[bold yellow]Địa chỉ[/]");
                            table.AddColumn("[bold yellow]Số điện thoại[/]");
                            table.AddColumn("[bold yellow]Tên phụ huynh[/]");
                            table.AddColumn("[bold yellow]Email phụ huynh[/]");
                            table.AddColumn("[bold yellow]Số điện thoại phụ huynh[/]");
                            table.AddColumn("[bold yellow]Địa chỉ phụ huynh[/]");

                            foreach (var student in studentsWithParents)
                            {
                                table.AddRow(
                                    $"{student.StudentId}",
                                    $"[#d7af00]{student.StudentName}[/]",
                                    $"{student.StudentGender}",
                                    $"{student.StudentDOB:yyyy-MM-dd}",
                                    $"{student.StudentClass}",
                                    $"[#5fd7ff]{student.StudentAddress}[/]",
                                    $"[#d70000]{student.StudentPhone}[/]",
                                    $"[#d7af00]{student.ParentName}[/]",
                                    $"[#5fd75f]{student.ParentEmail}[/]",
                                    $"[#d70000]{student.ParentPhone}[/]",
                                    $"[#5fd7ff]{student.ParentAddress}[/]"
                                );

                                ctx.UpdateTarget(table);
                                ctx.Refresh();
                            }

                        });
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Không có học sinh nào trong cơ sở dữ liệu.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi lấy danh sách học sinh và thông tin phụ huynh: {ex.Message}[/]");
            }
        }


        public async Task FilterByStudentId()
        {
            Console.Write("Nhập ID học sinh muốn xem báo cáo vắng mặt: ");
            int studentId;
            while (!int.TryParse(Console.ReadLine(), out studentId))
            {
                Console.Write("ID không hợp lệ. Vui lòng nhập lại: ");
            }

            try
            {
                var student = context.Students
                    .Where(s => s.StudentId == studentId)
                    .Select(s => new
                    {
                        StudentId = s.StudentId,
                        StudentName = s.Name,
                        StudentGender = s.Gender,
                        StudentDOB = s.DayOfBirth.ToShortDateString(),
                        StudentClass = s.Class,
                        StudentAddress = s.Address,
                        StudentPhone = s.Phone,
                        ParentName = s.Parent.Name,
                        ParentEmail = s.Parent.Email,
                        ParentPhone = s.Parent.Phone,
                        ParentAddress = s.Parent.Address
                    })
                    .SingleOrDefault();

                if (student != null)
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title("Thông tin học sinh và thông tin bố mẹ");
                            table.AddColumn("Thông tin");
                            table.AddColumn("Chi tiết");

                            table.AddRow("ID học sinh", $"{student.StudentId}");
                            table.AddRow("Tên học sinh", $"{student.StudentName}");
                            table.AddRow("Giới tính", $"{student.StudentGender}");
                            table.AddRow("Ngày sinh", $"{student.StudentDOB:yyyy-MM-dd}");
                            table.AddRow("Lớp", $"{student.StudentClass}");
                            table.AddRow("Địa chỉ", $"{student.StudentAddress}");
                            table.AddRow("Số điện thoại", $"{student.StudentPhone}");
                            table.AddRow("Tên phụ huynh", $"{student.ParentName}");
                            table.AddRow("Email phụ huynh", $"{student.ParentEmail}");
                            table.AddRow("Số điện thoại phụ huynh", $"{student.ParentPhone}");
                            table.AddRow("Địa chỉ phụ huynh", $"{student.ParentAddress}");


                            ctx.UpdateTarget(table);
                            ctx.Refresh();
                            
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Không tìm thấy học sinh với ID {studentId} trong cơ sở dữ liệu.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi lọc theo ID học sinh: {ex.Message}[/]");
            }
        }


        public async Task FilterByTimeRange(DateTime timeStart, DateTime timeEnd)
        {
            try
            {
                timeStart = timeStart.Date;
                timeEnd = timeEnd.Date.AddDays(1).AddSeconds(-1);

                var studentsWithReports = context.Students
                    .Where(s => s.AbsentReports.Any(ar => ar.CreateDay >= timeStart && ar.CreateDay <= timeEnd))
                    .Select(s => new
                    {
                        StudentId = s.StudentId,
                        StudentName = s.Name,
                        StudentGender = s.Gender,
                        StudentDOB = s.DayOfBirth.ToShortDateString(),
                        StudentClass = s.Class,
                        StudentAddress = s.Address,
                        StudentPhone = s.Phone,
                        ParentName = s.Parent.Name,
                        ParentEmail = s.Parent.Email,
                        ParentPhone = s.Parent.Phone,
                        ParentAddress = s.Parent.Address
                    })
                    .ToList();

                if (studentsWithReports.Any())
                {
                    await AnsiConsole.Live(new Table().Expand())
                        .StartAsync(async ctx =>
                        {
                            var table = new Table().Expand();
                            table.Title($"[Red]Danh sách học sinh có báo cáo vắng mặt từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}[/]").HeavyEdgeBorder().BorderColor(Color.Red);
                            table.AddColumn("[bold yellow]ID học sinh[/]");
                            table.AddColumn("[bold yellow]Tên học sinh[/]");
                            table.AddColumn("[bold yellow]Giới tính[/]");
                            table.AddColumn("[bold yellow]Ngày sinh[/]");
                            table.AddColumn("[bold yellow]Lớp[/]");
                            table.AddColumn("[bold yellow]Địa chỉ[/]");
                            table.AddColumn("[bold yellow]Số điện thoại[/]");
                            table.AddColumn("[bold yellow]Tên phụ huynh[/]");
                            table.AddColumn("[bold yellow]Email phụ huynh[/]");
                            table.AddColumn("[bold yellow]Số điện thoại phụ huynh[/]");
                            table.AddColumn("[bold yellow]Địa chỉ phụ huynh[/]");

                            foreach (var student in studentsWithReports)
                            {
                                table.AddRow(
                                    $"[green]{student.StudentId}[/]",
                                    $"[blue]{student.StudentName}[/]",
                                    $"[cyan]{student.StudentGender}[/]",
                                    $"[magenta]{student.StudentDOB}[/]",
                                    $"[green]{student.StudentClass}[/]",
                                    $"[blue]{student.StudentAddress}[/]",
                                    $"[cyan]{student.StudentPhone}[/]",
                                    $"[magenta]{student.ParentName}[/]",
                                    $"[green]{student.ParentEmail}[/]",
                                    $"[blue]{student.ParentPhone}[/]",
                                    $"[cyan]{student.ParentAddress}[/]"
                                );

                                ctx.UpdateTarget(table);
                                ctx.Refresh();
                          
                            }
                        });
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Không có học sinh nào có báo cáo vắng mặt trong khoảng thời gian từ {timeStart:yyyy-MM-dd} đến {timeEnd:yyyy-MM-dd}.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi khi lọc theo khoảng thời gian: {ex.Message}[/]");
            }
        }

    }
}