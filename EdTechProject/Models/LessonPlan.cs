﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdTechProject.Models
{
    public class LessonPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FileId { get; set; }
        public string Description { get; set; }

    }
}