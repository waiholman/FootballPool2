using System;
using System.Collections.Generic;
using System.Text;

namespace FootballPool.Simulator.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public Team HomeTeam { get; set; }
        public MatchTeamSetup HomeTeamSetup { get; set; }
        public Team AwayTeam { get; set; }
        public MatchTeamSetup AwayTeamSetup { get; set; }
        public MatchResult Result { get; set; }
    }
}
