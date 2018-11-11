using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Miv.Models
{
    public class Material
    {
        [Key]
        public int MaterialID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string imgUrl { get; set; }
        public ICollection<ParentChild> Children { get; set; }
        public ICollection<ParentChild> Parents { get; set; }
    }
}
