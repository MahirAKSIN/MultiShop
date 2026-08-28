using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Results.AddressResults
{
    public class AddressDeleteCommand
    {
        public AddressDeleteCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

    }
}
