using SharedProject.DbModels;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using SharedProject.Extensions;

namespace Client.Models
{
    public class AppState
    {
        private List<PortfolioProject> Projects { get; set; }
        public const string TechSelectPlaceholder = "-- Tech --";
        public const string TypeSelectPlaceholder = "-- Types --";

        public string LoginToken { get; set; } = "";

       

        private HttpClient Http {get;set;}
        public AppState()
        {
            Http = new HttpClient();
            Projects = new List<PortfolioProject>();
        }

        public async Task LoadAllProjects()
        {
            this.Projects = await Http.GetFromJsonAsync<List<PortfolioProject>>(SharedProject.Constants.Urls.GetAllProjects);
        }

        public PortfolioProject GetProject(int id)
        {
            return Projects.FirstOrDefault(p => p.ProjectId == id) ?? new PortfolioProject();
        }

        public List<PortfolioProject> GetProjectsWithQuery(Func<PortfolioProject, bool> predicate)
        {
            return Projects.Where(predicate).ToList();
        }

        /// <summary>
        /// Returns Id for new project
        /// </summary>
        /// <returns></returns>
        public int GetNextProjectId()
        {
            return Projects.Max(p => p.ProjectId);
        }

        public List<PortfolioProject> GetAllProjects()
        {
            return Projects.OrderByDescending(p => p.ProjectDate).ToList();
        }

        public List<string> GetProjectTypes()
        {
            return Projects.
                Select(s => s.Type)
                .Distinct()
                .AppendEmptyAndSort(TypeSelectPlaceholder)
                .ToList();
        }


        public List<string> GetProjectTechs()
        {
            return Projects
                .SelectMany(s => s.Tech.Split(",")
                                       .Where(s => !string.IsNullOrEmpty(s))
                                       .Select(s => s.Trim()))
                .Distinct()
                .AppendEmptyAndSort(TechSelectPlaceholder)
                .ToList();
        }

        public List<PortfolioProject> ProjectsFiltered(string name, string tech, string type)
        {
            name = name?.ToLower();

            bool searchName = !string.IsNullOrEmpty(name);

            IEnumerable<PortfolioProject> filtered = Projects.Where(
                p => !searchName ||
                  (p.Name.ToLower().StartsWith(name) ||
                  p.Name.ToLower().Contains(name)));

            if (tech != TechSelectPlaceholder && !string.IsNullOrEmpty(tech))
            {
                filtered = filtered.Where(p => p.Tech.Contains(tech));
            }

            if (type != TypeSelectPlaceholder && !string.IsNullOrEmpty(type))
            {
                filtered = filtered.Where(p => p.Type.Contains(type));
            }

            if (searchName)
            {
                filtered = filtered
                    .OrderBy(p => p.Name.ToLower().StartsWith(name))
                    .ThenBy(p => p.Name.ToLower().Contains(name));
            }
            else
            {
                filtered = filtered
                    .OrderByDescending(p => p.ProjectDate).ToList();
            }

            return filtered.ToList();
        }



        public bool HasProjects => Projects.Any();

    }
}
