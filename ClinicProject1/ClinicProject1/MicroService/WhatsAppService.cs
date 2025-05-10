using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ClinicProject1.MicroService
{
    public class WhatsAppService
    {
        private readonly string accountSid = "AC8a43a10def441caded0a8b833b03d51f";
        private readonly string authToken = "bddd31b172dacb7d2a0a5bed7fc363b3";

        public WhatsAppService()
        {
            TwilioClient.Init(accountSid, authToken);
        }

        public string SendMessage(string toNumber)
        {
            string formattedNumber = $"+2{toNumber}";

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber($"whatsapp:{formattedNumber}")
            );

            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.ContentSid = "HXb5b62575e6e4ff6129ad7c8efe1f983e"; // Make sure this is a valid approved template SID
            messageOptions.ContentVariables = "{\"1\":\"Welcome its HealthFirst clinic Your appoientment has been approved.Please be in time\"}";
            //messageOptions.Body = "Welcome to HealthFirst clinic!\nYour appointment has been approved. Please be in time";

            var message = MessageResource.Create(messageOptions);

            return $"Message status: {message.Status}, SID: {message.Sid}";
        }
    }
}
