using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadViewer.Models
{
    public class Material
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
    }
}
