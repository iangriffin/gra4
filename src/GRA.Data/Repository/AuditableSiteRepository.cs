using GRA.Domain.Repository;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace GRA.Data.Repository
{
    public class AuditableSiteRepository : IAuditableRepository<Domain.Model.Site>, ISiteRepository
    {
        private readonly Context context;
        private readonly AutoMapper.IMapper mapper;
        private readonly ILogger logger;
        private readonly GenericAuditableRepository<Model.Site, Domain.Model.Site> genericAuditableRepository;
        public AuditableSiteRepository(Context context, ILogger<AuditableSiteRepository> logger, AutoMapper.IMapper mapper)
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
            if(logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            this.logger = logger;
            genericAuditableRepository = new GenericAuditableRepository<Model.Site, Domain.Model.Site>(context, logger, mapper);
        }
        public void Add(int userId, Domain.Model.Site entity)
        {
            genericAuditableRepository.Add(userId, entity);
        }

        public IQueryable<Domain.Model.Site> GetAll()
        {
            return genericAuditableRepository.GetAll();
        }

        public Domain.Model.Site GetById(int id)
        {
            return genericAuditableRepository.GetById(id);
        }

        public IQueryable<Domain.Model.Site> PageAll(int skip, int take)
        {
            return genericAuditableRepository.PageAll(skip, take);
        }

        public void Remove(int userId, int id)
        {
            genericAuditableRepository.Remove(userId, id);
        }

        public void Remove(int userId, Domain.Model.Site entity)
        {
            genericAuditableRepository.Remove(userId, entity.Id);
        }

        public void Update(int userId, Domain.Model.Site entity)
        {
            genericAuditableRepository.Update(userId, entity);
        }
    }
}
