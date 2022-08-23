using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises.Models
{
    /// <summary>
    /// Objet Noeud
    /// </summary>
    /// <typeparam name="Tv">Valeur du noeud</typeparam>
    /// <typeparam name="Tq">Quantification de la valeur du noeud</typeparam>
    public class Node<Tv, Tq>
    {
        /// <summary> 
        /// Valeur du noeud 
        /// </summary>
        public string Value { get; }

        /// <summary> 
        /// Booléen pour savoir si on est passé par ce noeud ou pas 
        /// </summary>
        public bool Passed { get; set; }

        /// <summary> 
        /// Chemins vers d'autres noeuds depuis ce noeud ( Edge )
        /// </summary>
        public List<Tuple<Node<string, double>, double>> Edges { get; }

      
        /// <summary> 
        /// Le noeur parent (par quel noeud on passe pour arriver sur le noeud en question)
        /// </summary>
        public Tuple<Node<string, double>, double>? ParentNode { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="data">On passe la valeur du noeud en param</param>
        public Node(string data)
        {          
            Passed = false;
            Value = data;
            Edges = new List<Tuple<Node<string, double>, double>>();
            ParentNode = null;
        }
    }
}
