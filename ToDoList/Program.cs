using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ShowDir
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1 && Directory.Exists(args[0]))
            {
                ShowDirStructure(new Node<string>(GetValidatedFullPath(args[0])));
                return;
            }

            string dir = string.Empty;
            do
            {
                Console.WriteLine("Please enter a valid absolute or relative path to display the directory structure:");
                dir = Console.ReadLine();
                if (Directory.Exists(dir))
                {
                    ShowDirStructure(new Node<string>(GetValidatedFullPath(dir)));
                }
            } while (dir != "exit");
        }

        /// <summary>
        /// Validates the user input by removing the last backslash of the directory
        /// </summary>
        /// <param name="dir">The directory the user typed in</param>
        /// <returns>The validated full path</returns>
        private static string GetValidatedFullPath(string dir)
        {
            if (dir.Last() == '\\')
            {
                dir = dir.Remove(dir.Length - 1);
            }

            return Path.GetFullPath(dir);
        }

        /// <summary>
        /// The Main method to display the directory structure.
        /// </summary>
        /// <param name="root">The root directory node.</param>
        private static void ShowDirStructure(Node<string> root)
        {
            WriteNodeString(root, true);

            string[] dirs = { };
            string[] files = { };

            try
            {
                dirs = GetDirectories(root);
                files = Directory.GetFiles(GetFullPath(root));
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"{GetDirDepthMarkers(root)}| |... (No Access)");
                Console.WriteLine($"{GetDirDepthMarkers(root)}|");
                return;
            }

            foreach (var file in files)
            {
                Node<string> node = GetLastPathNode(file, root);
                WriteNodeString(node);
                if (file == files.Last() && !dirs.Any())
                {
                    WriteDepthMarkers(node);
                }
            }

            foreach (var dir in dirs)
            {
                ShowDirStructure(GetLastPathNode(dir, root));
            }

            if (!dirs.Any() && !files.Any())
            {
                WriteDepthMarkers(root);
            }
        }

        /// <summary>
        /// Writes the horizontal lines which are displaying the directory structure into the console 
        /// </summary>
        /// <param name="node"></param>
        private static void WriteDepthMarkers(Node<string> node) => Console.WriteLine(GetDirDepthMarkers(node));

        /// <summary>
        /// Gets the directories of a node.
        /// </summary>
        /// <param name="node">The node with the directories.</param>
        /// <returns>The directories of the node.</returns>
        private static string[] GetDirectories(Node<string> node) => Directory.GetDirectories(GetFullPath(node));

        /// <summary>
        /// Generates a new node from a directory or file and adds it to the parent directory.
        /// </summary>
        /// <param name="path">The full path of a directory or file.</param>
        /// <param name="parent">The parent, to which the new node will be added.</param>
        /// <returns>The new node.</returns>
        private static Node<string> GetLastPathNode(string path, Node<string> parent) => new Node<string>(path.Substring(path.LastIndexOf('\\') + 1), parent);

        /// <summary>
        /// Builds a full path from a node and its parent nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The full path.</returns>
        private static string GetFullPath(Node<string> node) => WhileNotRoot(ref node, (n, s) => { s.Insert(0, $"\\{n.Object}"); }, true).Insert(0, node.Object).ToString();

        /// <summary>
        /// Writes a node to the console, which represents a fale or a directory
        /// </summary>
        /// <param name="node">The node to be written</param>
        /// <param name="isDirectory"></param>
        private static void WriteNodeString(Node<string> node, bool isDirectory = false) => Console.WriteLine($"{GetDirDepthMarkers(node)}{(isDirectory ? "|>" : "|-")}{node.Object}");

        /// <summary>
        /// Evaluates the directory structure of the current and the parent directories to generate a string, 
        /// which displays the directory structure of the current node (e.g. ' |   | |') 
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>A generated directory structure string (e.g. ' |   | |')</returns>
        private static string GetDirDepthMarkers(Node<string> node)
        {
            Node<string> parent = node.Parent;
            return node.IsRoot ? string.Empty : WhileNotRoot(ref parent, (n, s) =>
            {
                if (GetDirectories(n.Parent).Last() == GetFullPath(n))
                {
                    s.Insert(0, "  ");
                }
                else
                {
                    s.Insert(0, "| ");
                }
            }).Insert(0, "  ").ToString();
        }

        /// <summary>
        /// Executes an action with a node and a string builder and jumps to the parent node 
        /// until it reaches the root node.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <param name="action">The action to be executed.</param>
        /// <param name="setNodeAsRoot">Indicates, whether it sets the input node as root</param>
        /// <returns>the generated string builder of the action.</returns>
        private static StringBuilder WhileNotRoot(ref Node<string> node, Action<Node<string>, StringBuilder> action, bool setNodeAsRoot = false)
        {
            StringBuilder sb = new StringBuilder();
            Node<string> current = node;
            while (!current.IsRoot)
            {
                action(current, sb);
                current = current.Parent;
            }

            node = setNodeAsRoot ? current : node;
            return sb;
        }
    }
}