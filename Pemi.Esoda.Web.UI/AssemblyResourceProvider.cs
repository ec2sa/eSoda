using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Hosting;
using System.IO;
using System.Reflection;

namespace Pemi.Esoda.Web.UI
{
    public class AssemblyResourceProvider : VirtualPathProvider
    {
        public AssemblyResourceProvider() { }
        private bool IsAppResourcePath(string virtualPath)
        {
            String checkPath =
               VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/CustomForms/", StringComparison.InvariantCultureIgnoreCase);
        }
        public override bool FileExists(string virtualPath)
        {
            return (IsAppResourcePath(virtualPath) || base.FileExists(virtualPath));
        }
        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsAppResourcePath(virtualPath))
                return new AssemblyResourceVirtualFile(virtualPath);
            else
                return base.GetFile(virtualPath);
        }
        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsAppResourcePath(virtualPath))
                return null;
            else
                return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        static AssemblyResourceProvider()
        {
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new AssemblyResourceProvider());
        }
    }

    class AssemblyResourceVirtualFile : VirtualFile
    {
        string path;
        public AssemblyResourceVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            path = VirtualPathUtility.ToAppRelative(virtualPath);
        }
        public override System.IO.Stream Open()
        {
            
            string[] parts = path.Split('/');
            string assemblyName = parts[2];
            string resourceName = parts[3];

            try
            {
               
              // assemblyName = Path.Combine(HttpRuntime.BinDirectory, string.Format(@"{1}\{0}", assemblyName, assemblyName.Replace(".dll", "")));
                assemblyName = Path.Combine(HttpRuntime.BinDirectory, string.Format(@"{1}\{0}.dll", assemblyName, assemblyName));


                System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(assemblyName);
                if (assembly != null)
                {
                    Stream s = assembly.GetManifestResourceStream(resourceName);
                    if (s == null)
                        throw new ArgumentException(string.Format("Unable to find \"{0}\" resource in assembly manifest", resourceName));


                    return s;
                }
            }
            catch
            {
                throw new ArgumentException("Unable to load assembly " +Path.GetFileName(assemblyName));
            }
            return null;
        }
    }

}
