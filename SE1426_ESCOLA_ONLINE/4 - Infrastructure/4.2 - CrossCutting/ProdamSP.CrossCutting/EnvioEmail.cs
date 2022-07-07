using ProdamSP.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.CrossCutting
{
    public class EnvioEmail
    {
        public static void EnviarEmailGenerico(string _NomeDe, string _EmailDe, string _strEmailPara, string _strEmailCC, string _strAssunto, string _strCorpoMensagem, System.Net.Mail.Attachment AnexoEmail, 
                                               string deliveryMethod, string diretorio, string host, int port, bool enableSsl)
        {
            try
            {

                SmtpClient client = new SmtpClient();
                if (deliveryMethod == "SmtpDeliveryMethod.SpecifiedPickupDirectory")
                {
                    client = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                        PickupDirectoryLocation = diretorio
                    };
                }
                else
                {
                    client = new SmtpClient
                    {
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Host = host,
                        Port=port,
                        EnableSsl=enableSsl
                    };

                }



                MailAddress EmailDe = new MailAddress(_EmailDe, _NomeDe, System.Text.Encoding.UTF8);
                MailMessage Mensagem = default(MailMessage);

                Mensagem = new MailMessage();
                Mensagem.From = EmailDe;
                //Adiciona todos os destinatários da mensagem
                foreach (string item in _strEmailPara.Split(';'))
                {
                    Mensagem.To.Add(new MailAddress(item));
                }

                //Adiciona todos os destinarários com Cópia (CC)
                if (!string.IsNullOrEmpty(_strEmailCC))
                {
                    foreach (string item in _strEmailCC.Split(';'))
                    {
                        Mensagem.CC.Add(new MailAddress(item));
                    }
                }

                Mensagem.Body = _strCorpoMensagem;
                Mensagem.IsBodyHtml = true;
                Mensagem.BodyEncoding = System.Text.Encoding.UTF8;
                Mensagem.Subject = _strAssunto;
                Mensagem.SubjectEncoding = System.Text.Encoding.UTF8;
                //Verifica se a mensagem possui anexos
                if ((AnexoEmail != null))
                    Mensagem.Attachments.Add(AnexoEmail);
                client.Send(Mensagem);
                Mensagem.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void EnviarEmail(EmailModel emailModel)
        {
            try
            {
                var objEmail = new MailMessage();


                foreach (string emailDestinatario in emailModel.ListaDestinatarios)
                {
                    objEmail.To.Add(emailDestinatario);

                }

                AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(emailModel.ConteudoHtmlEmail, null, MediaTypeNames.Text.Html);
                objEmail.IsBodyHtml = true;
                objEmail.AlternateViews.Add(htmlBody);
                objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                objEmail.Priority = MailPriority.Normal;
                objEmail.Subject = emailModel.SubTituloOuAssuntoMensagem;

                foreach (string key in emailModel.AttachmentsCorpoHtml.Keys)
                {
                    var data = Convert.FromBase64String(emailModel.AttachmentsCorpoHtml[key]);
                    MemoryStream streamBitmap = new MemoryStream(data);
                    LinkedResource imageToInline = new LinkedResource(streamBitmap, "image/png");
                    imageToInline.ContentId = key;
                    htmlBody.LinkedResources.Add(imageToInline);
                }

                var objSmtp = new SmtpClient();
                objSmtp.Send(objEmail);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao enviar email motivo=" + ex.Message, ex);
            }

        }

    }
}
