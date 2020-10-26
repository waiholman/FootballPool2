using System;
using System.Collections.Generic;
using System.Text;
using FootballPool.Simulator.FixtureCreation;
using FootballPool.Simulator.MatchPlayAlgorithm;

namespace FootballPool.Simulator.Factory
{
    /// <summary>
    /// Create different types of leagues with this factory
    /// </summary>
    public static class LeagueCreationFactory
    {
        /// <summary>
        /// Get basic match pool where teams are matched once against eachother
        /// </summary>
        /// <returns>A league with standard rules</returns>
        public static League GetBasicLeague()
        {
            var league = new League(new TeamPowerPlayAlgorithm(), new RoundRobinFixtureCreation(), new Leaderboard.Leaderboard());
            return league;
        }
    }
}
