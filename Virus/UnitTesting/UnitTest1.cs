using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Virus;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMoveBricksInAllDirections()
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
            board.MoveBrick(1, 0, 3, 1, 3);

        }
    }
}
