using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RazorPages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RazorPages.Services
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext context;

        public SQLEmployeeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Employee Add(Employee newEmployee)
        {
            context.Employees.Add(newEmployee);
            context.SaveChanges();
            return newEmployee;
        }

        public Employee Delete(int id)
        {
            Employee employee = context.Employees.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<DeptHeadCount> EmployeeCountByDept(Dept? dept)
        {
            IEnumerable<Employee> query = context.Employees;
            if (dept.HasValue)
            {
                query = query.Where(e => e.Department == dept.Value);
            }
            return query.GroupBy(e => e.Department).Select(g => new DeptHeadCount()
                                {
                                    Department = g.Key.Value,
                                    Count = g.Count()
                                }).ToList();
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            //return context.Employees;
            return context.Employees.FromSqlRaw<Employee>("SELECT * FROM Employees").ToList();
        }

        public Employee GetEmployee(int id)
        {
            SqlParameter parameter = new SqlParameter("@Id", id);
            return context.Employees.FromSqlRaw<Employee>("spGetEmployeeById @Id", parameter).ToList().FirstOrDefault();
        }

        public IEnumerable<Employee> Search(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return context.Employees;
            }
            search = search.ToLower();
            return context.Employees.Where(e => e.Name.ToLower().Contains(search) || e.Email.ToLower().Contains(search));
        }
        public Employee Update(Employee updatedEmployee)
        {
            var employee = context.Employees.Attach(updatedEmployee);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return updatedEmployee;
        }

    }
}
