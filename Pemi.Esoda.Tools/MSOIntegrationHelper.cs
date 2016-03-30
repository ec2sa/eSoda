using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.CustomXmlDataProperties;
using System.Xml;
using System.IO;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
using System.Configuration;
using System.Xml.Schema;


namespace Pemi.Esoda.Tools.MSOIntegrationHelper
{
    #region MSOGenerationHelper class
    public class MSOGenerationHelper
    {
        #region Custom XML structure

        #region BuildCustomXmlStructure
        /// <summary>
        /// Builds the custom XML structure.
        /// </summary>
        /// <param name="mainPart">The main part.</param>
        /// <returns></returns>
        public List<CustomXmlElement> BuildCustomXmlStructure(MainDocumentPart mainPart)
        {
            //list of root CustomXmlElements (usually there will be just one)
            List<CustomXmlElement> customXmlElements = new List<CustomXmlElement>();
            CustomXmlElement customXmlElement = null;

            //for each OpenXmlElement in document body
            for (int i = 0; i < mainPart.Document.Body.Elements<OpenXmlElement>().Count(); ++i)
            {
                OpenXmlElement element = mainPart.Document.Body.Elements<OpenXmlElement>().ElementAt(i);

                //check whether the element is of type: CustomXmlRun, CustomXmlBlock, CustomXmlCell or CusotmXmlRow
                if (CustomXmlElement.IsElementValid(element))
                {
                    //add new CustomXmlElement to the list 
                    customXmlElement = new CustomXmlElement(element, null);
                    customXmlElements.Add(customXmlElement);
                }

                //call method to build the CustomXml elements structure for the given OpenXml element
                BuildCustomXmlStructure(element, ref customXmlElement);
            }
            return customXmlElements;
        }
        #endregion

        #region BuildCustomXmlStructure
        /// <summary>
        /// Builds the custom XML structure.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="customXmlElement">The custom XML element.</param>
        private void BuildCustomXmlStructure(OpenXmlElement element, ref CustomXmlElement customXmlElement)
        {
            //for each OpenXmlElement in element
            for (int i = 0; i < element.Elements<OpenXmlElement>().Count(); ++i)
            {
                OpenXmlElement currentElement = element.Elements().ElementAt(i);
                CustomXmlElement currentCustomXmlElement = customXmlElement;

                //check whether current element is of type: CustomXmlRun, CustomXmlBlock, CustomXmlCell or CustomXmlRow
                if (CustomXmlElement.IsElementValid(currentElement))
                {
                    //check whether the custom xml element exists
                    if (customXmlElement == null)
                        //create new custom xml element
                        customXmlElement = new CustomXmlElement(currentElement, null);
                    else
                        //add child custom xml element
                        currentCustomXmlElement = customXmlElement.AddChildElement(new CustomXmlElement((OpenXmlElement)currentElement, customXmlElement));
                }
                //if current element is of type CustomXmlAttribute
                else if (currentElement is CustomXmlAttribute)
                {
                    //check whether the custom xml element exists
                    if (customXmlElement != null)
                    {
                        //add attribute
                        currentCustomXmlElement.AddAttribute((CustomXmlAttribute)currentElement);
                    }
                }
                //if current element is of type DataBinding and store item id is not an empty string
                else if (currentElement is DataBinding && string.IsNullOrEmpty(currentCustomXmlElement.StoreItemID))
                {
                    //check whether the custom xml element exists
                    if (customXmlElement != null)
                    {
                        //assign store item id from element to current custom xml element
                        currentCustomXmlElement.StoreItemID = ((DataBinding)currentElement).StoreItemId;
                    }
                }
                //if current element is of type BookmarkStart and property Bookmark for current custom xml element is not set
                else if (currentElement is BookmarkStart && currentCustomXmlElement.Bookmark == null)
                {
                    currentCustomXmlElement.Bookmark = (BookmarkStart)currentElement;
                }

                //build custom xml structure for current element
                BuildCustomXmlStructure(currentElement, ref currentCustomXmlElement);
            }
        }
        #endregion

        #region GetStoreItemID
        /// <summary>
        /// Gets the store item ID.
        /// </summary>
        /// <param name="customXmlElements">The custom XML elements.</param>
        /// <returns></returns>
        public string GetStoreItemID(List<CustomXmlElement> customXmlElements)
        {
            if (customXmlElements.Count > 0)
            {
                foreach (CustomXmlElement customXmlElement in customXmlElements)
                {
                    string storeItemID = customXmlElement.StoreItemID;
                    if (!string.IsNullOrEmpty(storeItemID))
                        return storeItemID;
                }
            }
            return null;
        }
        #endregion

        #endregion

        #region Data bindings

