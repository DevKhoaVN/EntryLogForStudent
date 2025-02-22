using Spectre.Console;

namespace EntryManagement.Menu
{
    internal class MenuParent
    {
        public static int ParentMenu()
        {


            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn[[[yellow]MenuParent[/]]]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Xem thông tin ra vào của học sinh",
                        "2. Báo cáo vắng học",
                        "3. Thoát"
                    }));

            // Mapping the selected option to an integer value
            int choice = choose switch
            {
                "1. Xem thông tin ra vào của học sinh" => 1,
                "2. Báo cáo vắng học" => 2,
                _ => 0
            };

            return choice;
        }

        public static int ParentEntryLog()
        {
           
            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn[[[yellow]MenuParent/thông tin học sinh ra vào[/]]]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Lọc theo thời gian",
                        "2. Hiển thị tất cả",
                        "0. Quay về trang trước đó"
                    }));

            // Mapping the selected option to an integer value
            int choice = choose switch
            {
                "1. Lọc theo thời gian" => 1,
                "2. Hiển thị tất cả" => 2,
                "0. Quay về trang trước đó" => 0,
                _ => 0
            };

            return choice;
        }

        public static int ParentAbsentReport()
        {
         

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn[[[yellow]MenuParent/báo cáo ra học sinh ra vào[/]]]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Báo cáo vắng học",
                        "2. Xem báo cáo vắng học",
                        "0. Quay về trang trước đó"
                    }));

            // Mapping the selected option to an integer value
            int choice = choose switch
            {
                "1. Báo cáo vắng học" => 1,
                "2. Xem báo cáo vắng học" => 2,
                "0. Quay về trang trước đó" => 0,
                _ => 0
            };

            return choice;
        }
    }
}
