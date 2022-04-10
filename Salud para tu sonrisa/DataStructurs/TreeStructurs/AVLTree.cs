using System;
using System.Collections.Generic;
using System.Text;
using DataStructurs.Interfaces;
/*
 * Basado en el árbol AVL del laboratorio 3
 */
namespace DataStructurs.TreeStructurs
{
    /// <summary>
    /// Clase para un árbol AVL
    /// </summary>
    /// <typeparam name="K">Tipo de llave del elemento</typeparam>
    /// <typeparam name="T">Tipo de elemento guardado en el árbol</typeparam>
    public class AVLTree<K, T> : IAVLtree<K, T>
    {
        private AVLNode<K, T> Raiz; //Raiz del árbol
        private CompareKeyDelegate<K> CompareKeyDelegate;
        private int count { get; set; } //cuenta de los nodos en el árbol
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="comparer">Delegado comparador de llaves</param>
        public AVLTree(CompareKeyDelegate<K> comparer)
        {
            this.Raiz = null;
            this.CompareKeyDelegate = comparer;
        }
        /// <summary>
        /// Obtiene la cantidad de nodos del árbol
        /// </summary>
        /// <returns></returns>
        public int getCount()
        {
            return count;
        }

        public virtual List<T> TreeToList()
        {
            List<T> ListedTree = new List<T>();
            TreeToList(this.Raiz, ListedTree);
            return ListedTree;
        }
        /// <summary>
        /// Recorre el árbol en orden de forma recursiva y coloca los elementos en una lista
        /// </summary>
        /// <param name="raiz">Elemento del árbol</param>
        /// <param name="Tree">Lista para llenar con el árbol</param>
        public virtual void TreeToList(AVLNode<K, T> raiz, List<T> Tree)
        {
            if (raiz != null)
            {
                TreeToList(raiz.Izquierda, Tree);
                Tree.Add(raiz.value);
                TreeToList(raiz.Derecha, Tree);
            }
        }
        public virtual Boolean Contains(K key)
        {
            return Contains(this.Raiz, key);
        }
        /// <summary>
        /// Compruebaa recursiva si la llave existe en el arbol
        /// </summary>
        /// <param name="raiz">Elemento del árbol</param>
        /// <param name="key">Llave a comprobar</param>
        /// <returns>true: si la llave existe</returns>
        public virtual Boolean Contains(AVLNode<K, T> raiz, K key)
        {
            if (raiz == null)
            {
                return false;
            }
            if (CompareKeyDelegate(key, raiz.key) < 0)
            {
                return Contains(raiz.Izquierda, key);
            }
            else
            {
                if (CompareKeyDelegate(key, raiz.key) > 0)
                {
                    return Contains(raiz.Derecha, key);
                }
            }
            return true;
        }
        public virtual T Find(K key)
        {
            return Find(this.Raiz, key);
        }
        /// <summary>
        /// Busca recursivamente un elemento en el arbol
        /// </summary>
        /// <param name="raiz">Elemento del árbol</param>
        /// <param name="key">Llave a buscar</param>
        /// <returns></returns>
        public virtual T Find(AVLNode<K, T> raiz, K key)
        {
            if (raiz == null)
            {
                return default(T);
            }
            if (CompareKeyDelegate(key, raiz.key) < 0)
            {
                return Find(raiz.Izquierda, key);
            }
            else
            {
                if (CompareKeyDelegate(key, raiz.key) > 0)
                {
                    return Find(raiz.Derecha, key);
                }
            }
            return raiz.value;
        }
        public virtual void Add(K key, T item)
        {
            bool flag = false;
            this.Raiz = AddAVL(this.Raiz, key, item, ref flag);
        }
        /// <summary>
        /// Añade un elemnto al árbol de forma recursiva
        /// </summary>
        /// <param name="raiz">nodo actual</param>
        /// <param name="key">llave del elemento a insertar</param>
        /// <param name="item">elemento a insertar</param>
        /// <param name="flag"> si cambió de altura true</param>
        /// <returns></returns>
        public virtual AVLNode<K, T> AddAVL(AVLNode<K, T> raiz, K key, T item, ref bool flag)
        {
            AVLNode<K, T> n1;
            if (raiz == null)
            {
                raiz = new AVLNode<K, T>(key, item);
                flag = true;
                count++;
            }
            else
            {
                if (CompareKeyDelegate(key, raiz.key) < 0)
                {
                    raiz.Izquierda = AddAVL(raiz.Izquierda, key, item, ref flag);
                    if (flag)
                    {
                        switch (raiz.balance)
                        {
                            case -1:
                                raiz.balance = 0;
                                flag = false;
                                break;
                            case 0:
                                raiz.balance = 1;
                                break;
                            case 1:
                                n1 = raiz.Izquierda;
                                if (n1.balance == 1)
                                {
                                    raiz = LeftLeftRotation(raiz, n1);

                                }
                                else
                                {
                                    raiz = LeftRightRotation(raiz, n1);

                                }
                                flag = false;
                                break;
                        }
                    }
                }
                else
                {
                    if (CompareKeyDelegate(key, raiz.key) > 0)
                    {
                        raiz.Derecha = AddAVL(raiz.Derecha, key, item, ref flag);
                        if (flag)
                        {
                            switch (raiz.balance)
                            {
                                case -1:
                                    n1 = raiz.Derecha;
                                    if (n1.balance == -1)
                                    {
                                        raiz = RightRightRotation(raiz, n1);

                                    }
                                    else
                                    {
                                        raiz = RightLeftRotation(raiz, n1);
                                    }
                                    flag = false;
                                    break;
                                case 0:
                                    raiz.balance = -1;
                                    break;
                                case 1:
                                    raiz.balance = 0;
                                    flag = false;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //error
                    }
                }
            }
            return raiz;
        }
        public virtual void Remove(K key)
        {
            bool flag = false;
            this.Raiz = RemoveAVL(this.Raiz, key, ref flag);
        }
        public virtual AVLNode<K, T> RemoveAVL(AVLNode<K, T> raiz, K key, ref bool flag)
        {
            if (raiz == null)
            {
                //error
            }
            if (CompareKeyDelegate(key, raiz.key) < 0)
            {
                raiz.Izquierda = RemoveAVL(raiz.Izquierda, key, ref flag);
                if (flag)
                {
                    raiz = LeftBalance(raiz, ref flag);
                }

            }
            else
            {
                if (CompareKeyDelegate(key, raiz.key) > 0)
                {
                    raiz.Derecha = RemoveAVL(raiz.Derecha, key, ref flag);
                    if (flag)
                    {
                        raiz = RightBalance(raiz, ref flag);
                    }
                }
                else
                {
                    AVLNode<K, T> q;
                    q = raiz;
                    if (q.Izquierda == null)
                    {
                        raiz = q.Derecha;
                        flag = true;
                    }
                    else
                    {
                        if (q.Derecha == null)
                        {
                            raiz = q.Izquierda;
                            flag = true;
                        }
                        else
                        {
                            raiz.Izquierda = Replace(raiz, raiz.Izquierda, ref flag);
                            if (flag)
                            {
                                raiz = LeftBalance(raiz, ref flag);
                            }
                            q = null;
                        }
                    }
                }
            }
            return raiz;
        }
        public virtual AVLNode<K, T> Replace(AVLNode<K, T> n, AVLNode<K, T> act, ref bool flag)
        {
            if (act.Derecha != null)
            {
                act.Derecha = Replace(n, act.Derecha, ref flag);
                if (flag)
                {
                    act = RightBalance(act, ref flag);
                }
            }
            else
            {
                n.value = act.value;
                n = act;
                act = act.Izquierda;
                n = null;
                flag = true;
            }
            return act;
        }
        public virtual AVLNode<K, T> LeftBalance(AVLNode<K, T> n, ref bool flag)
        {
            AVLNode<K, T> n1;
            switch (n.balance)
            {
                case 1:
                    n.balance = 0;
                    break;
                case 0:
                    n.balance = -1;
                    flag = false;
                    break;
                case -1:
                    n1 = n.Derecha;
                    if (n1.balance <= 0)
                    {
                        flag = false;
                        n = RightRightRotation(n, n1);
                    }
                    else
                    {
                        n = RightLeftRotation(n, n1);
                    }
                    break;
            }
            return n;
        }
        public virtual AVLNode<K, T> RightBalance(AVLNode<K, T> n, ref bool flag)
        {
            AVLNode<K, T> n1;
            switch (n.balance)
            {
                case 1:
                    n1 = n.Izquierda;
                    if (n1.balance >= 0)
                    {
                        if (n1.balance == 0)
                        {
                            flag = false;
                        }
                        n = LeftLeftRotation(n, n1);
                    }
                    else
                    {
                        n = LeftRightRotation(n, n1);
                    }
                    break;
                case 0:
                    n.balance = 1;
                    flag = false;
                    break;
                case -1:
                    n.balance = 0;
                    break;
            }
            return n;
        }
        public virtual AVLNode<K, T> LeftLeftRotation(AVLNode<K, T> n, AVLNode<K, T> n1)
        {
            n.Izquierda = n1.Derecha;
            n1.Derecha = n;
            if (n1.balance == 1)
            {
                n.balance = 0;
                n1.balance = 0;
            }
            else
            {
                n.balance = 1;
                n1.balance = -1;
            }
            return n1;
        }
        public virtual AVLNode<K, T> LeftRightRotation(AVLNode<K, T> n, AVLNode<K, T> n1)
        {
            AVLNode<K, T> n2 = n1.Derecha;
            n.Izquierda = n2.Derecha;
            n2.Derecha = n;
            n1.Derecha = n2.Izquierda;
            n2.Izquierda = n1;
            n1.balance = (n2.balance == -1) ? 1 : 0;
            n.balance = (n2.balance == 1) ? -1 : 0;
            n2.balance = 0;
            return n2;
        }
        public virtual AVLNode<K, T> RightRightRotation(AVLNode<K, T> n, AVLNode<K, T> n1)
        {
            n.Derecha = n1.Izquierda;
            n1.Izquierda = n;
            if (n1.balance == -1)
            {
                n.balance = 0;
                n1.balance = 0;
            }
            else
            {
                n.balance = -1;
                n1.balance = -1;
            }
            return n1;
        }
        public virtual AVLNode<K, T> RightLeftRotation(AVLNode<K, T> n, AVLNode<K, T> n1)
        {
            AVLNode<K, T> n2 = n1.Izquierda;
            n.Derecha = n2.Izquierda;
            n2.Izquierda = n;
            n1.Izquierda = n2.Derecha;
            n2.Derecha = n1;
            n.balance = (n2.balance == -1) ? 1 : 0;
            n1.balance = (n2.balance == 1) ? -1 : 0;
            n2.balance = 0;
            return n2;
        }
       
    }
}
