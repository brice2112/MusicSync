﻿using static System.Net.Mime.MediaTypeNames;
using System.Xml.Serialization;
using System.Xml;

namespace MusicSync
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            // ApplicationConfiguration.Initialize();
            // Application.Run(new Form1());

            // Init: Get playlist names
            xmlParser.ExtractPlaylistNames(@"C:\Users\Admin\Music\iTunes\iTunes Music Library.xml");

            // Method 1 : Read XML using XMLDocument
            xmlParser.ExtractFoldersAndPlaylists(@"C:\Users\Admin\Music\iTunes\iTunes Music Library.xml");

            //Method 2 : Deserialize XML
            //DeserializeXMl();

            Console.ReadLine();
        }

        static void ReadXml(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement Root = doc.DocumentElement;
            if (Root != null)
            {
                foreach (XmlNode node in Root.ChildNodes)
                {
                    String Header = node.InnerText;
                    String Value = "";
                    if (node.NextSibling != null)
                    {
                        Value = node.NextSibling.InnerText;
                    }

                    Console.WriteLine($"{Header} - {Value}");
                }
            }
        }

        static void DeserializeXMl()
        {
            XmlReader xmlDoc = XmlReader.Create(@"C:\Users\Admin\source\repos\MusicSync\MusicSync\test.xml");
            XmlSerializer xmlSer = new XmlSerializer(typeof(String));
            String deser = (string)xmlSer.Deserialize(xmlDoc);
            Console.WriteLine(deser );
        }
    }
}
