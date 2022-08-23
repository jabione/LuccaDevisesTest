using LuccaDevises.IServices;
using LuccaDevises.Serices;
using Unity;

namespace LuccaDevises
{
    public class UnityConfig
    {
        public static IUnityContainer? Container;

        /// <summary>
        /// instance de unitycontainer
        /// </summary>
        public static void Start()
        {
            Container = new UnityContainer();
            Register();
        }

        /// <summary>
        /// Binding Interface au service
        /// </summary>
        private static void Register()
        {
            Container.RegisterType<IFileService, FileService>();
            Container.RegisterType<INodeService, NodeService>();
            Container.RegisterType<IConversionService, ConversionService>();

        }
    }
}
