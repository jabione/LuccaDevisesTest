using LuccaDevises.IServices;
using LuccaDevises.Models;

namespace LuccaDevises.Serices
{
    /// <summary>
    /// Service qui gère la conversion des devises et qui hérite de l'interface IConversionService
    /// </summary>
    public class ConversionService : IConversionService
    {
        private IFileService _IFileService;
        private INodeService _INodeService;

        /// <summary>
        /// Constructeur du service conversion
        /// </summary>
        /// <param name="INodeService">Interface du service du noeud</param>
        /// <param name="IFileService">Interface de service de qui gère le fichier d'entrée</param>
        public ConversionService(INodeService inodeService, IFileService ifileService)
        {
            _INodeService = inodeService;
            _IFileService = ifileService;
        }

        /// <summary>
        /// Methode de conversion de devise
        /// </summary>
        /// <param name="filePath">chemin du fichier d'entrée</param>
        /// <returns>Le montant converti</returns>
        public int Convertir(string filePath)
        {
            //Read lines
            List<string> lines = _IFileService.GetLines(filePath);

            //Parsing 
            var firstLine = _IFileService.ToTupleFirstLine(lines[0].Split(';'));

            // instancier l'objet Files
            Files ft = new Files();
            ft.Depart = firstLine.Item1;
            ft.MontantInitial = firstLine.Item2;
            ft.Cible = firstLine.Item3;

            //list Tuple des changes avec le numero de ligne
             ft.listTauxChange = _IFileService.ToTupleLinesTauxChange(lines.Select((line, index) => new Tuple<int, string>(index + 1, line))
                .ToList()
                .GetRange(2, lines.Count - 2)
                .FindAll(t => t.Item2.Length != 0));

            double result = ft.MontantInitial;
            //Crée le chemin
            NodePath<string, double> path = _INodeService.CreateNodes(ft.Depart, ft.Cible, ft.listTauxChange);
            //Recherche de noeud
            List<Tuple<string, double>> childNode = _INodeService.SearchNode(ft.Depart, ft.Cible, path);
            //On boucle sur le childNode pour faire des multiplication de taux
            foreach ((string devise, double taux) in childNode)
            {
                if (devise != ft.Depart)
                    result = Math.Round(result * taux, 4);
            }
            // return le resultat en Int
            return (int)Math.Round(result);
        }
    }
}
