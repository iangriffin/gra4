using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GRA.Data
{
    public class Repository : Domain.IRepository
    {
        private const bool WriteAuditLog = true;
        private readonly ILogger<Repository> logger;
        private readonly Context context;
        private readonly AutoMapper.IMapper mapper;
        public Repository(ILogger<Repository> logger, Context context, AutoMapper.IMapper mapper)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            this.logger = logger;
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.context = context;
            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            this.mapper = mapper;
        }

        private bool AuditLog(string participantId,
            int objectId,
            object newObject,
            object priorObject = null)
        {
            if (WriteAuditLog)
            {
                var audit = new Data.Model.AuditLog
                {
                    EntityType = newObject.GetType().ToString(),
                    EntityId = objectId,
                    UpdatedBy = participantId,
                    UpdatedAt = DateTime.Now,
                    CurrentValue = JsonConvert.SerializeObject(newObject)
                };
                if (priorObject != null)
                {
                    audit.PreviousValue = JsonConvert.SerializeObject(priorObject);
                }
                context.AuditLogs.Add(audit);
                try
                {
                    if (context.SaveChanges() != 1)
                    {
                        logger.LogError($"Error writing audit log for {newObject.GetType()} id {objectId}");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(null, ex, $"Error writing audit log for {newObject.GetType()} id {objectId}");
                }
            }
            return true;
        }

        public IEnumerable<Domain.Model.Site> GetSites()
        {
            return context.Sites.ProjectTo<Domain.Model.Site>();
        }

        public bool AddSite(Domain.Model.Participant participant, Domain.Model.Site site)
        {
            var addSite = mapper.Map<Domain.Model.Site, Data.Model.Site>(site);
            context.Sites.Add(addSite);
            return context.SaveChanges() == 1
                ? AuditLog(participant.Id, addSite.Id, addSite)
                : false;
        }
    }
}
