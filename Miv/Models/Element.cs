using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miv.Models
{
    public class Element
    {
        public int MaterialID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string imgUrl { get; set; }

        public Element(int materialID, string name, string description, string imgUrl)
        {
            MaterialID = materialID;
            Name = name;
            Description = description;
            this.imgUrl = imgUrl;
        }
    }
}