        #region GenerateDataBindings
        /// <summary>
        /// Generates the data bindings.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="nsManager">The ns manager.</param>
        /// <param name="element">The element.</param>
        public void GenerateDataBindings(XmlNode node, XmlNamespaceManager nsManager, string customXmlPartID, ref CustomXmlElement element)
        {
            XmlNode currentNode = node;
            CustomXmlElement currentElement = element;

            //for each child element in custom xml element
            for (int i = 0; i < element.ChildElementsCount; ++i)
            {
                currentElement = element.GetChildElementAt(i);
                string uri = string.Empty;

                //check whether the current element and node are not nulls
                if (currentElement != null && node != null)
                {
                    //retrieve namespace prefix
                    string namespacePrefix = GetNamespacePrefix(currentElement.Uri, nsManager);

                    //set current xml node by selecting it based on element name
                    currentNode = node.SelectSingleNode(namespacePrefix + currentElement.Name + "[" + currentElement.Number + "]", nsManager);

                    //check whether the current node is not null
                    if (currentNode != null)
                    {
                        bool hasChildren = false;

                        //determine whether the current node has any child elements that are of type XmlElement
                        foreach (XmlNode childNode in currentNode.ChildNodes)
                        {
                            //if child node is of type XmlElement
                            if (childNode is XmlElement)
                            {
                                //set hasChildren to true
                                hasChildren = true;
                                break;
                            }
                        }

                        //if the current node has children
                        if (!hasChildren)
                        {
                            //retrieve current element's element 
                            OpenXmlElement openXmlElement = currentElement.Element;
                            //create data binding for the current node and open xml element
                            CreateDataBinding(currentNode, customXmlPartID, nsManager, ref openXmlElement);
                        }
                    }
                    //else
                    //{
                    //    throw new Exception("Wystąpiła niezgodność pomiędzy dokumentem Word i danymi XML.");
                    //}
                }
                //generate data bindings for current node and current element
                GenerateDataBindings(currentNode, nsManager, customXmlPartID, ref currentElement);
            }
        }
        #endregion

        #region CreateDataBinding
        /// <summary>
        /// Creates the data binding.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="element">The element.</param>
        private void CreateDataBinding(XmlNode node, string customXmlPartID, XmlNamespaceManager nsManager, ref OpenXmlElement element)
        {
            //for each open xml element in element
            for (int i = 0; i < element.Elements<OpenXmlElement>().Count(); ++i)
            {
                //assign element at i index to child
                OpenXmlElement child = element.Elements<OpenXmlElement>().ElementAt(i);
                //if child is of type SdtProperties
                if (child is DocumentFormat.OpenXml.Wordprocessing.SdtProperties)
                {
                    //if number of DataBinding elements within SdtProperties is equal to 0
                    if (child.Elements<DocumentFormat.OpenXml.Wordprocessing.DataBinding>().Count() == 0)
                    {
                        //create new DataBinding element
                        DocumentFormat.OpenXml.Wordprocessing.DataBinding dataBinding = new DocumentFormat.OpenXml.Wordprocessing.DataBinding();

                        string namespacePrefix = string.Empty;
                        string namespaceUri = string.Empty;

                        //check whether node is of type XmlAttribute
                        if (node is XmlAttribute)
                        {
                            //retrieve attribute's owner
                            XmlNode owner = ((XmlAttribute)node).OwnerElement;
                            //get namespace prefix 
                            namespacePrefix = GetNamespacePrefix(owner.NamespaceURI, nsManager);
                            //assign namespace uri
                            namespaceUri = ((XmlAttribute)node).OwnerElement.NamespaceURI;
                        }
                        else
                        {
                            //retrieve namespace prefix
                            namespacePrefix = GetNamespacePrefix(node.NamespaceURI, nsManager);
                            //assign namespace uri
                            namespaceUri = node.NamespaceURI;
                        }

                        string prefixMappings = GetPrefixMappings(node, nsManager);

                        //if prefix mappings is not an empty string
                        if (!string.IsNullOrEmpty(prefixMappings))
                        {
                            //create value for data binding's prefix mappings
                            dataBinding.PrefixMappings = prefixMappings;
                        }
                        //retrieve xpath for the data binding element
                        dataBinding.XPath = GetXPath(node, nsManager);
                        //check whether the custom xml part ID is not an empty string
                        if (!string.IsNullOrEmpty(customXmlPartID))
                            //assign custom xml part ID to data binding's store item ID
                            dataBinding.StoreItemId = customXmlPartID;

                        //append data binding to SdtProperties element
                        child.AppendChild(dataBinding);
                        return;
                    }
                }
                else
                {
                    //try to create data binding for child element
                    CreateDataBinding(node, customXmlPartID, nsManager, ref child);
                }
            }
        }
        #endregion

        #endregion

        #region Custom controls

