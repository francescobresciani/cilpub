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
            if (context.Contents.Any())

            {
                return;   // DB has been seeded
            }
            var contents = new Content[]
            {
                new Content{ContentID=1,Name="Cont01",Description="fejdf",ImgUrl="./cont.jpg"},
                new Content{ContentID=2,Name="Cont02",Description="errw",ImgUrl="./cont.jpg"},
                new Content{ContentID=3,Name="Cont03",Description="eeeee",ImgUrl="./cont.jpg"},
                new Content{ContentID=4,Name="Cont04",Description="vfbrgr",ImgUrl="./cont.jpg"},
                new Content{ContentID=5,Name="Cont05",Description="grgweg",ImgUrl="./cont.jpg"},
                new Content{ContentID=6,Name="Cont06",Description="weferw",ImgUrl="./cont.jpg"},
                new Content{ContentID=7,Name="Cont07",Description="wegwev",ImgUrl="./cont.jpg"}
            };

            foreach (Content c in contents)
            {
                context.Contents.Add(c);
            }
            context.SaveChanges();
            var materials = new Material[]
            {
                new Material{ID=1050,Name="mat01",Description="sdafwefwf",imgUrl="./mat.jpg"},
                new Material{ID=1051,Name="mat02",Description="dfdff",imgUrl="./mat.jpg"},
                new Material{ID=1052,Name="mat03",Description="sdafweqwwewefwf",imgUrl="./mat.jpg"},
                new Material{ID=1053,Name="mat04",Description="qweqwe",imgUrl="./mat.jpg"},
                new Material{ID=1054,Name="mat05",Description="sddbafwefwf",imgUrl="./mat.jpg"}
            };
            foreach (Material m in materials)
            {
                context.Materials.Add(m);
            }
            context.SaveChanges();
            var attachings = new Attaching[]
            {
                new Attaching{ContentID=1,MaterialID=1050},
                new Attaching{ContentID=2,MaterialID=1050},
                new Attaching{ContentID=3,MaterialID=1050},
                new Attaching{ContentID=4,MaterialID=1051},
                new Attaching{ContentID=5,MaterialID=1051},
                new Attaching{ContentID=6,MaterialID=1052},
                new Attaching{ContentID=7,MaterialID=1053},

            };
            foreach (Attaching a in attachings)
            {
                context.Attachings.Add(a);
            }
            context.SaveChanges();
        }
    }
}