using System;
using System.Collections.Generic;
using System.Text;
using FootballPool.Simulator;
using FootballPool.Simulator.enums;

namespace FootballPool.Wpf
{
    public static class DemoTeam
    {
        public static IEnumerable<Team> GetTeam()
        {
            var teams = new List<Team>();
            teams.Add(GenerateTeam(1, $"Best team", 100));
            teams.Add(GenerateTeam(2, $"Second best team", 70));
            teams.Add(GenerateTeam(3, $"Third best team", 50));
            teams.Add(GenerateTeam(4, $"Fourth best team", 30));

            return teams;
        }

        public static Team GenerateTeam(int id, string name, int mainSkillValue)
        {
            //Create a team of 11 players 1 Goalkeeper 4 defender 3 midfielder and 3 attackers
            var team = new Team()
            {
                Id = id,
                Name = name
            };
            
            team.Players.Add(GeneratePlayer($"{name} Keeper 1", mainSkillValue, PlayerRole.Keeper));
            team.Players.Add(GeneratePlayer($"{name} Defender 1", mainSkillValue, PlayerRole.Defender));
            team.Players.Add(GeneratePlayer($"{name} Defender 2", mainSkillValue, PlayerRole.Defender));
            team.Players.Add(GeneratePlayer($"{name} Defender 3", mainSkillValue, PlayerRole.Defender));
            team.Players.Add(GeneratePlayer($"{name} Defender 4", mainSkillValue, PlayerRole.Defender));
            team.Players.Add(GeneratePlayer($"{name} Midfield 1", mainSkillValue, PlayerRole.Midfielder));
            team.Players.Add(GeneratePlayer($"{name} Midfield 2", mainSkillValue, PlayerRole.Midfielder));
            team.Players.Add(GeneratePlayer($"{name} Midfield 3", mainSkillValue, PlayerRole.Midfielder));
            team.Players.Add(GeneratePlayer($"{name} Attacker 1", mainSkillValue, PlayerRole.Attacker));
            team.Players.Add(GeneratePlayer($"{name} Attacker 2", mainSkillValue, PlayerRole.Attacker));
            team.Players.Add(GeneratePlayer($"{name} Attacker 3", mainSkillValue, PlayerRole.Attacker));

            return team;
        }

        public static Player GeneratePlayer(string name, int mainSkillValue, PlayerRole role)
        {
            Player player = new Player()
            {
                Age = 25,
                Aggresive = GetNonMainSkillValue(mainSkillValue),
                Defensive = GetNonMainSkillValue(mainSkillValue),
                GoalKeeping = GetNonMainSkillValue(mainSkillValue),
                Role = role,
                Id = Guid.NewGuid(),
                Name = name
            };

            switch (role)
            {
                case PlayerRole.Attacker:
                    player.Aggresive = mainSkillValue;
                    break;
                case PlayerRole.Midfielder:
                    player.Aggresive = mainSkillValue;
                    player.Defensive = mainSkillValue;
                    break;
                case PlayerRole.Defender:
                    player.Defensive = mainSkillValue;
                    break;
                case PlayerRole.Keeper:
                    player.GoalKeeping = mainSkillValue;
                    break;
            }

            return player;
        }

        private static int GetNonMainSkillValue(int mainSkillMaximum)
        {
            var rnd = new Random();
            double secondarySkillPenalty = 0.7;

            return (int) (rnd.Next(mainSkillMaximum) * secondarySkillPenalty);
        }
    }
}
