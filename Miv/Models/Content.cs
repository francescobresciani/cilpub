using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Miv.Models
{
    public class Content
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public ICollection<Attaching> Attachings { get; set; }

    }
}
