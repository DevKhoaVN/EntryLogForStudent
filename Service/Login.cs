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
                    StudentID = user.StudentId;

                    AnsiConsole.MarkupLine("[#00ff00]Bạn đã đăng nhập thành công![/]");
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

    }
}
