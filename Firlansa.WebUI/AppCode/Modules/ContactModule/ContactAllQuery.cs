using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Firlansa.WebUI.Models.DataContexts;
using Firlansa.WebUI.Models.Entities;

namespace Firlansa.WebUI.AppCode.Modules.ContactModule
{
    public class ContactAllQuery : IRequest<IEnumerable<Contact>>
    {

        public class ContactAllQueryHandler : IRequestHandler<ContactAllQuery, IEnumerable<Contact>>
        {
            readonly FirlansaDbContext db;
            public ContactAllQueryHandler(FirlansaDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Contact>> Handle(ContactAllQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Contacts
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
