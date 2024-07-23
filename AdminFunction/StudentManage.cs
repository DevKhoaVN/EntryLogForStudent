using System;
using System.Globalization;
using System.Text.RegularExpressions;
using EntryManagement.Models; // Import các namespace cần thiết
using Spectre.Console;

namespace EntryManagement.AdminFunction
{
    internal class StudentManage
    {
        private readonly EntryLogManagementContext context; // DbContext để quản lý dữ liệu

        public StudentManage(EntryLogManagementContext _context)
        {
            context = _context; // Inject context vào constructor
        }

        // Phương thức nhập thông tin của phụ huynh từ người dùng
        public Parent EnterParent()
        {
            Parent parent = new Parent(); // Tạo đối tượng Parent mới

            parent.Name = AnsiConsole.Ask<string>("Nhập [green]tên phụ huynh[/]: "); // Nhập tên từ người dùng

            // Nhập số điện thoại từ người dùng và validate nó phải lớn hơn 0
            parent.Phone = AnsiConsole.Prompt(
               new TextPrompt<string>("Nhập [green]số điện thoại học sinh[/]: ")
          .Validate(phone =>
          {
              // Define a regex pattern for validating phone numbers
              string pattern = @"^[1-9]\d{8,12}$"; // Adjust the pattern as needed
              return Regex.IsMatch(phone, pattern) ?
                  ValidationResult.Success() :
                  ValidationResult.Error("[red]Số điện thoại không hợp lệ. Vui lòng nhập lại.[/]");
          }));


            // Nhập Email từ người dùng và validate là phải kết thúc bằng @gmail.com
            parent.Email = AnsiConsole.Prompt(
                new TextPrompt<string>("Nhập Email phụ huynh: ")
                    .Validate(email => email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Email không hợp lệ. Vui lòng nhập lại (phải là @gmail.com)[/].")));

            parent.Address = AnsiConsole.Ask<string>("Nhập [green]địa chỉ phụ huynh[/]: "); // Nhập địa chỉ từ người dùng

            return parent; // Trả về đối tượng Parent đã nhập
        }

        // Phương thức nhập thông tin của học sinh từ người dùng
        public Student EnterStudent()
        {
            Student student = new Student(); // Tạo đối tượng Student mới

            student.Name = AnsiConsole.Ask<string>("Nhập [green]tên học sinh[/]: "); // Nhập tên từ người dùng

            student.Gender = AnsiConsole.Ask<string>("Nhập [green]giới tính học sinh[/]: "); // Nhập giới tính từ người dùng

            // Nhập ngày sinh từ người dùng và validate định dạng dd/MM/yyyy
            student.DayOfBirth = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Nhập ngày sinh học sinh (dd/MM/yyyy): ")
                    .PromptStyle("green")
                    .Validate(dob => DateTime.TryParseExact(dob.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null, DateTimeStyles.None, out _)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Ngày sinh không hợp lệ. Vui lòng nhập lại (dd/MM/yyyy)[/].")));

            student.Class = AnsiConsole.Ask<string>("Nhập[green] lớp học sinh[/]: "); // Nhập lớp học từ người dùng

            student.Address = AnsiConsole.Ask<string>("[green]Nhập địa chỉ học sinh[/]: "); // Nhập địa chỉ từ người dùng

            // Nhập số điện thoại từ người dùng và validate nó phải lớn hơn 0
            student.Phone = AnsiConsole.Prompt(
                new TextPrompt<string>("Nhập [green]số điện thoại học sinh[/]: ")
                    .Validate(phone =>
                    {
                        // Define a regex pattern for validating phone numbers
                        string pattern = @"^[1-9]\d{8,12}$"; // Adjust the pattern as needed
                        return Regex.IsMatch(phone, pattern) ?
                            ValidationResult.Success() :
                            ValidationResult.Error("[red]Số điện thoại không hợp lệ[/]");
                    }));


            return student; // Trả về đối tượng Student đã nhập
        }

        // Phương thức thêm học sinh và phụ huynh mới vào cơ sở dữ liệu
        public void AddStudent()
        {
            try
            {
                var parent = EnterParent(); // Gọi phương thức nhập thông tin phụ huynh
                context.Parents.Add(parent); // Thêm phụ huynh vào DbContext
                context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                var student = EnterStudent(); // Gọi phương thức nhập thông tin học sinh
                student.ParentId = parent.ParentId; // Gán ParentId cho học sinh từ phụ huynh tương ứng
                context.Students.Add(student); // Thêm học sinh vào DbContext
                context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                AnsiConsole.MarkupLine("[#00ff00]Thêm học sinh thành công![/]"); // Thông báo thành công
                AnsiConsole.WriteLine();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi thêm học sinh: {ex.Message}[/]"); // Thông báo lỗi nếu có
                AnsiConsole.WriteLine();

            }
        }

        // Phương thức xóa học sinh từ cơ sở dữ liệu
        public void DeleteStudent()
        {
            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("Nhập ID học sinh muốn xóa: ")
                    .Validate(id => id > 0 ? ValidationResult.Success() : ValidationResult.Error("ID không hợp lệ. Vui lòng nhập lại.")));

            try
            {
                var student = context.Students.Find(id); // Tìm học sinh trong cơ sở dữ liệu bằng ID
                if (student != null)
                {
                    var parent = context.Parents.Find(student.ParentId); // Tìm phụ huynh tương ứng
                    if (parent != null)
                    {
                        context.Parents.Remove(parent); // Xóa phụ huynh
                    }
                    context.Students.Remove(student); // Xóa học sinh
                    context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    AnsiConsole.MarkupLine("[#00ff00]Đã xóa học sinh và phụ huynh thành công.[/]"); // Thông báo thành công
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Không tìm thấy học sinh với ID đã cho.[/]"); // Thông báo không tìm thấy học sinh
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi xóa học sinh: {ex.Message}[/]"); // Thông báo lỗi nếu có
                AnsiConsole.WriteLine();
            }
        }

        // Phương thức cập nhật thông tin học sinh
        public void UpdateStudent()
        {
            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("Nhập ID học sinh muốn chỉnh sửa: ")
                    .Validate(id => id > 0 ? ValidationResult.Success() : ValidationResult.Error("ID không hợp lệ. Vui lòng nhập lại.")));

            try
            {
                var student = context.Students.Find(id); // Tìm học sinh trong cơ sở dữ liệu bằng ID
                if (student != null)
                {
                    var parent = context.Parents.Find(student.ParentId); // Tìm phụ huynh tương ứng
                    if (parent != null)
                    {
                        UpdateParent(parent); // Gọi phương thức cập nhật thông tin phụ huynh
                    }

                    UpdateStudentInfo(student); // Gọi phương thức cập nhật thông tin học sinh

                    context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    AnsiConsole.MarkupLine("[#00ff00]Đã cập nhật thông tin học sinh thành công.[/]"); // Thông báo thành công
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Không tìm thấy học sinh với ID đã cho.[/]"); // Thông báo không tìm thấy học sinh
                    AnsiConsole.WriteLine();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi update học sinh: {ex.Message}[/]"); // Thông báo lỗi nếu có
                AnsiConsole.WriteLine();
            }
        }

        // Phương thức cập nhật thông tin phụ huynh
        private void UpdateParent(Parent parent)
        {
            // Nhập tên mới cho phụ huynh (nếu muốn thay đổi)
            string newName = AnsiConsole.Ask<string>("Nhập [green]tên phụ huynh mới (bỏ trống nếu không muốn thay đổi)[/]: ", parent.Name);
            if (!string.IsNullOrEmpty(newName)) parent.Name = newName;

            // Nhập số điện thoại mới cho phụ huynh (nếu muốn thay đổi)
            string newPhoneString = AnsiConsole.Ask<string>("Nhập [green]số điện thoại phụ huynh mới (bỏ trống nếu không muốn thay đổi)[/]: ", parent.Phone.ToString());
          

            // Nhập Email mới cho phụ huynh (nếu muốn thay đổi)
            string newEmail = AnsiConsole.Ask<string>("Nhập[green] Email phụ huynh mới (bỏ trống nếu không muốn thay đổi, phải là @gmail.com)[/]: ", parent.Email);
            if (!string.IsNullOrEmpty(newEmail) && newEmail.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)) parent.Email = newEmail;

            // Nhập địa chỉ mới cho phụ huynh (nếu muốn thay đổi)
            string newAddress = AnsiConsole.Ask<string>("Nhập [green]địa chỉ phụ huynh mới (bỏ trống nếu không muốn thay đổi)[/]: ", parent.Address);
            if (!string.IsNullOrEmpty(newAddress)) parent.Address = newAddress;
        }

        // Phương thức cập nhật thông tin học sinh
        private void UpdateStudentInfo(Student student)
        {
            // Nhập tên mới cho học sinh (nếu muốn thay đổi)
            string newName = AnsiConsole.Ask<string>("Nhập [green]tên học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Name);
            if (!string.IsNullOrEmpty(newName)) student.Name = newName;

            // Nhập giới tính mới cho học sinh (nếu muốn thay đổi)
            string newGender = AnsiConsole.Ask<string>("Nhập [green]giới tính học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Gender);
            if (!string.IsNullOrEmpty(newGender)) student.Gender = newGender;

            // Nhập ngày sinh mới cho học sinh (nếu muốn thay đổi)
            string newDobString = AnsiConsole.Ask<string>("Nhập [green]ngày sinh học sinh mới (dd/MM/yyyy, bỏ trống nếu không muốn thay đổi)[/]: ", student.DayOfBirth.ToString("dd/MM/yyyy"));
            if (!string.IsNullOrEmpty(newDobString) && DateTime.TryParseExact(newDobString, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime newDob)) student.DayOfBirth = newDob;

            // Nhập lớp mới cho học sinh (nếu muốn thay đổi)
            string newClass = AnsiConsole.Ask<string>("Nhập [green]lớp học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Class);
            if (!string.IsNullOrEmpty(newClass)) student.Class = newClass;

            // Nhập địa chỉ mới cho học sinh (nếu muốn thay đổi)
            string newAddress = AnsiConsole.Ask<string>("Nhập [green[]địa chỉ học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Address);
            if (!string.IsNullOrEmpty(newAddress)) student.Address = newAddress;


            // Nhập số điện thoại mới cho học sinh (nếu muốn thay đổi)
            string newPhoneString = AnsiConsole.Ask<string>("Nhập [green]số điện thoại học sinh mới (bỏ trống nếu không muốn thay đổi)[/]: ", student.Phone.ToString());
         
        }
    }
}
