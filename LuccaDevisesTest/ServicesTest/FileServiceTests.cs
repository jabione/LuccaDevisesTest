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
    public class FileServiceTests
    {
        private string DATA_FOLDER_PATH_TEST = Directory.GetCurrentDirectory() + @"\Resources";

        Mock<FileService> mockFileT = new();
        Mock<INodeService> mockNodePaths = new();

        [TestMethod]
        public void FileParser_ValidFileParsedNodePathIsCorrect()
        {

            string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
            foreach (string file in files.Where(a => a.Contains("validExampleNodePath.txt") || a.Contains("validExampleNodePathWithEndingNewLine.txt")))
            {
                Files ft = new();

                NodePath<string, double> MyNodePath = mockNodePaths.Object.CreateNodes(ft.Depart,ft.Cible, ft.listTauxChange);
                Assert.AreEqual(true, mockNodePaths.Object.ExistNode("AUD", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistNode("CHF", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistNode("JPY", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistNode("KRW", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistNode("INR", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistNode("EUR", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistNode("USD", MyNodePath));

                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("CHF", "AUD", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("AUD", "CHF", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("AUD", "JPY", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("JPY", "AUD", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("JPY", "KRW", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("KRW", "JPY", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("JPY", "INR", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("INR", "JPY", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("CHF", "EUR", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("EUR", "CHF", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("USD", "EUR", MyNodePath));
                Assert.AreEqual(true, mockNodePaths.Object.ExistPath("EUR", "USD", MyNodePath));

                Assert.AreEqual(0.9661, mockNodePaths.Object.SearchNode("AUD", "CHF", MyNodePath)[1].Item2);
                Assert.AreEqual(1.0351, mockNodePaths.Object.SearchNode("CHF", "AUD", MyNodePath)[1].Item2);
                Assert.AreEqual(0.0116, mockNodePaths.Object.SearchNode("JPY", "AUD", MyNodePath)[1].Item2);
                Assert.AreEqual(0.0762, mockNodePaths.Object.SearchNode("KRW", "JPY", MyNodePath)[1].Item2);
                Assert.AreEqual(0.6571, mockNodePaths.Object.SearchNode("JPY", "INR", MyNodePath)[1].Item2);
                Assert.AreEqual(1.5218, mockNodePaths.Object.SearchNode("INR", "JPY", MyNodePath)[1].Item2);
                Assert.AreEqual(0.8297, mockNodePaths.Object.SearchNode("CHF", "EUR", MyNodePath)[1].Item2);
                Assert.AreEqual(1.2053, mockNodePaths.Object.SearchNode("EUR", "CHF", MyNodePath)[1].Item2);
                Assert.AreEqual(0.7699, mockNodePaths.Object.SearchNode("USD", "EUR", MyNodePath)[1].Item2);
                Assert.AreEqual(1.2989, mockNodePaths.Object.SearchNode("EUR", "USD", MyNodePath)[1].Item2);
                Assert.AreEqual(13.1151, mockNodePaths.Object.SearchNode("JPY", "KRW", MyNodePath)[1].Item2);
                Assert.AreEqual(86.0305, mockNodePaths.Object.SearchNode("AUD", "JPY", MyNodePath)[1].Item2);
            }

        }

        [TestMethod]
        public void FileParser_ValidFileParsedFromCurrencyIsCorrect()
        {
            string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
            foreach (string file in files.Where(a => a.Contains("validExampleNodePath.txt") || a.Contains("validExampleNodePathWithEndingNewLine.txt")))
            {
                Files ft = new();
                Assert.AreEqual("EUR", ft.Depart);
            }
        }

        [TestMethod]
        public void FileParser_ValidFileParsedToCurrencyIsCorrect()
        {
            string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
            foreach (string file in files.Where(a => a.Contains("validExampleNodePath.txt") || a.Contains("validExampleNodePathWithEndingNewLine.txt")))
            {
                Files ft = new();
                Assert.AreEqual("JPY", ft.Cible);
            }
        }

        [TestMethod]
        public void FileParser_ValidFileParsedInitialAmountIsCorrect()
        {
            string[] files = Directory.GetFiles($"{DATA_FOLDER_PATH_TEST}");
            foreach (string file in files.Where(a => a.Contains("validExampleNodePath.txt") || a.Contains("validExampleNodePathWithEndingNewLine.txt")))
            {
                Files ft = new();
                Assert.AreEqual(550, ft.MontantInitial);
            }
        }
    }
}
