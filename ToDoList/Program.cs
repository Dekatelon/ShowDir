using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    class Program
    {
        static void Main(string[] args)
        {
            Node<string> root = new Node<string>(@"C:\Users\Dekat\Documents\Angular");
            Console.Write(SetDirStructure(root));
            Directory.GetCurrentDirectory();
            Console.Read();
        }
        

        static string SetDirStructure(Node<string> root) => SetDirStructure(root, new StringBuilder());
        static string SetDirStructure(Node<string> root, StringBuilder sb)
        {
            // sb.Append(GetNodeString(root));
            Console.Write(GetNodeString(root));
            string[] dirs = Directory.GetDirectories(GetFullPath(root));
            string[] files = Directory.GetFiles(GetFullPath(root));
            foreach (var file in files)
            {
                Node<string> node = new Node<string>(GetLastPathNode(file), root);
                //sb.Append(GetNodeString(node));
                Console.Write(GetNodeString(node));
            }

            foreach (var dir in dirs)
            {
                SetDirStructure(new Node<string>(GetLastPathNode(dir), root), sb);
            }

            return sb.ToString();
        }

        static string GetLastPathNode(string path) => path.Substring(path.LastIndexOf('\\') + 1);
        static string GetFullPath(Node<string> node)
        {
            StringBuilder sb = new StringBuilder();
            while (!node.IsRoot)
            {
                sb.Insert(0, $"\\{node.Object}");
                node = node.Parent;
            }

            sb.Insert(0, node.Object);
            return sb.ToString();
        }
        static string GetNodeString(Node<string> node)
        {
            StringBuilder sb = new StringBuilder();
            Node<string> current = node;
            while (!current.IsRoot)
            {
                sb.Append("| ");
                current = current.Parent;
            }
            return $"{sb.ToString()}|-{node.Object}\n";
        }
    }
}
