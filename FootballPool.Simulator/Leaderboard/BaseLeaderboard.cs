using System.Collections.Generic;
using System.Linq;
using FootballPool.Simulator.enums;
using FootballPool.Simulator.Models;
using FootballPool.Simulator.Util;

namespace FootballPool.Simulator.Leaderboard
{
    public abstract class BaseLeaderboard
    {
        protected IEnumerable<Match> _matches;

        /// <summary>
        /// Get the stand of the given matches in parameters
        /// </summary>
        /// <param name="matches">List of matches where the results will be used for parsing</param>
        /// <param name="teams">List of the participating teams</param>
        /// <returns>List of leaderboard entries sorted by rank</returns>
        public abstract List<LeaderboardEntry> GetStand(IEnumerable<Match> matches, IEnumerable<Team> teams);

        /// <summary>
        /// Sort the leaderboard entries ascending with the following criteria.
        /// 1. Total points
        /// 2. Goal score (Goals scored minus goals taken)
        /// 3. Goals scored
        /// 4. Goals taken
        /// 5. Match results between tied teams
        /// </summary>
        /// <param name="leaderbord">Leaderboard entries that will be ordered</param>
        /// <returns>Leaderboard entries ordered ascending with criteria</returns>
        protected List<LeaderboardEntry> SortLeaderboard(List<LeaderboardEntry> leaderboardEntries, IEnumerable<Match> matches)
        {
            _matches = matches;

            leaderboardEntries.Sort(
                CompareLeaderboardEntries);
            leaderboardEntries.Reverse();

            AssignRankToLeaderboardEntries(leaderboardEntries);

            return leaderboardEntries;
        }
        
        /// <summary>
        /// Assign ranks based on the sorted leaderboard entries
        /// </summary>
        /// <param name="unrankedSortedEntries">List where ranks will be assigned where first item will be rank 1</param>
        private void AssignRankToLeaderboardEntries(List<LeaderboardEntry> unrankedSortedEntries)
        {
            int rankCounter = 1;

            foreach (var unrankedSortedEntry in unrankedSortedEntries)
            {
                unrankedSortedEntry.Rank = rankCounter++;
            }
        }

        /// <summary>
        /// Compare two leaderboard entries to check which belongs on higher position
        /// </summary>
        /// <param name="x">First entry for comparison</param>
        /// <param name="y">Second entry for comparison</param>
        /// <returns>Negative if first entry must be first. 0 if both should be at same location. Positive if second item should be on higher position</returns>
        protected int CompareLeaderboardEntries(LeaderboardEntry x, LeaderboardEntry y)
        {
            int ret = x.Points.CompareTo(y.Points);

            if (ret == 0)
            {
                ret = x.GoalScore.CompareTo(y.GoalScore);
            }

            if (ret == 0)
            {
                ret = x.GoalsScored.CompareTo(y.GoalsScored);
            }

            if (ret == 0)
            {
                ret = x.GoalsAgainst.CompareTo(y.GoalsAgainst);
            }

            if (ret == 0)
            {
                ret = CompareLeaderboardEntriesWithMatchesCriteria(x, y);
            }

            return ret;
        }

        /// <summary>
        /// Comparison with entry X and entry Y to see who belongs to higher position.
        /// The comparison will be done by checking the matches of each other. Win will get higher priority on higher spot
        /// </summary>
        /// <param name="x">First entry to compare with</param>
        /// <param name="y">Second entry to compare to</param>
        /// <returns>Negative if first entry must be first. 0 if both should be at same location. Positive if second item should be on higher position</returns>
        protected int CompareLeaderboardEntriesWithMatchesCriteria(LeaderboardEntry x, LeaderboardEntry y)
        {
            //Get relevant matches
            //If none then according then return 0
            //If one team wins a match then favor that team in the higher rank

            //If played matches does not exists then they should be placed in same position
            if (_matches == null)
            {
                return 0;
            }

            IEnumerable<Match> relevantMatches = _matches.Where(c =>
                _matches.Any(d => c.HomeTeam.Id == x.TeamId || c.AwayTeam.Id == x.TeamId) &&
                _matches.Any(d => c.AwayTeam.Id == y.TeamId || c.HomeTeam.Id == y.TeamId) && c.Result != null);

            if (relevantMatches.Any() == false)
            {
                return 0;
            }

            //Determine from matches between each other who will be first and second
            //Lower than 0 is x on first place
            //Higher than 0 means y is on first place
            int ret = 0;

            foreach (var relevantMatch in relevantMatches)
            {
                if (relevantMatch.Result.ResultType == MatchResultEnum.Draw)
                {
                    continue;
                }

                if (relevantMatch.Result.ResultType == MatchResultEnum.Win)
                {
                    if (x.TeamId == relevantMatch.Result.TeamWinId)
                        ret--;
                    else
                        ret++;
                }
            }

            return ret;
        }
    }
}
