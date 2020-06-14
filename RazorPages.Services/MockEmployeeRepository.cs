using RazorPages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RazorPages.Services
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){Id=1,Name="Ayman",Department=Dept.IT,Email="ayman_shabana@outlook.com",Photopath="stitch.png"},
                new Employee(){Id=2,Name="Adham",Department=Dept.IT,Email="bb@ehi.com",Photopath="joey.jpg"},
                new Employee(){Id=3,Name="Ashraf",Department=Dept.HR,Email="cc@ehi.com"}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<DeptHeadCount> EmployeeCountByDept(Dept? dept)
        {
            IEnumerable<Employee> query = _employeeList;
            if (dept.HasValue)
            {
                query = query.Where(e => e.Department == dept.Value);
            }
            return query.GroupBy(e => e.Department).Select(e => new DeptHeadCount
            {
                Department = e.Key.Value,
                Count = e.Count()
            }).ToList();
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Employee> Search(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return _employeeList;
            }
            search = search.ToLower();
            return _employeeList.Where(e => e.Name.ToLower().Contains(search) || e.Email.ToLower().Contains(search));
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
                employee.Photopath = employeeChanges.Photopath;
            }
            return employee;
        }
    }
}
