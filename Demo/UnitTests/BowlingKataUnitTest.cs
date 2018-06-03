using System;

using BowlingKata;
using Unity;
using Xunit;

namespace BowlingKataTest
{
    public class BowlingKataUnitTest
    {
        Game game;
        IUnityContainer _container;

        public BowlingKataUnitTest()
        {
            _container = new UnityContainer();
            GameBootStrapper.Configure(_container);
            game = _container.Resolve<Game>();
        }

        #region Original Unit Tests

            [Fact]
            public void AllGutterBalls()
            {
                game.RollMany(20, 0);
                Assert.Equal(0, game.Score());
            }

            [Fact]
            public void PerfectGame()
            {
                game.RollMany(21, 10);
                Assert.Equal(300, game.Score());
            }

            [Fact]
            public void ThreeSpares()
            {
                game.RollMany(3, 5);
                Assert.Equal(20, game.Score());
            }

            [Fact]
            public void AllSpares()
            {
                game.RollMany(21, 5);
                Assert.Equal(150, game.Score());
            }

            [Fact]
            public void AllOnes()
            {
                game.RollMany(20, 1);
                Assert.Equal(20, game.Score());
            }

        #endregion

        #region Extended Domain Tests
            [Fact]
            public void Turkey()
            {
                game.RollMany(3, 10);
                Assert.Equal(40, game.Score());

            }

            [Fact]
            public void OneStrike()
            {
                game.Roll(10).Roll(5).Roll(5);
                Assert.Equal(25, game.Score());
            }

            [Fact]
            public void InvalidBonus()
            {
            Assert.Throws<InvalidOperationException>(() => game.RollMany(21, 1));
            }

            [Fact]
            public void TooManyPinsForFrame()
            {
                Assert.Throws<ArgumentException>(() => game.Roll(9).Roll(9));
            }

            [Fact]
            public void TooManyPinsForRoll()
            {
                Assert.Throws<ArgumentException>(() => game.Roll(11));
            }

            [Fact]
            public void TooManyRollsInFinalFrame()
            {
            Assert.Throws<InvalidOperationException>(() => game.RollMany(18, 10).RollMany(3,1));
            }

            [Fact]
            public void TooManyRolls()
            {
            Assert.Throws<InvalidOperationException>(() => game.RollMany(22, 1));
            }

            [Fact]
            public void TooManySpares()
            {
                Assert.Throws<InvalidOperationException>(() => game.RollMany(22, 5));
            }

            [Fact]
            public void TooManyStrikes()
            {
                Assert.Throws<InvalidOperationException>(() => game.RollMany(22, 10));
            }
        #endregion
    }

    static class NumericExtensions
    {
        public static bool Between(this int number, int low, int high)
        {
            return number >= low && number <= high;
        }

        public static int Max(this int number, int maximumValue)
        {
            return number > maximumValue ? maximumValue : number;
        }
    }
}
