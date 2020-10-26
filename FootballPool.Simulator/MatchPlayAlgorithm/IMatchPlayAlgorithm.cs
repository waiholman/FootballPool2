using System;
using System.Collections.Generic;
using System.Text;
using FootballPool.Simulator.enums;
using FootballPool.Simulator.Models;

namespace FootballPool.Simulator.MatchPlayAlgorithm
{
    public interface IMatchPlayAlgorithm
    {
        /// <summary>
        /// Simulate the match with the given Team setups
        /// </summary>
        /// <param name="homeTeam">Player selection for home team</param>
        /// <param name="awayTeam">Player selection for away team</param>
        /// <returns>Result of the match simulation</returns>
        public MatchResult SimulateMatch(MatchTeamSetup homeTeam, MatchTeamSetup awayTeam);
    }
}
