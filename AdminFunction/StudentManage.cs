using System.Globalization;
using EntryManagement.Models;
using Spectre.Console;

namespace EntryManagement.AdminFunction
{
    internal class StudentManage
    {
        private readonly EntryLogManagementContext context;

        public StudentManage(EntryLogManagementContext _context)
        {
            context = _context;
        }

        public Parent EnterParent()
        {
            Parent parent = new Parent();

            parent.Name = AnsiConsole.Ask<string>("Nhập tên phụ huynh: ");

            parent.Phone = AnsiConsole.Prompt(
                new TextPrompt<int>("Nhập số điện thoại phụ huynh: ")
                    .Validate(phone => phone > 0 ? ValidationResult.Success() : ValidationResult.Error("Số điện thoại không hợp lệ.")));

            parent.Email = AnsiConsole.Prompt(
                new TextPrompt<string>("Nhập Email phụ huynh: ")
                    .Validate(email => email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("Email không hợp lệ. Vui lòng nhập lại (phải là @gmail.com).")));

            parent.Address = AnsiConsole.Ask<string>("Nhập địa chỉ phụ huynh: ");

            return parent;
        }

        public Student EnterStudent()
        {
            Student student = new Student();

            student.Name = AnsiConsole.Ask<string>("Nhập tên học sinh: ");

            student.Gender = AnsiConsole.Ask<string>("Nhập giới tính học sinh: ");

            student.DayOfBirth = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Nhập ngày sinh học sinh (dd/MM/yyyy): ")
                    .PromptStyle("green")
                    .Validate(dob => DateTime.TryParseExact(dob.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null, DateTimeStyles.None, out _)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("Ngày sinh không hợp lệ. Vui lòng nhập lại (dd/MM/yyyy).")));

            student.Class = AnsiConsole.Ask<string>("Nhập lớp học sinh: ");

            student.Address = AnsiConsole.Ask<string>("Nhập địa chỉ học sinh: ");

            student.Phone = AnsiConsole.Prompt(
                new TextPrompt<int>("Nhập số điện thoại học sinh: ")
                    .Validate(phone => phone > 0 ? ValidationResult.Success() : ValidationResult.Error("Số điện thoại không hợp lệ.")));

            return student;
        }

        public void AddStudent()
        {
            try
            {
                var parent = EnterParent();
                context.Parents.Add(parent);
                context.SaveChanges();

                var student = EnterStudent();
                student.ParentId = parent.ParentId;
                context.Students.Add(student);
                context.SaveChanges();

                AnsiConsole.MarkupLine("[green]Thêm học sinh thành công![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi thêm học sinh: {ex.Message}[/]");
            }
        }

        public void DeleteStudent()
        {
            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("Nhập ID học sinh muốn xóa: ")
                    .Validate(id => id > 0 ? ValidationResult.Success() : ValidationResult.Error("ID không hợp lệ. Vui lòng nhập lại.")));

            try
            {
                var student = context.Students.Find(id);
                if (student != null)
                {
                    var parent = context.Parents.Find(student.ParentId);
                    if (parent != null)
                    {
                        context.Parents.Remove(parent);
                    }
                    context.Students.Remove(student);
                    context.SaveChanges();
                    AnsiConsole.MarkupLine("[green]Đã xóa học sinh và phụ huynh thành công.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Không tìm thấy học sinh với ID đã cho.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi xóa học sinh: {ex.Message}[/]");
            }
        }

        public void UpdateStudent()
        {
            int id = AnsiConsole.Prompt(
                new TextPrompt<int>("Nhập ID học sinh muốn chỉnh sửa: ")
                    .Validate(id => id > 0 ? ValidationResult.Success() : ValidationResult.Error("ID không hợp lệ. Vui lòng nhập lại.")));

            try
            {
                var student = context.Students.Find(id);
                if (student != null)
                {
                    var parent = context.Parents.Find(student.ParentId);
                    if (parent != null)
                    {
                        UpdateParent(parent); // Gọi phương thức UpdateParent để cập nhật thông tin phụ huynh
                    }

                    UpdateStudentInfo(student); // Gọi phương thức UpdateStudentInfo để cập nhật thông tin học sinh

                    context.SaveChanges();
                    AnsiConsole.MarkupLine("[green]Đã cập nhật thông tin học sinh thành công.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Không tìm thấy học sinh với ID đã cho.[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Lỗi update học sinh: {ex.Message}[/]");
            }
        }

        private void UpdateParent(Parent parent)
        {
            string newName = AnsiConsole.Ask<string>("Nhập tên phụ huynh mới (bỏ trống nếu không muốn thay đổi): ", parent.Name);
            if (!string.IsNullOrEmpty(newName)) parent.Name = newName;

            string newPhoneString = AnsiConsole.Ask<string>("Nhập số điện thoại phụ huynh mới (bỏ trống nếu không muốn thay đổi): ", parent.Phone.ToString());
            if (!string.IsNullOrEmpty(newPhoneString) && int.TryParse(newPhoneString, out int newPhone) && newPhone > 0) parent.Phone = newPhone;

            string newEmail = AnsiConsole.Ask<string>("Nhập Email phụ huynh mới (bỏ trống nếu không muốn thay đổi, phải là @gmail.com): ", parent.Email);
            if (!string.IsNullOrEmpty(newEmail) && newEmail.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)) parent.Email = newEmail;

            string newAddress = AnsiConsole.Ask<string>("Nhập địa chỉ phụ huynh mới (bỏ trống nếu không muốn thay đổi): ", parent.Address);
            if (!string.IsNullOrEmpty(newAddress)) parent.Address = newAddress;
        }

        private void UpdateStudentInfo(Student student)
        {
            string newName = AnsiConsole.Ask<string>("Nhập tên học sinh mới (bỏ trống nếu không muốn thay đổi): ", student.Name);
            if (!string.IsNullOrEmpty(newName)) student.Name = newName;

            string newGender = AnsiConsole.Ask<string>("Nhập giới tính học sinh mới (bỏ trống nếu không muốn thay đổi): ", student.Gender);
            if (!string.IsNullOrEmpty(newGender)) student.Gender = newGender;

            string newDobString = AnsiConsole.Ask<string>("Nhập ngày sinh học sinh mới (dd/MM/yyyy, bỏ trống nếu không muốn thay đổi): ", student.DayOfBirth.ToString("dd/MM/yyyy"));
            if (!string.IsNullOrEmpty(newDobString) && DateTime.TryParseExact(newDobString, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime newDob)) student.DayOfBirth = newDob;

            string newClass = AnsiConsole.Ask<string>("Nhập lớp học sinh mới (bỏ trống nếu không muốn thay đổi): ", student.Class);
            if (!string.IsNullOrEmpty(newClass)) student.Class = newClass;

            string newAddress = AnsiConsole.Ask<string>("Nhập địa chỉ học sinh mới (bỏ trống nếu không muốn thay đổi): ", student.Address);
            if (!string.IsNullOrEmpty(newAddress)) student.Address = newAddress;

            string newPhoneString = AnsiConsole.Ask<string>("Nhập số điện thoại học sinh mới (bỏ trống nếu không muốn thay đổi): ", student.Phone.ToString());
            if (!string.IsNullOrEmpty(newPhoneString) && int.TryParse(newPhoneString, out int newPhone) && newPhone > 0) student.Phone = newPhone;
        }
    }
}
