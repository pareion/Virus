﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(7);
            game.StartGame();
        }
    }
}
