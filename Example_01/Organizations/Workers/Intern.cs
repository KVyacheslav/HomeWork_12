using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_01.Organizations.Workers
{
    /// <summary>
    /// Класс, описывающий интерна.
    /// </summary>
    public class Intern : Worker
    {
        #region Constructors

        /// <summary>
        /// Создаем сотрудника. ВНИМАНИЕ! Зарплата у интерна почасовая.
        /// </summary>
        /// <param name="firstName">Имя сотрудника.</param>
        /// <param name="lastName">Фамилия сотрудника.</param>
        /// <param name="salary">Зарплата сотрудника за месяц.</param>
        /// <param name="department">Отдел.</param>
        public Intern(string firstName, string lastName, uint salary,
            Department department) :
            base(firstName, lastName, salary, department, "Интерн")
        {
        }
        
        #endregion


        #region Methods

        /// <summary>
        /// Выдать зарплату
        /// </summary>
        public override void GiveSalary()
        {
            if (DateTime.Now.Day != Organization.CurrentTime.Day)
            {
                this.Sum += this.Salary * 8;
            }
        }

        #endregion
    }
}
