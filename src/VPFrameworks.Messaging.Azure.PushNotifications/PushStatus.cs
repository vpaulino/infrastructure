using Microsoft.Azure.NotificationHubs;
using System;

namespace InfrastrutureClients.Messaging.Azure.PushNotifications
{
    /// <summary>
    /// 
    /// </summary>
    public class PushStatus
    {
        private string notificationId;
        private string tags;
        private NotificationOutcomeState state;
        private string targetPlatforms;
        private DateTime? enqueueTime;
        private DateTime? endTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="tags"></param>
        /// <param name="state"></param>
        /// <param name="targetPlatforms"></param>
        /// <param name="enqueueTime"></param>
        /// <param name="endTime"></param>
        public PushStatus(string notificationId, string tags, NotificationOutcomeState state, string targetPlatforms, DateTime? enqueueTime, DateTime? endTime)
        {
            this.NotificationId = notificationId;
            this.Tags = tags;
            this.State = state;
            this.TargetPlatforms = targetPlatforms;
            this.EnqueueTime = enqueueTime;
            this.EndTime = endTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public string NotificationId { get => notificationId; set => notificationId = value; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Tags { get => tags; set => tags = value; }
        
        /// <summary>
        /// 
        /// </summary>
        public NotificationOutcomeState State { get => state; set => state = value; }
        
        /// <summary>
        /// 
        /// </summary>
        public string TargetPlatforms { get => targetPlatforms; set => targetPlatforms = value; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EnqueueTime { get => enqueueTime; set => enqueueTime = value; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime { get => endTime; set => endTime = value; }
    }
}