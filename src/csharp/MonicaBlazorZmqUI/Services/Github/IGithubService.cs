using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonicaBlazorZmqUI.Services.Github
{
    public interface IGithubService
    {
        string RepoName { get; set; }
        string RepoOwner { get; set; }

        void SetRepoInfo(string repoPath);
        string GetRepoPath(string repoPath);
        string GetRepoResultPath(string repoPath);
        
        void CommitOnGit(string FileName, string JsonContent, string CsvContent, string username, string token, string monicaResultsPathOnGithub);
        void CreateFile(string username, string token);

        /// <summary>
        /// Return all directories and files inside the directory path.
        /// </summary>
        /// <param name="path">The directory path</param>
        /// <param name="username">The username of owner</param>
        /// <param name="toekn">The password of owner</param>
        /// <returns>Return list of existing contents int he path</returns>
        IEnumerable<RepositoryContent> GetContents(string path, string username, string toekn);

        /// <summary>
        /// Return all directories and files inside the directory path.
        /// </summary>
        /// <param name="path">The directory path</param>
        /// <param name="username">The username of owner</param>
        /// <param name="toekn">The password of owner</param>
        /// <returns>Return list of existing contents int he path</returns>
        Task<IEnumerable<RepositoryContent>> GetContentsAsync(string path, string username, string token);
        List<string> GetContentsList(string path, string username, string monicaResultsPathOnGithub);
        string GetDownloadPath(string path, string username, string token);
        Task<string> GetDownloadPathAsync(string path, string username, string token);
        string GetFileContent(string path, string username, string token);
        Task<string> GetFileContentAsync(string path, string username, string token);
        string GetFileContentUsingSha(string Sha, string username, string token);
        Task<string> GetFileContentUsingShaAsync(string Sha, string username, string token);
        bool IsExistPath(string path, string username, string token, string monicaResultsPathOnGithub);
        bool PathValidator(string path, string username, string token, string monicaResultsPathOnGithub);
        Task<bool> IsExistPathAsync(string path, string username, string token);
        bool Login(string username, string token);
        Uri OAuthLogin();
        string OAuthAuthorize(string code, string state);
        User GetCurrentUser(string token);
    }
}