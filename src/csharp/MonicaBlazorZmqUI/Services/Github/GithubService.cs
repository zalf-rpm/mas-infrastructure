using Application.Share;
using Microsoft.AspNetCore.Components;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MonicaBlazorZmqUI.Services.Github
{
    public class GithubService : IGithubService
    {
        private readonly string _githubHeader = "Monica-Blazor";

        private readonly IGitHubClient _gitHubClient;

        public string RepoOwner { get; set; }

        public string RepoName { get; set; }


        private readonly string clientId = Environment.GetEnvironmentVariable("GITHUB_CLIENT_ID");
        private readonly string clientSecret = Environment.GetEnvironmentVariable("GITHUB_CLIENT_SECRET");
        private GitHubClient client = new GitHubClient(new ProductHeaderValue("Monica"));

        //app settings
        //public string GithubUserName { get; set; }
        // public string Githubpassword.Decrypt() { get; set; }
        //[Inject]
        //protected AppData AppData { get; set; }

        public GithubService()
        {
            // var basicAuth = new Credentials("user", "pass"); 
            _gitHubClient = new GitHubClient(new ProductHeaderValue(_githubHeader));
        }

        public bool Login(string username, string password)
        {
            var basicAuth = new Credentials(username, password.Decrypt());
            _gitHubClient.Connection.Credentials = basicAuth;

            try
            {
                var result = _gitHubClient.Organization.GetAllForUser(username).Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(AuthorizationException))
                {
                    return false;
                }
                else
                    throw ex;
            }
            return true;
        }


        public Uri OAuthLogin()
        {
            var request = new OauthLoginRequest(clientId)
            {
                Scopes = { "read:user", "public_repo" }
            };
            var oauthLoginUrl = client.Oauth.GetGitHubLoginUrl(request);

            return oauthLoginUrl;
        }

        public string OAuthAuthorize(string code, string state)
        { 
            var request = new OauthTokenRequest(clientId, clientSecret, code);
            var token = client.Oauth.CreateAccessToken(request).Result;
            return token.AccessToken;
        }

        public User GetCurrentUser(string token)
        {
            SetCredential(token);
            var user = _gitHubClient.User.Current().Result;
            return user;
        }

        public void SetRepoInfo(string repoPath)
        {
            var pathItems = repoPath.Split("/");
            RepoOwner = pathItems[3];
            RepoName = pathItems[4];

        }

        public string GetRepoPath(string repoPath)
        {
            var pathItems = repoPath.Split("/");
            string path = string.Empty;

            if (pathItems.Length > 5)
            {
                for (int i = 5; i < pathItems.Length; i++)
                    path += "/" + pathItems[i];
            }
            else
                path = "/";

            return path;
        }

        public string GetRepoResultPath(string repoPath)
        {
            var pathItems = repoPath.Split("/");
            string path = string.Empty;

            if (pathItems.Length > 4)
            {
                for (int i = 4; i < pathItems.Length; i++)
                    path += pathItems[i];
            }
            else
                path = "/";

            return path;
        }

        public void CreateFile(string username, string token)
        {
            SetCredential(token);
            var result = _gitHubClient.Repository.Content.CreateFile(RepoOwner, RepoName, "export.txt",
                new CreateFileRequest("Added by server", "Hello Github")).Result;
        }
        // try to export the CSV result on the user's repository
        public void CommitOnGit(string FileName, string JsonContent, string CsvContent, string username, string token, string monicaResultsPathOnGithub)// monicaResultsPathOnGithub should be relative not absolute
        {
            SetCredential(token);
            RepoOwner = username;
            RepoName = monicaResultsPathOnGithub;

            var resultJson = _gitHubClient.Repository.Content.CreateFile(RepoOwner, RepoName, FileName + ".json",
                new CreateFileRequest("Json added by Blazor", JsonContent)).Result;
            var resultCsv = _gitHubClient.Repository.Content.CreateFile(RepoOwner, RepoName, FileName + ".csv",
               new CreateFileRequest("CSV added by Blazor", CsvContent)).Result;
        }

        public async Task<bool> IsExistPathAsync(string path, string username, string token)
        {
            SetCredential(token);
            return await _gitHubClient.Repository.Content.GetAllContents(RepoOwner, RepoName, path) != null;
        }

        public bool IsExistPath(string path, string username, string token, string monicaResultsPathOnGithub)
        {
            SetCredential(token);

            /*if (RepoOwner == null)
            {
                RepoOwner = username;
                RepoName = monicaResultsPathOnGithub;
            }*/

            try
            {
                var result = _gitHubClient.Repository.Content.GetAllContents(RepoOwner, RepoName).Result;
                return result != null;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public bool PathValidator(string path, string username, string token, string monicaResultsPathOnGithub)
        {
            SetCredential(token);

            try
            {
                var result = _gitHubClient.Repository.Content.GetAllContents(username, monicaResultsPathOnGithub).Result;
                return result != null;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public List<string> GetContentsList(string path, string username, string monicaResultsPathOnGithub)
        {
            RepoOwner = username;
            RepoName = monicaResultsPathOnGithub;
            List<string> result = new List<string>();
            return result;
        }

        public async Task<IEnumerable<RepositoryContent>> GetContentsAsync(string path, string username, string token)
        {
            SetCredential(token);
            var contents = await _gitHubClient.Repository.Content.GetAllContents(RepoOwner, RepoName, path);
            return contents;
        }

        public IEnumerable<RepositoryContent> GetContents(string path, string username, string token)
        {
            SetCredential(token);
            var contents = _gitHubClient.Repository.Content.GetAllContents(RepoOwner, RepoName, path).Result;
            return contents;
        }

        public string GetFileContent(string path, string username, string token)
        {
            SetCredential(token);

            try
            {
                var contents = _gitHubClient.Repository.Content.GetAllContents(RepoOwner, RepoName, path).Result;
                return contents?.FirstOrDefault()?.Content;
            }
            catch (Exception ex)
            {
                string fileName = path.Remove(0, path.LastIndexOf("/") + 1);
                path = path.Remove(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);
                var contents = _gitHubClient.Repository.Content.GetAllContents(RepoOwner, RepoName, path).Result;

                var fileContent = contents?.Where(s => s.Name == fileName).FirstOrDefault();

                if (fileContent != null)
                    return GetFileContentUsingSha(fileContent.Sha, username, token);
                else
                    throw ex;
            }
        }

        public async Task<string> GetFileContentAsync(string path, string username, string token)
        {
            SetCredential(token);

            try
            {
                var contents = await _gitHubClient.Repository.Content.GetAllContents(RepoOwner, RepoName, path);
                return contents?.FirstOrDefault()?.Content;
            }
            catch (Exception ex)
            {
                string fileName = path.Remove(0, path.LastIndexOf("/"));
                path = path.Remove(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);
                var contents = await _gitHubClient.Repository.Content.GetAllContents(RepoOwner, RepoName, path);

                var fileContent = contents?.Where(s => s.Name == fileName).FirstOrDefault();

                if (fileContent != null)
                    return await GetFileContentUsingShaAsync(fileContent.Sha, username, token);
                else
                    throw ex;
            }
        }

        public string GetFileContentUsingSha(string Sha, string username, string token)
        {
            SetCredential(token);

            var blob = _gitHubClient.Git.Blob.Get(RepoOwner, RepoName, Sha).Result;
            byte[] data = Convert.FromBase64String(blob.Content);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        public async Task<string> GetFileContentUsingShaAsync(string Sha, string username, string token)
        {
            SetCredential(token);

            var blob = await _gitHubClient.Git.Blob.Get(RepoOwner, RepoName, Sha);
            byte[] data = Convert.FromBase64String(blob.Content);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        public async Task<string> GetDownloadPathAsync(string path, string username, string token)
        {
            var contents = await GetContentsAsync(path, username, token);

            if (contents == null)
                return path;

            return contents.Select(s => s.DownloadUrl).FirstOrDefault();
        }

        public string GetDownloadPath(string path, string username, string token)
        {
            var contents = GetContents(path, username, token);

            if (contents == null)
                return path;

            return contents.Select(s => s.DownloadUrl).FirstOrDefault();
        }

        private void SetCredential(string token)
        {
            var basicAuth = new Credentials(token);
            _gitHubClient.Connection.Credentials = basicAuth;
        }
    }
}
