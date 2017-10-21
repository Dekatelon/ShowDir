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
                ShowDirStructure(new Node<string>(args[0]));
                return;
            }

            Console.WriteLine("Please enter a valid absolute or relative path to display the directory structure:");
            string dir = Console.ReadLine();
            if (Directory.Exists(dir))
            {
                ShowDirStructure(new Node<string>(Path.GetFullPath(dir)));
                Console.Read();
            }
        }

        private static void ShowDirStructure(Node<string> root)
        {
            Console.Write(GetNodeString(root, true));
            string[] dirs = Directory.GetDirectories(GetFullPath(root));
            string[] files = Directory.GetFiles(GetFullPath(root));
            foreach (var file in files)
            {
                Node<string> node = new Node<string>(GetLastPathNode(file), root);
                Console.Write(GetNodeString(node));
                if (file == files.Last())
                {
                    Console.WriteLine($"{GetDirDepthMarkers(node)}{(Directory.GetDirectories(GetFullPath(node.Parent)).Length > 0 ? "| " : "  ")}");
                }
            }

            foreach (var dir in dirs)
            {
                ShowDirStructure(new Node<string>(GetLastPathNode(dir), root));
            }
        }

        private static string GetLastPathNode(string path) => path.Substring(path.LastIndexOf('\\') + 1);

        private static string GetFullPath(Node<string> node) => WhileNotRoot(ref node, (n, s) => { s.Insert(0, $"\\{n.Object}"); }, true).Insert(0, node.Object).ToString();

        private static string GetNodeString(Node<string> node, bool isDirectory = false) => $"{GetDirDepthMarkers(node)}{(isDirectory ? "|>" : "|-")}{node.Object}\n";

        private static string GetDirDepthMarkers(Node<string> node)
        {
            Node<string> parent = node.Parent;
            return node.IsRoot ? string.Empty : WhileNotRoot(ref parent, (n, s) =>
            {
                if (Directory.GetDirectories(GetFullPath(n.Parent)).Last() == GetFullPath(n))
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