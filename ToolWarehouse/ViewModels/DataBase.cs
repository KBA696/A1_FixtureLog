using FixtureLog.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Windows.Documents;

namespace FixtureLog.ViewModels
{
    internal class DataBase
    {
        const string adress = ".\\FixtureLogDB.db";
        private DataBase()
        {
            CreatingDB();

            Fixture[]? collection = Выгрузка<Fixture>(SELECT(@"SELECT * FROM `" + nameof(Fixture) + "`"));
            Fixtures = collection == null ? new ObservableCollection<Fixture>() : new ObservableCollection<Fixture>(collection);

            FixtureTip[]? collection1 = Выгрузка<FixtureTip>(SELECT(@"SELECT * FROM `" + nameof(Models.FixtureTip) + "`"));
            FixturesTips = collection1 == null ? new ObservableCollection<FixtureTip>() : new ObservableCollection<FixtureTip>(collection1); ;
            FixturesTipsDictionary = new Dictionary<long, FixtureTip>();

            foreach (var item in FixturesTips)
            {
                FixturesTipsDictionary.Add(item.Id, item);
            }
        }

        /// <summary>
        /// Создание базы данных, если таковой нету
        /// </summary>
        void CreatingDB()
        {
            if (!File.Exists(adress))
            {
                SQLiteConnection.CreateFile(adress);

                using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=" + adress + "; Version=3;"))
                {
                    string commandText = @"CREATE TABLE `" + nameof(Models.FixtureTip) + "`" +
                    "(" +
                        "`" + nameof(Models.FixtureTip.Id) + "` INTEGER PRIMARY KEY AUTOINCREMENT," +
                        "`" + nameof(Models.FixtureTip.Name) + "` TEXT NOT NULL" +
                    ");" +

                    "INSERT INTO `" + nameof(Models.FixtureTip) + "`" +
                    "(" +
                        "`" + nameof(Models.FixtureTip.Name) + "`" +
                    ") " +
                    "VALUES " +
                    "('Сварочная оснастка'),('Штамповочная оснастка');" +

                    "CREATE TABLE `" + nameof(Fixture) + "`" +
                    "(" +
                        "`" + nameof(Fixture.Id) + "` INTEGER PRIMARY KEY AUTOINCREMENT," +
                        "`" + nameof(Fixture.Designation) + "` TEXT," +
                        "`" + nameof(Fixture.Name) + "` TEXT," +
                        "`" + nameof(Fixture.IdFixtureTip) + "` INTEGER," +
                        "`" + nameof(Fixture.DateCreation) + "` DATE," +
                        "`" + nameof(Fixture.DateManufacturing) + "` DATE," +
                        "`" + nameof(Fixture.Note) + "` TEXT" +
                    ");" +

                    "INSERT INTO `" + nameof(Fixture) + "`" +
                    "(" +
                        "`" + nameof(Fixture.Designation) + "`," +
                        "`" + nameof(Fixture.Name) + "`," +
                        "`" + nameof(Fixture.IdFixtureTip) + "`," +
                        "`" + nameof(Fixture.DateCreation) + "`," +
                        "`" + nameof(Fixture.Note) + "`" +
                    ") " +
                    "VALUES " +
                    "(" +
                        "'0800-12.000'," +
                        "'Для сварки угольника'," +
                        "1," +
                        "NULL," +
                        "''" +
                    "),(" +
                        "'7900-51.000'," +
                        "'Для штамповки угольника'," +
                        "2," +
                        "NULL," +
                        "''" +
                    "),(" +
                        "'0800-13.000'," +
                        "'Для сварки угольника'," +
                        "1," +
                        "'" + new DateTime(2022, 10, 04, 12, 30, 30).ToString("yyyy-MM-dd") + "'," +
                        "'В производстве'" +
                    ");";
                    SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                    Connect.Open();
                    Command.ExecuteNonQuery();
                }
            }
        }

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
        Dictionary<long, FixtureTip> FixturesTipsDictionary { get; }

        /// <summary>
        /// Вернет тип оснастки
        /// </summary>
        /// <param name="id">Id типа оснастки</param>
        /// <returns></returns>
        public string FixtureTip(long id)
        {
            return FixturesTipsDictionary[id].Name;
        }

        //-----!!! произвести обновление данных в виев при изменении списка
        /// <summary>
        /// Выгрузка данных из базы
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataRow[] SELECT(string commandText)
        {
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=" + adress + "; Version=3;"))
            {
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);

                Connect.Open();

                DataTable dataTable = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(Command);
                adapter.Fill(dataTable);

                return dataTable.Select();
            }
        }

