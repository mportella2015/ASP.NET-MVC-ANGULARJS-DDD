using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Agnus.Framework.Helper
{
    public static class XmlComparer
    {
        public static string CompareXml(string _feedList, string _feedRequest)
        {
            //FileInfo feedList = new FileInfo(_feedList);
            //FileInfo feedRequest = new FileInfo(_feedRequest);

            // Load the documents
            XmlDocument feedListXmlDoc = new XmlDocument();
            feedListXmlDoc.LoadXml(_feedList);

            // Load the documents
            XmlDocument feedRequestXmlDoc = new XmlDocument();
            feedRequestXmlDoc.LoadXml(_feedRequest);

            // Define a single node
            XmlNode feedListNode;

            // Get the root Xml element
            XmlElement feedListRoot = feedListXmlDoc.DocumentElement;
            XmlElement feedRequestRoot = feedRequestXmlDoc.DocumentElement;

            // Get a list of feeds for the stored list and the request
            XmlNodeList feedListXml = feedListRoot.ChildNodes;
            XmlNodeList feedRequestXml = feedRequestRoot.ChildNodes;//GetElementsByTagName("Feed");
            FilhoPorFilho(feedListXmlDoc, feedListXml, feedRequestXml);
            LimparTagsVazias(feedListXml);

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                feedListXmlDoc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                Console.Write(stringWriter.GetStringBuilder().ToString());
            }
            //feedListXmlDoc.Save(_resultPath);
            return feedListXmlDoc.InnerXml;
        }

        private static void LimparTagsVazias(XmlNodeList feedListXml)
        {
            XmlNode feedListNode;
            for (int i = 0; i < feedListXml.Count; i++)
            {
                feedListNode = feedListXml.Item(i);
                if (feedListNode.ChildNodes.Count < 1)
                {
                    feedListXml[i].ParentNode.RemoveChild(feedListXml[i]);
                    i--;
                }
            }
        }

        private static void FilhoPorFilho(XmlDocument feedListXmlDoc, XmlNodeList feedListXml, XmlNodeList feedRequestXml)
        {
            XmlNode feedListNode;
            XmlNode feedRequestNode;

            bool feedLocated = false;
            int j = 0;
            int? x = 0;

            try
            {
                // loop through list of current feeds
                for (int i = 0; i < feedListXml.Count; i++)
                {
                    feedListNode = feedListXml.Item(i);
                    //create status attribute
                    //XmlAttribute attr = feedListXmlDoc.CreateAttribute("status");
                    string feedListHash = feedListXml.Item(i).InnerText.ToString();
                    XmlNodeList feedListXmlChild = null;
                    string feedListName = feedListNode.Name;
                    if (feedListNode.ChildNodes.Count > 1)
                    {
                        feedListXmlChild = feedListNode.ChildNodes;
                        if ((x = GetIndexChildrenByName(feedListName, feedRequestXml)).HasValue)
                        {
                            feedRequestNode = feedRequestXml.Item(x.Value);
                            XmlNodeList feedRequestXmlChild = feedRequestNode.ChildNodes;
                            FilhoPorFilho(feedListXmlDoc, feedListXmlChild, feedRequestXmlChild);
                        }
                        VerificaTags(feedListNode, feedListXml, feedRequestXml, ref feedLocated, ref j, feedListName, feedListHash);
                    }
                    else
                    {
                        //check feed request list for a match
                        VerificaTags(feedListNode, feedListXml, feedRequestXml, ref feedLocated, ref j, feedListName, feedListHash);
                    }
                }
            }
            finally
            {
                //Debug.WriteLine("Result file has been written out at " + _resultPath);
            }
        }

        private static int? GetIndexChildrenByName(string _name, XmlNodeList list)
        {
            for (var i = 0; i < list.Count; i++)
                if (list[i].Name == _name)
                    return i;
            return null;
        }

        private static void VerificaTags(XmlNode feedListNode, XmlNodeList feedListXml, XmlNodeList feedRequestXml, ref bool feedLocated, ref int j, string feedListName, string feedListHash)
        {
            XmlNode feedRequestNode;
            while (j < feedRequestXml.Count && feedLocated == false)
            {
                feedRequestNode = feedRequestXml.Item(j);
                string feedRequestName = feedRequestNode.Name;

                //checks to see if feed names match
                if (feedRequestName == feedListName)
                {
                    string feedRequestHash = feedRequestXml.Item(j).InnerText.ToString();

                    //checks to see if hashCodes match
                    if (feedListHash == feedRequestHash)
                    {
                        //if name and code match, set status to ok
                        //attr.Value = "ok";

                        //Debug.WriteLine(feedListName + " name and hash match. Status: 'ok'");
                        //feedListXml[j].ParentNode.RemoveChild(feedListNode);
                        feedListNode.RemoveAll();
                        feedLocated = true;
                    }
                    else
                    {
                        //if hashCodes don't match, set status attribute to updated
                        //attr.Value = "updated";

                        //Debug.WriteLine(feedListName + " name matched but hash did not. Status: 'updated'");
                        //feedListNode.Attributes.Append(attr);
                        feedLocated = true;
                    }

                }
                else
                {
                    //names didn't match, checking to see if we're at the end of  the request list
                    if (j + 1 == feedRequestXml.Count)
                    {
                        //file name wasn't found in the request list, set status attribute to missing
                        //attr.Value = "missing";
                        //feedListNode.Attributes.Append(attr);
                        feedLocated = true;
                        j = 0;

                        //Debug.WriteLine("Reached the end of the file request list without a match. Status: 'missing'");
                    }
                    //file name wasn't located on this pass, move to next record
                    j++;
                }
            }
            feedLocated = false;
        }
    }
}
