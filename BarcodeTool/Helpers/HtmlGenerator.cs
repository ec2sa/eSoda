using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace BarcodeToolkit
{
    public class HtmlGenerator
    {
        private string _htmlFormat { 
            get
            {
                return 
@"<html> 
  <head> <style type='text/css'>*{margin:0;padding:0;}</style></head>
  <body onload='window.print();'> 
    <div id='BarcodeImage' style='position:absolute; left:{left};top:{top};'> 
      <img src='data:image/png;base64,{src}' /> 
    </div> 
  </body> 
</html>";
            }
        }
        private string _html { get; set; }
        private string _filename { get; set; }

        public void InsertCode(string codeFileName, int left, int top)
        {
            _html = _htmlFormat
                .Replace("{left}", string.Format("{0}mm", left))
                .Replace("{top}", string.Format("{0}mm", top))
                .Replace("{src}", string.Format("{0}", codeFileName));
        }

        public void InsertCode(byte[] imagaContent, int left, int top)
        {
            _html = _htmlFormat
                .Replace("{left}", string.Format("{0}mm", left))
                .Replace("{top}", string.Format("{0}mm", top))
                .Replace("{src}", string.Format("{0}", Convert.ToBase64String(imagaContent)));
        }

        public string GetHTML()
        {
            return _html;
        }

        public void Save(string filename)
        {
            _filename = filename;

            File.WriteAllText(_filename, _html);
        }

        public void Open()
        {
            Process.Start(_filename);
        }
    }
}
