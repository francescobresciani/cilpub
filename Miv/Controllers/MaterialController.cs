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

