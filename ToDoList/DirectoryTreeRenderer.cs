using System;
using System.IO;

namespace ShowDir
{
    internal sealed class DirectoryTreeRenderer
    {
        #region Constructors
        public DirectoryTreeRenderer(string directory)
        {
            RootDirectory = new Node<string>(GetFullPath(directory));
        }
        #endregion
        #region Properties
        internal Node<string> RootDirectory { get; set; }
        #endregion
        #region Methods
        internal void Render()
        {

        }

        private string GetFullPath(string directory)
        {
            ThrowIfDirectoryNotExists(directory);
            return Path.GetFullPath(directory);
        }

        private static void ThrowIfDirectoryNotExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new ArgumentException($"The given string is not a directory: {directory}");
            }
        }
        #endregion
    }
}
