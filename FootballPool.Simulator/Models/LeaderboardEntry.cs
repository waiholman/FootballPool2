using System;
using System.Collections.Generic;
using System.Text;

namespace FootballPool.Simulator.Models
{
    public class LeaderboardEntry
    {
        public int Rank { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Played => Win + Draw + Loss;
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalScore => GoalsScored - GoalsAgainst;
        public int Win { get; set; }
        public int Draw { get; set; }
        public int Loss { get; set; }
    }
}
