using System;
using System.Linq;
using System.Transactions;
using EntryManagement.Models;
using Spectre.Console;

namespace EntryManagement.Service
{
    public class Login
    {

        private readonly EntryLogManagementContext context;


        // khởi tạo database bằng hàm khởi tạo
        public Login(EntryLogManagementContext _context)
        {
            context = _context;
        }

        // chức năng xử lí đăng nhập
        public bool HandleLogin(out int userRole , out int? StudentID)
        {
            // thiết lập role mặc định
            userRole = 0;
            StudentID = 0;
           

            AnsiConsole.MarkupLine("[[[#DCA47C]Đăng nhập[/]]]");
            AnsiConsole.WriteLine();

            enterUserName:
            // Tào khoản đầu vào
            var userName = AnsiConsole.Ask<string>("Nhập [green]tài khoản: [/]");
            

            // Mật khẩu đầu vào
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Nhập [green]mật khẩu: [/]")
                    .PromptStyle("red")
                    .Secret());

            AnsiConsole.WriteLine();
            try
            {
                // truy vấn lấy lấy ra tài khoản mật khẩu của mộ user
                var user = context.Users.FirstOrDefault(x => x.UserName == userName);

                // Kiểm tra nếu user không null và password đúng
                if (user != null && user.Password == password)
                {
                    userRole = user.RoleId;
                    StudentID = user.StudentID;

                    AnsiConsole.MarkupLine("[red]Bạn đã đăng nhập thành công![/]");
                    AnsiConsole.WriteLine();
                   
                    return true;
                }
                // Kiểm tra nếu user không null và password sai
                else if (user != null && user.Password != password)
                {
                    AnsiConsole.MarkupLine("[red]Bạn nhập sai mật khẩu[/]");
                    AnsiConsole.WriteLine();
                    goto enterUserName;
                }
                // Trường hợp user null
                else
                {
                    AnsiConsole.MarkupLine("[red]Tài khoản không tồn tại[/]");
                    AnsiConsole.WriteLine();
                    goto enterUserName;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Lỗi đăng nhập vào hệ thống: [/] " + ex.Message);
                return false;
            }
        }

/*
        // Hàm xử lí đăng kí 
        public void HandleRegister()
        {

            AnsiConsole.MarkupLine("[#DCA47C]Đăng kí[/]");
            AnsiConsole.WriteLine();

            // dùng goto để người dùng nhập lại username
            EnterUserName:
            var userName = AnsiConsole.Ask<string>("Nhập [bold green]tên tài khoản[/] của bạn: ");

            // Kiểm tra xem username đã tồn tại chưa
            if (context.Users.Any(user => user.UserName == userName))
            {
                AnsiConsole.MarkupLine("[red]Tên tài khoản đã tồn tại. Vui lòng chọn tên khác.[/]");
                goto EnterUserName;
            }

            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Nhập [green] mật khẩu[/] của bạn: ")
                    .PromptStyle("#DD5353")
                    .Secret());

            var studentIdString = AnsiConsole.Ask<string>("Nhập ID con của bạn:");

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


                    AnsiConsole.MarkupLine("[#DCA47C]Đăng ký thành công![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[#DCA47C]]Học sinh không tồn tại trong hệ thống.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[#DCA47C]ID không hợp lệ. Vui lòng nhập đúng ID học sinh![/]");
                goto EnterUserName;
            }
        }*/



    }
}
