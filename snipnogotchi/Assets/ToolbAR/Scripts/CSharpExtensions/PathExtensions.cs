using System.IO;

namespace ToolbAR.CSharpExtensions
{
    public static class PathExtensions
    {

        public static string getRelativePathTo(this string path, string basePath)
        {
            System.Uri uri1 = new System.Uri(path);
            System.Uri uri2 = new System.Uri(basePath);
            System.Uri relativeUri = uri2.MakeRelativeUri(uri1);
            return relativeUri.ToString();
        }
    }
}