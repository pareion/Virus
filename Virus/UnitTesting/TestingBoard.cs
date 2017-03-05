using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Virus;

namespace UnitTesting
{
    [TestClass]
    public class TestingBoard
    {
        [TestMethod]
        public void TestMoveInAllDirections()
        {
            TempBoard board = new TempBoard(10);
            board.StartGame();
            board.playerTurnsOn = false;
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 3, 4), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 3, 2), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 2, 4), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 2, 3), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 2, 2), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 4, 4), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 4, 3), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 4, 2), -1);
        }
        [TestMethod]
        public void TestJumpInAllDirections()
        {
            TempBoard board = new TempBoard(10);
            board.StartGame();
            board.playerTurnsOn = false;
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 3, 5), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 5, 3, 3), -1);

            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 3, 1), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 3, 1, 3, 3), -1);

            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 1, 5), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 1, 5, 3, 3), -1);

            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 5, 1), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 5, 1, 3, 3), -1);

            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 5, 5), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 5, 5, 3, 3), -1);

            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 1, 1), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 1, 1, 3, 3), -1);

            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 1, 3), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 1, 3, 3, 3), -1);

            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 5, 3), -1);
            Assert.AreNotEqual(board.MoveBrick(1, 5, 3, 3, 3), -1);
        }
        [TestMethod]
        public void TestMoveIfNotYourTurn()
        {
            TempBoard board = new TempBoard(10);
            board.StartGame();
            board.playerTurnsOn = true;
            Assert.AreNotEqual(board.MoveBrick(1, 3, 3, 3, 4), -1);
            Assert.AreEqual(board.MoveBrick(1, 3, 3, 3, 4), -1);
            Assert.AreEqual(board.MoveBrick(1, 3, 3, 3, 2), -1);
            Assert.AreEqual(board.MoveBrick(1, 3, 3, 2, 4), -1);
            Assert.AreEqual(board.MoveBrick(1, 3, 3, 2, 3), -1);
            Assert.AreEqual(board.MoveBrick(1, 3, 3, 2, 2), -1);
            Assert.AreEqual(board.MoveBrick(1, 3, 3, 4, 4), -1);
            Assert.AreEqual(board.MoveBrick(1, 3, 3, 4, 3), -1);
            Assert.AreEqual(board.MoveBrick(1, 3, 3, 4, 2), -1);
        }
        [TestMethod]
        public void TestCapturePieces()
        {
            TempBoard board = new TempBoard(10);
            board.StartGame();
            board.playerTurnsOn = false;
            board.SetupBoardForCapture();
            Assert.AreEqual(board.MoveBrick(1, 6, 3, 6, 4), 3);
            Assert.AreEqual(board.MoveBrick(1, 6, 4, 6, 5), 3);
            Assert.AreEqual(board.MoveBrick(1, 6, 5, 6, 6), 4);
        }
    }
}
