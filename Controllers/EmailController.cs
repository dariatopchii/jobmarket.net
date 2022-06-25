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
                     From = new MailAddress("jobmarketukraine@gmail.com"),
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
                     UseDefaultCredentials = false,
                     Credentials = new NetworkCredential("jobmarketukraine@gmail.com", "jobmarket1"),
                     DeliveryMethod = SmtpDeliveryMethod.Network,
                };

               await smtp.SendMailAsync(message);
               return Ok();
             
        }
     }
 }