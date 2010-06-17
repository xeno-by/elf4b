using System;
using System.IO;
using System.Reflection;

namespace Playground.Helpers
{
    public static class ResourceHelper
    {
        public static String ReadFromResource(String resource)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            using (var rStream = thisAssembly.GetManifestResourceStream(resource))
            {
                if (rStream == null)
                {
                    throw new NotSupportedException(String.Format(
                        "Resource '{0}' not found.", resource));
                }
                else
                {
                    return new StreamReader(rStream).ReadToEnd();
                }
            }
        }
    }
}