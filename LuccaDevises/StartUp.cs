using LuccaDevises.IServices;

namespace LuccaDevises
{
    public class StartUp
    {
        private IConversionService _IConversionService;
        public StartUp(IConversionService Iconversionservice)
        {
            _IConversionService = Iconversionservice;
        }

        /// <summary>
        /// Run App
        /// </summary>
        /// <param name="filePath"></param>
        public void Run(string filePath)
        {
            Console.WriteLine($"{_IConversionService.Convertir(filePath)}");
        }

    }
}
