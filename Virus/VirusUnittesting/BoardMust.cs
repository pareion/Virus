using System;
using Virus2._0.Game;
using Xunit;

namespace VirusUnittesting
{
    public class BoardMust
    {
        [Fact]
        public void BeAbleToMakeAHorizontalMove()
        {
            var board = SetupBoard();

            Assert.True(board.IsMovePossible(3, 3, 2, 3));
            Assert.True(board.IsMovePossible(3, 3, 4, 3));
        }

        private static Board SetupBoard()
        {
            var board = new Board(7);
            board.Gameboard = new int[7, 7];

            board.Gameboard[3, 3] = 1;
            return board;
        }

        [Fact]
        public void BeAbleToMakeAVerticalMove()
        {
            var board = new Board(7);
            board.Gameboard = new int[7, 7];

            board.Gameboard[3, 3] = 1;

            Assert.True(board.IsMovePossible(3, 3, 3, 2));
            Assert.True(board.IsMovePossible(3, 3, 3, 4));
        }

        [Fact]
        public void BeAbleToMakeASlantMove()
        {
            var board = new Board(7);
            board.Gameboard = new int[7, 7];

            board.Gameboard[3, 3] = 1;

            Assert.True(board.IsMovePossible(3, 3, 2, 2));
            Assert.True(board.IsMovePossible(3, 3, 2, 4));
            Assert.True(board.IsMovePossible(3, 3, 4, 2));
            Assert.True(board.IsMovePossible(3, 3, 4, 4));
        }

        [Fact]
        public void BeAbleToMakeAJumpHorizontalMove()
        {
            var board = new Board(7);
            board.Gameboard = new int[7, 7];

            board.Gameboard[3, 3] = 1;

            Assert.True(board.IsMovePossible(3, 3, 1, 3));
            Assert.True(board.IsMovePossible(3, 3, 5, 3));
        }

        [Fact]
        public void BeAbleToMakeAJumpVerticalMove()
        {
            var board = new Board(7);
            board.Gameboard = new int[7, 7];

            board.Gameboard[3, 3] = 1;

            Assert.True(board.IsMovePossible(3, 3, 3, 1));
            Assert.True(board.IsMovePossible(3, 3, 3, 5));
        }

        [Fact]
        public void BeAbleToMakeAJumpSlantMove()
        {
            var board = new Board(7);
            board.Gameboard = new int[7, 7];

            board.Gameboard[3, 3] = 1;

            Assert.True(board.IsMovePossible(3, 3, 1, 1));
            Assert.True(board.IsMovePossible(3, 3, 1, 5));
            Assert.True(board.IsMovePossible(3, 3, 5, 1));
            Assert.True(board.IsMovePossible(3, 3, 5, 5));
        }
    }
}
