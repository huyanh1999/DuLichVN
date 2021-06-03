using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Domain;
using DC.Entities.Base;
using DC.Services.Cms;

namespace DC.Services.Cms
{
    public partial class ParameterService: GenericRepository<Parameter>, IParameterService
    {                              
        public IList<Parameter> GetAll(List<string> values)
        {
            return context.Parameters.Where(m => values.Contains(m.Value)).OrderBy(m => m.Id).ToList();
        }
    }
}
