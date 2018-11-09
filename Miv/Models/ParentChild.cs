using System;
namespace Miv.Models
{
    public class ParentChild
    {

        //public enum Grade
        //{
        //    A, B, C, D, F
        //}

        public int ParentChildID { get; set; }
        public int ParentID { get; set; }
        public int ChildID { get; set; }
        //public Grade? Grade { get; set; }
        public Material Parent { get; set; }
        public Material Child { get; set; }
    }
}


