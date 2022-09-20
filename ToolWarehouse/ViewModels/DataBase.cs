using FixtureLog.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FixtureLog.ViewModels
{
    internal class DataBase
    {
        private DataBase() 
        {
            Fixtures = new ObservableCollection<Fixture>();
            FixturesTips = new ObservableCollection<FixtureTip>();
            FixturesTipsDictionary = new Dictionary<int, FixtureTip>();

            //Загружаем из базы данных
            #region Временно пока нету базы
            Fixtures.Add(new Fixture() { Id = 1, Designation = "0800-12.000", IdFixtureTip = 1, Name = "Для сварки угольника" });
            Fixtures.Add(new Fixture() { Id = 2, Designation = "7900-51.000", IdFixtureTip = 2, Name = "Для штамповки угольника" });
            Fixtures.Add(new Fixture() { Id = 3, Designation = "0800-13.000", IdFixtureTip = 1, Name = "Для сварки угольника", DateCreation=new DateTime(2024, 08, 18, 12, 30, 30), Note="В производстве" });

            TempUnloading("Сварочная оснастка");
            TempUnloading("Штамповочная оснастка");
            #endregion
        }

        #region Временно пока нету базы
        /// <summary>
        /// Временный id от таблици типы оснасток
        /// </summary>
        int TemtIdFixtureTip = 1;
        /// <summary>
        /// Временный id от таблици оснасток
        /// </summary>
        int TemtIdFixture = 4;
        /// <summary>
        /// Вместо выгрузки из базы
        /// </summary>
        void TempUnloading(string Name)
        {
            var fixtureTip = new FixtureTip() { Id = TemtIdFixtureTip++, Name = Name };
            FixturesTips.Add(fixtureTip);
            FixturesTipsDictionary.Add(fixtureTip.Id, fixtureTip);
        }
        #endregion

        static DataBase? instance;

        public static DataBase GetInstance()
        {
            if (instance == null)
                instance = new DataBase();
            return instance;
        }

        /// <summary>
        /// Список оснасок
        /// </summary>
        public ObservableCollection<Fixture> Fixtures { get; }

        /// <summary>
        /// Список типов оснасок
        /// </summary>
        public ObservableCollection<FixtureTip> FixturesTips { get; }
        Dictionary<int, FixtureTip> FixturesTipsDictionary { get; }

        /// <summary>
        /// Вернет тип оснастки
        /// </summary>
        /// <param name="id">Id типа оснастки</param>
        /// <returns></returns>
        public string FixtureTip(int id)
        {
            return FixturesTipsDictionary[id].Name;
        }

        //-----!!! произвести обновление данных в виев при изменении списка
        
        /// <summary>
        /// Добавдение данных в таблицу
        /// </summary>
        /// <param name="data">Класс таблицы</param>
        /// <exception cref="ApplicationException"></exception>
        public void INSERT(object data)
        {
            switch(data)
            {
                case Fixture fixture:
                    fixture.Id = TemtIdFixture++;
                    Fixtures.Add(fixture);
                    break;
                case FixtureTip fixtureTip:
                    TempUnloading(fixtureTip.Name);
                    break;
                default:
                    throw new ApplicationException("В класcе "+nameof(DataBase)+ " В методе " + nameof(INSERT)+" Отсутствует переданный класс в переменную "+nameof(data));
            }
        }

        /// <summary>
        /// Удаление данных из таблици
        /// </summary>
        /// <param name="data">Класс таблицы</param>
        /// <exception cref="ApplicationException"></exception>
        public void DELETE(object data)
        {
            //искать чтобы не где небыло связей
            switch (data)
            {
                case Fixture fixture:
                    Fixtures.Remove(fixture);
                    break;
                case FixtureTip fixtureTip:
                    var FirstFixtureTip = FixturesTips.FirstOrDefault(x => x.Id == fixtureTip.Id);
                    if (FirstFixtureTip != null)
                    {
                        FixturesTips.Remove(FirstFixtureTip);
                        FixturesTipsDictionary.Remove(FirstFixtureTip.Id);
                    }
                    break;
                default:
                    throw new ApplicationException("В класcе " + nameof(DataBase) + " В методе " + nameof(DELETE) + " Отсутствует переданный класс в переменную " + nameof(data));
            }
        }

        /// <summary>
        /// Изменение данных в таблице
        /// </summary>
        /// <param name="data">Класс таблицы</param>
        /// <exception cref="ApplicationException"></exception>
        public void UPDATE(object data)
        {
            switch (data)
            {
                case Fixture fixture:
                    //Fixtures.Add(f);
                    break;
                case FixtureTip fixtureTip:
                    var FirstFixtureTip = FixturesTips.FirstOrDefault(x => x.Id == fixtureTip.Id);
                    if (FirstFixtureTip != null)
                    {
                        FirstFixtureTip.Name = fixtureTip.Name;
                    }
                    break;
                default:
                    throw new ApplicationException("В класcе " + nameof(DataBase) + " В методе " + nameof(UPDATE) + " Отсутствует переданный класс в переменную " + nameof(data));
            }
        }
    }
}
