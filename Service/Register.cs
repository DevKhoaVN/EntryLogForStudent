using EntryManagement.Models;
using Spectre.Console;

namespace EntryManagement.Service
{
    public class Register
    {
        private readonly EntryLogManagementContext context;

        // khởi tạo database bằng hàm khởi tạo
        public Register(EntryLogManagementContext _context)
        {
            context = _context;
        }

        public void HandleRegister()
        {

            AnsiConsole.MarkupLine("[[[#DCA47C]Đăng kí[/]]]");
            AnsiConsole.WriteLine();

        // dùng goto để người dùng nhập lại username
        EnterUserName:
            var userName = AnsiConsole.Ask<string>("Nhập [green]tên tài khoản[/] của bạn: ");

            // Kiểm tra xem username đã tồn tại chưa
            if (context.Users.Any(user => user.UserName == userName))
            {
                AnsiConsole.MarkupLine("[red]Tên tài khoản đã tồn tại. Vui lòng chọn tên khác.[/]");
                AnsiConsole.WriteLine();
                goto EnterUserName;
            }

            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Nhập [green] mật khẩu[/] của bạn: ")
                    .PromptStyle("#DD5353")
                    .Secret());

            
            var studentIdString = AnsiConsole.Ask<string>("Nhập [green] ID[/] học sinh:");

            // Kiểm tra id nhập vào
            if (int.TryParse(studentIdString, out int studentId))
            {
                // Kiểm tra xem ID học sinh có tồn tại trong cơ sở dữ liệu hay không
                var student = context.Students.FirstOrDefault(x => x.StudentId == studentId);

                if (student != null)
                {
                    // Tạo người dùng mới
                    var newUser = new User
                    {
                        UserName = userName,
                        Password = password,
                        RoleId = 2, // Đặt một UserRoleId mặc định , (  tài khoản Admin do lập trình viên cấp)
                    };

                    // Thêm và lưu vào database
                    context.Users.Add(newUser);
                    context.SaveChanges();


                    AnsiConsole.MarkupLine("[red]Đăng ký thành công![/]");
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Học sinh không tồn tại trong hệ thống.[/]");
                    AnsiConsole.WriteLine();
                    goto EnterUserName;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]ID không hợp lệ. Vui lòng nhập đúng ID học sinh![/]");
                AnsiConsole.WriteLine();
                goto EnterUserName;
            }
        }

        }
}
