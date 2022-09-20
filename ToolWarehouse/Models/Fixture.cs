using System;
using FixtureLog.ViewModels;
using FixtureLog.ViewModels.MVVM;

namespace FixtureLog.Models
{
    /// <summary>
    /// Оснастка
    /// </summary>
    public class Fixture : NotificationObject
    {
        int _Id;
        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get { return _Id; }
            set
            {
                if (value == _Id) return;
                _Id = value;
                OnPropertyChanged();
            }
        }

        string? _Designation;
        /// <summary>
        /// Обозначение
        /// </summary>
        public string? Designation
        {
            get { return _Designation; }
            set
            {
                if (value == _Designation) return;
                _Designation = value;
                OnPropertyChanged();
            }
        }

        string? _Name;
        /// <summary>
        /// Название
        /// </summary>
        public string? Name
        {
            get { return _Name; }
            set
            {
                if (value == _Name) return;
                _Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Наименование типа оснастки
        /// </summary>
        public string FixtureTip
        {
            get
            {
                return DataBase.GetInstance().FixtureTip(IdFixtureTip);
            }
        }

        int _IdFixtureTip;
        /// <summary>
        /// Id типа оснастки
        /// </summary>
        public int IdFixtureTip
        {
            get { return _IdFixtureTip; }
            set
            {
                if (value == _IdFixtureTip) return;
                _IdFixtureTip = value;
                OnPropertyChanged();
            }
        }

        DateTime? _DateCreation;
        /// <summary>
        /// Дата создания чертежей
        /// </summary>
        public DateTime? DateCreation
        {
            get { return _DateCreation; }
            set
            {
                if (value == _DateCreation) return;
                _DateCreation = value;
                OnPropertyChanged();
            }
        }

        DateTime? _DateManufacturing;
        /// <summary>
        /// Дата изготовления
        /// </summary>
        public DateTime? DateManufacturing
        {
            get { return _DateManufacturing; }
            set
            {
                if (value == _DateManufacturing) return;
                _DateManufacturing = value;
                OnPropertyChanged();
            }
        }

        string? _Note;
        /// <summary>
        /// Примечание (место хронения)
        /// </summary>
        public string? Note
        {
            get { return _Note; }
            set
            {
                if (value == _Note) return;
                _Note = value;
                OnPropertyChanged();
            }
        }
    }
}
