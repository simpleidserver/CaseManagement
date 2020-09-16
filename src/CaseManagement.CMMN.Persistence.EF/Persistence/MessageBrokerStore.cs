using CaseManagement.CMMN.Infrastructure.Bus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Persistence.EF.Persistence
{
    public class MessageBrokerStore : IMessageBrokerStore
    {
        private readonly CaseManagementDbContext _dbContext;

        public MessageBrokerStore(CaseManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Queue(QueueMessage message, CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                _dbContext.QueueMessageLst.Add(new Models.QueueMessageModel
                {
                    CreateDateTime = message.CreationDateTime,
                    SerializedContent = message.SerializedContent,
                    QueueName = message.QueueName
                });
                await _dbContext.SaveChangesAsync(token);
            }
        }

        public async Task Queue(ScheduledMessage message, CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                _dbContext.ScheduledMessageLst.Add(new Models.ScheduledMessageModel
                {
                    ElapsedTime = message.ElapsedTime,
                    QueueName = message.QueueName,
                    SerializedContent = message.SerializedContent
                });
                await _dbContext.SaveChangesAsync(token);
            }
        }

        public async Task<QueueMessage> Dequeue(string queueName, CancellationToken cancellationToken)
        {
            using (var lck = await _dbContext.Lock())
            {
                var lastQueueMessage = await _dbContext.QueueMessageLst.OrderBy(_ => _.Id).Where(_ => _.QueueName == queueName).FirstOrDefaultAsync(cancellationToken);
                if (lastQueueMessage == null)
                {
                    return null;
                }

                var result = new QueueMessage
                {
                    CreationDateTime = lastQueueMessage.CreateDateTime,
                    Id = lastQueueMessage.Id,
                    QueueName = lastQueueMessage.QueueName,
                    SerializedContent = lastQueueMessage.SerializedContent
                };
                _dbContext.QueueMessageLst.Remove(lastQueueMessage);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return result;
            }
        }

        public async Task<List<ScheduledMessage>> DequeueScheduledMessage(CancellationToken token)
        {
            using (var lck = await _dbContext.Lock())
            {
                var currentDateTime = DateTime.UtcNow;
                var result = await _dbContext.ScheduledMessageLst.Where(_ => _.ElapsedTime <= currentDateTime).ToListAsync();
                return result.Select(_ => new ScheduledMessage
                {
                    ElapsedTime = _.ElapsedTime,
                    QueueName = _.QueueName,
                    SerializedContent = _.SerializedContent
                }).ToList();
            }
        }
    }
}
