using System;
using System.Collections.Generic;
using System.Text;
using FootballPool.Simulator.enums;
using FootballPool.Simulator.MatchPlayAlgorithm;
using FootballPool.Simulator.Models;

namespace FootballPool.Simulator.Util
{
    public static class MatchExtension
    {
        /// <summary>
        /// Get the goals of the winning team. If it is a draw the home team goals will be used
        /// </summary>
        /// <param name="match"></param>
        /// <returns>Amount of scored goals by winning team</returns>
        public static int GetWinTeamGoals(this Match match)
        {
            if (match.Result.ResultType == MatchResultEnum.Draw)
                return match.Result.HomeTeamGoal;

            if (match.Result.TeamWinId == match.HomeTeam.Id)
                return match.Result.HomeTeamGoal;
            else
                return match.Result.AwayTeamGoal;
        }

        /// <summary>
        /// Get the goals of the losing team. If it is a draw the away team goals will be used
        /// </summary>
        /// <param name="match"></param>
        /// <returns>Amount of scored goals by lost team</returns>
        public static int GetLostTeamGoals(this Match match)
        {
            if (match.Result.ResultType == MatchResultEnum.Draw)
                return match.Result.AwayTeamGoal;

            if (match.Result.TeamWinId == match.HomeTeam.Id)
                return match.Result.AwayTeamGoal;
            else
                return match.Result.HomeTeamGoal;
        }

        /// <summary>
        /// Simulate the match
        /// </summary>
        /// <param name="match"></param>
        /// <param name="matchAlgorithm">Algorithm to use for simulation</param>
        public static void PlayMatch(this Match match, IMatchPlayAlgorithm matchAlgorithm)
        {
            MatchResult matchResult = matchAlgorithm.SimulateMatch(match.HomeTeamSetup, match.AwayTeamSetup);
            match.Result = matchResult;
        }
    }
}
