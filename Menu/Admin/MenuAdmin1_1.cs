using Spectre.Console;

namespace EntryManagement.Menu.Admin
{
    internal class MenuAdmin1_1
    {
        public static int AdminStudentManagement_1()
        {

            var choose = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn một tùy chọn[[[yellow]Quản lí Admin/Quản lí học sinh/Thêm, sửa, xóa học sinh[/]]]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Thêm học sinh",
                        "2. Xóa học sinh",
                        "3. Chỉnh sửa thông tin học sinh",
                        "0. Quay về trang trước đó"
                    }));

            // Mapping the selected option to an integer value
            int choice = choose switch
            {
                "1. Thêm học sinh" => 1,
                "2. Xóa học sinh" => 2,
                "3. Chỉnh sửa thông tin học sinh" => 3,
                "0. Quay về trang trước đó" => 0,
                _ => 0
            };

            return choice;
        }
    }
}
