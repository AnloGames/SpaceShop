using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShop_ViewModels
{
    public class QueryViewModel
    {
        public QueryHeader QueryHeader { get; set; }
        public IEnumerable<QueryDetail> QueryDetails { get; set; }
    }
}
