using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DbModels
{
    public class PortfolioProject
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime ProjectDate { get; set; }
        public int ProjectDurationWeeks { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Tech { get; set; }
        public string Urls { get; set; }
        public int? Views { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }
    }
}
