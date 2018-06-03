using System;

using BowlingKata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace BowlingKataTest
{
    [TestClass]
    public class BowlingKataUnitTest
    {
        Game game;
        IUnityContainer _container;

        public BowlingKataUnitTest()
        {
            _container = new UnityContainer();
            GameBootStrapper.Configure(_container);
        }

        [TestInitialize]
        public void Cleanup()
        {         
            game = _container.Resolve<Game>();
        }

        #region Original Unit Tests

            [TestMethod]
            public void AllGutterBalls()
            {
                game.RollMany(20, 0);
                Assert.AreEqual(0, game.Score());
            }

            [TestMethod]
            public void PerfectGame()
            {
                game.RollMany(21, 10);
                Assert.AreEqual(300, game.Score());
            }

            [TestMethod]
            public void ThreeSpares()
            {
                game.RollMany(3, 5);
                Assert.AreEqual(20, game.Score());
            }

        [TestMethod]
            public void AllSpares()
            {
                game.RollMany(21, 5);
                Assert.AreEqual(150, game.Score());
            }

            [TestMethod]
            public void AllOnes()
            {
                game.RollMany(20, 1);
                Assert.AreEqual(20, game.Score());
            }

        #endregion

        #region Extended Domain Tests
            [TestMethod]
            public void Turkey()
            {
                game.RollMany(3, 10);
                Assert.AreEqual(40, game.Score());

            }

            [TestMethod]
            public void OneStrike()
            {
                game.Roll(10).Roll(5).Roll(5);
                Assert.AreEqual(25, game.Score());
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void InvalidBonus()
            {
                game.RollMany(21, 1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void TooManyPinsForFrame()
            {
                game.Roll(9).Roll(9);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void TooManyPinsForRoll()
            {
                game.Roll(11);
            }

        [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void TooManyRollsInFinalFrame()
            {
                game.RollMany(18, 10).RollMany(3,1);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void TooManyRolls()
            {
                game.RollMany(22, 1);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void TooManySpares()
            {
                game.RollMany(22, 5);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void TooManyStrikes()
            {
                game.RollMany(22, 10);
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
