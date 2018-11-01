using System;
namespace Miv.Models
{
    public class Attaching
    {

        public enum Grade
        {
            A, B, C, D, F
        }

        public int AttachingID { get; set; }
        public int MaterialID { get; set; }
        public int ContentID { get; set; }
        //public Grade? Grade { get; set; }
        public Material Material { get; set; }
        public Content Content { get; set; }
    }
}


