using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Chat.Services.Utils;

namespace Chat.Services.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost]
        [ActionName("upload-file")]
        public async Task<HttpResponseMessage> Post()
        {
            string folderName = @"Uploads";
            string PATH = HttpContext.Current.Server.MapPath("~/" + folderName);
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);

            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = new MultipartFormDataStreamProvider(PATH);
                //var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IEnumerable<FileDesc>>(t =>
                //{

                //    //if (t.IsFaulted || t.IsCanceled)
                //    //{
                //    //    throw new HttpResponseException(HttpStatusCode.BadGateway);
                //    //}

                //    var fileInfo = streamProvider.FileData.Select(i =>
                //    {
                //        var info = new FileInfo(i.LocalFileName);
                //        return new FileDesc(info.Name, rootUrl+"/"+folderName+"/"+info.Name, info.Length);
                //    });
                //    return fileInfo;
                //});

                //return task;

                try
                {
                    // Read the form data.
                    await Request.Content.ReadAsMultipartAsync(streamProvider);

                    //var dbUser = unitOfWork.Users.All().FirstOrDefault(x => x.Sessionkey == sessionkey);
                    var uploader = new DropboxUploader();
                    List<string> urls = new List<string>();
                    foreach (MultipartFileData file in streamProvider.FileData)
                    {
                        //Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                        //Trace.WriteLine("Server file path: " + file.LocalFileName);
                        string fileName = file.LocalFileName;
                        var url = uploader.UploadFileToDropBox(fileName, file.Headers.ContentDisposition.FileName);
                        urls.Add(url.ToString());
                        //dbUser.ProfilePicture = url;

                        //break;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, urls);
                }
                catch (System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }

        }
    }
}
