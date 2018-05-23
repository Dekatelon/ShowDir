using System;
using System.Collections.Generic;
using System.Linq;

namespace ShowDir
{
    internal sealed class Node<T>
    {
        #region Fields
        private List<Node<T>> childList = new List<Node<T>>();
        #endregion Fields
        #region Constructors
        internal Node(T obj) : this(obj, null, null) { }

        internal Node(T obj, Node<T> parent) : this(obj, parent, null) { }

        internal Node(T obj, IEnumerable<Node<T>> childs) : this(obj, null, childs) { }

        internal Node(T obj, Node<T> parent, IEnumerable<Node<T>> childs)
        {
            Object = obj;
            this.SetParent(parent);
            this.AddChilds(childs);
        }
        #endregion Constructors
        #region Properties
        internal Node<T> Parent { get; private set; } = null;
        internal T Object { get; set; }
        internal IEnumerable<Node<T>> Childs => childList;
        internal bool IsRoot => Parent == null;
        internal bool HasChilds => childList.Count != 0;
        #endregion Properties
        #region Methods
        internal void SetParent(Node<T> parent)
        {
            if (parent == null)
            {
                return;
            }

            this.Parent = parent;
            parent.childList.Add(this);
        }

        internal void AddChilds(IEnumerable<Node<T>> childs)
        {
            if (childs == null)
            {
                return;
            }

            this.childList.AddRange(childs);
            foreach (var child in childs)
            {
                child.Parent = this;
            }
        }

        internal void RemoveChilds(IEnumerable<Node<T>> childsToRemove)
        {
            foreach (var childToRemove in childsToRemove)
            {
                this.ThrowIfObjectIsNotChildsParent(childToRemove);
                childToRemove.RemoveParent();
            }            
        }

        internal void RemoveParent()
        {
            this.Parent.childList.Remove(this);
            this.Parent = null;
        }
        
        private void ThrowIfObjectIsNotChildsParent(Node<T> child)
        {
            if (child.Parent != this)
            {
                throw new InvalidOperationException("Object is not childs parent!");
            }
        }
        #endregion Methods
    }
}