using System.Globalization;
using System.Text;
using EntryManagement;
using EntryManagement.AdminFunction;
using EntryManagement.Menu;
using EntryManagement.Menu.Admin;
using EntryManagement.Menu.Parent;
using EntryManagement.Models;
using EntryManagement.ParentFunction;
using EntryManagement.Service;
using Spectre.Console;
namespace EntryManagement
{

    internal class Program
    {

        public static async Task Main(string[] args)
        {

           
            // Hiển thị tiếng việt
            Console.OutputEncoding = Encoding.UTF8;

            AnsiConsole.Write(new FigletText("EntryLogManagement").Color(Color.Aquamarine1_1).Centered());

            // Khởi tạo Dbcontext
            EntryLogManagementContext context = new EntryLogManagementContext();

            bool isCheckAll = true;
            while (isCheckAll)
            {
                

                switch (MainMenu.IndexMenu())
                {
                    // xử lí đăng kí tài khoản
                    case 1:
                        // khởi tạo và gọi phương thức đăng kí
                        Register register = new Register(context);
                        register.HandleRegister();

                        break;
                    // Xử lí đăng nhập
                    case 2:
                        //
                        // xác định vai trò 
                        int role = 0;
                        int? StudentID = 0;
                        Login login = new Login(context);

                        if (login.HandleLogin(out role , out StudentID))
                        {
                            switch (role)
                            {
                                //Thao tác của admin
                                case 1:
                                     
                                    bool isCheckAdmin = true;

                                    while (isCheckAdmin)
                                    {
                                        // Menucủa admin
                                        switch (MenuAdmin.AdminMenu())
                                        {
                                            // Thực hiện 1.Quản lí học sinh
                                            case 1:
                                                bool isCheckAdmin_1 = true;
                                                while (isCheckAdmin_1)
                                                {
                                                    switch (MenuAdmin1.AdminStudentManagement())
                                                    {
                                                        //  1.Quản lí học sinh -> 1.Thêm xửa xóa
                                                        case 1:

                                                            StudentManage studentManage = new StudentManage(context);

                                                            bool isCheckAdmin_1_1 = true;
                                                            while (isCheckAdmin_1_1)
                                                            {
                                                                switch (MenuAdmin1_1.AdminStudentManagement_1())
                                                                {
                                                                    // Thêm
                                                                    case 1:
                                                                        studentManage.AddStudent();
                                                                        break;

                                                                    // Xóa
                                                                    case 2:
                                                                        studentManage.DeleteStudent();
                                                                        break;

                                                                    // chỉnh
                                                                    case 3:
                                                                        studentManage.UpdateStudent();
                                                                        break;

                                                                    case 0:

                                                                        isCheckAdmin_1_1 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        // 1.Quản lí học sinh-> 2.xem báo cáo vắng học
                                                        case 2:

                                                            AbsentReportManage absentReportManage = new AbsentReportManage(context);
                                                            bool isCheckAdmin_1_2 = true;
                                                            while (isCheckAdmin_1_2)
                                                            {
                                                                switch (MenuAdmin1_2.AdminStudentManagement_2())
                                                                {
                                                                    // Học theo id học sinh
                                                                    case 1:
                                                                        await absentReportManage.DisplayReportById();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:

                                                                        Console.WriteLine("Nhập ngày bắt đầu (yyyy/MM/dd): ");
                                                                        DateTime startDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }

                                                                        Console.WriteLine("Nhập ngày kết thúc (yyyy/MM/dd): ");
                                                                        DateTime endDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }

                                                                        await absentReportManage.DisplayReportsByTime(startDate, endDate);
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:
                                                                        await absentReportManage.DisplayAllReports();
                                                                        break;

                                                                    case 0:

                                                                        isCheckAdmin_1_2 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        // 1.Quản lí học sinh -> 3.Xem cảnh báo
                                                        case 3:

                                                            AlertManage alertManage = new AlertManage(context);
                                                            bool isCheckAdmin_1_3 = true;
                                                            while (isCheckAdmin_1_3)
                                                            {
                                                                switch (MenuAdmin1_3.AdminStudentManagement_3())
                                                                {
                                                                    // Học theo id học sinh
                                                                    case 1:

                                                                        alertManage.FindAlertsByStudentId();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:

                                                                        Console.WriteLine("Nhập ngày bắt đầu (yyyy/MM/dd): ");
                                                                        DateTime startDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }

                                                                        Console.WriteLine("Nhập ngày kết thúc (yyyy/MM/dd): ");
                                                                        DateTime endDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }

                                                                        alertManage.FilterAlertsByTimeRange(startDate, endDate);
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:
                                                                        alertManage.DisplayAllAlerts();
                                                                        break;

                                                                    case 0:

                                                                        isCheckAdmin_1_3 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        //1.Quản lí học sinh -> 4.Xem thông tin học sinh
                                                        case 4:
                                                            StudentInformationManage studentInformationManage = new StudentInformationManage(context);
                                                            bool isCheckAdmin_1_4 = true;
                                                            while (isCheckAdmin_1_4)
                                                            {
                                                                switch (MenuAdmin1_4.AdminStudentManagement_4())
                                                                {
                                                                    // Học theo id học sinh
                                                                    case 1:

                                                                        studentInformationManage.FilterByStudentId();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:

                                                                        Console.WriteLine("Nhập ngày bắt đầu (yyyy/MM/dd): ");
                                                                        DateTime startDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }

                                                                        Console.WriteLine("Nhập ngày kết thúc (yyyy/MM/dd): ");
                                                                        DateTime endDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }
                                                                        studentInformationManage.FilterByTimeRange(startDate, endDate);
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:
                                                                        studentInformationManage.DisplayStudentsWithParentsInfo();
                                                                        break;

                                                                    case 0:

                                                                        isCheckAdmin_1_4 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }
                                                            break;

                                                        case 0:

                                                            isCheckAdmin_1 = false;
                                                            break;

                                                        default:
                                                            Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                            break;
                                                    }
                                                }

                                                break;

                                            // Thực hiện 2.Quản lí ra vào --------------------------------
                                            case 2:

                                                bool isCheckEntry = true;
                                                while (isCheckEntry)
                                                {
                                                    switch (MenuAdmin2.AdminEntryLogManagement())
                                                    {
                                                        // 2.Quản lí ra vào -> 1.Xem bảng học sinh ra vào
                                                        case 1:
                                                            EntryManage entryManage = new EntryManage(context);
                                                            bool isCheckEntry_1 = true;
                                                            while (isCheckEntry_1)
                                                            {
                                                                switch (AdminMenu2_1.AdminEntryLogManagement_1())
                                                                {
                                                                    // Học theo id học sinh
                                                                    case 1:

                                                                        entryManage.DisplayEntryLogsByStudentId();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:

                                                                        Console.WriteLine("Nhập ngày bắt đầu (yyyy/MM/dd): ");
                                                                        DateTime startDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }

                                                                        Console.WriteLine("Nhập ngày kết thúc (yyyy/MM/dd): ");
                                                                        DateTime endDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }
                                                                        entryManage.DisplayEntryLogsByTimeRange(startDate, endDate);
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:

                                                                      await entryManage.DisplayAllEntryLogs();
                                                                        break;

                                                                    case 0:

                                                                        isCheckEntry_1 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }

                                                            break;

                                                        // 2.Quản lí ra vào -> 2.Xem báo cáo đi muộn
                                                        case 2:

                                                            EntryLaterManage entryLaterManage = new EntryLaterManage(context);
                                                            bool isCheckEntry_2 = true;
                                                            while (isCheckEntry_2)
                                                            {
                                                                switch (MenuAdmin2_2.AdminEntryLogManagement_2())
                                                                {
                                                                    // Học theo id học sinh
                                                                    case 1:
                                                                        entryLaterManage.FilterByStudentId();
                                                                        break;

                                                                    // Lọc theo thời gian
                                                                    case 2:

                                                                        Console.WriteLine("Nhập ngày bắt đầu (yyyy/MM/dd): ");
                                                                        DateTime startDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }

                                                                        Console.WriteLine("Nhập ngày kết thúc (yyyy/MM/dd): ");
                                                                        DateTime endDate;
                                                                        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                                                                        {
                                                                            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                                        }

                                                                      
                                                                        break;

                                                                    // Hiển thị tất cả
                                                                    case 3:
                                                                     
                                                                       

                                                                        break;

                                                                    case 0:

                                                                        isCheckEntry_2 = false;
                                                                        break;

                                                                    default:
                                                                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                                        break;
                                                                }
                                                            }

                                                            break;

                                                        //2.Quản lí ra vào -> 3.Điều chỉnh thời gian cảnh báo 
                                                        case 3:

                                                            int hour1, miute1, hour2, miute2;

                                                            AdjustTimeSchedule adjustTimeSchedule = new AdjustTimeSchedule();
                                                            adjustTimeSchedule.AdjustTime(out hour1, out miute1, out hour2, out miute2);

                                                            SchedulerService service = new SchedulerService();
                                                            await service.StartScheduler(hour1, miute1, hour2, miute2);

                                                            break;
                                                       

                                                        default:
                                                            Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                            break;

                                                    }

                                                }
                                                break;

                                            // Thực hiện 3.Thực hiện kiểm tra ra vào ( con , cha) -----------------------
                                            case 3:

                                                Camera camera = new Camera();
                                                camera.TurnOn();
                                                break;

                                            case 0:

                                                return;
                                              
                                        }
                                        break;
                                    }
                                    break;

                                // Thao tác của parent
                                case 2:

                                    bool isCheckParent = true;
                                    while (isCheckParent)
                                    {
                                        switch (MenuParent.ParentMenu())
                                        {
                                            // 1. Xem thông tin ra vào
                                            case 1:

                                                EntryLogStudentOfParent entryLogStudentOfParent = new EntryLogStudentOfParent(context, StudentID);

                                                bool isCheckParent1 = true;
                                                while (isCheckParent1)
                                                {
                                                    switch (ParentMenu1.ParentEntryLog())
                                                    {
                                                        // Lọc theo thời gian
                                                        case 1:

                                                            Console.WriteLine("Nhập ngày bắt đầu (yyyy/MM/dd): ");
                                                            DateTime startDate;
                                                            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                                                            {
                                                                Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                            }

                                                            Console.WriteLine("Nhập ngày kết thúc (yyyy/MM/dd): ");
                                                            DateTime endDate;
                                                            while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                                                            {
                                                                Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại (yyyy/MM/dd): ");
                                                            }
                                                            entryLogStudentOfParent.DisplayEntryLogsByTimeRange(startDate, endDate);
                                                            break;

                                                        // Hiển thị tất cả
                                                        case 2:
                                                            entryLogStudentOfParent.DisplayAllEntryLogs();
                                                            break;

                                                        case 0:

                                                            isCheckParent1 = false;
                                                            break;

                                                        default:
                                                            Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                            break;
                                                    }
                                                }
                                                break;

                                            // 2. Xem báo cáo vắng học
                                            case 2:

                                                AbsentReportOfParent absentReportOfParent = new AbsentReportOfParent(context , StudentID);

                                                bool isCheckParent2 = true;
                                                while (isCheckParent2)
                                                {
                                                    switch (ParentMenu2.ParentAbsentReport())
                                                    {
                                                        // Gửi báo cáo vắng học
                                                        case 1:

                                                            absentReportOfParent.SendReport();
                                                            break;

                                                        // Hiển thị tất cả
                                                        case 2:
                                                            absentReportOfParent.DisplayReport();
                                                            break;

                                                        case 0:

                                                            isCheckParent2 = false;
                                                            break;

                                                        default:
                                                            Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                            break;
                                                    }
                                                }
                                                break;

                                            // Hiển thị tất cả


                                            case 0:

                                                return;
                                            

                                            default:
                                                Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                                                break;
                                        }
                                    }

                                    break;
                            }

                        }

                        break;

                    case 0:

                        isCheckAll = false;
                        break;

                    default:

                        Console.WriteLine("Vui lòng nhập đúng lựa chọn!");
                        break;

                }
            }


        }
    }
}
// "Data Source=DESKTOP-Q51CKKR\\SQLEXPRESS01;Initial Catalog=EntryLogManagement;Integrated Security=True;Trust Server Certificate=True"


// dotnet ef dbcontext scaffold -o Models -f -d "Data Source=DESKTOP-Q51CKKR\SQLEXPRESS01;Initial Catalog=EntryLogManagement;Integrated Security=True;Trust Server Certificate=True" "Microsoft.EntityFrameworkCore.SqlServer"