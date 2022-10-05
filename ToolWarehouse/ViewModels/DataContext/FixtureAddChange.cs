using FixtureLog.Models;
using FixtureLog.ViewModels.MVVM;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FixtureLog.ViewModels.DataContext
{
    /// <summary>
    /// Добавление или изменение оснастки
    /// </summary>
    internal class FixtureAddChange
    {
        public FixtureAddChange(string addFixture, MainWindow mainWindow)
        {
            OpenTypesFixtures = mainWindow.OpenTypesFixtures;
            action = (addFixture == "0");
            if (action)
            {
                Temp = new Fixture();
                _fixtureTip = DataBase.GetInstance().FixturesTips.First();
            }
            else
            {
                Temp = new Fixture(mainWindow.FixtureSelected);
                _fixtureTip = DataBase.GetInstance().FixturesTips.First(x => x.Id == mainWindow.FixtureSelected.IdFixtureTip);               
            }
        }

        /// <summary>
        /// Добавить оснастку
        /// </summary>
        bool action;

        /// <summary>
        /// Текст заголовка окна и кнопки
        /// </summary>
        public string Text { get { return (action) ? "Добавить оснастку" : "Изменить оснастку"; } }

        /// <summary>
        /// Список типов оснасок
        /// </summary>
        public ObservableCollection<FixtureTip> FixturesTips
        {
            get
            {
                return DataBase.GetInstance().FixturesTips;
            }
        }

        FixtureTip _fixtureTip;
        /// <summary>
        /// Выбранный тип оснастки
        /// </summary>
        public FixtureTip fixtureTip
        {
            get { return _fixtureTip; }
            set
            {
                if (value == _fixtureTip) return;
                _fixtureTip = value;
                Temp.IdFixtureTip = _fixtureTip.Id;
            }
        }

        /// <summary>
        /// Изменяемая оснастка
        /// </summary>
        public Fixture Temp { get; set; }

        /// <summary>
        /// Команда на открытие окна с добавлением типов оснасток
        /// </summary>
        public ICommand OpenTypesFixtures { get; }

        ICommand? _AddChange;
        /// <summary>
        /// Добавить или изменить оснастку
        /// </summary>
        public ICommand AddChange
        {
            get
            {
                return _AddChange ??= new RelayCommand<object>(e =>
                {
                    if (action)
                    {
                        DataBase.GetInstance().INSERT(Temp);
                    }
                    else
                    {
                        DataBase.GetInstance().UPDATE(Temp);
                    }

                    ((Window)e).Close();
                });
            }
        }
    }
}
