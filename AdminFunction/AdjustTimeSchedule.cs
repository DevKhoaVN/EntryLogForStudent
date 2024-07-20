using EntryManagement.Service;
using Quartz.Impl;
using Spectre.Console;
using System;

namespace EntryManagement.AdminFunction
{
    internal class AdjustTimeSchedule
    {
        private SchedulerService schedulerService;

        public AdjustTimeSchedule(SchedulerService service)
        {
            schedulerService = service;
        }

        public void AdjustTime()
        {
            int hour1 = GetValidHour("Nhập [green]giờ bạn muốn thay đổi (buổi sáng)[/]: ");
            int minutes1 = GetValidMinute("Nhập [green]phút bạn muốn thay đổi (buổi sáng)[/]: ");

            int hour2 = GetValidHour("Nhập [green]giờ bạn muốn thay đổi (buổi chiều)[/]: ");
            int minutes2 = GetValidMinute("Nhập [green]phút bạn muốn thay đổi (buổi chiều)[/]: ");

            // Dừng và khởi động lại scheduler với thời gian mới
             AdjustScheduler(hour1, minutes1, hour2, minutes2);
        }

        private int GetValidHour(string prompt)
        {
            int hour;
            while (true)
            {
                AnsiConsole.Markup(prompt);
                if (int.TryParse(Console.ReadLine(), out hour) && hour >= 0 && hour <= 23)
                {
                    break;
                }
                Console.WriteLine("Giờ không hợp lệ. Vui lòng nhập giá trị từ 0 đến 23.");
            }
            return hour;
        }

        private int GetValidMinute(string prompt)
        {
            int minute;
            while (true)
            {
                AnsiConsole.Markup(prompt);
                if (int.TryParse(Console.ReadLine(), out minute) && minute >= 0 && minute <= 59)
                {
                    break;
                }
                Console.WriteLine("Phút không hợp lệ. Vui lòng nhập giá trị từ 0 đến 59.");
            }
            return minute;
        }

        private async void AdjustScheduler(int hour1, int minutes1, int hour2, int minutes2)
        {
            // Dừng scheduler hiện tại
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Shutdown();

            // Khởi động lại scheduler với thời gian mới
            await schedulerService.StartScheduler(hour1, minutes1, hour2, minutes2);
        }
    }
}
