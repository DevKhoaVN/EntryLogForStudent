using Spectre.Console;

namespace EntryManagement.Menu.Admin
{
    internal class MenuAdmin2
    {
        public static int AdminEntryLogManagement()
        {

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn[[[yellow]Quản lí Admin/Quản lí ra vào[/]]]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Xem bảng học sinh ra vào",
                        "2. Xem báo cáo đi muộn",
                        "3. Điều chỉnh thời gian cảnh báo",
                        "0. Quay về trang trước đó"
                    }));

            // Mapping the selected option to an integer value
            int choice = choose switch
            {
                "1. Xem bảng học sinh ra vào" => 1,
                "2. Xem báo cáo đi muộn" => 2,
                "3. Điều chỉnh thời gian cảnh báo" => 3,
                "0. Quay về trang trước đó" => 0,
                _ => 0
            };

            return choice;
        }
    }
}
