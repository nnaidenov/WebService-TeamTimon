using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Chat.Models;
using Chat.Repositories;
using Chat.Services.Utils;
using PubNubMessaging.Core;

namespace Chat.Services.Controllers
{
    public class UploadController : ApiController
    {
        private readonly IRepository<User> userRepository;
        private readonly ChatEntities db = new ChatEntities();

        public UploadController(IRepository<User> repository)
        {
            this.userRepository = repository;
        }

        [HttpPost]
        [ActionName("upload-file")]
        public async Task<HttpResponseMessage> Post(string sessionKey, string channel)
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
                    Pubnub pubnub = new Pubnub("demo", "demo");
                    string pubnubChannel = channel;
                    //var dbUser = unitOfWork.Users.All().FirstOrDefault(x => x.Sessionkey == sessionkey);
                    var uploader = new DropboxUploader();
                    List<string> urls = new List<string>();
                    foreach (MultipartFileData file in streamProvider.FileData)
                    {
                        //Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                        //Trace.WriteLine("Server file path: " + file.LocalFileName);
                        string fileName = file.LocalFileName.Normalize();
                        var url = uploader.UploadFileToDropBox(fileName.Trim(), file.Headers.ContentDisposition.FileName.Replace("\"", ""));
                        urls.Add(url.ToString());
                        //dbUser.ProfilePicture = url;
                        pubnub.Publish(pubnubChannel, url.ToString(), (object obj) => {});
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

        [HttpPost]
        [ActionName("upload-avatar")]
        public async Task<HttpResponseMessage> Post(string sessionKey)
        {
            string folderName = @"Uploads";
            string PATH = HttpContext.Current.Server.MapPath("~/" + folderName);
            string rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);

            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = new MultipartFormDataStreamProvider(PATH);

                try
                {
                    // Read the form data.
                    await Request.Content.ReadAsMultipartAsync(streamProvider);

                    IQueryable<User> users = this.userRepository.GetAll();

                    var result = from u in users
                                 where u.SessionKey == sessionKey
                                 select u;

                    User dbUser = result.FirstOrDefault();
                    db.Set<User>().Attach(dbUser);
                    var uploader = new DropboxUploader();
                    var avatarUrl = "";
                    foreach (MultipartFileData file in streamProvider.FileData)
                    {
                        string fileName = file.LocalFileName;
                        var url = uploader.UploadFileToDropBox(fileName.Trim(), file.Headers.ContentDisposition.FileName.Replace("\"", ""));
                        avatarUrl = url;
                        break;
                    }
                    dbUser.Avatar = avatarUrl;


                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, dbUser);
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
