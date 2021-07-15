using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Example_01.Annotations;

namespace Example_01.Organizations.Workers
{
    public enum CriterionSort
    {
        FirstName,
        LastName,
        Salary,
        Sum,
        Position
    }

    /// <summary>
    /// Абстрактный класс, описывающий рабочего.
    /// </summary>
    public abstract class Worker : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// Имя рабочего.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия рабочего.
        /// </summary>
        public string LastName { get; set; }

        public uint salary;

        /// <summary>
        /// Зарплата рабочего.
        /// </summary>
        public uint Salary
        {
            get => this.salary;
            set
            {
                this.salary = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Salary)));
            }
        }

        private uint sum;

        /// <summary>
        /// Полная сумма.
        /// </summary>
        public uint Sum
        {
            get => this.sum;
            set
            {
                this.sum = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sum)));
            }
        }

        /// <summary>
        /// Дата получения зарплаты.
        /// </summary>
        public DateTime DateReceiptSalary { get; set; }

        /// <summary>
        /// Должность работника.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Отдел рабочего.
        /// </summary>
        public Department Department { get; set; }

        /// <summary>
        /// Организация рабочего.
        /// </summary>
        public Organization Organization { get; set; }

        #endregion


        #region Constructors

        public Worker()
        {
        }

        /// <summary>
        /// Создаем рабочего.
        /// </summary>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="salary">Зарплата.</param>
        /// <param name="department">Отдел.</param>
        /// <param name="organization">Организация.</param>
        protected Worker(string firstName, string lastName, uint salary, 
            Department department, Organization organization)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Salary = salary;
            this.DateReceiptSalary = DateTime.Now;
            this.Department = department;
            this.Organization = organization;
        }

        /// <summary>
        /// Создаем рабочего.
        /// </summary>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="salary">Зарплата.</param>
        /// <param name="department">Отдел.</param>
        /// <param name="position">Должность сотрудника.</param>
        protected Worker(string firstName, string lastName, uint salary,
            Department department, string position)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Salary = salary;
            this.DateReceiptSalary = DateTime.Now;
            this.Department = department;
            this.Position = position;
        }

        #endregion

        #region Nested Constructors

        /// <summary>
        /// Сортировка по имени.
        /// </summary>
        public class SortByFirstName : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return string.Compare(x.FirstName, y.FirstName, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Сортировка по фамилии.
        /// </summary>
        public class SortByLastName : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return string.Compare(x.LastName, y.LastName, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Сортировка по зарплате.
        /// </summary>
        public class SortBySalary : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return x.salary.CompareTo(y.salary);
            }
        }

        /// <summary>
        /// Сортировка по сумме полученной зарплаты.
        /// </summary>
        public class SortBySum : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return x.sum.CompareTo(y.sum);
            }
        }

        /// <summary>
        /// Сортировка по должности.
        /// </summary>
        public class SortByPosition : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return string.Compare(x.Position, y.Position, StringComparison.Ordinal);
            }
        }

        #endregion

        #region Methods

        public static IComparer<Worker> SorteBy(CriterionSort criterionSort)
        {
            switch (criterionSort)
            {
                case CriterionSort.FirstName:
                    return new SortByFirstName();
                case CriterionSort.LastName:
                    return new SortByLastName();
                case CriterionSort.Salary:
                    return new SortBySalary();
                case CriterionSort.Sum:
                    return new SortBySum();
                default:
                    return new SortByPosition();
            }
        }

        /// <summary>
        /// Выдать зарплату.
        /// </summary>
        public virtual void GiveSalary()
        {
            var daysInMonth = DateTime.DaysInMonth(
                this.DateReceiptSalary.Year,
                this.DateReceiptSalary.Month);
           if ((Organization.CurrentTime - this.DateReceiptSalary).Days == daysInMonth)
           {
               this.Sum += this.Salary;
               this.DateReceiptSalary = Organization.CurrentTime;
           }
        }

        #endregion


        #region Override Methods
        public override string ToString()
        {
            return $"First name: {this.FirstName}\nLast name: {this.LastName}\nSalary: {this.Salary}";
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
