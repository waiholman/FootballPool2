using System;
using System.Collections.Generic;
using System.Text;

namespace FootballPool.Simulator.Models
{
    public class MatchTeamSetup
    {
        public int TeamId { get; set; }
        public Guid MatchId { get; set; }
        public IEnumerable<Player> Players { get; set; }

        public MatchTeamSetup(int teamId, Guid matchId, IEnumerable<Player> players)
        {
            TeamId = teamId;
            MatchId = matchId;
            Players = players;
        }
    }
}
