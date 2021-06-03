using DC.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.Entities.Base;

namespace DC.Services.Cms
{
    public partial interface IParameterService : IGenericRepository<Parameter>
    {
        IList<Parameter> GetAll(List<string> values);
    }
}
