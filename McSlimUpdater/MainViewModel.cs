using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Cafemoca.McSlimUpdater
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _log = "Initializing...";
        public string Log
        {
            get { return this._log; }
            set
            {
                this._log = value;
                this.OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
        }

        public void AppendLog(string text)
        {
            this.Log += text;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T property, ref T value, [CallerMemberName] string propertyName = null)
        {
            property = value;
            this.OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanged(string[] propertyNames)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            foreach (var propertyName in propertyNames)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanged<T>(params Expression<Func<T>>[] propertyExpression)
        {
            this.OnPropertyChanged(propertyExpression.Select(x => ((MemberExpression)x.Body).Member.Name).ToArray());
        }

        #endregion
    }
}
