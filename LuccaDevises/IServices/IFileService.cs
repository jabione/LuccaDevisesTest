namespace LuccaDevises.IServices
{
    /// <summary>
    /// Interface de service Files
    /// </summary>
    public interface IFileService
    {

        /// <summary>
        /// Methode qui retourne les lignes du fichier
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        List<string> GetLines(string filePath);

        /// <summary>
        /// Methode qui fait le parsing de la première ligne du fichier en Tuple
        /// </summary>
        /// <param name="firstline">list à parser en Tuple et qui contient la devise de départ,taux de change et la devise d'arrivée </param>
        /// <returns>Objet tuple qui contient la première ligne du fichier</returns>
        Tuple<string, int, string> ToTupleFirstLine(string[] firstline);

        /// <summary>
        /// Methode qui fait le parsing de ligne de taux de change en Tuple
        /// </summary>
        /// <param name="lineNumber">Numéro de ligne</param>
        /// <param name="line">Ligne à Parser</param>
        /// <returns>Objet Tuple qui contient la devise de depart,le taux de change et la devise d'arrivée </returns>
        Tuple<string, double, string> ToTupleTauxChange(int lineNum, string[] line);

        /// <summary>
        /// Methode qui fait le parsing des listes des taux de change avec les devises en Tuple
        /// </summary>
        /// <param name="lines">List Tuple des numéros de lignes et des changes</param>
        /// <returns>List Tuple des changes</returns>
        List<Tuple<string, double, string>> ToTupleLinesTauxChange(List<Tuple<int, string>> lines);
    }
}
