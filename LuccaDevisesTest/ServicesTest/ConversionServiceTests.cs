using LuccaDevises.IServices;
using LuccaDevises.Models;
using LuccaDevises.Serices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;

namespace LuccaDevisesTest.ServicesTest
{
    [TestClass]
    public class ConversionServiceTests
    {
        private string DATA_FOLDER_PATH_TEST = Directory.GetCurrentDirectory() + @"\Resources";

        Mock<FileService> mockFileT = new();
        Mock<NodeService> mockGraphs = new();

        [TestMethod]
        [ExpectedException(typeof(Exception), "Il n'y a pas de conversion possible, throw exception.")]
        public void ConvertirDevise_NoConversionPathShouldThrow()
        {
            string file = @"\validNodesNoConversionPath.txt";
            string fullPath = $"{DATA_FOLDER_PATH_TEST}{file}";
            ConversionService mockFileBlR = new(mockGraphs.Object, mockFileT.Object);

            mockFileBlR.Convertir(fullPath);
        }

        [TestMethod]
        public void ConvertirDevise_ValidFileShouldConvert()
        {
            string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
            ConversionService mockFileBlR = new(mockGraphs.Object, mockFileT.Object);
            foreach (string file in files.Where(a => a.Contains("validExampleNodes.txt") || a.Contains("validExampleNodesWithEndingNewLine.txt")))
            {
                int result = mockFileBlR.Convertir(file);
                Assert.AreEqual(59033, result);
            }
        }
    }
}
