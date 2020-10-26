using System;
using System.Collections.Generic;
using System.Text;
using FootballPool.Simulator.Models;

namespace FootballPool.Simulator.FixtureCreation
{
    public interface IFixtureCreation
    {
        /// <summary>
        /// Create fixture with a defined algorithm
        /// </summary>
        /// <param name="participatingTeams">Teams that will be parsed with the fixture</param>
        /// <returns>Fixture with matches</returns>
        public Fixture CreateFixture(IEnumerable<Team> participatingTeams);
    }
}
