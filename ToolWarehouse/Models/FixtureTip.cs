using FixtureLog.ViewModels.MVVM;

namespace FixtureLog.Models
{
    /// <summary>
    /// Тип оснастки
    /// </summary>
    public class FixtureTip : NotificationObject
    {
        long _Id;
        /// <summary>
        /// Id оснастки
        /// </summary>
        public long Id
        {
            get { return _Id; }
            set
            {
                if (value == _Id) return;
                _Id = value;
                OnPropertyChanged();
            }
        }

        string _Name="";
        /// <summary>
        /// Наименование типа оснастки
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value == _Name) return;
                _Name = value;
                OnPropertyChanged();
            }
        }
    }
}
