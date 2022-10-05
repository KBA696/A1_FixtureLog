using FixtureLog.Models;
using FixtureLog.ViewModels.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FixtureLog.ViewModels.DataContext
{
    public class MainWindow : NotificationObject
    {
        public MainWindow()
        {
            DataBase.GetInstance().Fixtures.CollectionChanged += Fixtures_CollectionChanged;
        }
        ~MainWindow()
        {
            DataBase.GetInstance().Fixtures.CollectionChanged -= Fixtures_CollectionChanged;
        }

        /// <summary>
        /// Обновить список оснасок если произайдет в коде изменение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fixtures_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Fixtures));
        }

        /// <summary>
        /// варианты поиска оснастки
        /// </summary>
        public string[] SearchOptions { get; } = new string[] { "Поиск по обозначению", "Поиск по названию", "Поиск по типу", "Поиск по примечанию" };

        string _SearchOptionsSelected = "Поиск по обозначению";
        /// <summary>
        /// Выбранный критерий поиска
        /// </summary>
        public string SearchOptionsSelected
        {
            get { return _SearchOptionsSelected; }
            set
            {
                if (value == _SearchOptionsSelected) return;
                _SearchOptionsSelected = value;
                OnPropertyChanged(nameof(Fixtures));
            }
        }

        string _SearchBar = "";
        /// <summary>
        /// поисковая строка
        /// </summary>
        public string SearchBar
        {
            get { return _SearchBar; }
            set
            {
                if (value == _SearchBar) return;
                _SearchBar = value;
                ClearButton = string.IsNullOrEmpty(_SearchBar) ? Visibility.Hidden : Visibility.Visible;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Fixtures));
            }
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
        /// Очистка поисковой строки
        /// </summary>
        public ICommand Clear
        {
            get
            {
                return _Clear ??= new RelayCommand<object>(a =>
                {
                    SearchBar = "";
                });
            }
        }

        /// <summary>
        /// Список оснасток, при заполненом SearchBar отфильтровывает ненужное
        /// </summary>
        public ObservableCollection<Fixture> Fixtures
        {
            get
            {
                IEnumerable<Fixture> vd = DataBase.GetInstance().Fixtures.OrderByDescending(x => x.Id);

                if (!string.IsNullOrEmpty(SearchBar))
                {
                    vd = vd.Where(x =>
                    {
                        return SearchOptionsSelected switch
                        {
                            "Поиск по обозначению" => !string.IsNullOrEmpty(x.Designation) && x.Designation.ToUpper().Contains(SearchBar.ToUpper()),
                            "Поиск по названию" => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(SearchBar.ToUpper()),
                            "Поиск по типу" => !string.IsNullOrEmpty(x.FixtureTip) && x.FixtureTip.ToUpper().Contains(SearchBar.ToUpper()),
                            "Поиск по примечанию" => !string.IsNullOrEmpty(x.Note.ToString()) && x.Note.ToString().ToUpper().Contains(SearchBar.ToUpper()),
                            _ => true
                        };
                    });
                }

                return new ObservableCollection<Fixture>(vd);
            }
        }

        /// <summary>
        /// Выбранная оснастка
        /// </summary>
        public Fixture FixtureSelected { get; set; }


        ICommand? _OpenTypesFixtures;
        /// <summary>
        /// Открытие окна с типами оснасок
        /// </summary>
        public ICommand OpenTypesFixtures
        {
            get
            {
                return _OpenTypesFixtures ??= new RelayCommand<CancelEventArgs>(e =>
                {
                    Views.TypesFixtures typesFixtures = new() { DataContext = new TypesFixtures() };
                    typesFixtures.ShowDialog();
                });
            }
        }

        ICommand? _OpenFixtureAddChange;
        /// <summary>
        /// Открытие окна добавление или изменения оснастки
        /// </summary>
        public ICommand OpenFixtureAddChange
        {
            get
            {
                return _OpenFixtureAddChange ?? (_OpenFixtureAddChange = new RelayCommand<string>(e =>
                {
                    if (e == "1" && FixtureSelected == null) { return; }
                    Views.FixtureAddChange typesFixtures = new() { DataContext = new FixtureAddChange(e, this) };
                    typesFixtures.ShowDialog();
                }));
            }
        }

        ICommand? _RemoveSnap;
        /// <summary>
        /// Удаление выбранной оснастки
        /// </summary>
        public ICommand RemoveSnap
        {
            get
            {
                return _RemoveSnap ??= new RelayCommand<Fixture>(e =>
                {
                    DataBase.GetInstance().DELETE(e);
                });
            }
        }
    }
}
