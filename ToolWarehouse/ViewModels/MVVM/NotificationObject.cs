using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace FixtureLog.ViewModels.MVVM
{
    public class NotificationObject : INotifyPropertyChanged /*Обновление данных из кода в биндинг*/
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Чтобы обновить все поля достаточно прописать ""
        /// </summary>
        /// <param name="name"></param>

        public void OnPropertyChanged(params string[] name)
        {
            if (name.FirstOrDefault(x => string.IsNullOrEmpty(x)) == null)
            {
                foreach (var item in name)
                {
                    OnPropertyChanged(item);
                }
            }
            else
            {
                OnPropertyChanged("");
            }
        }
    }
}