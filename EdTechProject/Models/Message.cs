﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdTechProject.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}