using Spectre.Console;

namespace EntryManagement.Menu.Admin
{
    internal class MenuAdmin1
    {
        public static int AdminStudentManagement()
        {

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn[[[yellow]Quản lí Admin/Quản lí học sinh[/]]]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Thêm, sửa, xóa học sinh",
                        "2. Xem báo cáo vắng học",
                        "3. Xem cảnh báo",
                        "4. Xem thông tin học sinh",
                        "0. Quay về trang trước đó"
                    }));

            // Mapping the selected option to an integer value
            int choice = choose switch
            {
                "1. Thêm, sửa, xóa học sinh" => 1,
                "2. Xem báo cáo vắng học" => 2,
                "3. Xem cảnh báo" => 3,
                "4. Xem thông tin học sinh" => 4,
                "0. Quay về trang trước đó" => 0,
                _ => 0
            };

            return choice;
        }
    }
}
