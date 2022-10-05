using FixtureLog.ViewModels;
using FixtureLog.ViewModels.MVVM;

namespace FixtureLog.Models
{
    /// <summary>
    /// Оснастка
    /// </summary>
    public class Fixture : NotificationObject
    {
        public Fixture() { }
        /// <summary>
        /// Создаем копию оснастки
        /// </summary>
        /// <param name="fixture"></param>
        public Fixture(Fixture fixture)
        {
            Id = fixture.Id;
            Designation = fixture.Designation;
            Name = fixture.Name;
            IdFixtureTip = fixture.IdFixtureTip;
            DateCreation = fixture.DateCreation;
            DateManufacturing = fixture.DateManufacturing;
            Note = fixture.Note;
        }

        long _Id;
        /// <summary>
        /// Id
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

        string _Designation = "";
        /// <summary>
        /// Обозначение
        /// </summary>
        public string Designation
        {
            get { return _Designation; }
            set
            {
                if (value == _Designation) return;
                _Designation = value;
                OnPropertyChanged();
            }
        }

        string? _Name = "";
        /// <summary>
        /// Название
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

        long _IdFixtureTip = 1;
        /// <summary>
        /// Id типа оснастки
        /// </summary>
        public long IdFixtureTip
        {
            get { return _IdFixtureTip; }
            set
            {
                if (value == _IdFixtureTip) return;
                _IdFixtureTip = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FixtureTip));
            }
        }

        object? _DateCreation;
        /// <summary>
        /// Дата создания чертежей
        /// </summary>
        public object? DateCreation
        {
            get { return _DateCreation; }
            set
            {
                if (value == _DateCreation) return;
                _DateCreation = value;
                OnPropertyChanged();
            }
        }

        object? _DateManufacturing;
        /// <summary>
        /// Дата изготовления
        /// </summary>
        public object? DateManufacturing
        {
            get { return _DateManufacturing; }
            set
            {
                if (value == _DateManufacturing) return;
                _DateManufacturing = value;
                OnPropertyChanged();
            }
        }

        object _Note = "";
        /// <summary>
        /// Примечание (место хронения)
        /// </summary>
        public object Note
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