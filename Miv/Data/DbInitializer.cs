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
                new Material{MaterialID=1,Name="mat01",Description="sdafwefwf",imgUrl="./mat.jpg"},
                new Material{MaterialID=2,Name="mat02",Description="dfdff",imgUrl="./mat.jpg"},
                new Material{MaterialID=3,Name="mat03",Description="sdafweqwwewefwf",imgUrl="./mat.jpg"},
                new Material{MaterialID=10,Name="mat04",Description="qweqwe",imgUrl="./mat.jpg"},
                new Material{MaterialID=11,Name="mat05",Description="sddbafwefwf",imgUrl="./mat.jpg"},
                new Material{MaterialID=12,Name="mat05",Description="sddbafwefwf",imgUrl="./mat.jpg"},
                new Material{MaterialID=21,Name="mat05",Description="sddbafwefwf",imgUrl="./mat.jpg"}
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