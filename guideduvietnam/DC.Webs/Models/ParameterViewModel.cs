using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using DC.Models.Cms;

namespace DC.Webs.Models
{
    public class ParameterViewModel : BaseViewModel
    {
        public List<ParametersModel> ParameterItems { get; set; }
        public ParametersModel ParameterInfo { get; set; }
    }
}