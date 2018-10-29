using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdTechProject.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public bool Archived { get; set; }
    }
}