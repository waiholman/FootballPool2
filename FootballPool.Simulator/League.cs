using System.Collections.Generic;
using System.Linq;
using FootballPool.Simulator.FixtureCreation;
using FootballPool.Simulator.Leaderboard;
using FootballPool.Simulator.MatchPlayAlgorithm;
using FootballPool.Simulator.Models;
using FootballPool.Simulator.Util;

namespace FootballPool.Simulator
{
    public class League
    {
        private List<Team> _teams = new List<Team>();
        private Fixture _matchPlan;
        private Leaderboard.Leaderboard _standing;
        private IMatchPlayAlgorithm _matchAlgorithm;
        private IFixtureCreation _fixtureCreation;
        private BaseLeaderboard _leaderboard;

        public League(IMatchPlayAlgorithm matchAlgorithm, IFixtureCreation fixtureCreation, BaseLeaderboard leaderboard)
        {
            _matchAlgorithm = matchAlgorithm;
            _fixtureCreation = fixtureCreation;
            _leaderboard = leaderboard;
        }

        /// <summary>
        /// Initialize the league and setup everything required to run
        /// </summary>
        /// <param name="participatingTeams">Teams that will be participating in this league</param>
        public void CreateLeague(List<Team> participatingTeams)
        {
            _teams = participatingTeams;
            _matchPlan = _fixtureCreation.CreateFixture(_teams);
        }

        /// <summary>
        /// Simulate the following round
        /// </summary>
        /// <returns>Played matches</returns>
        public IEnumerable<Match> PlayNextRound()
        {
            IEnumerable<Match> currentRoundMatches = _matchPlan.GetCurrentRoundMatches();

            foreach (var match in currentRoundMatches)
            {
                //Simplify the match setup creation for the demo by just adding whole team to team setup
                var homeTeamPlayers = _teams.FirstOrDefault(c => c.Id == match.HomeTeam.Id);
                var awayTeamPlayers = _teams.FirstOrDefault(c => c.Id == match.AwayTeam.Id);
                match.HomeTeamSetup = new MatchTeamSetup(match.HomeTeam.Id, match.Id, homeTeamPlayers.Players);
                match.AwayTeamSetup = new MatchTeamSetup(match.AwayTeam.Id, match.Id, awayTeamPlayers.Players);

                //simulate the match
                match.PlayMatch(_matchAlgorithm);
            }

            _matchPlan.AdvanceToNextRound();
            return currentRoundMatches;
        }

        /// <summary>
        /// Get the standings of the current played matches
        /// </summary>
        /// <returns>Leaderboard entries sorted on rank</returns>
        public IEnumerable<LeaderboardEntry> GetCurrentStand()
        {
            return _leaderboard.GetStand(_matchPlan.GetPlayedMatches(), _teams);
        }

        /// <summary>
        /// Check if this league is finished
        /// </summary>
        /// <returns>If the league is finished</returns>
        public bool IsFinished()
        {
            return _matchPlan.GetUnplayedMatches().Any() == false;
        }

        /// <summary>
        /// Get played match sorted by round number
        /// </summary>
        /// <returns>Sorted match</returns>
        public IEnumerable<Match> GetPlayerMatches()
        {
            return _matchPlan.GetPlayedMatches().OrderBy(c => c.RoundNumber);
        }
    }
}
