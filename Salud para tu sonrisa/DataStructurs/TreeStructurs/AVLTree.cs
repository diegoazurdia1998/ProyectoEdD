using System;
using System.Collections.Generic;
using System.Text;
using DataStructurs.Interfaces;
using System.IO;
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
        public StreamWriter _log;
        private int count { get; set; } //cuenta de los nodos en el árbol
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="comparer">Delegado comparador de llaves</param>
        public AVLTree(CompareKeyDelegate<K> comparer)
        {
            this.Raiz = null;
            this.CompareKeyDelegate = comparer;
            this._log = null;
        }
        public AVLTree(CompareKeyDelegate<K> comparer, String name)
        {
            this.Raiz = null;
            this.CompareKeyDelegate = comparer;
            this._log = new StreamWriter(name);
        }
        /// <summary>
        /// Obtiene la cantidad de nodos del árbol
        /// </summary>
        /// <returns></returns>
        public int getCount()
        {
            return count;
        }
        /// <summary>
        /// Crea una lista con los elementos del árbol
        /// </summary>
        /// <returns></returns>
        public virtual List<T> TreeToList()
        {

            List<T> ListedTree = new List<T>();
            TreeToList(this.Raiz, ListedTree);
            //log
            if (_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Recorrido");
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
            //log
            if (_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Busqueda");
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
            //log
            if(_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Inserción");
        }
        /// <summary>
        /// Añade un elemnto al árbol de forma recursiva
        /// </summary>
        /// <param name="raiz">nodo actual</param>
        /// <param name="key">llave del elemento a insertar</param>
        /// <param name="item">elemento a insertar</param>
        /// <param name="flag"> si cambió de altura true</param>
        /// <returns>Retorna el nodo actual</returns>
        public virtual AVLNode<K, T> AddAVL(AVLNode<K, T> raiz, K key, T item, ref bool flag)
        {
            AVLNode<K, T> n1; //nodo auxiliar para balancear árbol
            if (raiz == null) //si el nodo es nulo
            {
                raiz = new AVLNode<K, T>(key, item); //se crea un nuevo nodo 
                flag = true; //la bandera indica que la altura cambió
                count++; //aumenta la cantidad de nodos en el árbol
            }
            else //si el nodo no es nulo se llama recursivamente y se balancea el árbol si la altura cambió
            {
                if (CompareKeyDelegate(key, raiz.key) < 0) //si el valor de la llave es menor a la llave actual de la raiz se inserta a la izquierda
                {
                    raiz.Izquierda = AddAVL(raiz.Izquierda, key, item, ref flag); //se asigna a la izquierda de la raiz llamando recursivamente al método
                    if (flag) //si la bandera es verdadera se ajusta el balance del nodo segun el caso
                    {
                        switch (raiz.balance) //analiza el balance del nodo y aplica un caso
                        {
                            case -1: //el nodo tiene derecha
                                raiz.balance = 0; //el balance del nodo cambia a 0 (tiene izquierda y derecha
                                flag = false; //la bandera indica que la altura no cambió
                                break;
                            case 0: //el nodo no tiene hijos
                                raiz.balance = 1; //el balance del nodo cambia a 1
                                break;
                            case 1: //el nodo tiene izquierda
                                n1 = raiz.Izquierda; //se asigna la izquierda del nodo a n1
                                if (n1.balance == 1) //si la izquierda tiene balance 1 (tiene izquierda)
                                {
                                    raiz = LeftLeftRotation(raiz, n1); //se hace una rotacion izquierda simple en la raiz

                                }
                                else
                                {
                                    raiz = LeftRightRotation(raiz, n1); //se hace una rotacion izquierda doble en la raiz

                                }
                                flag = false; //la bandera indica que la altura no cambió
                                break;
                        }
                    }
                }
                else
                {
                    if (CompareKeyDelegate(key, raiz.key) > 0) //si el valor de la llave es mayor a la llave actual de la raiz se inserta a la izquierda
                    {
                        raiz.Derecha = AddAVL(raiz.Derecha, key, item, ref flag); //se asigna a la derecha de la raiz llamando recursivamente al método
                        if (flag) //si la bandera es verdadera se ajusta el balance del nodo segun el caso
                        {
                            switch (raiz.balance) //analiza el balance del nodo y aplica un caso
                            {
                                case -1: //el nodo tiene derecha
                                    n1 = raiz.Derecha;//se asigna la derecha del nodo a n1
                                    if (n1.balance == -1) //si la derecha tiene balance -1 (tiene derecha)
                                    {
                                        raiz = RightRightRotation(raiz, n1); //se hace una rotacion derecha simple en la raiz

                                    }
                                    else
                                    {
                                        raiz = RightLeftRotation(raiz, n1); //se hace una rotacion derecha doble en la raiz
                                    }
                                    flag = false; //la bandera indica que la altura no cambió
                                    break;
                                case 0://el nodo no tiene hijos
                                    raiz.balance = -1; //el balance es -1 (ahora tiene derecha)
                                    break;
                                case 1: //el ndo tiene izquierda
                                    raiz.balance = 0; //el balance del nodo cambia a 0 (tiene izquierda y derecha)
                                    flag = false; //la bandera indica que la altura no cambió
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (_log != null)
                            _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Fallo al insertar");

                        //error. El valor no es ni mayor ni menor, es igual
                    }
                }
            }
            return raiz;//reteorna la raiz
        }
        public virtual void Remove(K key)
        {
            bool flag = false;
            this.Raiz = RemoveAVL(this.Raiz, key, ref flag);
            //log
            if(_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Eliminación");
        }
        /// <summary>
        /// Elimina recursivamente un elemento de la lista
        /// </summary>
        /// <param name="raiz">nodo del árbol</param>
        /// <param name="key">llave del elemento a buscar</param>
        /// <param name="flag">indica si la altura del árbol cambió</param>
        /// <returns>Retorna el nodo actual</returns>
        public virtual AVLNode<K, T> RemoveAVL(AVLNode<K, T> raiz, K key, ref bool flag)
        {
            if (raiz == null) //El nodo actual es nulo
            {
                //error. No se encontró el elemento a buscar
            }
            if (CompareKeyDelegate(key, raiz.key) < 0) //Si la llave del elemento es menor a la llave del nodo actual
            {
                raiz.Izquierda = RemoveAVL(raiz.Izquierda, key, ref flag); //la izquierda del nodo llama recursivamente al método
                if (flag) //si la altura cambió
                {
                    raiz = LeftBalance(raiz, ref flag); //se balancea el árbol por la izquierda
                }

            }
            else
            {
                if (CompareKeyDelegate(key, raiz.key) > 0) //Si la llave del elemento es mayor a la llave del nodo actual
                {
                    raiz.Derecha = RemoveAVL(raiz.Derecha, key, ref flag); //la derecha del nodo llama recursivamente al método
                    if (flag)
                    {
                        raiz = RightBalance(raiz, ref flag); //se balancea el árbol por la derecha
                    }
                }
                else
                {
                    AVLNode<K, T> q; //nodo temporal
                    q = raiz; // se asigna el valor de la raiz a q
                    if (q.Izquierda == null) //si la izquierda de q es nula
                    {
                        raiz = q.Derecha; //la raiz es gual a su derecha
                        flag = true; //la bandera indica que la altura cambió
                    }
                    else
                    {
                        if (q.Derecha == null) //si la derecha de q es nula
                        {
                            raiz = q.Izquierda; //la raiz es igual a su izquierda
                            flag = true;
                        }
                        else
                        {
                            raiz.Izquierda = Replace(raiz, raiz.Izquierda, ref flag); //la izquirdad e la raiz se balancea
                            if (flag) //si la altura cambió
                            {
                                raiz = LeftBalance(raiz, ref flag); //se balancea por la izquierda
                            }
                            q = null; //se elimina q
                        }
                    }
                }
            }
            return raiz; //retorna el nodo actual
        }
        /// <summary>
        /// Reemplaza un nodo por el mayor de la izquierda
        /// </summary>
        /// <param name="n">nodo a extraer</param>
        /// <param name="act">nodo mayor de la izquierda</param>
        /// <param name="flag">referencia de la bandera que indica si cambió la altura</param>
        /// <returns>retorna el nodo actual</returns>
        public virtual AVLNode<K, T> Replace(AVLNode<K, T> n, AVLNode<K, T> act, ref bool flag)
        {
            if (act.Derecha != null)//si la derecha no es nula
            {
                act.Derecha = Replace(n, act.Derecha, ref flag); //la derecha del nodo actual llama recursivamente
                if (flag) //si la altura cambió
                {
                    act = RightBalance(act, ref flag); //el nodo actual se balancea por la derecha
                }
            }
            else
            {
                n.value = act.value;//el valor del nodo n es reemplazado con el valor del nodo actual
                n = act; //el nodo n es gual el noso actual
                act = act.Izquierda; //el nodo actual es igual a su izquierda
                n = null; //es nodo n se vuelve nulo
                flag = true; //la bandera indica que la altura cambió
            }
            //log
            if (_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Reemplazo por mayor de los menores.");
            return act;//retorna el nodo actual
        }
        /// <summary>
        /// Verifica si la rotacion es necesaria al eliminar un nodo de la izquierda
        /// </summary>
        /// <param name="n">Nodo a verificar</param>
        /// <param name="flag">Referencia de la bandera que indica si la altura cambio</param>
        /// <returns></returns>
        public virtual AVLNode<K, T> LeftBalance(AVLNode<K, T> n, ref bool flag)
        {
            AVLNode<K, T> n1; //nodo auxiliar
            switch (n.balance) //analiza el balance de n
            {
                case 1: //el nodo tiene izquierda
                    n.balance = 0; //el balance del nodo se vuelve 0
                    break;
                case 0:
                    n.balance = -1; //el balance del nodo se vuelve -1
                    flag = false; // la bandera indica que la altura no cambió
                    break;
                case -1: //el nodo tiene derecha
                    n1 = n.Derecha; //la derecha de n se asigna a n1
                    if (n1.balance <= 0) //si el balance es menor o igual a 0
                    {
                        flag = false; //la bandera indica que la altura no cambió
                        n = RightRightRotation(n, n1); 
                    }
                    else
                    {
                        n = RightLeftRotation(n, n1); 
                    }
                    //log
                    if (_log != null)
                        _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Balanceo luego de extraer nodo de izquierda");
                    break;
            }
            return n; //retorna el nodo actual
        }
        /// <summary>
        /// Verifica si la rotacion es necesaria al eliminar un nodo de la derecha
        /// </summary>
        /// <param name="n">Nodo a verificar</param>
        /// <param name="flag">Referencia de la bandera que indica si la altura cambio</param>
        /// <returns></returns>
        public virtual AVLNode<K, T> RightBalance(AVLNode<K, T> n, ref bool flag)
        {
            AVLNode<K, T> n1; //nodo temporal
            switch (n.balance) //analiza el balance de n
            {
                case 1: //tiene izquierda
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
                    //log
                    if (_log != null)
                        _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Balanceo luego de extraer nodo de derecha");
                    break;
                case 0:// no tiene hijos
                    n.balance = 1;
                    flag = false;
                    break;
                case -1: //tiene derecha
                    n.balance = 0;
                    break;
            }
            return n;
        }
        /// <summary>
        /// Totación simple izquierda izquierda
        /// </summary>
        /// <param name="n">Nodo actual</param>
        /// <param name="n1">Izquierda del nodo actual</param>
        /// <returns></returns>
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
            //log
            if (_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Rotación simple izquiqerda - izquierda");
            return n1;
        }
        /// <summary>
        /// Rotación doble izquierda derecha
        /// </summary>
        /// <param name="n">Nodo actual</param>
        /// <param name="n1">Izquierda del nodo actual</param>
        /// <returns></returns>
        public virtual AVLNode<K, T> LeftRightRotation(AVLNode<K, T> n, AVLNode<K, T> n1)
        {
            //derecha de la izquierda del nodo actual
            AVLNode<K, T> n2 = n1.Derecha;
            n.Izquierda = n2.Derecha;
            n2.Derecha = n;
            n1.Derecha = n2.Izquierda;
            n2.Izquierda = n1;
            n1.balance = (n2.balance == -1) ? 1 : 0;
            n.balance = (n2.balance == 1) ? -1 : 0;
            n2.balance = 0;
            //log
            if (_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Rotación doble izquiqerda - derecha");
            return n2;
        }
        /// <summary>
        /// Rotación simple derecha derecha
        /// </summary>
        /// <param name="n">Nodo actual</param>
        /// <param name="n1">Derecha del nodo actual</param>
        /// <returns>Nueva raíz</returns>
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
            //log
            if (_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Rotación simple derecha - derecha");
            return n1;
        }
        /// <summary>
        /// Rotacion doble derecha izquierda
        /// </summary>
        /// <param name="n">Nodo actual</param>
        /// <param name="n1">Derecha del nodo actual</param>
        /// <returns></returns>
        public virtual AVLNode<K, T> RightLeftRotation(AVLNode<K, T> n, AVLNode<K, T> n1)
        {
            //izquierda de la derecha del nodo actual
            AVLNode<K, T> n2 = n1.Izquierda;
            n.Derecha = n2.Izquierda;
            n2.Izquierda = n;
            n1.Izquierda = n2.Derecha;
            n2.Derecha = n1;
            n.balance = (n2.balance == -1) ? 1 : 0;
            n1.balance = (n2.balance == 1) ? -1 : 0;
            n2.balance = 0;
            //log
            if (_log != null)
                _log.WriteLine(DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " Rotación doble derecha - izquierda");
            return n2;
        }
       
    }
}
