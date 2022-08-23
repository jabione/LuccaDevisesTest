using LuccaDevises.IServices;
using LuccaDevises.Models;

namespace LuccaDevises.Serices
{
    /// <summary>
    /// Service qui gère la création des noeuds et des chemins
    /// </summary>
    public class NodeService : INodeService
    {

        /// <summary>
        /// Methode qui permet d'avoir le noeud grace à sa valeur
        /// </summary>
        /// <param name="nodepath">chemins des noeuds</param>
        /// <param name="nodeValue">Valeur recherchée</param>
        /// <returns>Noeud</returns>
        private Node<string, double> GetNode(string nodeValue, NodePath<string, double> nodepath)
        {
            if (nodeValue == null)
            {
                throw new ArgumentNullException("Erreur ! La valeur du nœud ne doit pas être nulle");
            }
            if (nodepath == null)
            {
                throw new ArgumentNullException("Erreur ! Le chemin des nœuds ne doit pas être nulle");
            }
            // On récupère le Noeud à partir de sa valeur
            Node<string, double>? node = nodepath.nodes.Find(n => EqualityComparer<string>.Default.Equals(n.Value, nodeValue));

            if (node == null)
            {
                throw new NullReferenceException("Erreur! dans la valeur :" + nodeValue.ToString());
            }
            return node;
        }

        /// <summary>
        /// Effectue une recherche du noeud de départ au noeud d'arrivée
        /// </summary>
        /// <param name="depart">Noeud de départ</param>
        /// <param name="nodepath">nodepath</param>
        /// <param name="cible">Noeud d'arrivée</param>
        /// <returns>Une liste de l'ensemble des chemins empruntés pour atteindre le noeud d'arrivée</returns>
        public List<Tuple<string, double>> SearchNode(string depart, string cible, NodePath<string, double> nodepath)
        {
            //Remise à zero 
            ResetExploredNodes(nodepath);
            // Récupération du noeud de départ
            Node<string, double> startNode = GetNode(depart, nodepath);
            // Récupération du noeaud d'arrivée
            Node<string, double> endNode = GetNode(cible, nodepath);
            //Intansation de la queue pour crée le chemin des noeaud
            Queue<Node<string, double>> queue = new();
            //Noeud déjà passé
            startNode.Passed = true;
            //Création de la queue
            queue.Enqueue(startNode);

            while (queue.Count != 0)
            {
                Node<string, double> currentNode = queue.Dequeue();
                //Si le noeud parent = le noeud enfant on retur en arrière
                if (currentNode == endNode)                
                    return ReturnBack(currentNode);
                
                foreach (Tuple<Node<string, double>, double> edge in currentNode.Edges)
                {
                    // Si on a pas encore passé ce noeud
                    if (!edge.Item1.Passed)
                    {
                        edge.Item1.Passed = true;
                        edge.Item1.ParentNode = new(currentNode, edge.Item2);
                        queue.Enqueue(edge.Item1);
                    }
                }
            }

            throw new Exception($"Aucun chemin trouvé de {depart} à {cible}");
        }


        /// <summary> Réinitialiser toutes les données du nodepath
        ///  <param name="nodepath">chemin des noeuds</param>
        /// </summary>
        private void ResetExploredNodes(NodePath<string, double> nodepath)
        {
            nodepath.nodes.ForEach(n =>
            {
                n.Passed = false;
                n.ParentNode = null;
            });
        }

