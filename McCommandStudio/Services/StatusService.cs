using Livet;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Cafemoca.McCommandStudio.Services
{
    /// <summary>
    /// Original: https://github.com/Grabacr07/KanColleViewer/blob/master/Grabacr07.KanColleViewer/Models/StatusService.cs
    /// </summary>
    public class StatusService : NotificationObject
    {
        private static readonly StatusService _current = new StatusService();
        public static StatusService Current
        {
            get { return _current; }
        }

        private readonly Subject<string> notifier;

        private string persisitentMessage = string.Empty;
        private string notificationMessage;

        #region MainMessage 変更通知プロパティ

        public string MainMessage
        {
            get { return this.notificationMessage ?? this.persisitentMessage; }
            set
            {
                this.persisitentMessage = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region SubMessage 変更通知プロパティ

        private string _subMessage = string.Empty;
        public string SubMessage
        {
            get { return this._subMessage; }
            set
            {
                this._subMessage = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        private StatusService()
        {
            this.notifier = new Subject<string>();
            this.notifier
                .Do(x =>
                {
                    this.notificationMessage = x;
                    this.RaiseMessagePropertyChanged();
                })
                .Throttle(TimeSpan.FromMilliseconds(5000))
                .Subscribe(_ =>
                {
                    this.notificationMessage = null;
                    this.RaiseMessagePropertyChanged();
                });
        }

        public void SetMain(string message)
        {
            this.MainMessage = message;
        }

        public void SetSub(string message)
        {
            this.SubMessage = message;
        }

        public void Notify(string message)
        {
            this.notifier.OnNext(message);
        }

        private void RaiseMessagePropertyChanged()
        {
            this.RaisePropertyChanged(() => this.MainMessage);
        }
    }
}
