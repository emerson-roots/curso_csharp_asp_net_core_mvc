using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // alterado na aula 264
        // aplicando conceito async com tasks
        public async Task<List<Department>> FindAllAsync()
        {
            // INTERESSANTE: retorna a lista de departamentos ja devidamente
            // ordenados pelo Name
            //
            // alterado na aula 264 p/ async
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
