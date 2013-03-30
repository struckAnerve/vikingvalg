using System;
using System.IO;
using System.Text;

namespace Demina
{
	public class Util
	{
		// creates a relative path to "targetFile" that is relative to "path"
		public static string CreateRelativePath(string targetFile, string path)
		{
			if ((File.GetAttributes(path) & FileAttributes.Directory) != FileAttributes.Directory)
			{
				path = Path.GetDirectoryName(path);
			}

			string[] absDirs = path.Split('\\');
			string[] relDirs = targetFile.Split('\\');

			// Get the shortest of the two paths
			int length = absDirs.Length < relDirs.Length ? absDirs.Length :
			relDirs.Length;

			// Use to determine where in the loop we exited
			int lastCommonRoot = -1;
			int index;

			// Find common root
			for (index = 0; index < length; index++)
			{
				if (absDirs[index] == relDirs[index]) lastCommonRoot = index;
				else break;
			}

			// If we didn't find a common prefix then throw
			if (lastCommonRoot == -1)
			{
				return Path.GetFullPath(targetFile);
				//throw new ArgumentException("Paths do not have a common base");
			}

			// Build up the relative path
			StringBuilder relativePath = new StringBuilder();

			// Add on the ..
			for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
			{
				if (absDirs[index].Length > 0) relativePath.Append("..\\");
			}

			// Add on the folders
			for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
			{
				relativePath.Append(relDirs[index] + "\\");
			}
			relativePath.Append(relDirs[relDirs.Length - 1]);

			return relativePath.ToString();
		}
	}
}
