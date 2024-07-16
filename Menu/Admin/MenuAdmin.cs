using Spectre.Console;

namespace EntryManagement.Menu.Admin
{
    internal class MenuAdmin
    {
        public static int AdminMenu()
        {
           

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn [[[yellow]Quản lí Admin [/]]]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Quản lí học sinh",
                        "2. Quản lí ra vào",
                        "3. Thực hiện kiểm tra ra vào",
                        "0. Thoát"
                    }));

            // Mapping the selected option to an integer value
            int choice = choose switch
            {
                "1. Quản lí học sinh" => 1,
                "2. Quản lí ra vào" => 2,
                "3. Thực hiện kiểm tra ra vào" => 3,
                "0. Quay lại trang trước đó" => 0,
                _ => 0
            };

            return choice;
        }
    }
}
