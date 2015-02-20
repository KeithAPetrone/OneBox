using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GoogleDriveDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            QuickStart();
        }

        private static void QuickStart()
        {
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "320329124466-6krqbu5gkdr0d8tfv91plfn31no65l00.apps.googleusercontent.com",
                    ClientSecret = "qZ2oSBLR-NS3OK529S4UrTq4",
                },
                new[] { DriveService.Scope.Drive },
                "user",
                CancellationToken.None).Result;

            // Create the service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "OneBox",
            });

            File body = new File();
            body.Title = "My document";
            body.Description = "A test document";
            body.MimeType = "text/plain";

            byte[] byteArray = System.IO.File.ReadAllBytes("../../document.txt");
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            FilesResource.ListRequest listRequest = service.Files.List();
            FileList files = listRequest.Execute();
            IEnumerable<File> daFiles = files.Items;

            foreach (var item in daFiles)
            {
                if (item.Title == "Test.txt")
                {
                    var v = service.HttpClient.GetStreamAsync(item.DownloadUrl);
                    var result = v.Result;
                    using (var fileStream = System.IO.File.Create("Test.txt"))
                    {
                        result.CopyTo(fileStream);
                    }
                }               
            }

            
            //FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, "text/plain");
            //request.Upload();

            //File file = request.ResponseBody;
            //Console.WriteLine("File id: " + file.Id);
            //Console.WriteLine("Press Enter to end this process.");
            Console.ReadLine();
        }
    }
}
