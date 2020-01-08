using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// FileProvider Extension
    /// </summary>
    public static class PhysicalFileProviderExtension
    {
        public static IEnumerable<IFileInfo> GetDirectoryDepthContents(this PhysicalFileProvider fileProvider, string subpath)
        {
            var rootPath = fileProvider.Root;
            List<IFileInfo> fileInfos = new List<IFileInfo>();
            var files = fileProvider.GetDirectoryContents(subpath).ToList();
            foreach (var dir in files.Where(a => a.IsDirectory))
            {
                fileInfos.AddRange(GetDirectoryDepthContents(fileProvider, dir.PhysicalPath.Substring(rootPath.Length)));
            }
            fileInfos.AddRange(files);
            return fileInfos;
        }
    }
}
