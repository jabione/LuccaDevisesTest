using LuccaDevises.Models;

namespace LuccaDevises.IServices
{
    /// <summary>
    /// Interface du service noeud
    /// </summary>
    public interface INodeService
    {
        /// <summary>
        /// Ajouter un noeud au chemin passé en param
        /// </summary>
        /// <param name="nodeValue">valeur du noeud</param>
        /// <param name="nodepath">nodepath</param>
        void AddNode(string nodeValue, NodePath<string, double> nodepath);

        /// <summary>
        /// Ajouter un chemin d'un noeud à un autre mais dans un sens unique A vers B seulement mais pas B vers A
        /// </summary>
        /// <param name="nodeA">Noeud de départ</param>
        /// <param name="nodePath">chemin des noeuds</param>
        /// <param name="nodeB">Noeud d'arrivée</param>
        /// <param name="pathValue">Valeur du chemin reliant les 2 noeuds</param>
        void AddPath(string nodeA, string nodeB, double pathValue, NodePath<string, double> nodePath);

        /// <summary>
        /// Créer un nodepath basé sur les nœuds existants avec les chemins (des taux de change) qui les relient
        /// </summary>
        /// <param name="startNode">Noeud de départ</param>
        /// <param name="endNode">Noeud d'arrivée</param>
        /// <param name="paths">Les différents taux de change qui existent</param>
        /// <returns>Un nodepathique qui représente tous les échanges possibles</returns>
        NodePath<string, double> CreateNodes(string startNode, string endNode, List<Tuple<string, double, string>> paths);

        /// <summary>
        /// Effectue une recherche du noeud de départ au noeud d'arrivée
        /// </summary>
        /// <param name="depart">Noeud de départ</param>
        /// <param name="nodepath">nodepath</param>
        /// <param name="cible">Noeud d'arrivée</param>
        /// <returns>Une liste de l'ensemble des chemins empruntés pour atteindre le noeud d'arrivée</returns>
        public List<Tuple<string, double>> SearchNode(string depart, string cible, NodePath<string, double> nodepath);

        /// <summary>
        /// Booleen pour savoir si le noeud est existant
        /// </summary>
        /// <param name="nodepath">Chemin des noeuds</param>
        /// <param name="nodeValue">valeur du noeud</param>
        /// <returns>vrai s'il existe un nœud existant avec les mêmes données de nœud, sinon c'est faux</returns>
        bool ExistNode(string nodeValue, NodePath<string, double> nodepath);


        /// <summary>
        /// Booleen pour savoir si le chemin existe
        /// </summary>
        /// <param name="depart">Noeud de départ</param>
        /// <param name="nodepath">Chemins des noeuds</param>
        /// <param name="cible">Noeud d'arrivée</param>
        /// <returns>vrai si un chemin existe, faux sinon</returns>
        bool ExistPath(string depart, string cible, NodePath<string, double> nodepath);
    }
}
