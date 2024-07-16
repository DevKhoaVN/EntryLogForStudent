using System;
using System.Threading.Tasks;

namespace EntryManagement.AdminFunction
{
    internal class AdjustTimeSchedule
    {
        public  void AdjustTime(out int hour1, out int minutes1, out int hour2, out int minutes2)
        {
            Console.Write("Nhập giờ bạn muốn thay đổi : (buổi sáng) ");
            hour1 = int.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Nhập phút bạn muốn thay đổi : (buổi sáng) ");
            minutes1 = int.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Nhập giờ bạn muốn thay đổi : (buổi chiều) ");
            hour2 = int.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.Write("Nhập phút bạn muốn thay đổi : (buổi chiều) ");
            minutes2 = int.Parse(Console.ReadLine());
            Console.WriteLine();


        }
    }
}
