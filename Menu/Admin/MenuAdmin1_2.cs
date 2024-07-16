using Spectre.Console;

namespace EntryManagement
{
    internal class MenuAdmin1_2
    {
        public static int AdminStudentManagement_2()
        {
           

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn[[[yellow]Quản lí Admin/Quản lí học sinh/Báo cáo vắng học[/]]]")
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
