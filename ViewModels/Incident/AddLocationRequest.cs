using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Incident
{
    public class AddLocationRequest
    {
        public long IncidentId { get; set; }
        public string Address { get; set; }
    }
}
