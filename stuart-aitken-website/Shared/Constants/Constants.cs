using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public static class Constants
    {
        public static class Urls
        {

#if DEBUG
            public static readonly string FunctionsBaseUrl = @"http://localhost:7071/api/";
#else
            public static readonly string FunctionsBaseUrl = @"https://witty-sand-091d9dc03.azurestaticapps.net/api/";
#endif
            public static readonly string GetAllProjects = FunctionsBaseUrl + "GetAllProjects";
        }
    }
}
