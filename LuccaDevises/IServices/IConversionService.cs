namespace LuccaDevises.IServices
{
    //Interface du service conversion
    public interface IConversionService
    {
        /// <summary>
        ///  Methode de conversion de devise
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        int Convertir(string filePath);
    }
}
