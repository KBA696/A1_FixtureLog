using FixtureLog.Models;
using FixtureLog.ViewModels.MVVM;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FixtureLog.ViewModels.DataContext
{
    /// <summary>
    /// Работа с типами оснасток
    /// </summary>
    public class TypesFixtures : NotificationObject
    {
        /// <summary>
        /// Панель поиска или работы с новой или изменением выбранного типа
        /// </summary>
        string? _SearchBar;
        public string? SearchBar
        {
            get { return _SearchBar; }
            set
            {
                if (value == _SearchBar) return;
                _SearchBar = value;

                ClearButton = string.IsNullOrEmpty(_SearchBar) ? Visibility.Hidden : Visibility.Visible;

                OnPropertyChanged();

                OnPropertyChanged(nameof(FixturesTips));
            }
        }

        bool _changingType = false;
        /// <summary>
        /// Изменить выбранный тип
        /// </summary>
        bool changingType
        {
            get { return _changingType; }
            set
            {
                if (value == _changingType) return;
                _changingType = value;

                OnPropertyChanged(nameof(ButtonName));
            }
        }

        /// <summary>
        /// Имя у кнопки с действием набранного текста
        /// </summary>
        public string ButtonName
        {
            get { return changingType ? "Изменить выбранный тип" : "Добавить набранный тип"; }
        }

        Visibility _ClearButton = Visibility.Hidden;
        /// <summary>
        /// Отображение кнопки очистки поиска
        /// </summary>
        public Visibility ClearButton
        {
            get { return _ClearButton; }
            set
            {
                if (value == _ClearButton) return;
                _ClearButton = value;
                OnPropertyChanged();
            }
        }

        ICommand? _Clear;
        /// <summary>
        /// Команда на очистку SearchBar
        /// </summary>
        public ICommand Clear
        {
            get
            {
                return _Clear ??= new RelayCommand<object>(a =>
                {
                    SearchBar = "";
                    changingType = false;
                });
            }
        }

        /// <summary>
        /// Список типов оснасток, при заполненом SearchBar отфильтровывает ненужное
        /// </summary>
        public ObservableCollection<FixtureTip> FixturesTips
        {
            get
            {
                var vd = DataBase.GetInstance().FixturesTips;

                if (!string.IsNullOrEmpty(SearchBar))
                {
                    vd = new ObservableCollection<FixtureTip>(vd.Where(x => x.Name.Contains(SearchBar)));
                }

                return vd;
            }
        }

        /// <summary>
        /// Выбранный тип оснастки
        /// </summary>
        public FixtureTip FixtureTipSelected { get; set; }

        /// <summary>
        /// Сохраняет тип оснастки который хочет изменить
        /// </summary>
        public FixtureTip TempFixtureTipSelected { get; set; }

        ICommand? _Add;
        /// <summary>
        /// Добавление типа оснастки
        /// </summary>
        public ICommand Add
        {
            get
            {
                return _Add ??= new RelayCommand<object>(a =>
                {
                    if (!string.IsNullOrEmpty(SearchBar))
                    {
                        //поиск на совпадение
                        if (DataBase.GetInstance().FixturesTips.FirstOrDefault(x => x.Name == SearchBar) == null)
                        {
                            if (changingType)
                            {
                                DataBase.GetInstance().UPDATE(new FixtureTip() { Id = TempFixtureTipSelected.Id, Name = SearchBar });

                                changingType = false;
                            }
                            else
                            {
                                DataBase.GetInstance().INSERT(new FixtureTip() { Name = SearchBar });
                                OnPropertyChanged(nameof(FixturesTips));
                            }
                            Clear.Execute(null);
                        }
                        else
                        {
                            MessageBox.Show("Данный тип уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                });
            }
        }

        ICommand? _Change;
        /// <summary>
        /// Изменение выбранного типа
        /// </summary>
        public ICommand Change
        {
            get
            {
                return _Change ??= new RelayCommand<object>(a =>
                {
                    SearchBar = FixtureTipSelected.Name;
                    TempFixtureTipSelected = FixtureTipSelected;
                    changingType = true;
                });
            }
        }

        ICommand? _Remove;
        /// <summary>
        /// Удаление выбранного типа
        /// </summary>
        public ICommand Remove
        {
            get
            {
                return _Remove ??= new RelayCommand<object>(a =>
                {
                    DataBase.GetInstance().DELETE(new FixtureTip() { Id = (long)a });
                });
            }
        }
    }
}