        #region GenerateCustomControls
        /// <summary>
        /// Generates the custom controls.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="nsManager">The ns manager.</param>
        /// <param name="elementsDictionary">The elements dictionary.</param>
        /// <param name="element">The element.</param>
        public void GenerateCustomControls(XmlNode node, XmlNamespaceManager nsManager, string customXmlPartID, ref CustomXmlElement element)
        {
            //check whether node is not null and has child nodess
            if (node != null && node.HasChildNodes)
            {
                //assign child nodes of the node to new variable
                XmlNodeList nodes = node.ChildNodes;
                //for each node in child nodes
                for (int i = 0; i < nodes.Count; ++i)
                {
                    //assign current node to the new variable
                    XmlNode currentNode = nodes[i];
                    //if current node is of type XmlElement
                    if (currentNode is XmlElement)
                    {
                        //retrieve namespace prefix
                        string namespacePrefix = GetNamespacePrefix(currentNode.NamespaceURI, nsManager);
                        //select other nodes with the same name and uri from the parent node
                        XmlNodeList twinNodes = node.SelectNodes(namespacePrefix + currentNode.LocalName, nsManager);
                        //retrieve number of the same elements from current xml element
                        int count = GetNumberOfTwinElements(element, currentNode.LocalName);

                        CustomXmlElement newCustomXmlElement = element;
                        //if number of the same nodes in xml is greater than the number of same custom xml elements
                        if (twinNodes.Count > count)
                        {
                            //if number of the same custom xml elements is greater than 0
                            if (count > 0)
                            {
                                string nodeName = currentNode.LocalName;
                                XmlNode twinNode = currentNode.NextSibling;

                                //try to find the next xml sibling with the same name
                                while (twinNode != null && twinNode.NextSibling != null)
                                {
                                    if (twinNode.LocalName == nodeName)
                                        break;
                                    twinNode = twinNode.NextSibling;
                                }
                                //create custom control
                                newCustomXmlElement = CreateCustomControl(twinNode != null ? twinNode : currentNode, nodeName, nsManager, ref element);
                            }
                            else
                            {
                                string elementName = string.Empty;
                                //retrieve previous sibling's name
                                if (currentNode.PreviousSibling != null)
                                    elementName = currentNode.PreviousSibling.LocalName;

                                OpenXmlElement newElement = new CustomXmlBlock();
                                //retrieve open xml element used to insert newly created element
                                OpenXmlElement elementToInsert = element.Element;
                                OpenXmlElement elementToInsertAfter = null;

                                //find custom xml element after which the newly created element is to be inserted
                                CustomXmlElement foundElement = FindElement(currentNode.PreviousSibling, ref element);

                                //if a found element is not null
                                if (foundElement != null)
                                {
                                    //assign element to insert as a found element's parent
                                    elementToInsert = foundElement.Element.Parent;
                                    //assign element to insert after as a found element
                                    elementToInsertAfter = foundElement.Element;
                                }

                                //in case the element to insert after is not null
                                if (elementToInsertAfter != null)
                                    //insert newly created element after the element found
                                    elementToInsert.InsertAfter(newElement, elementToInsertAfter);
                                else
                                    //insert newly created element at position = 0
                                    elementToInsert.InsertAfter(newElement, elementToInsert.Last());

                                //create new custom xml element
                                newCustomXmlElement = new CustomXmlElement(newElement, element);
                                //and add it to the main element 
                                element.AddChildElement(newCustomXmlElement);
                                //create custom block for current node and new element
                                CreateCustomXmlBlock(currentNode, customXmlPartID, nsManager, ref newElement);
                            }
                        }

                        CustomXmlElement childElement = null;
                        //for each child element in element
                        for (int j = 0; j < element.ChildElementsCount; ++j)
                        {
                            childElement = element.GetChildElementAt(j);
                            //check whether the child element's uri and name are the same as uri and name associated with the current node
                            if (childElement.Uri == currentNode.NamespaceURI && childElement.Name == currentNode.LocalName)
                                break;
                        }
                        //Generate custom controls for current node and child element
                        GenerateCustomControls(currentNode, nsManager, customXmlPartID, ref childElement);
                    }
                }
            }
        }
        #endregion

        #region CreateCustomControl
        /// <summary>
        /// Creates the custom control.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="nsManager">The ns manager.</param>
        /// <param name="element">The element.</param>
        private CustomXmlElement CreateCustomControl(XmlNode node, string elementName, XmlNamespaceManager nsManager, ref CustomXmlElement element)
        {
            //find custom xml element to copy

            CustomXmlElement newCustomXmlElement = null;
            CustomXmlElement customXmlElementToCopy = null;

            //for each child element in custom xml element
            for (int i = 0; i < element.ChildElementsCount; ++i)
            {
                //if child element's is the same as the element name passed as an input parameter
                if (element.GetChildElementAt(i).Name == elementName)
                    //assign custom xml element to copy
                    customXmlElementToCopy = element.GetChildElementAt(i);
            }

            //try to find a parent table row of the custom xml element to copy and assign it to the open xml element to copy
            OpenXmlElement elementToCopy = GetParentTableRow(customXmlElementToCopy.Element);

            //if element to copy is null
            if (elementToCopy == null)
                //assign element to copy an element from custom xml element to copy 
                elementToCopy = customXmlElementToCopy.Element;

            //if element to copy is not null
            if (elementToCopy != null)
            {
                //retrieve namespace prefix
                string namespacePrefix = GetNamespacePrefix(node.NamespaceURI, nsManager);
                //retrieve x path
                string xpath = GetXPath(node, nsManager);

                //clone element to copy and assign it to the new open xml element
                OpenXmlElement newElement = (OpenXmlElement)elementToCopy.Clone();

                //update custom control properties
                UpdateCustomControlProperties(xpath, ref newElement);

                //try to retrieve and assign a parent table to an element used to insert the newly created element
                OpenXmlElement elementToInsert = GetParentTable(elementToCopy);
                //assign element after which the newly created element is to be inserted
                OpenXmlElement elementToInsertAfter = elementToCopy;

                //if element to insert is null
                if (elementToInsert == null)
                {
                    CustomXmlElement foundElement = null;
                    if (node.PreviousSibling != null)
                        foundElement = FindElement(node.PreviousSibling, ref element);
                    if (foundElement != null)
                    {
                        elementToInsert = foundElement.Element.Parent;
                        elementToInsertAfter = foundElement.Element;
                    }
                    else
                    {
                        elementToInsert = elementToCopy.Parent;
                        elementToInsertAfter = elementToCopy;
                    }
                }

                elementToInsert.InsertAfter(newElement, elementToInsertAfter);

                if (CustomXmlElement.IsElementValid(newElement))
                {
                    newCustomXmlElement = new CustomXmlElement(newElement, element);
                    BuildCustomXmlStructure(newElement, ref newCustomXmlElement);
                    element.AddChildElement(newCustomXmlElement);
                }
                else
                    BuildCustomXmlStructure(newElement, ref element);
            }

            return newCustomXmlElement;
        }
        #endregion

