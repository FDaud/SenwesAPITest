using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SenwesAssignment_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenwesAssignment_API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly LoadData _loadData;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
            _loadData = new LoadData();
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>Returns a list of all employees</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var employeeData = _loadData.LoadEmployeeData();
            return Ok(employeeData);
        }


        [Route("Get/{empId}")]
        [HttpGet]
        public IActionResult GetByEmployeeId(int empId)
        {
            var employee = _loadData.LoadEmployeeData().Where(x => x.EmpID == empId).FirstOrDefault();
            return Ok(employee);
        }

        /// <summary>
        /// Get all employees that joined the company in the last 5 years (DateOfJoining) 
        /// </summary>
        /// <param name="years"></param>
        /// <returns></returns>
        [Route("GetEmpByYears/{years}")]
        [HttpGet]
        public IActionResult GetEmployeeByYears(double years = 5)
        {
            var employees = _loadData.LoadEmployeeData().Where(y => y.YearsInCompany >= years).ToList();
            return Ok(employees);
        }

        /// <summary>
        /// Get all employees older than 30 (Age)
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        [Route("GetEmpByAge/{age}")]
        [HttpGet]
        public IActionResult GetEmployeeByAge(double age = 30)
        {
            var employees = _loadData.LoadEmployeeData().Where(y => y.Age >= age).OrderBy(a => a.Age).ToList();
            return Ok(employees);
        }
        /// <summary>
        /// Get the top 10 highest paid males/females
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        [Route("GetTopHighestPaid/{top}")]
        [HttpGet]
        public IActionResult GetHighestPaid(int top = 10)
        {
            var employees = _loadData.LoadEmployeeData().OrderByDescending(s => s.Salary).ThenBy(g => g.Gender).Take(top);
            return Ok(employees);
        }

        /// <summary>
        /// Search for an employee to return anyone with the specific name or surname && city
        /// and the city equals city
        /// </summary>
        /// <param name="name"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        [Route("GetEmpByNameCity/{name}/{city}")]
        [HttpGet]
        public IActionResult GetEmployeeByNameCity(string name, string city)
        {
            var employees = _loadData.LoadEmployeeData().Where(n => n.FirstName.Contains(name) || n.LastName.Contains(name) && n.City == city).ToList();
            return Ok(employees);
        }

        /// <summary>
        /// Get all employees with a first name of “Treasure” and then return their salary
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("GetEmpName/{name}")]
        [HttpGet]
        public IActionResult GetEmployeeByName(string name = "Treasure")
        {
            var salaries = _loadData.LoadEmployeeData().Where(t => t.FirstName.ToLower() == name.ToLower()).Select(s => s.Salary).ToList();
            return Ok(salaries);
        }
        /// <summary>
        /// Get All City names only and allow unauthenticated users to access this end point
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        [Route("GetCities")]
        [HttpGet]
        public IActionResult GetAllCities()
        {
            var cities = _loadData.LoadEmployeeData().Select(x => x.City).Distinct().OrderBy(c => c).ToList();
            return Ok(cities);
        }
    }
}
