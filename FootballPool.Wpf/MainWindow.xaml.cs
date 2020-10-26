using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FootballPool.Simulator;
using FootballPool.Simulator.Factory;
using FootballPool.Simulator.Models;

namespace FootballPool.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            dtgrdScoreboard.ItemsSource = _leaderboard;
        }

        private ObservableCollection<LeaderboardEntry> _leaderboard = new ObservableCollection<LeaderboardEntry>();
        private League _currentLeague;

        private void btnSimulate_Click(object sender, RoutedEventArgs e)
        {
            CreateLeague();
        }

        private void btnSimulateRound_Click(object sender, RoutedEventArgs e)
        {
            if (_currentLeague == null)
            {
                //Msgbox
                MessageBox.Show($"Please create a league first");
                return;
            }

            IEnumerable<Match> currentRoundMatches = _currentLeague.PlayNextRound();
            _leaderboard.Clear();

            ParseLeaderboard(_currentLeague.GetCurrentStand());
            ParseMatchHistory(currentRoundMatches, false);
        }

        private void ParseLeaderboard(IEnumerable<LeaderboardEntry> leaderboardEntries)
        {
            foreach (var leaderboardEntry in leaderboardEntries)
            {
                _leaderboard.Add(leaderboardEntry);
            }
        }

        private void ParseMatchHistory(IEnumerable<Match> matches, bool reset)
        {
            if (reset)
                txtMatchResult.Text = string.Empty;

            var sb = new StringBuilder();

            foreach (var match in matches.Where(c=>c.Result != null))
            {
                sb.AppendLine(
                    $"{match.RoundNumber}\t{match.HomeTeam.Name}\t{match.Result.HomeTeamGoal}-{match.Result.AwayTeamGoal}\t{match.AwayTeam.Name}");
            }

            txtMatchResult.Text += sb.ToString();
        }

        private void CreateLeague()
        {
            IEnumerable<Team> teams = DemoTeam.GetTeam();
            League simulatedLeague = LeagueCreationFactory.GetBasicLeague();
            simulatedLeague.CreateLeague(teams.ToList());
            _currentLeague = simulatedLeague;
            _leaderboard.Clear();
            txtMatchResult.Text = string.Empty;
        }

        private void btnSimulateWholeLeague_Click(object sender, RoutedEventArgs e)
        {
            CreateLeague();
            List<Match> playedMatches = new List<Match>();

            while (_currentLeague.IsFinished() == false)
            {
                IEnumerable<Match> currentRoundMatches = _currentLeague.PlayNextRound();
                playedMatches.AddRange(currentRoundMatches);
            }

            ParseLeaderboard(_currentLeague.GetCurrentStand());
            ParseMatchHistory(playedMatches, true);
        }
    }
}
