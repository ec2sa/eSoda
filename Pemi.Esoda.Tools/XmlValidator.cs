using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.IO;
using System.Xml;

namespace Pemi.Esoda.Tools
{
    public class XmlValidator
    {
        /// <summary>
        /// List of validation errors (if any)
        /// </summary>
        public List<string> Errors { get; private set; }

        /// <summary>
        /// Determines if validation has raised any error
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return !(Errors == null || Errors.Count == 0);
            }
        }

        /// <summary>
        /// Creates instance of XmlValidator
        /// </summary>
        public XmlValidator()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Validates XML document against XML Schema
        /// </summary>
        /// <param name="xmlPath">Path and filename of XML Document</param>
        /// <param name="schemaPath">Path and filename of XML Schema</param>
        /// <returns>True when validation was OK, otherwise false</returns>
        public bool ValidateWithFiles(string xmlPath, string schemaPath)
        {
            //preparing to validate
            if (!File.Exists(xmlPath))
                Errors.Add("XML File does not exists.");
            if (!File.Exists(schemaPath))
                Errors.Add("XML Schema does not exists.");
            if (Errors.Count > 0)
                return false;

            return Validate(File.ReadAllText(xmlPath), File.ReadAllText(schemaPath));
        }

        /// <summary>
        /// Validates XML document against XML Schema
        /// </summary>
        /// <param name="xmlContent">string containing XML document to validate</param>
        /// <param name="schemaContent">string containing XML Schema to validate against</param>
        /// <returns>True when validation was OK, otherwise false</returns>
        public bool Validate(string xmlContent, string schemaContent)
        {
            Errors.Clear();

            
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(settings_ValidationEventHandler);

            XmlSchemaSet sset = new XmlSchemaSet();
            try
            {
                using (XmlReader schemaReader = XmlReader.Create(new StringReader(schemaContent)))
                {
                    // adding schema to set with derived namespace
                    sset.Add(null, schemaReader);
                    settings.Schemas = sset;
                    using (XmlReader documentReader = XmlReader.Create(new StringReader(xmlContent), settings))
                    {
                        //validating
                        while (documentReader.Read())
                            ;
                    }
                }
                return !HasErrors;
            }
            catch (Exception ex)
            {
                Errors.Add(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Validates XML document against XML Schema
        /// </summary>
        /// <param name="xmlContent">string containing XML document to validate</param>
        /// <param name="schemaPath">Path and filename of XML Schema</param>
        /// <returns>True when validation was OK, otherwise false</returns>
        public bool ValidateWithSchemaFile(string xmlContent, string schemaPath)
        {
            Errors.Clear();
            if (Errors.Count > 0)
                return false;

            if (!File.Exists(schemaPath))
                Errors.Add("XML Schema does not exists.");

            return Validate(xmlContent, File.ReadAllText(schemaPath));
        }

        /// <summary>
        /// Validates XML document against XML Schema
        /// </summary>
        /// <param name="xmlPath"> Path and filename of XML document to validate</param>
        /// <param name="schemaContent">String containing XML Schema</param>
        /// <returns>True when validation was OK, otherwise false</returns>
        public bool ValidateWithXmlFile(string xmlPath, string schemaContent)
        {
            Errors.Clear();
            if (Errors.Count > 0)
                return false;

            if (!File.Exists(xmlPath))
                Errors.Add("XML Document does not exists.");

            return Validate(File.ReadAllText(xmlPath),schemaContent);
        }

        /// <summary>
        /// Handles validation events and collecting errors
        /// </summary>
        void settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Errors.Add(e.Message);
        }


    }
}
