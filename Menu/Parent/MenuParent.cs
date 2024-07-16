using Spectre.Console;

namespace EntryManagement.Menu.Parent
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
    }
}
