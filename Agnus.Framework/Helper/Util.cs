using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Agnus.Framework.Helper
{
    public static class Util
    {


        public static T ConverterEnum<T>(object valor) where T : new()
        {
            foreach (var enumValue in typeof(T).GetEnumValues())
            {
                if (enumValue.ToString() == valor.ToString())
                    return (T)enumValue;

            }
            return new T();
        }

        public static string GetEnumDescription(object value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description ?? "Undefined" : value.ToString();
        }

        public static Dictionary<int, string> GetEnumValues(Type enumType)
        {
            var itemList = new Dictionary<int, string>();

            var listOfValues = Enum.GetValues(enumType);
            foreach (var value in listOfValues)
            {
                var index = (int)value;
                var description = GetEnumDescription(value);
                itemList.Add(index, description);
            }

            return itemList;
        }

        public static string HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        // Verify a hash against a string. 
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string CreatePassword()
        {
            const string SenhaCaracteresValidos = "abcdefghijklmnopqrstuvwxyz1234567890@#!?";

            int valormaximo = SenhaCaracteresValidos.Length;

            Random random = new Random(DateTime.Now.Millisecond);

            StringBuilder senha = new StringBuilder(8);

            for (int indice = 0; indice < 8; indice++)

                senha.Append(SenhaCaracteresValidos[random.Next(0, valormaximo)]);

            return senha.ToString();
        }

        public static string IndentXml(XmlDocument doc)
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        public static string IndentXml(string XML)
        {
            var result = string.Empty;

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(XML);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                String FormattedXML = sReader.ReadToEnd();

                result = FormattedXML;
            }
            catch (XmlException)
            {
            }

            mStream.Close();
            writer.Close();

            return result;
        }

        public static string FormatarCPFCNPJ(string cpfcnpj)
        {
            if (!string.IsNullOrEmpty(cpfcnpj))
            {
                cpfcnpj = cpfcnpj.Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty);
                cpfcnpj = cpfcnpj.Count() > 11 ? Convert.ToUInt64(cpfcnpj).ToString(@"00\.000\.000\/0000\-00") : Convert.ToUInt64(cpfcnpj).ToString(@"000\.000\.000\-00");
            }
            return cpfcnpj;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static DateTime ConverterStringDataPraDatetime(string stringData)
        {
            return DateTime.Parse(stringData, new System.Globalization.CultureInfo("pt-BR", false));
        }
    }
}
