using FileuploadAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet;

namespace FileuploadAPI.Controllers
{
    [Route("api/fileUpload")]
    [ApiController]
    public class fileUploadController : ControllerBase
    {
        [HttpPost("content")]
        public string Upload([FromBody] byte[] documentData)
        {
            //byte[] myByteArray = new byte[10];
            MemoryStream stream = new MemoryStream();
             stream.Write(documentData, 0, documentData.Length);
            PrivateKeyFile keyFile = new PrivateKeyFile(@"");
            var keyFiles = new[] { keyFile };
            // These variables can also be stored on key vault instead of local.settings.json. If it is stored in keyvault, you will need to change the ref. to that of the location of the keys.
            string host = "ab"; //Environment.GetEnvironmentVariable("serveraddress");
            string sftpUsername = "123"; //Environment.GetEnvironmentVariable("sftpusername");
            var methods = new List<AuthenticationMethod>();
            methods.Add(new PrivateKeyAuthenticationMethod(sftpUsername, keyFiles));


            // Connect to SFTP Server and Upload file 
            Renci.SshNet.ConnectionInfo con = new Renci.SshNet.ConnectionInfo(host, 22, sftpUsername, methods.ToArray());
            using (var client = new SftpClient(con))
            {
                client.Connect();
                client.UploadFile(stream,"");


                var files = client.ListDirectory("/fdarchived");
                foreach (var file in files)
                {
                    //log.LogInformation(file.Name);
                }
                client.Disconnect();
            }

            return "File uploaded Successfully";
        }


    }
}
