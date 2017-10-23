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

        private static string GetValidatedFullPath(string dir)
        {
            if (dir.Last() == '\\')
            {
                dir = dir.Remove(dir.Length - 1);
            }

            return Path.GetFullPath(dir);
        }

        private static void ShowDirStructure(Node<string> root)
        {
            WriteNodeString(root, true);
            string[] dirs = GetDirectories(root);
            string[] files = Directory.GetFiles(GetFullPath(root));
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

        private static void WriteDepthMarkers(Node<string> node) => Console.WriteLine(GetDirDepthMarkers(node));

        private static string[] GetDirectories(Node<string> node) => Directory.GetDirectories(GetFullPath(node));

        private static Node<string> GetLastPathNode(string path, Node<string> parent) => new Node<string>(path.Substring(path.LastIndexOf('\\') + 1), parent);

        private static string GetFullPath(Node<string> node) => WhileNotRoot(ref node, (n, s) => { s.Insert(0, $"\\{n.Object}"); }, true).Insert(0, node.Object).ToString();

        private static void WriteNodeString(Node<string> node, bool isDirectory = false) => Console.WriteLine($"{GetDirDepthMarkers(node)}{(isDirectory ? "|>" : "|-")}{node.Object}");

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