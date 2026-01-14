using LexisNexis.DAL.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.DAL.Models
{
    record Category : EntityBase
    {
        public string Name { get; set; } = "";
        public int? ParentId { get; set; }
    }
}
