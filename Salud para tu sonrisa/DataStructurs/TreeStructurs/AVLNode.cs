using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructurs.TreeStructurs
{
    public class AVLNode<K, T>
    {
        public T value { get; set; }
        public K key { get; set; }
        public AVLNode<K, T> Izquierda { get; set; }
        public AVLNode<K, T> Derecha { get; set; }
        public int balance { get; set; }
        public AVLNode() { }
        public AVLNode(K key, T value)
        {
            this.key = key;
            this.value = value;
        }


    }
}
