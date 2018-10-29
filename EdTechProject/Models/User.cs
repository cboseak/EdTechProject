using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdTechProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public bool Archived { get; set; }
    }
}