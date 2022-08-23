using Unity;

namespace LuccaDevises
{
    class Program
    {
        /// <summary>
        /// Lancer le main du programme
        /// </summary>
        /// <param name="args">Arguments d'entrée</param>
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    // Si le fichier existe
                    if (File.Exists(args[0]))
                    {
                        // lancer le startup
                        UnityConfig.Start();
                        var consoleAdapter = UnityConfig.Container.Resolve<StartUp>();
                        consoleAdapter.Run(args[0]);
                    }
                    else
                    {
                        Console.WriteLine("Le chemin de fichier est non valide");
                    }
                }
                else
                {
                    Console.WriteLine("Argument invalide : Veuillez renseigner le chemin de fichier d'entrée");
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(@"Une erreur est survenue! détail erreur:");
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
