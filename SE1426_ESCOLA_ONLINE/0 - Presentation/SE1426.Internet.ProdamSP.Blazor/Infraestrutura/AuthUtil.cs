using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace SE1426.Internet.ProdamSP.Blazor.Infraestrutura
{

    public class AuthUtil
    {


        public static string GetToken(IConfiguration configuration)
        {
            try
            {
                string result;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(configuration.GetSection("CacTKS:CacLoginApi").Value);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"usuario\":\"" + configuration.GetSection("CacTKS:cacuserName").Value + "\",\"senha\":\"" + configuration.GetSection("CacTKS:cacpassword").Value + "\",\"siglaHierarquia\":\"" + configuration.GetSection("CacTKS:cachierarquia").Value + "\",\"codigoSistema\":\"" + configuration.GetSection("CacTKS:cacsistema").Value + "\",\"codigoModuloSistema\":\"" + configuration.GetSection("CacTKS:cacmodulosistema").Value + "\"}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result.ToString().Replace("\"", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao gerar o token de autenticação");
            }
        }
    }
}
