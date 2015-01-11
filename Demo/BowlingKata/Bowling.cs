using System;
using System.Collections.Generic;
using System.Linq;

using JasonSoft.RDD;

namespace BowlingKata
{
    public class Game : IDomainAggregate<Game>
    {
        public IReadOnlyList<Frame> Frames { get; private set; }        
        public virtual Func<IEnumerable<Frame>, int> ScoreRule { private get; set; }
        public virtual Func<Frame, int, bool> TooManyPins { private get; set; }

        LinkedList<Frame> _frames = new LinkedList<Frame>();

        public StateRule<Game>[] Rules { get; set; }

        public Game()
        {
            _frames.AddFirst(new FinalFrame());

            for (int i = 0; i < 9; i++)
                _frames.AddFirst(new Frame(_frames.First()));

            Frames = _frames.ToList();

            CurrentFrame = _frames.First();
        }

        [ProcessRules(ProcessRulesAdvise.Around)]
        public virtual Game Roll(UInt16 pins)
        {
            CurrentFrame = _frames.First(f => !f.IsComplete);

            if (TooManyPins(CurrentFrame, pins))
                throw new ArgumentException("Too many pins");

            CurrentFrame.AddRoll(pins);

            return this;
        }

        public Frame CurrentFrame { get; private set; }

        public int Score()
        {
            return ScoreRule(_frames);
        }
    }

    public class Frame
    {
        public Frame NextFrame { get; internal set; }

        protected List<ushort> _rolls;

        public IReadOnlyList<ushort> Rolls { get; protected set; }

        internal Frame()
        {
            _rolls = new List<ushort>(2);
            Rolls = _rolls;
        }

        internal Frame(Frame nextFrame) : this()
        {
            NextFrame = nextFrame;
        }

        public virtual bool IsComplete
        {
            get { return _rolls.Count == 2; }
        }

        public ushort Score
        {
            get { return (ushort)((ScoreRule ?? new Func<Frame, int>((f) => 0))(this)); }
        }

        public virtual Func<Frame, int> ScoreRule { private get; set; }

        internal void AddRoll(ushort pins)
        {
            _rolls.Add(pins);
        }

        public virtual bool IsFinalFrame
        {
            get { return false; }
        }

        public bool IsStrikeOrSpare
        {
            get { return Rolls.Take(2).Sum(i => i) >= 10; }
        }
    }

    public class FinalFrame : Frame
    {
        internal FinalFrame()
        {
            _rolls = new List<ushort>(3);
            Rolls = _rolls;
        }

        public override bool IsComplete
        {
            get
            {
                return (IsStrikeOrSpare && _rolls.Count == 3) || (!IsStrikeOrSpare && _rolls.Count == 2);
            }
        }

        public override bool IsFinalFrame
        {
            get { return true; }
        }
    }
}
