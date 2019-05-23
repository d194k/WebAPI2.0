using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeApplication.Controllers
{
    public class EmployeeV2Controller : ApiController
    {
        public IHttpActionResult Get()
        {
            using (OrionEntities oEntity = new OrionEntities())
            {
                return Ok(oEntity.Employees.ToList());
            }
        }

        public IHttpActionResult Get(int id)
        {
            using (OrionEntities oEntity = new OrionEntities())
            {
                if (oEntity.Employees.Any(e => e.Id == id))
                {
                    return Ok(oEntity.Employees.First(e => e.Id == id));
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
