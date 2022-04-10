using System;
using System.Collections.Generic;
using System.Text;

public delegate int CompareKeyDelegate<K>(K key1, K key2);

namespace DataStructurs.Interfaces
{
    /// <summary>
    /// Interface para un árbol AVL
    /// </summary>
    /// <typeparam name="T">Tipo de dato que se guarda en el árbol</typeparam>
    interface IAVLtree<K, T>
    {
        /// <summary>
        /// Comprueba si la llave existe en el árbol
        /// </summary>
        /// <param name="key">Llave del elemento tipo K</param>
        /// <returns></returns>
        Boolean Contains(K key);
        /// <summary>
        /// Busca un elemento en el árbol
        /// </summary>
        /// <param name="key">Llave del elemento tipo K</param>
        /// /// <returns>Elemento encontrado</returns>
        T Find(K key);
        /// <summary>
        /// Añade un elemento a la lista
        /// </summary>
        /// /// <param name="key">Llave del elemento tipo K</param>
        /// <param name="item">Elemento a añadir tipo T</param>
        void Add(K key, T item);
        /// <summary>
        /// Elimina un elemento de la lista
        /// </summary>
        /// /// <param name="key">Llave del elemento tipo K</param>
        void Remove(K key);

    }
}
