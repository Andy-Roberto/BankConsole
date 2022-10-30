using MailKit.Net.Smtp;
using MimeKit;
namespace BankConsole;
public static class EmailService{
    public static void SendMail(){
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Andy", "andyroberto@gmail.com"));
        message.To.Add(new MailboxAddress("Admin","andyroberto@gmail.com"));
        message.Subject = "BankConsole: Usuarios nuevos";
        message.Body = new TextPart("plain"){
            Text = GetEmailText()
        };
        using (var client = new SmtpClient()){
            client.Connect("smtp.gmail.com",587,false);
            client.Authenticate("andyroberto@gmail.com","12312312312");
            client.Send(message);
            client.Disconnect(true);
        }
    }
    private static string GetEmailText(){
        List<User> newUsers = Storage.GetNewUsers();
        if(newUsers.Count == 0){
            return "No hay usuarios nuevos.";
        }
        string emailText = "Usuarios agregados hoy:\n";
        foreach(User user in newUsers){
            emailText += $"\t+ {user.ShowData()}\n";
        }
        return emailText;
    }

}
