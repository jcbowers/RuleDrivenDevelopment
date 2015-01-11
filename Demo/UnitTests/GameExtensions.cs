using System;
using BowlingKata;

namespace BowlingKataTest
{
    static class GameExtensions
    {
        public static Game RollMany(this Game game, int rolls, UInt16 pins)
        {
            for (int i = 0; i < rolls; i++)
            {
                game.Roll(pins);
            }

            return game;
        }
    }
}
