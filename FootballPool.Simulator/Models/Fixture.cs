using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballPool.Simulator.Models
{
    public class Fixture
    {
        private int _currentRoundNr = 1;
        private IEnumerable<Match> _matches;

        public Fixture(IEnumerable<Match> matches)
        {
            _matches = matches;
        }

        /// <summary>
        /// Get games matches which matches with the round number
        /// </summary>
        /// <param name="roundNumber">Roundnumber of the matches</param>
        /// <returns>Matches with the supplied round number</returns>
        public IEnumerable<Match> GetMatchByRoundNumber(int roundNumber)
        {
            return _matches.Where(c => c.RoundNumber == roundNumber);
        }

        /// <summary>
        /// Get all matches of the current round
        /// </summary>
        /// <returns>Matches with current round number</returns>
        public IEnumerable<Match> GetCurrentRoundMatches()
        {
            return GetMatchByRoundNumber(_currentRoundNr);
        }

        /// <summary>
        /// Filters all matches with result
        /// </summary>
        /// <returns>Matches which have been simulated</returns>
        public IEnumerable<Match> GetPlayedMatches()
        {
            return _matches.Where(c => c.Result != null);
        }

        /// <summary>
        /// Filters all matches without result
        /// </summary>
        /// <returns>Matches which have not been simulated</returns>
        public IEnumerable<Match> GetUnplayedMatches()
        {
            return _matches.Where(c => c.Result == null);
        }

        /// <summary>
        /// Advance the fixture to next round
        /// </summary>
        public void AdvanceToNextRound()
        {
            _currentRoundNr++;
        }

        /// <summary>
        /// Get the last round number of this fixture.
        /// </summary>
        /// <returns>Latest round number</returns>
        public int GetLastRoundNumber()
        {
            return _matches.Max(c => c.RoundNumber);
        }
    }
}
