using System.Collections.Generic;
using System.Linq;
using FootballPool.Simulator.enums;
using FootballPool.Simulator.Models;

namespace FootballPool.Simulator.Leaderboard
{
    public class Leaderboard : BaseLeaderboard
    {
        /// <summary>
        /// Leaderboard calculation with standard points calculation
        /// 3 points for win 1 point for draw and 0 point for loss
        /// </summary>
        /// <param name="matches">List of matches where the results will be used for parsing</param>
        /// <returns>List of leaderboard entries sorted by rank</returns>
        public override List<LeaderboardEntry> GetStand(IEnumerable<Match> matches, IEnumerable<Team> teams)
        {
            var result = new List<LeaderboardEntry>();

            foreach (Match match in matches.Where(c=>c.Result != null))
            {
                //Check if entries exists in the result list and create it it does not exists
                LeaderboardEntry homeTeamEntry = result.FirstOrDefault(c => c.TeamId == match.HomeTeam.Id);
                if (homeTeamEntry == null)
                {
                    Team homeTeam = teams.FirstOrDefault(c => c.Id == match.HomeTeam.Id);

                    homeTeamEntry = new LeaderboardEntry()
                    {
                        TeamId = match.HomeTeam.Id,
                        TeamName = homeTeam?.Name ?? match.HomeTeam.Id.ToString()
                    };
                    result.Add(homeTeamEntry);
                }

                LeaderboardEntry awayTeamEntry = result.FirstOrDefault(c => c.TeamId == match.AwayTeam.Id);
                if (awayTeamEntry == null)
                {
                    Team awayTeam = teams.FirstOrDefault(c => c.Id == match.AwayTeam.Id);

                    awayTeamEntry = new LeaderboardEntry()
                    {
                        TeamId = match.AwayTeam.Id,
                        TeamName = awayTeam?.Name ?? match.AwayTeam.Id.ToString()
                    };
                    result.Add(awayTeamEntry);
                }

                //Assign the goal scores for both teams
                homeTeamEntry.GoalsScored += match.Result.HomeTeamGoal;
                homeTeamEntry.GoalsAgainst += match.Result.AwayTeamGoal;

                awayTeamEntry.GoalsScored += match.Result.AwayTeamGoal;
                awayTeamEntry.GoalsAgainst += match.Result.HomeTeamGoal;

                //Assign points
                if (match.Result.ResultType == MatchResultEnum.Draw)
                {
                    homeTeamEntry.Points += 1;
                    homeTeamEntry.Draw++;
                    awayTeamEntry.Points += 1;
                    awayTeamEntry.Draw++;
                }else if (match.Result.ResultType == MatchResultEnum.Win)
                {
                    if (match.Result.TeamWinId == homeTeamEntry.TeamId)
                    {
                        homeTeamEntry.Points += 3;
                        homeTeamEntry.Win++;
                        awayTeamEntry.Loss++;
                    }
                    else
                    {
                        awayTeamEntry.Points += 3;
                        awayTeamEntry.Win++;
                        homeTeamEntry.Loss++;
                    }
                }
            }

            //Sort the leaderboard with right ranking
            result = SortLeaderboard(result, matches);

            return result;
        }
    }
}
