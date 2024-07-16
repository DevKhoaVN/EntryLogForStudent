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

            DateTime morningStartTime = new DateTime(today.Year, today.Month, today.Day, 20, 0, 0);
            DateTime morningLateTime = morningStartTime.AddMinutes(59); // 7:30 AM

            DateTime afternoonStartTime = new DateTime(today.Year, today.Month, today.Day, 14, 0, 0);
            DateTime afternoonLateTime = afternoonStartTime.AddMinutes(30); // 2:30 PM


        }

        // 1.hiển thị thời gian đi muộn của học sinh trong ngày
        // 2. hiển thị theo id của học sinh
        // 3. hiển thị theo giai đoạn nhận 2 time


        // 1.hiển thị thời gian đi muộn của học sinh trong ngày
        public void FilterStudentLater()
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

            if(studentLate!= null)

            // Thêm dữ liệu vào bảng
            foreach (var student in studentLate)
            {
                table.AddRow(student.ID.ToString(), student.Name, student.Class, student.TimeLate.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            // Hiển thị bảng
            AnsiConsole.Write(table);

        }
        public void FilterByStudentId()
        {
            Console.WriteLine("Nhập vào ID của học sinh:");
            int id;

            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("ID không đúng định dạng!");
                Console.Write("Nhập lại ID học sinh: ");
            }
            var studentLog = context.EntryLogs.Where(e =>
                          ((e.LogTime > morningStartTime && e.LogTime <= morningLateTime) ||
                          (e.LogTime > afternoonStartTime && e.LogTime <= afternoonLateTime) & e.StudentId == id)
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
            }
            else
            {
                Console.WriteLine("Không tìm thấy bản ghi nào cho ID học sinh này.");
            }
        }

        public void FilterByRangeTime()
        {
            try
            {
                // Prompt user for start and end time
                Console.WriteLine("Nhập thời gian bắt đầu (yyyy-MM-dd HH:mm:ss): ");
                DateTime startTime;
                while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                {
                    Console.WriteLine("Thời gian không hợp lệ. Vui lòng nhập lại: ");
                }

                Console.WriteLine("Nhập thời gian kết thúc (yyyy-MM-dd HH:mm:ss): ");
                DateTime endTime;
                while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
                {
                    Console.WriteLine("Thời gian không hợp lệ. Vui lòng nhập lại: ");
                }

                // Query logs within the specified time range and time of day
                var studentLate = context.EntryLogs
                                  .Where(e => (e.LogTime >= startTime && e.LogTime <= endTime) &&
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

                // Display results in a table
                if (studentLate != null)
                {
                    var table = new Table().Expand();
                    table.Title($"Các bản ghi từ {startTime:yyyy-MM-dd HH:mm:ss} đến {endTime:yyyy-MM-dd HH:mm:ss} và trong khoảng giờ quy định");
                    table.AddColumn("ID");
                    table.AddColumn("Học sinh");
                    table.AddColumn("Lớp");
                    table.AddColumn("Thời gian");
                    table.AddColumn("Trạng thái");

                    foreach (var studentLog in studentLate)
                    {
                        table.AddRow($"{studentLog.ID}",
                           $"{studentLog.Name}",
                           $"{studentLog.Class}",
                           $"{studentLog.Status}",
                           $"{studentLog.TimeLate}");
                          
                    }

                    AnsiConsole.Render(table);
                }
                else
                {
                    Console.WriteLine($"Không có bản ghi nào trong khoảng thời gian từ {startTime:yyyy-MM-dd HH:mm:ss} đến {endTime:yyyy-MM-dd HH:mm:ss} và trong khoảng giờ quy định.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lọc theo khoảng thời gian và giờ: {ex.Message}");
            }
        }



    }
}
