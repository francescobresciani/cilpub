using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Miv.Models;


namespace Miv.Controllers
{
    public class MaterialController : Controller
    {
        public IActionResult ShowGrid()
        {
            return View();
        }

        [HttpPost]
        private HttpWebRequest soapRequest()
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

        //funzione di supporto per che ritorna il nomespace + la tag xml
        private static XName nameSpace(string tagXml)
        {
            return XNamespace.Get("http://miv.materials.com") + tagXml;
        }

        //funzione che riceve i detagli della richiesta SOAP, riceve i dati e ritorna in un formato List<Element>
        [HttpPost]
        private List<Element> retrieveData(XmlDocument SOAPReqBody)
        {
            //Calling il metodo SOAP Request 1
            HttpWebRequest request = soapRequest();

            using (Stream stream = request.GetRequestStream())
            {
                SOAPReqBody.Save(stream);
            }

            List<Element> lista = new List<Element>();

            using (WebResponse Service = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(Service.GetResponseStream()))
                {
                    string ServiceResult = reader.ReadToEnd();

                    XDocument doc = XDocument.Parse(ServiceResult.ToString());

                    foreach (XElement item in doc.Descendants(nameSpace("Material")))
                    {
                        lista.Add(new Element(Int32.Parse(item.Element(nameSpace("id")).Value), item.Element(nameSpace("name")).Value,
                            item.Element(nameSpace("description")).Value, item.Element(nameSpace("imgUrl")).Value));
                    }

                    return lista;
                }
            }
        }

        //Ritorna tutti i materiali
        [HttpPost]
        public IActionResult SoapGetMaterials()
        {
            //Calling il metodo SOAP Request 1
            //HttpWebRequest request = soapRequest();

            //SOAP Body Request
            XmlDocument SOAPReqBody = new XmlDocument();
            SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:miv=""http://miv.materials.com"">
            <soapenv:Header/>                
                <soapenv:Body>
                    <miv:GetMaterialsRequest></miv:GetMaterialsRequest>
                </soapenv:Body>
            </soapenv:Envelope>");

            return Json(retrieveData(SOAPReqBody));

        }

        //Ritorna i materiali cercati
        [HttpPost]
        public IActionResult SoapSearch(int searchData)
        {

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

            return Json(retrieveData(SOAPReqBody));

        }

        //ritorna i materiali che sono 'figli' del parent cercato
        [HttpPost]
        public IActionResult SoapLoadChild(int parMaterialId)
        {

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

            return Json(retrieveData(SOAPReqBody));

        }

        //ritorna l'anteprima dell'imagine
        public String LoadImage(String imageName)
        {
            String path = "wwwroot/" + imageName;
            byte[] imageByteData = System.IO.File.ReadAllBytes(path);
            string imreBase64Data = Convert.ToBase64String(imageByteData);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            return imgDataURL;
        }

        //download dell'imagine
        public ActionResult DownloadImage(String imageName)
        {
            String path = "wwwroot/images/" + imageName;
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            //il file salvato non è leggibile. controllare codifica

            return File(fileBytes, "image/jpg", imageName);

        }
    }
}

