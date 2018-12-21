using Miv.Models;
using System;
using System.Linq;
namespace Miv.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MivContext context)
        {
            context.Database.EnsureCreated();
            // Look for any content.
            if (context.Materials.Any())

            {
                return;   // DB has been seeded
            }




            //materials
            var materials = new Material[]
            {
                new Material{MaterialID=1,Name="parent 1",Description="i have 3 children",imgUrl="images/01.jpg"},
                new Material{MaterialID=2,Name="parent 2",Description="i have 2 children",imgUrl="images/02.jpg"},
                new Material{MaterialID=3,Name="parent 3",Description="i have no children",imgUrl="images/03.jpg"},
                new Material{MaterialID=10,Name="1st child of 1",Description="i am a child",imgUrl="images/01.jpg"},
                new Material{MaterialID=11,Name="2nd child of 1",Description="i am a child",imgUrl="images/02.jpg"},
                new Material{MaterialID=12,Name="3rd child of 1",Description="i am a child",imgUrl="images/03.png"},
                new Material{MaterialID=21,Name="only child of 2",Description="i am a child",imgUrl="images/01.jpg"}
            };
            foreach (Material m in materials)
            {
                context.Materials.Add(m);
            }
            context.SaveChanges();





            //parentchild
            var relationships = new ParentChild[]
            {
                new ParentChild{ChildID=10,ParentID=1},
                new ParentChild{ChildID=11,ParentID=1},
                new ParentChild{ChildID=12,ParentID=1},
                new ParentChild{ChildID=21,ParentID=2},


            };
            foreach (ParentChild rel in relationships)
            {
                context.ParentChild.Add(rel);
            }
            context.SaveChanges();
        }
    }
}