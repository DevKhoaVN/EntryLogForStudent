using Spectre.Console;

namespace EntryManagement.Menu.Parent
{
    internal class ParentMenu1
    {
        public static int ParentEntryLog()
        {
            AnsiConsole.Write(new Rule("[yellow]Chào mừng đến với bảng xem ra vào học sinh[/]"));

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn")
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
    }
}
