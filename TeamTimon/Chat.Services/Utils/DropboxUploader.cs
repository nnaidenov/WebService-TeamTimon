using System;
using Spring.IO;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.Social.OAuth1;

namespace Chat.Services.Utils
{
    public class DropboxUploader
    {
        private const string DropboxAppKey = "u45v5vze1tqrko8";
        private const string DropboxAppSecret = "01g02ukz96pyldu";

        public string UploadFileToDropBox(string filePath, string fileName)
        {
            DropboxServiceProvider dropboxServiceProvider =
                           new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

            // Authenticate the application (if not authenticated) and load the OAuth token
            // TODO - throw exception if auth file is not found
            //if (!File.Exists(OAuthTokenFileName))
            //{
            //    AuthorizeAppOAuth(dropboxServiceProvider);
            //}
            OAuthToken oauthAccessToken = LoadOAuthToken();

            // Login in Dropbox
            IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

            // Display user name (from his profile)
            //DropboxProfile profile = dropbox.GetUserProfileAsync().Result;
            //Console.WriteLine("Hi " + profile.DisplayName + "!");

            // Create new folder
            string newFolderName = DateTime.Now.Ticks.ToString();
            //Entry createFolderEntry = dropbox.CreateFolderAsync(newFolderName).Result;
            Entry createFolderEntry = dropbox.CreateFolder(newFolderName);
            // Upload a file
            Entry uploadFileEntry = dropbox.UploadFile(
                new FileResource(filePath),
                "/" + newFolderName + "/" + fileName);

            // Share a file
            //DropboxLink sharedUrl = dropbox.GetShareableLinkAsync(uploadFileEntry.Path).Result;
            DropboxLink sharedUrl = dropbox.GetShareableLink(uploadFileEntry.Path);
            var ret = sharedUrl.Url.ToString();
            return ret;
        }

        private OAuthToken LoadOAuthToken()
        {
            OAuthToken oauthAccessToken = new OAuthToken("hsmlwuul3nbd72go", "z183wdbxk9dj49i");
            return oauthAccessToken;
        }

        //private void AuthorizeAppOAuth(DropboxServiceProvider dropboxServiceProvider)
        //{
        //    // Authorization without callback url
        //    OAuthToken oauthToken = dropboxServiceProvider.OAuthOperations.FetchRequestTokenAsync(null, null).Result;
        //    OAuth1Parameters parameters = new OAuth1Parameters();
        //    string authenticateUrl = dropboxServiceProvider.OAuthOperations.BuildAuthorizeUrl(
        //        oauthToken.Value, parameters);
        //    AuthorizedRequestToken requestToken = new AuthorizedRequestToken(oauthToken, null);
        //    OAuthToken oauthAccessToken =
        //        dropboxServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;
        //    string[] oauthData = new string[] { oauthAccessToken.Value, oauthAccessToken.Secret };
        //    File.WriteAllLines(OAuthTokenFileName, oauthData);
        //}
    }
}