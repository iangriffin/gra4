using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRA.Data.Repository
{
    using Microsoft.Extensions.Logging;

    public class BaseRepository
    {
        protected readonly Context context;

        protected readonly AutoMapper.IMapper mapper;

        protected readonly ILogger logger;

        public BaseRepository(
            Context context, 
            ILogger<AuditableSiteRepository> logger, 
            AutoMapper.IMapper mapper)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            this.context = context;
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            this.mapper = mapper;
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            this.logger = logger;
        }
    }
}