        /// <summary>
        /// Crée une liste d'étapes (ensemble de chemins) pour aller au noeud sans parent depuis le noeud en cours
        /// </summary>
        /// <param name="node">Retracer le chemin à partir de ce noeud</param>
        /// <returns>Une liste d'étapes avec à chaque fois les données de chemin qui ont été associées (la première étape a une valeur par défaut en tant que données de chemin car il ne s'agit pas d'une étape effectuée)</returns>
        private static List<Tuple<string, double>> ReturnBack(Node<string, double> node)
        {
            List<Tuple<string, double>> path = new();
            while (node != null)
            {
                if (node.ParentNode == null)
                {
                    path.Add(new(node.Value, default));
                    break;
                }
                path.Add(new(node.Value, node.ParentNode.Item2));
                node = node.ParentNode.Item1;
            }
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Booleen pour savoir si le noeud est existant
        /// </summary>
        /// <param name="nodepath">Chemin des noeuds</param>
        /// <param name="nodeValue">valeur du noeud</param>
        /// <returns>vrai s'il existe un nœud existant avec les mêmes données de nœud, sinon c'est faux</returns>
        public bool ExistNode(string nodeValue, NodePath<string, double> nodepath)
        {
            Node<string, double>? node = nodepath.nodes.Find(n => EqualityComparer<string>.Default.Equals(n.Value, nodeValue));
            return node != null;
        }

        /// <summary>
        /// Vérification si le chemin existe
        /// </summary>
        /// <param name="depart">Noeud de départ</param>
        /// <param name="nodepath">Chemins des noeuds</param>
        /// <param name="cible">Noeud d'arrivée</param>
        /// <returns>vrai si un chemin existe, faux sinon</returns>
        /// <exception cref="ArgumentException">Si au moins une des données de nœud données est nulle</exception>
        public bool ExistPath(string depart, string cible, NodePath<string, double> nodepath)
        {
            Node<string, double> nodeDepart = GetNode(depart, nodepath);
            Node<string, double> nodeCible = GetNode(cible, nodepath);
            return nodeDepart.Edges.Find(t => EqualityComparer<string>.Default.Equals(t.Item1.Value, nodeCible.Value)) != null;
        }



        /// <summary>
        /// Créer un nodepath basé sur les nœuds existants avec les chemins (des taux de change) qui les relient
        /// </summary>
        /// <param name="startNode">Noeud de départ</param>
        /// <param name="endNode">Noeud d'arrivée</param>
        /// <param name="paths">Les différents taux de change qui existent</param>
        /// <returns>Un nodepathique qui représente tous les échanges possibles</returns>
        public NodePath<string, double> CreateNodes(string startNode, string endNode, List<Tuple<string, double, string>> paths)
        {
            NodePath<string, double> path = new NodePath<string, double>();

            AddNode(startNode, path);
            AddNode(endNode, path);

            foreach ((string depart, double tauxChange, string cible) in paths)
            {
                //Si le noeud parent n'existe pas
                if (!ExistNode(depart, path))
                {
                    AddNode(depart, path);
                }
                //Si le noeud enfant n'existe pas
                if (!ExistNode(cible, path))
                {
                    AddNode(cible, path);
                }

                //Si le chemin n'exsite pas
                if (!ExistPath(depart, cible, path))
                {
                    //Ajout chemin normal de taux
                    AddPath(depart, cible, tauxChange, path);
                    //Ajout chemin reverse de taux
                    AddPath(cible, depart, Math.Round(1.0 / tauxChange, 4), path);
                }
                else
                {
                    throw new Exception($"Erreur ! : la conversion de {depart} à {cible} est répétée plusieurs fois !!");
                }
            }

            return path;
        }


        /// <summary>
        /// Ajouter un noeud au chemin passé en param
        /// </summary>
        /// <param name="nodeValue">valeur du noeud</param>
        /// <param name="nodepath">nodepath</param>
        public void AddNode(string nodeValue, NodePath<string, double> nodepath)
        {
            Node<string, double> newNode = new(nodeValue);
            if (ExistNode(nodeValue, nodepath))
            {
                throw new ArgumentException($"Noeud {nodeValue} existe déjà !");
            }
            // Ajout d'un noeud
            nodepath.nodes.Add(newNode);
        }

        /// <summary>
        /// Ajouter un chemin d'un noeud à un autre mais dans un sens unique A vers B seulement mais pas B vers A
        /// </summary>
        /// <param name="nodeA">Noeud de départ</param>
        /// <param name="nodePath">chemin des noeuds</param>
        /// <param name="nodeB">Noeud d'arrivée</param>
        /// <param name="pathValue">Valeur du chemin reliant les 2 noeuds</param>
        public void AddPath(string nodeA, string nodeB, double pathValue, NodePath<string, double> nodePath)
        {
            //Obtenir les noeud relié au noeud de départ et d'arrivée
            Node<string, double>? pathNodeA = GetNode(nodeA, nodePath);
            Node<string, double>? pathNodeB = GetNode(nodeB, nodePath);
            //Ajout d'un chemin vers un autre noeud
            AddEdge(pathNodeB, pathValue, pathNodeA);
        }


        /// <summary>
        /// Ajouter un nouveau chemin à partir de ce noeud
        /// </summary>
        /// <param name="newEdge">noeud d'arrivée depuis le noeud en court</param>
        /// <param name="node">nodepath</param>
        /// <param name="pathValue">valeur associée au chemin (taux)</param>
        public void AddEdge(Node<string, double> newEdge, double pathValue, Node<string, double> node)
        {
            Tuple<Node<string, double>, double> listElement = new(newEdge, pathValue);
            if (node.Edges.Contains(listElement))
            {
                throw new ArgumentException($"Le chemin {newEdge} existe déjà !");
            }
            //Ajout d'un chemin vers un autre noeud
            node.Edges.Add(listElement);
        }
    }
}
