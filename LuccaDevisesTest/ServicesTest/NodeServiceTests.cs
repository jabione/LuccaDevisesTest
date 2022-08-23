using LuccaDevises.Serices;
using LuccaDevises.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace LuccaDevisesTest.ServicesTest
{
    [TestClass]
    public class NodeServiceTests
    {

        NodePath<string, double> nodePath = new();
        Mock<NodeService> mockNodePaths = new();

        [TestMethod]
        public void AddNode_AddNewValidNodeTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            Assert.IsTrue(nodePath.nodes.Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Ajouter un noeud qui existe déjà, ça doit lever une argument exception.")]
        public void AddNode_AddAlreadyExistingNodeTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            mockNodePaths.Object.AddNode("AUD", nodePath);
        }

        [TestMethod]
        public void AddPath_AddNewValidPathTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            mockNodePaths.Object.AddNode("CHF", nodePath);
        }

        [TestMethod]
        public void AddNode_AddValidSameNodePathButDifferentPathDataTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            mockNodePaths.Object.AddNode("CHF", nodePath);
            mockNodePaths.Object.AddPath("AUD", "CHF", 1.0, nodePath);
            mockNodePaths.Object.AddPath("AUD", "CHF", 1.5, nodePath);
        }

        [TestMethod]
        public void ExecBFS_ValidTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            mockNodePaths.Object.AddNode("CHF", nodePath);
            mockNodePaths.Object.AddNode("JPY", nodePath);
            mockNodePaths.Object.AddNode("KWU", nodePath);
            mockNodePaths.Object.AddNode("EUR", nodePath);
            mockNodePaths.Object.AddNode("USD", nodePath);
            mockNodePaths.Object.AddNode("INR", nodePath);

            mockNodePaths.Object.AddPath("AUD", "CHF", 0.9661, nodePath);
            mockNodePaths.Object.AddPath("CHF", "AUD", Math.Round(1.0 / 0.9661, 4), nodePath);
            mockNodePaths.Object.AddPath("JPY", "KWU", 13.1151, nodePath);
            mockNodePaths.Object.AddPath("KWU", "JPY", Math.Round(1.0 / 13.1151, 4), nodePath);
            mockNodePaths.Object.AddPath("EUR", "CHF", 1.2053, nodePath);
            mockNodePaths.Object.AddPath("CHF", "EUR", Math.Round(1.0 / 1.2053, 4), nodePath);
            mockNodePaths.Object.AddPath("AUD", "JPY", 86.0305, nodePath);
            mockNodePaths.Object.AddPath("JPY", "AUD", Math.Round(1.0 / 86.0305, 4), nodePath);
            mockNodePaths.Object.AddPath("EUR", "USD", 1.2989, nodePath);
            mockNodePaths.Object.AddPath("USD", "EUR", Math.Round(1.0 / 1.2989, 4), nodePath);
            mockNodePaths.Object.AddPath("JPY", "INR", 0.6571, nodePath);
            mockNodePaths.Object.AddPath("INR", "JPY", Math.Round(1.0 / 0.6571, 4), nodePath);

            List<Tuple<string, double>> result = mockNodePaths.Object.SearchNode("EUR", "JPY", nodePath);

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("EUR", result[0].Item1);
            Assert.AreEqual("CHF", result[1].Item1);
            Assert.AreEqual("AUD", result[2].Item1);
            Assert.AreEqual("JPY", result[3].Item1);
            Assert.AreEqual(1.2053, result[1].Item2);
            Assert.AreEqual(1.0351, result[2].Item2);
            Assert.AreEqual(86.0305, result[3].Item2);
        }

        [TestMethod]
        public void ExecBFS_ValidCircularNodePathTest()
        {
            mockNodePaths.Object.AddNode("A", nodePath);
            mockNodePaths.Object.AddNode("B", nodePath);
            mockNodePaths.Object.AddNode("C", nodePath);
            mockNodePaths.Object.AddNode("D", nodePath);
            mockNodePaths.Object.AddNode("E", nodePath);
            mockNodePaths.Object.AddNode("F", nodePath);
            mockNodePaths.Object.AddNode("G", nodePath);

            mockNodePaths.Object.AddPath("A", "B", 1, nodePath);
            mockNodePaths.Object.AddPath("B", "A", 2, nodePath);
            mockNodePaths.Object.AddPath("A", "C", 3, nodePath);
            mockNodePaths.Object.AddPath("C", "A", 4, nodePath);
            mockNodePaths.Object.AddPath("B", "D", 5, nodePath);
            mockNodePaths.Object.AddPath("D", "B", 6, nodePath);
            mockNodePaths.Object.AddPath("C", "E", 7, nodePath);
            mockNodePaths.Object.AddPath("E", "C", 8, nodePath);
            mockNodePaths.Object.AddPath("D", "F", 9, nodePath);
            mockNodePaths.Object.AddPath("F", "D", 10, nodePath);
            mockNodePaths.Object.AddPath("E", "F", 11, nodePath);
            mockNodePaths.Object.AddPath("F", "E", 12, nodePath);
            mockNodePaths.Object.AddPath("F", "G", 13, nodePath);
            mockNodePaths.Object.AddPath("G", "F", 14, nodePath);
            mockNodePaths.Object.AddPath("A", "D", 15, nodePath);
            mockNodePaths.Object.AddPath("D", "A", 16, nodePath);
            mockNodePaths.Object.AddPath("A", "E", 17, nodePath);
            mockNodePaths.Object.AddPath("E", "A", 18, nodePath);

            List<Tuple<string, double>> result = mockNodePaths.Object.SearchNode("A", "G", nodePath);

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("A", result[0].Item1);
            Assert.AreEqual("D", result[1].Item1);
            Assert.AreEqual("F", result[2].Item1);
            Assert.AreEqual("G", result[3].Item1);
            Assert.AreEqual(15, result[1].Item2);
            Assert.AreEqual(9, result[2].Item2);
            Assert.AreEqual(13, result[3].Item2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Un chemin n'est pas trouvé, donc envoi d'exception.")]
        public void ExecBFS_ThrowIfNoPathWasFoundTest()
        {
            mockNodePaths.Object.AddNode("JPY", nodePath);
            mockNodePaths.Object.AddNode("EUR", nodePath);
            mockNodePaths.Object.AddNode("USD", nodePath);

            mockNodePaths.Object.AddPath("EUR", "USD", 1.2989, nodePath);
            mockNodePaths.Object.AddPath("USD", "EUR", Math.Round(1.0 / 1.2989, 4), nodePath);

            mockNodePaths.Object.SearchNode("JPY", "EUR", nodePath);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Un chemin est manquant, insolvable, donc envoi d'exception.")]
        public void ExecBFS_NoSolutionCircularNodePathTest()
        {
            mockNodePaths.Object.AddNode("A", nodePath);
            mockNodePaths.Object.AddNode("B", nodePath);
            mockNodePaths.Object.AddNode("C", nodePath);
            mockNodePaths.Object.AddNode("D", nodePath);

            mockNodePaths.Object.AddPath("A", "B", 1, nodePath);
            mockNodePaths.Object.AddPath("B", "A", 2, nodePath);
            mockNodePaths.Object.AddPath("A", "C", 3, nodePath);
            mockNodePaths.Object.AddPath("C", "A", 4, nodePath);
            mockNodePaths.Object.AddPath("B", "C", 5, nodePath);
            mockNodePaths.Object.AddPath("C", "B", 6, nodePath);

            mockNodePaths.Object.SearchNode("A", "D", nodePath);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Un noeud n'est pas trouvé, donc envoi d'exception.")]
        public void SearchNode_ThrowIfAnInvalidNodeIsProvidedTest()
        {
            mockNodePaths.Object.AddNode("JPY", nodePath);
            mockNodePaths.Object.AddNode("EUR", nodePath);
            mockNodePaths.Object.AddNode("USD", nodePath);

            mockNodePaths.Object.AddPath("EUR", "USD", 1.2989, nodePath);
            mockNodePaths.Object.AddPath("USD", "EUR", Math.Round(1.0 / 1.2989, 4), nodePath);
            mockNodePaths.Object.SearchNode("AUD", "EUR", nodePath);
        }

        [TestMethod]
        public void HasExistingNode_EqualTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            Assert.AreEqual(true, mockNodePaths.Object.ExistNode("AUD", nodePath));
        }

        [TestMethod]
        public void HasExistingNode_NotEqualEmptyNodePathTest()
        {
            Assert.AreNotEqual(true, mockNodePaths.Object.ExistNode("AUD", nodePath));
        }

        [TestMethod]
        public void HasExistingNode_NotEqualTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            Assert.AreNotEqual(true, mockNodePaths.Object.ExistNode("EUR", nodePath));
        }

        [TestMethod]
        public void HasExistingPath_EqualTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            mockNodePaths.Object.AddNode("EUR", nodePath);
            mockNodePaths.Object.AddPath("AUD", "EUR", 1, nodePath);
            Assert.AreEqual(true, mockNodePaths.Object.ExistPath("AUD", "EUR", nodePath));
        }

        [TestMethod]
        public void HasExistingPath_NotEqualTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            mockNodePaths.Object.AddNode("EUR", nodePath);
            Assert.AreNotEqual(true, mockNodePaths.Object.ExistPath("AUD", "EUR", nodePath));
        }

        [TestMethod]
        public void HasExistingPath_InversePathShouldNotEqualTest()
        {
            mockNodePaths.Object.AddNode("AUD", nodePath);
            mockNodePaths.Object.AddNode("EUR", nodePath);
            mockNodePaths.Object.AddPath("EUR", "AUD", 1, nodePath);
            Assert.AreNotEqual(true, mockNodePaths.Object.ExistPath("AUD", "EUR", nodePath));
        }

    }
}
