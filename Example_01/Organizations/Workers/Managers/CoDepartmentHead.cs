using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_01.Organizations.Managers
{
    /// <summary>
    /// Класс, описывающий заместителя начальника отдела.
    /// </summary>
    public class CoDepartmentHead : Manager
    {
        public CoDepartmentHead(string firstName, string lastName, Department department) :
            base(firstName, lastName, department, "Зам начальника отдела")
        {
        }
    }
}
