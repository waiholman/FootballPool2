using System;
using System.Collections.Generic;
using System.Text;
using FootballPool.Simulator.enums;

namespace FootballPool.Simulator.Models
{
    public class MatchResult
    {
        public MatchResultEnum ResultType { get; set; }
        public int TeamWinId { get; set; }
        public int HomeTeamGoal { get; set; }
        public int AwayTeamGoal { get; set; }
    }
}