        #region Выгрузка из базы в этот класс
        static public T[]? Выгрузка<T>(DataRow[] ДанныеИзБД) where T : new()
        {
            T[]? returnПеременная = default;
            if (ДанныеИзБД != null)
            {
                int ЧислоДанных = ДанныеИзБД.Length;
                returnПеременная = new T[ЧислоДанных];

                //Огромный минус что каждый раз проверяет есть ли это строка или нету
                for (int i = 0; i < ЧислоДанных; i++)
                {
                    returnПеременная[i] = Выгрузка<T>(ДанныеИзБД[i]);
                }
            }
            return returnПеременная;
        }

        static public T? Выгрузка<T>(DataRow ДанныеИзБД) where T : new()
        {
            T? returnПеременная = default;
            if (ДанныеИзБД != null)
            {
                returnПеременная = new T();
                foreach (var field in returnПеременная.GetType().GetProperties())
                {
                    if (ДанныеИзБД.Table.Columns.Contains(field.Name))
                    {
                        field.SetValue(returnПеременная, ДанныеИзБД[field.Name]);
                    }
                }
            }
            return returnПеременная;
        }

        static public string fds23(byte[] fgh)
        {
            if (fgh != null)
            {
                string tr = "";// "\\x";
                foreach (var item in fgh)
                {
                    tr += Convert.ToString(item, 16).PadLeft(2, '0');
                }
                return tr;
            }
            else
            {
                return "";
            }
        }

        static public string rew<T>(T[] CardType)
        {
            string cardType = "{";
            if (CardType != null)
            {
                foreach (var item1 in CardType)
                {
                    cardType += item1 + ",";
                }
            }
            else { cardType += " "; }

            return cardType.Substring(0, cardType.Length - 1) + "}";
        }
        #endregion

        /// <summary>
        /// Добавдение данных в таблицу
        /// </summary>
        /// <param name="data">Класс таблицы</param>
        /// <exception cref="ApplicationException"></exception>
        public void INSERT(object data)
        {
            using (SQLiteConnection Connect = new(@"Data Source=" + adress + "; Version=3;"))
            {
                string commandText = "";
                switch (data)
                {
                    case Fixture fixture:
                        commandText = @"INSERT INTO `" + nameof(Fixture) + "`" +
                        "(" +
                            "`" + nameof(Fixture.Designation) + "`," +
                            "`" + nameof(Fixture.Name) + "`," +
                            "`" + nameof(Fixture.IdFixtureTip) + "`," +
                            "`" + nameof(Fixture.DateCreation) + "`," +
                            "`" + nameof(Fixture.DateManufacturing) + "`," +
                            "`" + nameof(Fixture.Note) + "`" +
                        ") " +
                        "VALUES " +
                        "(" +
                            "'" + fixture.Designation + "'," +
                            "'" + fixture.Name + "'," +
                            "" + fixture.IdFixtureTip + "," +
                            ((fixture.DateCreation != null && fixture.DateCreation is DateTime) ? "'" + ((DateTime)fixture.DateCreation).ToString("yyyy-MM-dd") + "'," : "NULL,") +
                            ((fixture.DateManufacturing != null && fixture.DateManufacturing is DateTime) ? "'" + ((DateTime)fixture.DateManufacturing).ToString("yyyy-MM-dd") + "'," : "NULL,") +
                            "'" + fixture.Note + "'" +
                        ") RETURNING " + nameof(Fixture.Id) + "";

                        break;
                    case FixtureTip fixtureTip:
                        commandText = @"INSERT INTO `" + nameof(Models.FixtureTip) + "`" +
                        "(" +
                            "`" + nameof(Models.FixtureTip.Name) + "`" +
                        ") " +
                        "VALUES " +
                        "(" +
                            "'" + fixtureTip.Name + "'" +
                        ") RETURNING " + nameof(Models.FixtureTip.Id) + "";
                        break;
                    default:
                        throw new ApplicationException("В класcе " + nameof(DataBase) + " В методе " + nameof(INSERT) + " Отсутствует переданный класс в переменную " + nameof(data));
                }

                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                var executeScalar = Command.ExecuteScalar();
                if (executeScalar != null)
                {
                    var rr = int.Parse(executeScalar.ToString());
                    switch (data)
                    {
                        case Fixture fixture:
                            fixture.Id = rr;
                            Fixtures.Add(fixture);
                            break;
                        case FixtureTip fixtureTip:
                            fixtureTip.Id = rr;
                            FixturesTips.Add(fixtureTip);
                            FixturesTipsDictionary[rr] = fixtureTip;
                            break;
                        default:
                            throw new ApplicationException("В класcе " + nameof(DataBase) + " В методе " + nameof(INSERT) + " Отсутствует переданный класс в переменную " + nameof(data));
                    }
                }

            }
        }

