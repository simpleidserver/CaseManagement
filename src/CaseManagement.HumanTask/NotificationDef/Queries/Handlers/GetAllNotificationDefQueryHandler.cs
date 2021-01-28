using CaseManagement.HumanTask.NotificationDef.Results;
using CaseManagement.HumanTask.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.NotificationDef.Queries.Handlers
{
    public class GetAllNotificationDefQueryHandler : IRequestHandler<GetAllNotificationDefQuery, ICollection<NotificationDefResult>>
    {
        private readonly INotificationDefQueryRepository _notificationDefQueryRepository;

        public GetAllNotificationDefQueryHandler(INotificationDefQueryRepository notificationDefQueryRepository)
        {
            _notificationDefQueryRepository = notificationDefQueryRepository;
        }

        public async Task<ICollection<NotificationDefResult>> Handle(GetAllNotificationDefQuery request, CancellationToken cancellationToken)
        {
            var result = await _notificationDefQueryRepository.GetAll(cancellationToken);
            return result.Select(_ => NotificationDefResult.ToDto(_)).ToList();
        }
    }
}
