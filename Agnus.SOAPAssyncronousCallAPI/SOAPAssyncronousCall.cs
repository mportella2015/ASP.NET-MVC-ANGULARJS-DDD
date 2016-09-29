    using System.Xml;
using System.Net;
using System.IO;
using System;
using System.Xml.Serialization;

namespace Agnus.SOAPAssyncronousCallAPI
{
    public static class SOAPAssyncronousCall
    {
        public static string CallWebService(string url, XmlDocument soapEnvelopeXml)
        {
            HttpWebRequest webRequest = CreateWebRequest(url);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);           

            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    return (soapResult);
                }
            }
        }

        private static HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        public static string CreateEnvelope<T>(T request, string action)
        {
            string soapEnvelope =
                @"<soap:Envelope
                    xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
                    xmlns:xsd='http://www.w3.org/2001/XMLSchema'
                    xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                <soap:Body><{0} xmlns='http://tempuri.org/'><request>{1}</request></{0}></soap:Body></soap:Envelope>";

            var xml = SerializeObject<T>(request);
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(xml);

            xml = soapEnvelopeXml.DocumentElement.InnerXml;

            soapEnvelope = string.Format(soapEnvelope, action, xml);

            return soapEnvelope;
        }

        public static string SerializeObject<T>(T toSerialize)
        {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}