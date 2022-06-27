 using System;
 using System.IO;
 using System.Net;
 using System.Net.Mail;
 using System.Threading.Tasks;
 using JobMarket.Files.Settings;
 using JobMarket.Models;
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.Extensions.Options;

 namespace JobMarket.Controllers
 {
     [Route("api/[controller]")]
        public class EmailController: Controller
     {
         // private readonly EmailSettings _emailSettings;
         // private readonly AppSettings _appSettings;
         //
         // public EmailController(EmailSettings emailSettings,AppSettings appSettings )
         // {
         //     _emailSettings = emailSettings;
         //     _appSettings = appSettings;
         // }
         
         // POST api/values
         [HttpPost]
         public async Task<IActionResult> SendEmail([FromBody]EmailModel email)
        {
            
             var subject = "JobMarket";
             var body = "body";
                    // .Replace("{UserName}", email.Name);
                var message = new MailMessage
                 {
                     From = new MailAddress("slackonminimals@gmail.com"),
                     Subject = subject,
                     IsBodyHtml = false,
                     Body = body,
                 };
                 message.To.Add(new MailAddress(email.Email));

                 var smtp = new SmtpClient
                 {
                     Port = 587,
                     Host =  "smtp.gmail.com",
                     EnableSsl = true,
                     // UseDefaultCredentials = false,
                     Credentials = new NetworkCredential("slackonminimals@gmail.com", "GN&j8*VJ^4LMW8jR7G"),
                     // DeliveryMethod = SmtpDeliveryMethod.Network,
                };

               await smtp.SendMailAsync(message);
               return Ok();
         // public class EmailService : IEmailService
         // {
//              private const string HelpDeskEmail = "hdblocks.production@gmail.com";
//              private const string HelpDeskName = "HD Blocks";
//              private const string HelpDeskPwd = "123456Qwerty";
//         
//              public Task SendMailInvitationAsync(string email, string tempPassword, string role)
//              {
//                  MailAddress from = new MailAddress(HelpDeskEmail, HelpDeskName);
//                  MailAddress to = new MailAddress(email);
//                  MailMessage mail = new MailMessage(from, to)
//                  {
//                      Subject = "Registration in HD Blocks system", 
//                      Body = $@"
// <h2>Registration in HD Blocks system</h2>
// <p>You has bee registered as {role} in Help Desk</p>
// <p>Your temporary password is <b>{tempPassword}</b></p>
// ", 
//                      IsBodyHtml = true
//                  };
//                  SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
//                  {
//                      Credentials = new NetworkCredential(HelpDeskEmail, HelpDeskPwd), EnableSsl = true
//                  };
//                  return smtp.SendMailAsync(mail);
//              }

             
        }
     }
 }