        #region UpdateCustomControlProperties
        /// <summary>
        /// Updates the custom control properties.
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <param name="element">The element.</param>
        private void UpdateCustomControlProperties(string xpath, ref OpenXmlElement element)
        {
            for (int i = 0; i < element.ChildElements.Count(); ++i)
            {
                var child = element.ChildElements.ElementAt(i);
                if (child is DocumentFormat.OpenXml.Wordprocessing.DataBinding)
                {
                    int index = xpath.LastIndexOf("[") + 1;
                    int length = xpath.LastIndexOf("]") - index;
                    int val = int.Parse(xpath.Substring(index, length));
                    ((DocumentFormat.OpenXml.Wordprocessing.DataBinding)child).XPath.Value = ((DocumentFormat.OpenXml.Wordprocessing.DataBinding)child).XPath.Value.Remove(index, length).Insert(index, val.ToString());
                }
                else if (child is SdtId)
                {
                    ((SdtId)child).Val += 123456; //?? - to do...
                }

                UpdateCustomControlProperties(xpath, ref child);
            }
        }
        #endregion

        #region GetNumberOfTwinElements
        /// <summary>
        /// Gets the number of twin elements.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns></returns>
        private int GetNumberOfTwinElements(CustomXmlElement element, string elementName)
        {
            int count = 0;
            for (int i = 0; i < element.ChildElementsCount; ++i)
            {
                if (element.GetChildElementAt(i).Name == elementName)
                {
                    ++count;
                }
            }
            return count;
        }
        #endregion

        #endregion

        #region Attributes

        #region GenerateAttributes
        /// <summary>
        /// Generates the attributes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="nsManager">The ns manager.</param>
        /// <param name="elementsDictionary">The elements dictionary.</param>
        /// <param name="customXmlElement">The custom XML element.</param>
        public void GenerateAttributes(XmlNode node, XmlNamespaceManager nsManager, ref CustomXmlElement customXmlElement)
        {
            if (node != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Name.StartsWith("xmlns"))
                        continue;

                    bool found = false;
                    for (int i = 0; i < customXmlElement.AttributesCount; ++i)
                    {
                        if (attribute.LocalName == customXmlElement.GetAttributeAt(i).Name)
                        {
                            customXmlElement.GetAttributeAt(i).Val = attribute.Value;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        CreateAttribute(attribute, ref customXmlElement);
                    }
                }


                for (int i = 0; i < customXmlElement.ChildElementsCount; ++i)
                {
                    CustomXmlElement currentCustomXmlElement = customXmlElement.GetChildElementAt(i);
                    XmlNode currentNode = node;
                    string namespacePrefix = GetNamespacePrefix(currentCustomXmlElement.Uri, nsManager);

                    currentNode = node.SelectSingleNode(namespacePrefix + currentCustomXmlElement.Name + "[" + currentCustomXmlElement.Number + "]", nsManager);

                    GenerateAttributes(currentNode, nsManager, ref currentCustomXmlElement);
                }
            }
            //else
            //{
            //    throw new Exception("Niezgodność pomiędzy dokumentem Word i danymi XML.");
            //}
        }
        #endregion

        #region CreateAttribute
        /// <summary>
        /// Generates the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="customXmlElement">The custom XML element.</param>
        private void CreateAttribute(XmlAttribute attribute, ref CustomXmlElement customXmlElement)
        {
            for (int i = 0; i < customXmlElement.Element.Elements<OpenXmlElement>().Count(); ++i)
            {
                OpenXmlElement element = customXmlElement.Element.Elements<OpenXmlElement>().ElementAt(i);
                if (element is CustomXmlProperties)
                {
                    CustomXmlAttribute customXmlAttribute = new CustomXmlAttribute();
                    customXmlAttribute.Name = attribute.LocalName;
                    customXmlAttribute.Val = attribute.Value;
                    element.AppendChild<CustomXmlAttribute>(customXmlAttribute);
                    customXmlElement.AddAttribute(customXmlAttribute);
                }
            }
        }
        #endregion

        #endregion

        #region Custom xml blocks

        #region GenerateCustomXmlBlocks
        /// <summary>
        /// Generates the custom XML blocks.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="element">The element.</param>
        public void GenerateCustomXmlBlocks(XmlNode xmlNode, string customXmlPartID, XmlNamespaceManager nsManager, ref OpenXmlElement element)
        {
            XmlNodeList nodes = xmlNode.ChildNodes;

            foreach (XmlAttribute attribute in xmlNode.Attributes)
            {
                if (attribute.LocalName.StartsWith("xmlns"))
                    continue;

                DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph = CreateParagraph(attribute);
                element.AppendChild<DocumentFormat.OpenXml.Wordprocessing.Paragraph>(paragraph);

                OpenXmlElement sdtRun = CreateSdtRun();
                paragraph.AppendChild<OpenXmlElement>(sdtRun);

                CreateDataBinding(attribute, customXmlPartID, nsManager, ref sdtRun);
            }

            foreach (XmlNode node in nodes)
            {
                if (node is XmlElement)
                {
                    OpenXmlElement newElement = new CustomXmlBlock();

                    CreateCustomXmlBlock(node, customXmlPartID, nsManager, ref newElement);

                    element.AppendChild(newElement);

                    GenerateCustomXmlBlocks(node, customXmlPartID, nsManager, ref newElement);
                }
            }
        }
        #endregion

        #region CreateCustomXmlBlock
        /// <summary>
        /// Creates the custom XML block.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="element">The element.</param>
        private void CreateCustomXmlBlock(XmlNode xmlNode, string customXmlPartID, XmlNamespaceManager nsManager, ref OpenXmlElement element)
        {
            //determine whether the xml node has children
            bool hasChildren = false;
            //for each child node in xml node
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                //check whether there is at least one child node that is of type XmlElement
                if (node is XmlElement)
                {
                    hasChildren = true;
                    break;
                }
            }

            //assign element name to the custom xml block
            ((CustomXmlBlock)element).Element = new StringValue(xmlNode.LocalName);
            //create paragraph
            DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph = CreateParagraph(xmlNode);
            //append paragraph to the current element
            element.AppendChild<DocumentFormat.OpenXml.Wordprocessing.Paragraph>(paragraph);

