using Cafemoca.McCommandStudio.Services;
using Cafemoca.McCommandStudio.Settings;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Bases;
using Codeplex.Reactive;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cafemoca.McCommandStudio.ViewModels.Layouts.Tools
{
    public class CompletionEditorViewModel : ToolViewModel
    {
        public const string ToolContentId = "CompletionEditor";

        #region PlayerNames

        public ObservableCollection<string> PlayerNames
        {
            get { return Setting.Current.ExtendedOptions.PlayerNames; }
            set { Setting.Current.ExtendedOptions.PlayerNames = value; }
        }
        public ReactiveProperty<string> PlayerName { get; private set; }
        public ReactiveProperty<string> SelectedPlayer { get; private set; }
        public ReactiveCommand AddPlayerCommand { get; private set; }
        public ReactiveCommand DelPlayerCommand { get; private set; }

        #endregion

        #region ScoreNames

        public ObservableCollection<string> ScoreNames
        {
            get { return Setting.Current.ExtendedOptions.ScoreNames; }
            set { Setting.Current.ExtendedOptions.ScoreNames = value; }
        }
        public ReactiveProperty<string> ScoreName { get; private set; }
        public ReactiveProperty<string> SelectedScore { get; private set; }
        public ReactiveCommand AddScoreCommand { get; private set; }
        public ReactiveCommand DelScoreCommand { get; private set; }

        #endregion

        #region TeamNames

        public ObservableCollection<string> TeamNames
        {
            get { return Setting.Current.ExtendedOptions.TeamNames; }
            set { Setting.Current.ExtendedOptions.TeamNames = value; }
        }
        public ReactiveProperty<string> TeamName { get; private set; }
        public ReactiveProperty<string> SelectedTeam { get; private set; }
        public ReactiveCommand AddTeamCommand { get; private set; }
        public ReactiveCommand DelTeamCommand { get; private set; }

        #endregion

        public CompletionEditorViewModel()
            : base("名前設定")
        {
            this.ContentId.Value = ToolContentId;

            this.PlayerName = new ReactiveProperty<string>("");
            this.SelectedPlayer = new ReactiveProperty<string>("");

            this.AddPlayerCommand = new ReactiveCommand();
            this.AddPlayerCommand.Subscribe(_ =>
                this.AddCollection(this.PlayerNames, this.PlayerName));

            this.DelPlayerCommand = new ReactiveCommand();
            this.DelPlayerCommand.Subscribe(_ =>
                this.RemoveCollection(this.PlayerNames, this.SelectedPlayer));

            this.ScoreName = new ReactiveProperty<string>("");
            this.SelectedScore = new ReactiveProperty<string>("");

            this.AddScoreCommand = new ReactiveCommand();
            this.AddScoreCommand.Subscribe(_ =>
                this.AddCollection(this.ScoreNames, this.ScoreName));

            this.DelScoreCommand = new ReactiveCommand();
            this.DelScoreCommand.Subscribe(_ =>
                this.RemoveCollection(this.ScoreNames, this.SelectedScore));

            this.TeamName = new ReactiveProperty<string>("");
            this.SelectedTeam = new ReactiveProperty<string>("");

            this.AddTeamCommand = new ReactiveCommand();
            this.AddTeamCommand.Subscribe(_ =>
                this.AddCollection(this.TeamNames, this.TeamName));

            this.DelTeamCommand = new ReactiveCommand();
            this.DelTeamCommand.Subscribe(_ =>
                this.RemoveCollection(this.TeamNames, this.SelectedTeam));
        }

        private void AddCollection(ObservableCollection<string> collection, ReactiveProperty<string> value)
        {
            if (collection == null || value.Value.IsEmpty())
            {
                return;
            }
            else if (value.Value.Any(x => "\r\n\0\t ".Contains(x)))
            {
                StatusService.Current.Notify("キーに空白を含むことはできません。");
            }
            else if (!collection.Contains(value.Value))
            {
                collection.Add(value.Value);
            }
            else
            {
                StatusService.Current.Notify("その名前は既に存在しています。");
            }
            value.Value = string.Empty;
        }

        private void RemoveCollection(ObservableCollection<string> collection, ReactiveProperty<string> value)
        {
            if (collection.Any(x => x == value.Value))
            {
                collection.Remove(value.Value);
            }
        }
    }
}
