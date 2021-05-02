using Microsoft.Azure.NotificationHubs;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 

namespace InfrastrutureClients.Messaging.Azure.PushNotifications
{
    /// <summary>
    /// 
    /// </summary>
    public class GooglePushNotifications : NotificationService 
    {

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="textSerializer"></param>
        /// <param name="settings"></param>
        public GooglePushNotifications(string messageTemplate, ITextSerializer textSerializer, ConnectionSettings settings) : base(messageTemplate, textSerializer, settings)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override NotificationPlatform GetPlatform()
        {
            return NotificationPlatform.Fcm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override async Task<NotificationOutcome> BroadCastNotificationAsync(string payload, CancellationToken token)
        {
            var result = await this.hubClient.SendFcmNativeNotificationAsync(payload, token);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="tag"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override async Task<NotificationOutcome> MulticastNotificationAsync(string payload, string tag, CancellationToken token)
        {
            var result = await this.hubClient.SendFcmNativeNotificationAsync(payload, tag, token);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="deviceId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override async Task<NotificationOutcome> UnicastNotificationAsync(Notification notification, string deviceId, CancellationToken token)
        {
            var outcomeFcmByTag = await hubClient.SendDirectNotificationAsync(notification, deviceId, token);
            return outcomeFcmByTag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payloadSerialized"></param>
        /// <returns></returns>
        protected override Notification BuildNotification(string payloadSerialized)
        {
            return new FcmNotification(payloadSerialized);
        }
    }
}
