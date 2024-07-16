using Spectre.Console;

namespace EntryManagement.Menu.Admin
{
    internal class AdminMenu2_1
    {
        public static int AdminEntryLogManagement_1()
        {

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn[[[yellow]Quản lí Admin/Quản lí ra vào/Học sinh ra vào[/]]]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Lọc theo id học sinh",
                        "2. Lọc theo thời gian",
                        "3. Hiển thị tất cả",
                        "0. Quay về trang trước đó"
                    }));

            // Mapping the selected option to an integer value
            int choice = choose switch
            {
                "1. Lọc theo id học sinh" => 1,
                "2. Lọc theo thời gian" => 2,
                "3. Hiển thị tất cả" => 3,
                "0. Quay về trang trước đó" => 0,
                _ => 0
            };

            return choice;
        }
    }
}
