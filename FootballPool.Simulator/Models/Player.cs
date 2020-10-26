using System;
using FootballPool.Simulator.enums;

namespace FootballPool.Simulator
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Aggresive { get; set; }
        public int Defensive { get; set; }
        public int GoalKeeping { get; set; }
        public int Age { get; set; }
        public PlayerRole Role { get; set; }
    }
}