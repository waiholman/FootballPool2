using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballPool.Simulator.enums;
using FootballPool.Simulator.Models;

namespace FootballPool.Simulator.MatchPlayAlgorithm
{
    public class TeamPowerPlayAlgorithm : IMatchPlayAlgorithm
    {
        /// <summary>
        /// Simulate match result based on the team attacking and defending power
        /// </summary>
        /// <param name="homeTeam">Team setup of the home team</param>
        /// <param name="awayTeam">Team setup of the away team</param>
        /// <returns>Result of the match</returns>
        public MatchResult SimulateMatch(MatchTeamSetup homeTeam, MatchTeamSetup awayTeam)
        {
            var rnd = new Random();
            int randomWeight = 15;
            double skillWeight = 0.85;

            //Max goals for this simulation
            int maxGoals = 5;

            //Home team score chances
            int homeTeamSkillScore = CalculateAttackingSkill(homeTeam.Players, awayTeam.Players);
            //Fix home team skill score. Check the weight
            int homeTeamScoreChance = (int)(rnd.Next(randomWeight) + homeTeamSkillScore * skillWeight);
            int homeTeamGoals = CalculateTotalGoals(homeTeamScoreChance, maxGoals);

            //Away team score chances
            int awayTeamSkillScore = CalculateAttackingSkill(awayTeam.Players, homeTeam.Players);
            int awayTeamScoreChance = (int) (rnd.Next(randomWeight) + awayTeamSkillScore * skillWeight);
            int awayTeamGoals = CalculateTotalGoals(awayTeamScoreChance, maxGoals);

            var matchResult = new MatchResult()
            {
                HomeTeamGoal = homeTeamGoals,
                AwayTeamGoal = awayTeamGoals,
            };

            if (homeTeamGoals == awayTeamGoals)
            {
                matchResult.ResultType = MatchResultEnum.Draw;
            }
            else
            {
                matchResult.ResultType = MatchResultEnum.Win;
                matchResult.TeamWinId = homeTeamGoals > awayTeamGoals ? homeTeam.TeamId : awayTeam.TeamId;
            }

            return matchResult;
        }

        /// <summary>
        /// Calculate the weight of the attacking team to score
        /// </summary>
        /// <param name="attackingTeam">Players of the attacking team</param>
        /// <param name="defendingTeam">Player of the defending team</param>
        /// <returns></returns>
        protected int CalculateAttackingSkill(IEnumerable<Player> attackingTeam, IEnumerable<Player> defendingTeam)
        {
            double defenderWeight = 0.6;
            double keeperWeight = 0.4;

            double homeTeamSkillScore = CalculateTeamStrength(attackingTeam, PlayerRole.Attacker) -
                                        (CalculateTeamStrength(defendingTeam, PlayerRole.Defender) * defenderWeight +
                                         CalculateTeamStrength(defendingTeam, PlayerRole.Keeper) * keeperWeight);

            return (int) Math.Max(0, homeTeamSkillScore);
        }

        /// <summary>
        /// Calculate each possible goals
        /// </summary>
        /// <param name="scoreChance">Chance for team to score</param>
        /// <param name="maxGoals">Amount of chance</param>
        /// <returns>The number of goals</returns>
        protected int CalculateTotalGoals(int scoreChance, int maxGoals)
        {
            var rnd = new Random();
            int goals = 0;

            for (int i = 0; i < maxGoals; i++)
            {
                int goalScoreChanceValue = rnd.Next(100);

                if (scoreChance >= goalScoreChanceValue)
                {
                    goals++;
                }
            }

            return goals;
        }

        /// <summary>
        /// Calculate the strength of the team based on the role
        /// </summary>
        /// <param name="playersInTeam">List of players in the team</param>
        /// <param name="roleStrength">Which role to calculate</param>
        /// <returns>The strength of the team of selected role</returns>
        protected int CalculateTeamStrength(IEnumerable<Player> playersInTeam, PlayerRole roleStrength)
        {
            IEnumerable<Player> relevantPlayers = playersInTeam.Where(c => c.Role == roleStrength);

            switch (roleStrength)
            {
                case PlayerRole.Keeper:
                    return (int) relevantPlayers.Average(c => c.GoalKeeping);
                case PlayerRole.Midfielder:
                    return (int) relevantPlayers.Average(c=> (c.Aggresive + c.Defensive) / 2);
                case PlayerRole.Attacker:
                    return (int) relevantPlayers.Average(c=>c.Aggresive);
                case PlayerRole.Defender:
                    return (int) relevantPlayers.Average(c => c.Defensive);
                default:
                    throw new ArgumentException($"Unknown role in roleStrength parameter");
            }
        }
    }
}
