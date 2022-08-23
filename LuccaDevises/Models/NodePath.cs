using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises.Models
{
    /// <summary>
    /// l'objet des chemins des noeuds
    /// </summary>
    /// <typeparam name="TDevise">Devise</typeparam>
    /// <typeparam name="TValue">Taux</typeparam>
    public class NodePath<TDevise, TValue>
    {
        /// <summary>
        /// Liste des noeuds 
        /// </summary>
        public List<Node<string, double>> nodes;

        /// <summary> 
        /// Constructeur 
        /// </summary>
        public NodePath()
        {
            nodes = new();
        }
    }
}
