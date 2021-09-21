using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;


namespace SalesWebMvc.Services
{
    // aula 257 - sempre lembrar de registrar Services no Startup.cs
    public class DepartmentService
    {

        // READONLY - boa prática da comunidade dotnet 
        // previne a dependência de ser alterada
        private readonly SalesWebMvcContext _context;

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Department> FindAll()
        {
            // INTERESSANTE: retorna a lista de departamentos ja devidamente
            // ordenados pelo Name
            return _context.Department.OrderBy(x => x.Name).ToList();
        }
    }
}
