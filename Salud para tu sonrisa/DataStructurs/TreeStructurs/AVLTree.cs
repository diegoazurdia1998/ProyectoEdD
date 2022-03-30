using System;
using System.Collections.Generic;
using System.Text;
using DataStructurs.Interfaces;

namespace DataStructurs.TreeStructurs
{
    /// <summary>
    /// Clase para un árbol AVL
    /// </summary>
    /// <typeparam name="T">Tipo de elemento guardado en el árbol</typeparam>
    public class AVLTree<T> : IAVLtree<T>
    {
        private AVLNode<T> Raiz; //Raiz del árbol
        private int count { get; set; } //cuenta de los nodos en el árbol
        /// <summary>
        /// Constructor sin parametros de la clase AVLTree
        /// </summary>
        public AVLTree()
        {
            this.Raiz = null;
        }

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
        public virtual void TreeToList(AVLNode<T> raiz, List<T> Tree)
        {
            if (raiz != null)
            {
                TreeToList(raiz.Izquierda, Tree);
                Tree.Add(raiz.value);
                TreeToList(raiz.Derecha, Tree);
            }
        }
        public virtual Boolean Contains(T item, IComparer<T> comparer)
        {
            return Contains(this.Raiz, item, comparer);
        }
        public virtual Boolean Contains(AVLNode<T> raiz, T item, IComparer<T> comparer)
        {
            if (raiz == null)
            {
                return false;
            }
            if (comparer.Compare(item, raiz.value) < 0)
            {
                return Contains(raiz.Izquierda, item, comparer);
            }
            else
            {
                if (comparer.Compare(item, raiz.value) > 0)
                {
                    return Contains(raiz.Derecha, item, comparer);
                }
            }
            return true;
        }
        public virtual T Find(T item, IComparer<T> comparer)
        {
            return Find(this.Raiz, item, comparer);
        }
        public virtual T Find(AVLNode<T> raiz, T item, IComparer<T> comparer)
        {
            if (raiz == null)
            {
                return default(T);
            }
            if (comparer.Compare(item, raiz.value) < 0)
            {
                return Find(raiz.Izquierda, item, comparer);
            }
            else
            {
                if (comparer.Compare(item, raiz.value) > 0)
                {
                    return Find(raiz.Derecha, item, comparer);
                }
            }
            return raiz.value;
        }
        public virtual void Add(T item, IComparer<T> comparer)
        {
            bool flag = false;
            this.Raiz = AddAVL(this.Raiz, item, ref flag, comparer);
        }
        public virtual AVLNode<T> AddAVL(AVLNode<T> raiz, T item, ref bool flag, IComparer<T> comparer)
        {
            AVLNode<T> n1;
            if (raiz == null)
            {
                raiz = new AVLNode<T>(item);
                flag = true;
                count++;
            }
            else
            {
                if (comparer.Compare(item, raiz.value) < 0)
                {
                    raiz.Izquierda = AddAVL(raiz.Izquierda, item, ref flag, comparer);
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
                    if (comparer.Compare(item, raiz.value) > 0)
                    {
                        raiz.Derecha = AddAVL(raiz.Derecha, item, ref flag, comparer);
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

        public virtual void Remove(T item, IComparer<T> comparer)
        {
            bool flag = false;
            this.Raiz = RemoveAVL(this.Raiz, item, ref flag, comparer);
        }
        public virtual AVLNode<T> RemoveAVL(AVLNode<T> raiz, T item, ref bool flag, IComparer<T> comparer)
        {
            if (raiz == null)
            {
                //error
            }
            if (comparer.Compare(item, raiz.value) < 0)
            {
                raiz.Izquierda = RemoveAVL(raiz.Izquierda, item, ref flag, comparer);
                if (flag)
                {
                    raiz = LeftBalance(raiz, ref flag);
                }

            }
            else
            {
                if (comparer.Compare(item, raiz.value) > 0)
                {
                    raiz.Derecha = RemoveAVL(raiz.Derecha, item, ref flag, comparer);
                    if (flag)
                    {
                        raiz = RightBalance(raiz, ref flag);
                    }
                }
                else
                {
                    AVLNode<T> q;
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
        public virtual AVLNode<T> Replace(AVLNode<T> n, AVLNode<T> act, ref bool flag)
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
        public virtual AVLNode<T> LeftBalance(AVLNode<T> n, ref bool flag)
        {
            AVLNode<T> n1;
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
        public virtual AVLNode<T> RightBalance(AVLNode<T> n, ref bool flag)
        {
            AVLNode<T> n1;
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
        public virtual AVLNode<T> LeftLeftRotation(AVLNode<T> n, AVLNode<T> n1)
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
        public virtual AVLNode<T> LeftRightRotation(AVLNode<T> n, AVLNode<T> n1)
        {
            AVLNode<T> n2 = n1.Derecha;
            n.Izquierda = n2.Derecha;
            n2.Derecha = n;
            n1.Derecha = n2.Izquierda;
            n2.Izquierda = n1;
            n1.balance = (n2.balance == -1) ? 1 : 0;
            n.balance = (n2.balance == 1) ? -1 : 0;
            n2.balance = 0;
            return n2;
        }
        public virtual AVLNode<T> RightRightRotation(AVLNode<T> n, AVLNode<T> n1)
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
        public virtual AVLNode<T> RightLeftRotation(AVLNode<T> n, AVLNode<T> n1)
        {
            AVLNode<T> n2 = n1.Izquierda;
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
