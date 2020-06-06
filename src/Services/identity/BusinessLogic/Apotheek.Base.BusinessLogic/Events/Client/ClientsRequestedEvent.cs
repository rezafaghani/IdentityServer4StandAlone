using Apotheek.Base.AuditLogging.Events;
using Apotheek.Base.BusinessLogic.Dtos.Configuration;


namespace Apotheek.Base.BusinessLogic.Events.Client
{
    public class ClientsRequestedEvent : AuditEvent
    {
        public ClientsDto ClientsDto { get; set; }

        public ClientsRequestedEvent(ClientsDto clientsDto)
        {
            ClientsDto = clientsDto;
        }
    }
}