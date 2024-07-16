using Spectre.Console;

namespace EntryManagement.Menu.Parent
{
    internal class ParentMenu2
    {
        public static int ParentAbsentReport()
        {
            AnsiConsole.Write(new Rule("[yellow]Chào mừng đến với bảng báo cáo vắng học[/]"));

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn")
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
