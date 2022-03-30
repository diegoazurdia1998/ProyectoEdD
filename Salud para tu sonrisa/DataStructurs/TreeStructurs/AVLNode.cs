using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructurs.TreeStructurs
{
    public class AVLNode<T>
    {
        public T value { get; set; }
        public AVLNode<T> Izquierda { get; set; }
        public AVLNode<T> Derecha { get; set; }
        public int balance { get; set; }
        public AVLNode() { }
        public AVLNode(T value)
        {
            this.value = value;
        }


    }
}
