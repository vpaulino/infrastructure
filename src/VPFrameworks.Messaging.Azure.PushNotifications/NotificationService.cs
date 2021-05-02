using InfrastrutureClients.Messaging.Abstractions;
using InfrastrutureClients.Serialization.Abstractions;
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
    public abstract class NotificationService
    {
        private ConnectionSettings settings;
        private string messageTemplate;

        /// <summary>
        /// hubClient
        /// </summary>
        protected NotificationHubClient hubClient;
        private ITextSerializer textSerializer;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="textSerializer"></param>
        /// <param name="settings"></param>
        protected NotificationService(string messageTemplate, ITextSerializer textSerializer, ConnectionSettings settings)
        {
            this.settings = settings;
            this.textSerializer = textSerializer;
            this.messageTemplate = messageTemplate;
            hubClient = new NotificationHubClient(this.settings.ConnectionString, this.settings.Path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual string CreateMessageTemplate()
        {
            return this.messageTemplate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="tags"></param>
        /// <param name="installationId"></param>
        /// <returns></returns>
        public virtual async Task RegisterDevicesAsync(string deviceId, string[] tags, string installationId) 
        {
            var installation = new Installation()
            {
                InstallationId = installationId,
                Tags = tags,
                PushChannel = deviceId,
                Platform = GetPlatform(),
                PushChannelExpired = false
                
            };

            await this.hubClient.CreateOrUpdateInstallationAsync(installation);

            return;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PushStatus> GetPushStatusAsync(string notificationId, CancellationToken token) 
        {
            var outcomeStatus = await this.hubClient.GetNotificationOutcomeDetailsAsync(notificationId, token);

            return new PushStatus(outcomeStatus.NotificationId, outcomeStatus.Tags, outcomeStatus.State, outcomeStatus.TargetPlatforms, outcomeStatus.EnqueueTime, outcomeStatus.EndTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract NotificationPlatform GetPlatform();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payloadSerialized"></param>
        /// <returns></returns>
        protected virtual string BuildMessage(string payloadSerialized)
        {
            return string.Format(this.CreateMessageTemplate(), payloadSerialized);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="payload"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<PushStatus> PublishBroadCastAsync<T>(T payload, CancellationToken token) 
        {
            string payloadSerialized = await this.textSerializer.Serialize<T>(payload, new SerializationSettings("application/json", Encoding.UTF8));
            var result = await this.BroadCastNotificationAsync(BuildMessage(payloadSerialized), token);
            return await GetPushStatusAsync(result.NotificationId, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <param name="payload"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<PushStatus> PublishByTagAsync<T>(string tag, T payload, CancellationToken token) 
        {

            string payloadSerialized = await this.textSerializer.Serialize<T>(payload, new SerializationSettings("application/json", Encoding.UTF8));
            var result = await MulticastNotificationAsync(BuildMessage(payloadSerialized), tag, token);
            return await GetPushStatusAsync(result.NotificationId, token);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deviceId"></param>
        /// <param name="payload"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<PushStatus> PublishDirectAsync<T>(string deviceId, T payload, CancellationToken token)
        {
            string payloadSerialized = await this.textSerializer.Serialize<T>(payload, new SerializationSettings("application/json", Encoding.UTF8));

            var result = await UnicastNotificationAsync(BuildNotification(payloadSerialized), deviceId, token);
            return await GetPushStatusAsync(result.NotificationId, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected abstract Task<NotificationOutcome> BroadCastNotificationAsync(string payload, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="tag"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected abstract Task<NotificationOutcome> MulticastNotificationAsync(string payload, string tag, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="tag"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected abstract Task<NotificationOutcome> UnicastNotificationAsync(Notification notification, string tag, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payloadSerialized"></param>
        /// <returns></returns>
        protected abstract Notification BuildNotification(string payloadSerialized);

    }
}
