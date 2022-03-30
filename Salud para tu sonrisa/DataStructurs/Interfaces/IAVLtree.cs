using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructurs.Interfaces
{
    /// <summary>
    /// Interface para un árbol AVL
    /// </summary>
    /// <typeparam name="T">Tipo de dato que se guarda en el árbol</typeparam>
    interface IAVLtree<T>
    {
        /// <summary>
        /// Verifica si un elemento se encuentra en el árbol
        /// </summary>
        /// <param name="item">Elemento a encontrar</param>
        /// <param name="comparer">comparador para el elemento</param>
        /// <returns>true: si existe el elemento, false: si no existe</returns>
        Boolean Contains(T item, IComparer<T> comparer);
        /// <summary>
        /// Busca un elemento en el árbol
        /// </summary>
        /// <param name="item">Elemento con la información a buscar</param>
        /// <param name="comparer">comparador para el elemento</param>
        /// <returns>Elemento encontrado</returns>
        T Find(T item, IComparer<T> comparer);
        /// <summary>
        /// Añade un elemento a la lista
        /// </summary>
        /// <param name="item">Elemento a añadir</param>
        /// <param name="comparer">comparador para el elemento</param> 
        void Add(T item, IComparer<T> comparer);
        /// <summary>
        /// Elimina un elemento de la lista
        /// </summary>
        /// <param name="item">Elemento a eliminar</param>
        /// <param name="comparer">comparador para el elemento</param>
        void Remove(T item, IComparer<T> comparer);

    }
}
