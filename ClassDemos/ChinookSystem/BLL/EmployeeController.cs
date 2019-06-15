using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Data.Entities;
using System.ComponentModel; //ods
using ChinookSystem.Data.POCOs;
using ChinookSystem.Data.DTOs;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class EmployeeController
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SupportEmployee> Employees_ListSupportEmployees()
        {
            using (var context = new ChinookSystemContext())
            {
                var emplist = from emp in context.Employees
                              where emp.Title.Contains("Support")
                              orderby emp.LastName, emp.FirstName
                              select new SupportEmployee
                              {
                                  Name = emp.LastName + ", " + emp.FirstName,
                                  EmpTitle = emp.Title,
                                  ClientCount = emp.Customers.Count(),
                                  ClientList = (from cus in emp.Customers
                                                orderby cus.LastName, cus.FirstName
                                                select new Client
                                                {
                                                    Name = cus.LastName + ", " + cus.FirstName,
                                                    Phone = cus.Phone
                                                }).ToList()
                              };

                return emplist.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> Employee_ListNames()
        {
            using (var context = new ChinookSystemContext())
            {
                var employeelist = from x in context.Employees
                                   orderby x.LastName, x.FirstName
                                   select new SelectionList
                                   {
                                       DisplayText = x.LastName + ", " + x.FirstName,
                                       IDValueField = x.EmployeeId
                                   };
                return employeelist.ToList();
            }
        }
    }
}
