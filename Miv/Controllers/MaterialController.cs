using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Miv.Data;
using Miv.Models;


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


        // DELETE: Material 
        public void Delete(int id)
        {
            Console.Out.Write("deleted" + id);
        }

        //InitialData
        [HttpPost]
        public IActionResult InitialData(int varCode)
        {
            var _material = _context.Materials.Where(m => m.Parents.Any(p => p.ParentID == varCode));
            return Json(_material);
        }

        //LoadData
        [HttpPost]
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


        [HttpPost]
        public IActionResult Search(string searchData)
        {

            var materialData = (from material in _context.Materials
                                select material);


            var Results = materialData.Where(m => (m.MaterialID).ToString().Contains(searchData));

            return Json(Results);

        }

        //LoadData2
        [HttpPost]
        public IActionResult LoadData2()
        {
            var VarMaterial = (from material in _context.Materials
                               select material);

            return Json(VarMaterial);
        }

        //LoadChildren
        [HttpPost]
        public IActionResult LoadChild(int parMaterialId)
        {
            // getting the Children data
            //var materialChildren = (from material in _context.ParentChild
            //               select material.ParentID==parMaterialId);

            var materialChildren = _context.Materials.Where(m => m.Parents.Any(p => p.ParentID.Equals(parMaterialId)));

            //var materialChildren = _context.ParentChild.Where(p => p.ParentID == parMaterialId).Select(p => p.)
            //Returning Json Data  
            return Json(materialChildren);

        }

        [HttpPost]
        public HttpWebRequest soapRequest01()
        {
            //fa la Web Request
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"http://localhost:8080/ws");
            //SOAP Action
            Req.Headers.Add(@"SOAPAction:");
            //Content Type
            Req.ContentType = "text/xml;charset=\"utf-8\"";
            Req.Accept = "text/xml";
            //HTTP Method
            Req.Method = "POST";
            //return HttpWebRequest
            return Req;
        }


        static XName nameSpace(string name)
        {
            return XNamespace.Get("http://miv.materials.com") + name;
        }

        [HttpPost]
        public IActionResult InvokeSoap()
        {
            //Calling il metodo SOAP Request 1
            HttpWebRequest request = soapRequest01();

            //SOAP Body Request
            XmlDocument SOAPReqBody = new XmlDocument();
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:miv=""http://miv.materials.com"">
            <soapenv:Header/>                
                <soapenv:Body>
                    <miv:GetMaterialsRequest></miv:GetMaterialsRequest>
                </soapenv:Body>
            </soapenv:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }

            List<Element> lista = new List<Element>(); 

            using (WebResponse Service = request.GetResponse())
            {
                using(StreamReader reader = new StreamReader(Service.GetResponseStream()))
                {
                    var ServiceResult = reader.ReadToEnd();

                    XDocument doc = XDocument.Parse(ServiceResult.ToString()) ;

                    foreach (XElement item in doc.Descendants(nameSpace("MaterialList")))
                    {
                        lista.Add(new Element(Int32.Parse(item.Element(nameSpace("id")).Value), item.Element(nameSpace("name")).Value,
                            item.Element(nameSpace("description")).Value, item.Element(nameSpace("imgUrl")).Value));
                    }
                    
                    return Json(lista);
                }
            }

        }

        [HttpPost]
        public IActionResult SoapSearch(int searchData)
        {
            //Calling il metodo SOAP Request 1
            HttpWebRequest request = soapRequest01();

            //SOAP Body Request
            XmlDocument SOAPReqBody = new XmlDocument();
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:miv=""http://miv.materials.com"">
            <soapenv:Header/>                
                <soapenv:Body>
                    <miv:GetMaterialsByIDRequest>
                        <miv:partial-id>"+searchData+@"</miv:partial-id>
                    </miv:GetMaterialsByIDRequest>
                </soapenv:Body>
            </soapenv:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }

            List<Element> lista = new List<Element>();

            using (WebResponse Service = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(Service.GetResponseStream()))
                {
                    var ServiceResult = reader.ReadToEnd();

                    XDocument doc = XDocument.Parse(ServiceResult.ToString());

                    foreach (XElement item in doc.Descendants(nameSpace("Material")))
                    {
                        lista.Add(new Element(Int32.Parse(item.Element(nameSpace("id")).Value), item.Element(nameSpace("name")).Value,
                            item.Element(nameSpace("description")).Value, item.Element(nameSpace("imgUrl")).Value));
                    }

                    return Json(lista);
                }
            }

        }


        [HttpPost]
        public IActionResult SoapLoadChild(int parMaterialId)
        {
            //Calling il metodo SOAP Request 1
            HttpWebRequest request = soapRequest01();

            //SOAP Body Request
            XmlDocument SOAPReqBody = new XmlDocument();
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:miv=""http://miv.materials.com"">
            <soapenv:Header/>                
                <soapenv:Body>
                    <miv:GetMaterialsByParentIDRequest>
                        <miv:parent-id>"+ parMaterialId + @"</miv:parent-id>
                    </miv:GetMaterialsByParentIDRequest>
                </soapenv:Body>
            </soapenv:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }

            List<Element> lista = new List<Element>();

            using (WebResponse Service = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(Service.GetResponseStream()))
                {
                    var ServiceResult = reader.ReadToEnd();

                    XDocument doc = XDocument.Parse(ServiceResult.ToString());

                    foreach (XElement item in doc.Descendants(nameSpace("Material")))
                    {
                        lista.Add(new Element(Int32.Parse(item.Element(nameSpace("id")).Value), item.Element(nameSpace("name")).Value,
                            item.Element(nameSpace("description")).Value, item.Element(nameSpace("imgUrl")).Value));
                    }

                    return Json(lista);
                }
            }

        }


    }
}
