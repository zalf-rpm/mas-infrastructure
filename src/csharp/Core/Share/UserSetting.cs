using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Core.Share
{
    public class UserSetting
    {
        public string ServerPushAddress { get; set; } = "tcp://localhost";
        public int ServerPushPort { get; set; } = 6666;
        public string ServerPullAddress { get; set; } = "tcp://localhost";
        public int ServerPullPort { get; set; } = 7777;
        public bool ChartIsRun { get; set; } = false;
        public string GithubUserName { get; set; } = "Please Login";
        public string GithubPassword { get; set; } = "PASSWORD";
        public bool GithubLoggedIn { get; set; } = false;
        public string MonicaResultsPathOnGithub { get; set; } = "";
        public string MonicaParametersPathOnGithub { get; set; } = "";
        public bool MonicaPathOnGitIsValid { get; set; } = false;
        public bool SaveResultInGit { get; set; }
        public string GithubToken { get; set; }
    }
}
