using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Dtos.AppRole
{
    public class PermissionInput
    {
        public string FunctionName { get; set; }
        public string Action { get; set; }
        public string Role { get; set; }
    }
}
