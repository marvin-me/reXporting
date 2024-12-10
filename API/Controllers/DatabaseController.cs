using Core.UserDatabase;
using Microsoft.AspNetCore.Mvc;
using Model.UserDatabase;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    [HttpGet]
    public ActionResult<IDatabase> GetDatabase(string connectionString)
    {
        return new Database(new SqlServerDataProvider(new SqlServerDataContext(connectionString)));
    }
}