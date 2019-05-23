using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeApplication.Controllers
{
    [Authorize]
    //[RoutePrefix("api/values")]
    public class EmployeeV1Controller : ApiController
    {
        [HttpGet]
        //[Route("")]
        public HttpResponseMessage GetAllEmployee(string gender = "All")
        {
            using (OrionEntities eDbContext = new OrionEntities())
            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, eDbContext.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, eDbContext.Employees.Where(e => e.Gender.ToLower() == gender.ToLower()).ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, eDbContext.Employees.Where(e => e.Gender.ToLower() == gender.ToLower()).ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The gender value should be either one of these all, male, female");
                }
            }
        }

        [HttpGet]
        //[Route("{id}")]
        public HttpResponseMessage GetEmployee([FromUri]int id)
        {
            try
            {
                using (OrionEntities eDbContext = new OrionEntities())
                {
                    var emp = eDbContext.Employees.FirstOrDefault(s => s.Id == id);
                    if (emp != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, emp);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID : " + id.ToString() + " not found!");
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPost]
        //[Route("")]
        //[AllowAnonymous]
        public HttpResponseMessage AddEmployee([FromBody]Employee emp)
        {
            try
            {
                using (OrionEntities entity = new OrionEntities())
                {
                    if (entity.Employees.Any(e => e.Id == emp.Id))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Employee with ID :" + emp.Id.ToString() + " alreay exists!");
                    }
                    else
                    {
                        entity.Employees.Add(emp);
                        entity.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                        message.Headers.Location = new Uri(Request.RequestUri + "/" + emp.Id.ToString());
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        //[Route("{id}")]
        public HttpResponseMessage RemoveEmployee([FromUri]int id)
        {
            try
            {
                using (OrionEntities eDb = new OrionEntities())
                {
                    if (eDb.Employees.Any(e => e.Id == id))
                    {
                        eDb.Employees.Remove(eDb.Employees.First(e => e.Id == id));
                        eDb.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        var message = Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID: " + id + " not found!");
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPut]
        //[Route("")]
        public HttpResponseMessage UpdateEmployee([FromBody]Employee emp)
        {
            try
            {
                using (OrionEntities eDB = new OrionEntities())
                {
                    if (eDB.Employees.Any(e => e.Id == emp.Id))
                    {
                        var employee = eDB.Employees.First(e => e.Id == emp.Id);

                        employee.Name = emp.Name;
                        employee.Gender = emp.Gender;
                        employee.Salary = emp.Salary;
                        employee.Location = emp.Location;
                        eDB.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "EMployee with ID: " + emp.Id + " not found!");
                    }
                }
            }
            catch (Exception e)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
    }
}
