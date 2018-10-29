using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdTechProject.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int LessonPlanId { get; set; }
        public int FromUserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}