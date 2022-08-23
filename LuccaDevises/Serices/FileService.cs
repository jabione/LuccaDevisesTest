using LuccaDevises.IServices;
using System.Globalization;

namespace LuccaDevises.Serices
{
    /// <summary>
    /// Service qui gère le fichier d'entrée et qui hérite de l'interface IFileService
    /// </summary>
    public class FileService : IFileService
    {

        /// <summary>
        /// Methode qui retourne les lignes du fichier
        /// </summary>
        /// <param name="filePath">Chemin du fichier</param>
        /// <returns>Les lignes du fichier</returns>
        /// <exception cref="Exception"></exception>
        public List<string> GetLines(string filePath)
        {
            try
            {
                List<string> lines = new List<string>();
                //Lecture du fichier
                foreach (string line in File.ReadLines(filePath))
                {
                    //Ajout des lignes
                    lines.Add(line.TrimEnd('\r', '\n'));
                }

                //Vérification si le nombre des lignes de fichier sont correct
                if (lines.Count <= 2)
                {
                    throw new FormatException(@"Le fichier doit contenir minimum 3 lignes");
                }

                //Vérification si le nombre des lignes est entier
                int lineNum;
                if (!Int32.TryParse(lines[1], out lineNum) || lineNum <= 0)
                {
                    throw new FormatException(@"Erreur ! Nombre des lignes doit être un nombre entier");
                }

                //Comparer le nombre des lignes avec la liste des taux de change
                if (!(lineNum == (lines.Count() - 2)))
                {
                    throw new FormatException(@"Erreur ! Nombre des lignes doit correspondre au nombre des lignes de taux de change");
                }

                return lines;
            }
            catch (FileLoadException e)
            {
                throw new FileLoadException(e.ToString());
            }
        }

        /// <summary>
        /// Methode qui fait le parsing de la première ligne du fichier en Tuple
        /// </summary>
        /// <param name="firstline">list à parser en Tuple et qui contient la devise de départ,taux de change et la devise d'arrivée </param>
        /// <returns>Objet tuple qui contient la première ligne du fichier</returns>
        public Tuple<string, int, string> ToTupleFirstLine(string[] firstline)
        {
            int Montant;

            if (firstline.Length != 3)
            {
                throw new FormatException(@"Erreur ! La première ligne doit avoir le format (D1;M;D2)");
            }
            if (firstline[0].Length != 3)
            {
                throw new FormatException(@"Erreur ! D1 ou devise de départ doit être un code de 3 caractères");
            }
            if (!Int32.TryParse(firstline[1], out Montant) || Montant <= 0)
            {
                throw new FormatException(@"Erreur ! M doit être un nombre entier");
            }
            if (firstline[2].Length != 3)
            {
                throw new FormatException(@"Erreur ! D2 ou devise d'arrivée doit être un code de 3 caractères");
            }
            if (firstline[0] == firstline[2])
            {
                throw new FormatException(@"Erreur ! D1 et D2 ne doivent pas être identiques");
            }
            return new(firstline[0], Montant, firstline[2]);
        }

        /// <summary>
        /// Methode qui fait le parsing de ligne de taux de change en Tuple
        /// </summary>
        /// <param name="lineNumber">Numéro de ligne dans le fichier</param>
        /// <param name="line">Ligne à Parser</param>
        /// <returns>Objet Tuple qui contient la devise de départ, taux de change et la devise d'arrivée </returns>
        public Tuple<string, double, string> ToTupleTauxChange(int lineNum, string[] lines)
        {
            double tauxChange;

            if (lines.Length != 3)
            {
                throw new FormatException(@"Erreur ! Format de la ligne {lineNum} doit être le suivant : DD;T;DA ");
            }
            if (lines[0].Length != 3)
            {
                throw new FormatException(@"Erreur ! Format de DD de la ligne {lineNum} est invalide : il doit être un code de 3 caractères");
            }
            if (lines[1].Length != 3)
            {
                throw new FormatException(@"Erreur ! Format de DA de la ligne {lineNum} est invalide : il doit être un code de 3 caractères");
            }

            if (lines[2].Split('.').Length != 2 || lines[2].Split('.')[1].Length != 4)
            {
                throw new FormatException(@"Erreur ! Format de T de la ligne {lineNum} est invalide, T doit être un nombre à 4 décimales séparé par '.' ");
            }

            if (!Double.TryParse(lines[2], NumberStyles.Number, CultureInfo.InvariantCulture, out tauxChange) || tauxChange <= 0)
            {
                throw new FormatException(@"Erreur ! Format de T de la ligne {lineNum} est invalide, T doit être un nombre à 4 décimales séparé par '.'");
            }

            return new(lines[0], tauxChange, lines[1]);
        }

        /// <summary>
        /// Methode qui fait le parsing des listes des taux de change avec les devises en Tuple
        /// </summary>
        /// <param name="lines">List Tuple des numéros de lignes et des taux de change</param>
        /// <returns>List Tuple des taux de change</returns>
        public List<Tuple<string, double, string>> ToTupleLinesTauxChange(List<Tuple<int, string>> lines)
        {
            List<Tuple<string, double, string>> listTauxChanges = new();
            foreach ((int lineNum, string line) in lines)
            {
                listTauxChanges.Add(ToTupleTauxChange(lineNum, line.Split(';')));
            }
            return listTauxChanges;
        }

    }
}
