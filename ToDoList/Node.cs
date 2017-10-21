using System;
using System.Collections.Generic;

namespace ShowDir
{
    internal sealed class Node<T>
    {
        #region Fields
        private List<Node<T>> childList;

        #endregion Fields

        #region Constructors

        public Node(T obj)
        {
            Object = obj;
            Parent = null;
            childList = new List<Node<T>>();
        }

        public Node(T obj, Node<T> parent)
        {
            Object = obj;
            this.SetParent(parent);
            childList = new List<Node<T>>();
        }

        public Node(T obj, IEnumerable<Node<T>> childs)
        {
            Object = obj;
            Parent = null;
            childList = new List<Node<T>>();
            this.AddChilds(childs);
        }

        public Node(T obj, Node<T> parent, IEnumerable<Node<T>> childs)
        {
            Object = obj;
            this.SetParent(parent);
            childList = new List<Node<T>>();
            this.AddChilds(childs);
        }

        #endregion Constructors

        #region Properties

        public Node<T> Parent { get; private set; }

        public IEnumerable<Node<T>> Childs => childList;

        public T Object { get; set; }

        public bool IsRoot => Parent == null;

        public bool HasChilds => childList.Count != 0;

        #endregion Properties

        #region Methods

        public void SetParent(Node<T> parent)
        {
            this.Parent = parent;
            parent.childList.Add(this);
        }

        public void AddChilds(IEnumerable<Node<T>> childs)
        {
            this.childList.AddRange(childs);
            foreach (var child in childs)
            {
                child.Parent = this;
            }
        }

        public void RemoveParent()
        {
            this.Parent.childList.Remove(this);
            this.Parent = null;
        }

        public void RemoveChilds(IEnumerable<Node<T>> childs)
        {
            foreach (var child in childs)
            {
                if (child.Parent != this)
                {
                    throw new InvalidOperationException("Object is not childs parent!");
                }
            }
            foreach (var child in Childs)
            {
                this.childList.Remove(child);
                child.Parent = null;
            }
        }

        #endregion Methods
    }
}