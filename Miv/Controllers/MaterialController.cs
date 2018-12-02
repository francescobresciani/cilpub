using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Miv.Data;
using Miv.Models;
using Microsoft.AspNetCore.Hosting;


namespace Miv.Controllers
{

    public class MaterialController : Controller
    {
        private readonly MivContext _context;

        public MaterialController(MivContext context)
        {
            _context = context;
        }

        // GET: Material
        public IActionResult ShowGrid()
        {
            return View();
        }


        //LoadData
        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();

                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                // getting all Customer data  
                var materialData = (from material in _context.Materials
                                    select material);
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    //materialData = materialData.OrderBy(sortColumn + " " + sortColumnDirection);


                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    materialData = materialData.Where(m => (m.MaterialID).ToString().Contains(searchValue) || m.Name.Contains(searchValue));
                }

                //total number of rows counts   
                recordsTotal = materialData.Count();
                //Paging   
                var data = materialData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }

        }





     public String LoadImage(String imageName)
            {

            String path = "wwwroot/" + imageName;
            byte[] imageByteData = System.IO.File.ReadAllBytes(path);
            string imreBase64Data = Convert.ToBase64String(imageByteData);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);


            return imgDataURL;
            }



      



    }







}

