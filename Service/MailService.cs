using System.Net.Mail;
using System.Net;

public class MailService
{
    private string _fromMail = "khoavanle2@gmail.com";
    private string _password = "jqbt ywhe tmjl xsgm"; // Ensure this is your app password or actual password
    private string Toemail = "khoavanle3@gmail.com";

    public void SendEmail(string StudentName)
    {
        using (var _mail = new MailMessage())
        {
            try
            {
                _mail.From = new MailAddress(_fromMail);
                _mail.To.Add(new MailAddress(Toemail));
                _mail.Subject = $"Thông tin về Học sinh {StudentName}";
                _mail.Body = "Con của bạn hiện không thuộc quản lý của nhà trường. Vui lòng liên hệ hotline : ******** để biết thêm thông tin chi tiết";
                _mail.IsBodyHtml = true;

                using (var _smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    _smtp.UseDefaultCredentials = false;
                    _smtp.Credentials = new NetworkCredential(_fromMail, _password);
                    _smtp.EnableSsl = true;

                    _smtp.Send(_mail);
                    Console.WriteLine($"Email đã được gửi thành công tới {Toemail}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gửi email thất bại tới {Toemail}. Lỗi: {ex.Message}");
                // Handle or log the exception appropriately for your application
            }
        }
    }
}
