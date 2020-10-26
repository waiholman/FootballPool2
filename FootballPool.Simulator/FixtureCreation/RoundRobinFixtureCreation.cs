using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballPool.Simulator.Models;

namespace FootballPool.Simulator.FixtureCreation
{
    public class RoundRobinFixtureCreation : IFixtureCreation
    {
        /// <summary>
        /// Create fixture using Round robin strategy where team will only meet each other once
        /// </summary>
        /// <param name="participatingTeams"></param>
        /// <returns></returns>
        public Fixture CreateFixture(IEnumerable<Team> participatingTeams)
        {
            //Algorithm description from https://en.wikipedia.org/wiki/Round-robin_tournament
            var matches = new List<Match>();
            int currentRoundNumber = 1;

            //A good fixture should have at least two teams
            if (participatingTeams.Count() <= 1)
            {
                throw new ArgumentException($"Insufficient teams to create a fixture");
            }

            //Plus one is done to handle uneven teams.
            //when team size is 4 each half will be 2 (5/2)
            //when team size is 5 each half will be 3 (6/2)
            int eachHalfTeamSize = (participatingTeams.Count() + 1)/ 2;

            List<Team> firstHalfTeam = participatingTeams.Take(eachHalfTeamSize).ToList();
            List<Team> secondHalfTeam = participatingTeams.Skip(eachHalfTeamSize).Take(eachHalfTeamSize).Reverse().ToList();

            if (participatingTeams.Count() % 2 != 0)
            {
                //Uneven team found. Add dummy team. Will be removed in match creations
                secondHalfTeam.Insert(0, new Team()
                {
                    Id = -1,
                    Name = "Dummy"
                });
            }

            //Get the first team ID from second half to identify when everything has been matched once, so the loop can stop
            int originalTeamIdSecondHalf = secondHalfTeam[0].Id;

            do
            {
                for (int i = 0; i < firstHalfTeam.Count; i++)
                {
                    Team firstTeam = firstHalfTeam[i];
                    Team secondTeam = secondHalfTeam[i];

                    //Skip dummy team matches.
                    if (firstTeam.Id == -1 || secondTeam.Id == -1)
                    {
                        continue;
                    }

                    matches.Add(new Match()
                    {
                        Id = Guid.NewGuid(),
                        HomeTeam = firstTeam,
                        AwayTeam = secondTeam,
                        RoundNumber = currentRoundNumber
                    });
                }

                //Rearrange team halves for next iteration. Second half's first team to first team second position. 
                Team secondToFirstHalfTeam = secondHalfTeam[0];
                secondHalfTeam.Remove(secondToFirstHalfTeam);
                firstHalfTeam.Insert(1, secondToFirstHalfTeam);

                //First half's last team to second half's last position
                Team firstToSecondHalfTeam = firstHalfTeam.Last();
                firstHalfTeam.Remove(firstToSecondHalfTeam);
                secondHalfTeam.Insert(secondHalfTeam.Count, firstToSecondHalfTeam);

                currentRoundNumber++;
            } while (secondHalfTeam[0].Id != originalTeamIdSecondHalf);

            return new Fixture(matches);
        }
    }
}
