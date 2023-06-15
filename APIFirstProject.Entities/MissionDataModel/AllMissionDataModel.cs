using APIFirstProject.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIFirstProject.Entities.MissionDataModel
{
    public class AllMissionDataModel
    {
        [Key]

        public long mission_id { get; set; } = 0;

        public long theme_id { get; set; } = 0;

        public long city_id { get; set; } = 0;

        public long country_id { get; set; } = 0;

        public string title { get; set; } = null!;

        public string? short_description { get; set; } = string.Empty!;

        public string? description { get; set; } = String.Empty!;

        public DateTime? start_date { get; set; } = DateTime.MinValue;

        public DateTime? end_date { get; set; } = DateTime.Now;

        public string mission_type { get; set; } = null!;

        public string status { get; set; } = null!;

        public string? org_name { get; set; } = String.Empty!;

        public string? org_details { get; set; } = String.Empty!;

        public string availability { get; set; } = null!;

        public string? seat_left { get; set; } = "10";

        public DateTime? deadline { get; set; } = default!;
        public string City_Name { get; set; } = string.Empty;
        public string Country_Name { get; set; } = String.Empty;
        public string theme_name { get; set; } = String.Empty;
        public string media_path { get; set; }

        public string? goal_text { get; set; } = string.Empty;
        public int? goal_value { get; set; } = 0;

        public string approval_status { get; set; } = string.Empty;

        public long? mission_application_id { get; set; } = 0;
        public int? rating { get; set; } = 0;

        public int? average_rating { get; set; } = 0;
    }
}
