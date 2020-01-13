using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// FileProvider Extension
    /// </summary>
    public static class PhysicalFileProviderExtension
    {
        /// <summary>
        /// Get Directory Depth Contents
        /// </summary>
        /// <param name="fileProvider">Physical File Provider</param>
        /// <param name="subpath">Subpath</param>
        /// <returns>Get depth file or directory path for subpath.</returns>
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