            //if the xml node has not any children 
            if (!hasChildren)
            {
                //create sdt run
                SdtRun sdtRun = CreateSdtRun();
                //append sdt run to the paragraph
                paragraph.AppendChild<SdtRun>(sdtRun);
                //create data binding for the current element
                CreateDataBinding(xmlNode, customXmlPartID, nsManager, ref element);
            }
        }
        #endregion

        #region CreateParagraph
        /// <summary>
        /// Creates the paragraph.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateParagraph(XmlNode node)
        {
            DocumentFormat.OpenXml.Wordprocessing.Paragraph paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            ParagraphProperties paragraphProperties = new ParagraphProperties();
            RunProperties runProperties = new RunProperties();
            Bold bold = new Bold();
            runProperties.AppendChild<Bold>(bold);
            paragraphProperties.AppendChild<RunProperties>(runProperties);
            paragraph.AppendChild<ParagraphProperties>(paragraphProperties);
            Run run = new Run();
            Text text = new Text();
            text.PreserveSpace = true;
            text.Text = node.LocalName + " ";
            run.AppendChild<Text>(text);
            paragraph.AppendChild<Run>(run);

            return paragraph;
        }
        #endregion

        #region CreateSdtRun
        /// <summary>
        /// Creates the SDT run.
        /// </summary>
        /// <returns></returns>
        private SdtRun CreateSdtRun()
        {
            SdtRun sdtRun = new SdtRun();
            SdtProperties sdtProperties = new SdtProperties();
            sdtRun.AppendChild<SdtProperties>(sdtProperties);
            SdtPlaceholder placeHolder = new SdtPlaceholder();
            sdtProperties.AppendChild<SdtPlaceholder>(placeHolder);
            DocPartReference docPart = new DocPartReference();
            placeHolder.AppendChild<DocPartReference>(docPart);
            docPart.Val = "DefaultPlaceholder_22675703";
            TextSdt textSdt = new TextSdt();
            sdtProperties.AppendChild<TextSdt>(textSdt);

            SdtContentRun sdtContentRun = new SdtContentRun();
            Run sdtContentRunRun = new Run();
            RunProperties sdtContentRunProperties = new RunProperties();
            sdtContentRunProperties.RunStyleId = new RunStyleId();
            sdtContentRunProperties.RunStyleId.Val = "Tekstzastpczy";
            Text sdtContentText = new Text();
            sdtContentText.PreserveSpace = true;
            sdtContentText.Text = "Kliknij tutaj, aby wprowadzić tekst.";
            sdtContentRunRun.AppendChild<RunProperties>(sdtContentRunProperties);
            sdtContentRunRun.AppendChild<Text>(sdtContentText);
            sdtContentRun.AppendChild<Run>(sdtContentRunRun);
            sdtRun.AppendChild<SdtContentRun>(sdtContentRun);

            return sdtRun;
        }
        #endregion

        #endregion

        #region Common

        #region GetNamespacePrefix
        /// <summary>
        /// Sets the namespace.
        /// </summary>
        /// <param name="namespaceUri">The namespace URI.</param>
        /// <param name="namespacePrefix">The namespace prefix.</param>
        /// <param name="nsManager">The ns manager.</param>
        /// <returns></returns>
        public string GetNamespacePrefix(string namespaceUri, XmlNamespaceManager nsManager)
        {
            return (string.IsNullOrEmpty(nsManager.LookupPrefix(namespaceUri)) ? string.Empty : nsManager.LookupPrefix(namespaceUri) + ":");
        }
        #endregion

        #region SetNamespaces
        /// <summary>
        /// Sets the namespaces.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="index">The index.</param>
        /// <param name="nsManager">The ns manager.</param>
        public void SetNamespaces(XmlNode node, ref int index, ref XmlNamespaceManager nsManager)
        {
            if (node != null && nsManager != null)
            {
                string prefix = string.Empty;
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    Dictionary<string, string> namespaces = (Dictionary<string, string>)nsManager.GetNamespacesInScope(XmlNamespaceScope.All);
                    if (!string.IsNullOrEmpty(childNode.NamespaceURI) && childNode.Prefix != "xmlns" && childNode.Prefix != "xml" && !namespaces.Values.Contains(childNode.NamespaceURI, StringComparer.InvariantCulture))
                    {
                        prefix = childNode.Prefix;
                        if (string.IsNullOrEmpty(prefix))
                        {
                            prefix = "ns" + index.ToString();
                            ++index;
                        }

                        nsManager.AddNamespace(prefix, childNode.NamespaceURI);
                    }
                    SetNamespaces(childNode, ref index, ref nsManager);
                }
            }
        }
        #endregion

        #region GetXPath
        /// <summary>
        /// Gets the X path.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="namespacePrefix">The namespace prefix.</param>
        /// <returns></returns>
        private string GetXPath(XmlNode node, XmlNamespaceManager nsManager)
        {
            Dictionary<string, string> xpathElements = new Dictionary<string, string>();

            if (node is XmlAttribute)
            {
                xpathElements.Add("@" + node.LocalName, null);
                node = ((XmlAttribute)node).OwnerElement;
            }

            while (node.ParentNode != null)
            {
                int index = 1;
                string name = node.LocalName;
                string uri = node.NamespaceURI;
                while (node.PreviousSibling != null && !(node.PreviousSibling is XmlDeclaration))
                {
                    node = node.PreviousSibling;
                    if (name == node.LocalName)
                        ++index;
                }
                if (node is XmlElement)
                    xpathElements.Add(name + "[" + index + "]", uri);
                node = node.ParentNode;
            }
            StringBuilder xpath = new StringBuilder();
            for (int i = xpathElements.Count - 1; i >= 0; --i)
            {
                xpath.Append("/" + GetNamespacePrefix(xpathElements.ElementAt(i).Value, nsManager) + xpathElements.ElementAt(i).Key);
            }
            return xpath.ToString();
        }
        #endregion

        #region GetPrefixMappings
        /// <summary>
        /// Gets the prefix mappings.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="nsManager">The ns manager.</param>
        /// <returns></returns>
        private string GetPrefixMappings(XmlNode node, XmlNamespaceManager nsManager)
        {
            Dictionary<string, string> namespaces = (Dictionary<string, string>)nsManager.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml);
            StringBuilder prefixMappings = new StringBuilder();
            foreach (KeyValuePair<string, string> nspace in namespaces)
            {
                prefixMappings.Append("xmlns:" + nspace.Key + "='" + nspace.Value + "' ");
            }
            return prefixMappings.ToString();
        }
        #endregion

        #region FindElement
        /// <summary>
        /// Finds the element.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private CustomXmlElement FindElement(XmlNode node, ref CustomXmlElement element)
        {
            if (node != null)
            {
                int nodeCount = 0;
                string nodeName = node.LocalName;

                while (node.PreviousSibling != null)
                {
                    node = node.PreviousSibling;
                    if (node.LocalName == nodeName)
                        ++nodeCount;
                }

                int elementCount = 0;
                for (int i = 0; i < element.ChildElementsCount; ++i)
                {
                    CustomXmlElement currentElement = element.GetChildElementAt(i);
                    if (currentElement.Name == nodeName)
                    {
                        if (elementCount == nodeCount)
                            return currentElement;
                        ++elementCount;
                    }
                }
            }
            return null;
        }
        #endregion

        #region GetParentTableRow
        /// <summary>
        /// Gets the parent table row.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private OpenXmlElement GetParentTableRow(OpenXmlElement element)
        {
            while (element.Parent != null)
            {
                if (element.Parent is DocumentFormat.OpenXml.Wordprocessing.TableRow)
                {
                    return element.Parent;
                }
                element = element.Parent;
            }
            return null;
        }
        #endregion

        #region GetParentTable
        /// <summary>
        /// Gets the parent table.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private OpenXmlElement GetParentTable(OpenXmlElement element)
        {
            while (element.Parent != null)
            {
                if (element.Parent is DocumentFormat.OpenXml.Wordprocessing.Table)
                {
                    return element.Parent;
                }
                element = element.Parent;
            }
            return null;
        }
        #endregion

        #endregion
    }
    #endregion

    #region CustomXmlElement class
    public class CustomXmlElement
    {
        #region Fields and properties

        /// <summary>
        /// Parent element
        /// </summary>
        private CustomXmlElement _parent;

        /// <summary>
        /// The element
        /// </summary>
        public OpenXmlElement Element { get; set; }

        /// <summary>
        /// List of child elements
        /// </summary>
        private List<CustomXmlElement> _childElements;

        /// <summary>
        /// List of attributes
        /// </summary>
        private List<CustomXmlAttribute> _attributes;

        private string _storeItemID;
        /// <summary>
        /// Id of store item associated with the element
        /// </summary>
        public string StoreItemID
        {
            get
            {
                if (string.IsNullOrEmpty(_storeItemID))
                {
                    foreach (CustomXmlElement childElement in _childElements)
                    {
                        string storeItemID = childElement.StoreItemID;
                        if (!string.IsNullOrEmpty(storeItemID))
                            return storeItemID;
                    }
                }
                else
                {
                    return _storeItemID;
                }

                return null;
            }
            set
            {
                _storeItemID = value;
            }
        }

        /// <summary>
        /// Bookmark within the element
        /// </summary>
        public BookmarkStart Bookmark { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomXmlElement"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="parent">The parent.</param>
        public CustomXmlElement(OpenXmlElement element, CustomXmlElement parent)
        {
            Element = element;
            _parent = parent;
            _childElements = new List<CustomXmlElement>();
            _attributes = new List<CustomXmlAttribute>();
        }
        #endregion

        #region AddChildElement
        /// <summary>
        /// Adds the child element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public CustomXmlElement AddChildElement(CustomXmlElement element)
        {
            _childElements.Add(element);
            return _childElements.Last();
        }
        #endregion

        #region AddAttribute
        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public CustomXmlAttribute AddAttribute(CustomXmlAttribute attribute)
        {
            _attributes.Add(attribute);
            return _attributes.Last();
        }
        #endregion

        #region GetChildElementAt
        /// <summary>
        /// Gets the child element at.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public CustomXmlElement GetChildElementAt(int i)
        {
            return _childElements[i];
        }
        #endregion

        #region GetAttributeAt
        /// <summary>
        /// Gets the attribute at.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public CustomXmlAttribute GetAttributeAt(int i)
        {
            return _attributes[i];
        }
        #endregion

        #region ChildElementsCount
        /// <summary>
        /// Gets the child elements count.
        /// </summary>
        /// <value>The child elements count.</value>
        public int ChildElementsCount
        {
            get
            {
                return _childElements.Count;
            }
        }
        #endregion

        #region AttributesCount
        /// <summary>
        /// Gets the attributes count.
        /// </summary>
        /// <value>The attributes count.</value>
        public int AttributesCount
        {
            get
            {
                return _attributes.Count;
            }
        }
        #endregion

        #region Name
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                if (Element is CustomXmlRun)
                    return ((CustomXmlRun)Element).Element.Value;
                else if (Element is CustomXmlBlock)
                    return ((CustomXmlBlock)Element).Element.Value;
                else if (Element is CustomXmlCell)
                    return ((CustomXmlCell)Element).Element.Value;
                else if (Element is CustomXmlRow)
                    return ((CustomXmlRow)Element).Element.Value;
                else
                    return null;
            }
        }
        #endregion

        #region Uri
        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <value>The URI.</value>
        public string Uri
        {
            get
            {
                if (Element is CustomXmlRun)
                    return ((CustomXmlRun)Element).Uri;
                else if (Element is CustomXmlBlock)
                    return ((CustomXmlBlock)Element).Uri;
                else if (Element is CustomXmlCell)
                    return ((CustomXmlCell)Element).Uri;
                else if (Element is CustomXmlRow)
                    return ((CustomXmlRow)Element).Uri;
                else
                    return null;
            }
        }
        #endregion

        #region Number
        /// <summary>
        /// Gets the number.
        /// </summary>
        /// <value>The number.</value>
        public int Number
        {
            get
            {
                int count = 1;
                if (_parent != null)
                {
                    for (int i = 0; i < _parent.ChildElementsCount; ++i)
                    {
                        if (_parent.GetChildElementAt(i) == this)
                            break;
                        if (_parent.GetChildElementAt(i).Name == this.Name)
                            ++count;
                    }
                }
                return count;
            }
        }
        #endregion

        #region IsElementValid
        /// <summary>
        /// Determines whether [is element valid] [the specified element].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// 	<c>true</c> if [is element valid] [the specified element]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsElementValid(OpenXmlElement element)
        {
            if (element is CustomXmlRun || element is CustomXmlBlock || element is CustomXmlCell || element is CustomXmlRow)
                return true;
            return false;
        }
        #endregion

    }
    #endregion

    #region MSOProcessingHelper class
    public class MSOProcessingHelper
    {
        #region GetMSODirectoryPath
        /// <summary>
        /// Gets the MSO directory path.
        /// </summary>
        /// <returns></returns>
        public static string MSODirectoryPath
        {
            get
            {
                return ConfigurationManager.AppSettings["katalogDokumentow"] + Path.DirectorySeparatorChar + ConfigurationManager.AppSettings["katalogMSO"];
            }
        }
        #endregion

        #region SetDocumentData
        /// <summary>
        /// Sets the document data.
        /// </summary>
        /// <param name="xmlData">The XML data.</param>
        /// <param name="contextIndex">Index of the context.</param>
        /// <param name="customXmlPartID">The custom XML part ID.</param>
        /// <param name="mainPart">The main part.</param>
        public XmlDocument SetDocumentData(Modes mode, int documentID, int index, out string storeItemID, ref MainDocumentPart mainPart)
        {
            XmlDocument xmlData = null;
            storeItemID = null;

            if (mode == Modes.Create)
            {
                if (index != -1)
                {
                    CustomXmlPart customXmlPart = mainPart.CustomXmlParts.ElementAt(index);
                    xmlData = new XmlDocument();
                    xmlData.Load(customXmlPart.GetStream());
                }
            }
            else if (mode == Modes.Edit)
            {
                xmlData = GetCustomFormData(documentID);
            }

            if (xmlData != null)
            {
                CustomXmlPart customXmlPart = null;

                if (index != -1)
                {
                    customXmlPart = mainPart.CustomXmlParts.ElementAt(index);
                    storeItemID = customXmlPart.CustomXmlPropertiesPart.DataStoreItem.ItemId;
                }
                else
                    customXmlPart = mainPart.AddNewPart<CustomXmlPart>();

                if (customXmlPart != null)
                {
                    Stream stream = customXmlPart.GetStream();
                    stream.SetLength(0);
                    xmlData.Save(stream);
                    stream.Close();
                }
            }

            return xmlData;
        }
        #endregion

        #region SetEsodaData - obsolete
        /// <summary>
        /// Sets the esoda data.
        /// </summary>
        /// <param name="documentID">The document ID.</param>
        /// <param name="index">The index.</param>
        /// <param name="withSchema">if set to <c>true</c> [with schema].</param>
        /// <param name="mainPart">The main part.</param>
        //public void SetEsodaData(bool isBin, bool withSchema, int documentID, string guid, string description, int index, ref MainDocumentPart mainPart)
        //{
        //    Stream stream;
        //    XmlDocument eSodaData = null;

        //    if (isBin)
        //        eSodaData = new MSOIntegrationDAO().GetMSOServiceContextBin(documentID, EsodaConfigurationParameters.TicketDuration, string.IsNullOrEmpty(guid) ? null : ((Guid?)new Guid(guid)), description, null);
        //    else
        //        eSodaData = new MSOIntegrationDAO().GetMSOServiceContext(documentID, EsodaConfigurationParameters.TicketDuration, withSchema);

        //    if (eSodaData != null)
        //    {
        //        CustomXmlPart customXmlPart = null;

        //        if (index > -1)
        //        {
        //            customXmlPart = mainPart.CustomXmlParts.ElementAt(index);
        //        }
        //        else
        //        {
        //            customXmlPart = mainPart.AddNewPart<CustomXmlPart>();
        //        }

        //        if (customXmlPart != null)
        //        {
        //            stream = customXmlPart.GetStream();
        //            stream.SetLength(0);
        //            eSodaData.Save(stream);
        //            stream.Close();
        //        }
        //    }
        //}
        #endregion

        #region SetEsodaData
        /// <summary>
        /// Sets the esoda data.
        /// </summary>
        /// <param name="documentID">The document ID.</param>
        /// <param name="index">The index.</param>
        /// <param name="withSchema">if set to <c>true</c> [with schema].</param>
        /// <param name="mainPart">The main part.</param>
        public void SetEsodaData(XmlDocument eSodaData, int index, ref MainDocumentPart mainPart)
        {
            Stream stream;

            if (eSodaData != null)
            {
                CustomXmlPart customXmlPart = null;

                if (index > -1)
                {
                    customXmlPart = mainPart.CustomXmlParts.ElementAt(index);
                }
                else
                {
                    customXmlPart = mainPart.AddNewPart<CustomXmlPart>();
                }

                if (customXmlPart != null)
                {
                    stream = customXmlPart.GetStream();
                    stream.SetLength(0);
                    eSodaData.Save(stream);
                    stream.Close();
                }
            }
        }
        #endregion

        #region GetCustomFormData
        /// <summary>
        /// Gets the custom form data.
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetCustomFormData(int documentID)
        {
            CustomFormDTO cusotmFormDTO = new CustomFormDAO().GetCustomFormData(documentID,false);


            if (cusotmFormDTO != null && !string.IsNullOrEmpty(cusotmFormDTO.XmlData))
            {
                XmlDocument xmlData = new XmlDocument();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(cusotmFormDTO.XmlData));
                xmlData.Load(ms);
                return xmlData;
            }
            return null;
        }
        #endregion

        #region GetCustomXmlPartsIndexes - obsolete
        /// <summary>
        /// Gets the custom XML parts indexes.
        /// </summary>
        /// <param name="mainPart">The main part.</param>
        /// <param name="storeItemID">The store item ID.</param>
        /// <returns></returns>
        //public Dictionary<string, int> GetCustomXmlPartsIndexes(MainDocumentPart mainPart, string storeItemID)
        //{
        //    Dictionary<string, int> customXmlPartsIndexes = new Dictionary<string, int>();

        //    string targetNamespace = string.Empty;


        //    for (int i = 0; i < mainPart.CustomXmlParts.Count(); ++i)
        //    {
        //        CustomXmlPart customXmlPart = mainPart.CustomXmlParts.ElementAt(i);

        //        if (!string.IsNullOrEmpty(storeItemID) && (customXmlPart.CustomXmlPropertiesPart != null && customXmlPart.CustomXmlPropertiesPart.DataStoreItem != null && customXmlPart.CustomXmlPropertiesPart.DataStoreItem.ItemId == storeItemID))
        //            customXmlPartsIndexes.Add("xmlData", i);
        //        else
        //        {
        //            XmlDocument content = new XmlDocument();
        //            content.Load(customXmlPart.GetStream());
        //            if (content.DocumentElement.NamespaceURI == "http://esoda.pl/schemas/MSOcontext")
        //                customXmlPartsIndexes.Add("context", i);
        //            else if (content.DocumentElement.NamespaceURI.StartsWith("http://schemas.openxmlformats.org"))
        //                continue;
        //            else
        //                customXmlPartsIndexes.Add("xmlData", i);
        //        }
        //    }

        //    return customXmlPartsIndexes;
        //}
        #endregion

        #region GetCustomXmlPartsIndexes
        /// <summary>
        /// Gets the custom XML parts indexes.
        /// </summary>
        /// <param name="mainPart">The main part.</param>
        /// <param name="storeItemID">The store item ID.</param>
        /// <returns></returns>
        public Dictionary<string, int> GetCustomXmlPartsIndexes(MainDocumentPart mainPart, string storeItemID, string[] targetNamespaces)
        {
            Dictionary<string, int> customXmlPartsIndexes = new Dictionary<string, int>();

            for (int i = 0; i < mainPart.CustomXmlParts.Count(); ++i)
            {
                CustomXmlPart customXmlPart = mainPart.CustomXmlParts.ElementAt(i);

                if (!string.IsNullOrEmpty(storeItemID) && (customXmlPart.CustomXmlPropertiesPart != null && customXmlPart.CustomXmlPropertiesPart.DataStoreItem != null && customXmlPart.CustomXmlPropertiesPart.DataStoreItem.ItemId == storeItemID))
                    customXmlPartsIndexes.Add("xmlData", i);
                else
                {
                    XmlDocument content = new XmlDocument();
                    content.Load(customXmlPart.GetStream());

                    //if (content.DocumentElement.NamespaceURI.StartsWith("http://schemas.openxmlformats.org"))
                    //    continue;

                    if (content.DocumentElement.NamespaceURI == "http://esoda.pl/schemas/MSOcontext")
                        customXmlPartsIndexes.Add("context", i);

                    else if (targetNamespaces != null && targetNamespaces.Contains(content.DocumentElement.NamespaceURI))
                        customXmlPartsIndexes.Add("xmlData", i);
                }
            }

            return customXmlPartsIndexes;
        }
        #endregion

        #region GetTargetNamespaces
        /// <summary>
        /// Gets the target namespaces.
        /// </summary>
        /// <param name="eSodaData">The e soda data.</param>
        /// <returns></returns>
        public string[] GetTargetNamespaces(XmlDocument eSodaData)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(eSodaData.NameTable);
            ns.AddNamespace("c", "http://esoda.pl/schemas/MSOcontext");
            ns.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            List<string> targetNamespaces = new List<string>();

            XmlNodeList schemas = eSodaData.DocumentElement.SelectNodes("c:schema/xs:schema", ns);

            foreach (XmlNode schemaNode in schemas)
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(schemaNode.OuterXml)))
                {
                    XmlSchema schema = XmlSchema.Read(ms, null);
                    targetNamespaces.Add(schema.TargetNamespace);
                }
            }

            return targetNamespaces.ToArray<string>();
        }
        #endregion
    }
    #endregion

    #region Modes enum

    public enum Modes { Create, Edit, NotDefined };

    #endregion
}