        /// <summary>
        /// Удаление данных из таблици
        /// </summary>
        /// <param name="data">Класс таблицы</param>
        /// <exception cref="ApplicationException"></exception>
        public void DELETE(object data)
        {
            using (SQLiteConnection Connect = new(@"Data Source=" + adress + "; Version=3;"))
            {
                string commandText = "";
                switch (data)
                {
                    case Fixture fixture:
                        commandText = @"DELETE FROM `" + nameof(Fixture) + "` " +
                        "WHERE  " +
                            "`" + nameof(Fixture.Id) + "` = '" + fixture.Id + "'";

                        break;
                    case FixtureTip fixtureTip:
                        //искать чтобы не где небыло связей

                        commandText = @"DELETE FROM `" + nameof(FixtureTip) + "` " +
                        "WHERE  " +
                            "`" + nameof(Models.FixtureTip.Id) + "` = '" + fixtureTip.Id + "'";
                        break;
                    default:
                        throw new ApplicationException("В класcе " + nameof(DataBase) + " В методе " + nameof(INSERT) + " Отсутствует переданный класс в переменную " + nameof(data));
                }

                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                var executeScalar = Command.ExecuteNonQuery();
                if (executeScalar > 0)
                {
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

            }
        }

        /// <summary>
        /// Изменение данных в таблице
        /// </summary>
        /// <param name="data">Класс таблицы</param>
        /// <exception cref="ApplicationException"></exception>
        public void UPDATE(object data)
        {
            using (SQLiteConnection Connect = new(@"Data Source=" + adress + "; Version=3;"))
            {
                string commandText = "";
                switch (data)
                {
                    case Fixture fixture:

                        commandText = @"UPDATE `" + nameof(Fixture) + "` " +
                        "SET " +
                            "`" + nameof(Fixture.Designation) + "` = '" + fixture.Designation + "', " +
                            "`" + nameof(Fixture.Name) + "` = '" + fixture.Name + "', " +
                            "`" + nameof(Fixture.IdFixtureTip) + "` = " + fixture.IdFixtureTip + ", " +
                            "`" + nameof(Fixture.DateCreation) + "` = " + ((fixture.DateCreation != null && fixture.DateCreation is DateTime) ? "'" + ((DateTime)fixture.DateCreation).ToString("yyyy-MM-dd") + "'," : "NULL,") +
                            "`" + nameof(Fixture.DateManufacturing) + "` = " + ((fixture.DateManufacturing != null && fixture.DateManufacturing is DateTime) ? "'" + ((DateTime)fixture.DateManufacturing).ToString("yyyy-MM-dd") + "'," : "NULL,") +
                            "`" + nameof(Fixture.Note) + "` = '" + fixture.Note + "' " +
                        "WHERE  " +
                            "`" + nameof(Fixture.Id) + "` = " + fixture.Id + "";

                        break;
                    case FixtureTip fixtureTip:
                        commandText = @"UPDATE `" + nameof(FixtureTip) + "` " +
                        "SET " +
                            "`" + nameof(Models.FixtureTip.Name) + "` = '" + fixtureTip.Name + "' " +
                        "WHERE  " +
                            "`" + nameof(Models.FixtureTip.Id) + "` = " + fixtureTip.Id + "";
                        break;
                    default:
                        throw new ApplicationException("В класcе " + nameof(DataBase) + " В методе " + nameof(INSERT) + " Отсутствует переданный класс в переменную " + nameof(data));
                }

                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                var executeScalar = Command.ExecuteNonQuery();
                if (executeScalar > 0)
                {
                    switch (data)
                    {
                        case Fixture fixture:
                            var FirstFixtures = Fixtures.First(x => x.Id == fixture.Id);
                            var Index = Fixtures.IndexOf(FirstFixtures);
                            if (Index != -1)
                            {
                                Fixtures[Index] = fixture;
                            }
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
    }
}
