using CaseManagement.CMMN.Persistence.EF.Models;
using CaseManagement.Common.Bus;

namespace CaseManagement.CMMN.Persistence.EF.DomainMapping
{
    public static class QueueMessageMapper
    {
        #region To domain

        public static QueueMessage ToDomain(this QueueMessageModel model)
        {
            return new QueueMessage
            {
                Id = model.Id,
                QueueName = model.QueueName,
                SerializedContent = model.SerializedContent,
                CreationDateTime = model.CreateDateTime
            };
        }

        #endregion

        #region To model

        public static QueueMessageModel ToModel(this QueueMessage message)
        {
            return new QueueMessageModel
            {
                QueueName = message.QueueName,
                SerializedContent = message.SerializedContent,
                CreateDateTime = message.CreationDateTime
            };
        }

        #endregion
    }
}
