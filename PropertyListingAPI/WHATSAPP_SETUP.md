# WhatsApp Business API Integration Setup

This document provides step-by-step instructions for setting up WhatsApp Business API integration with the ResDirect property listing application.

## Prerequisites

1. **Facebook Business Account**: You need a Facebook Business account
2. **WhatsApp Business Account**: A verified WhatsApp Business account
3. **Meta Developer Account**: Access to Meta for Developers platform
4. **Phone Number**: A dedicated phone number for WhatsApp Business

## Step 1: Create Meta App

1. Go to [Meta for Developers](https://developers.facebook.com/)
2. Click "Create App" and select "Business" as the app type
3. Fill in your app details:
   - App Name: `ResDirect WhatsApp Integration`
   - Contact Email: Your business email
   - Business Account: Select your Facebook Business account

## Step 2: Add WhatsApp Product

1. In your Meta app dashboard, click "Add Product"
2. Find "WhatsApp" and click "Set up"
3. This will add WhatsApp Business API to your app

## Step 3: Get API Credentials

1. In the WhatsApp section of your app:
   - **Access Token**: Copy the temporary access token (valid for 24 hours)
   - **Phone Number ID**: Note the test phone number ID provided
   - **Business Account ID**: Found in the app settings

## Step 4: Configure Webhook

1. In your WhatsApp configuration, set up webhook:
   - **Webhook URL**: `https://yourdomain.com/api/whatsapp/webhook`
   - **Verify Token**: Choose a secure token (e.g., `your_secure_verify_token_123`)
   - **Webhook Fields**: Select `messages` and `message_deliveries`

## Step 5: Update Application Configuration

Update your `appsettings.json` with the credentials:

```json
{
  "WhatsAppSettings": {
    "AccessToken": "YOUR_PERMANENT_ACCESS_TOKEN",
    "PhoneNumberId": "YOUR_PHONE_NUMBER_ID",
    "ApiVersion": "v18.0",
    "VerifyToken": "your_secure_verify_token_123"
  }
}
```

## Step 6: Get Permanent Access Token

The temporary token expires in 24 hours. To get a permanent token:

1. Go to **System Users** in your Facebook Business account
2. Create a new system user for your app
3. Assign the system user to your app with appropriate permissions
4. Generate a permanent access token for the system user

## Step 7: Add Phone Number

1. In WhatsApp Manager, add your business phone number
2. Verify the phone number through SMS or voice call
3. Complete the business verification process

## Step 8: Test the Integration

Use the provided test endpoints to verify the setup:

### Send Test Message
```bash
POST /api/whatsapp/test-message
{
  "phoneNumber": "+27123456789",
  "message": "Hello from ResDirect!"
}
```

### Send Template Message
```bash
POST /api/whatsapp/test-template
{
  "phoneNumber": "+27123456789",
  "templateName": "hello_world",
  "parameters": ["John"]
}
```

### Send Media Message
```bash
POST /api/whatsapp/test-media
{
  "phoneNumber": "+27123456789",
  "mediaUrl": "https://example.com/image.jpg",
  "mediaType": "image",
  "caption": "Check out this property!"
}
```

## Features Implemented

### 1. Text Messages
- Send simple text messages to any WhatsApp number
- Auto-reply functionality for common keywords

### 2. Template Messages
- Send pre-approved template messages
- Support for parameterized templates
- Multi-language support

### 3. Media Messages
- Send images, videos, audio files, and documents
- Include captions with media messages
- Support for various media formats

### 4. Location Messages
- Send location coordinates
- Include location name and address
- Perfect for sharing property locations

### 5. Webhook Handling
- Receive incoming messages
- Track message delivery status
- Process different message types

## Usage in Property Application

### Guest Viewing Requests
When a guest submits a viewing request, the system automatically:
1. Sends an email notification to the agent
2. Sends a WhatsApp message to the agent with guest details

Example message sent to agent:
```
New guest viewing request from John Doe (0123456789) for 'Beautiful 3BR House' on 15 Jan 2025. Preferred time: 10:00 AM
```

### Agent-to-Guest Communication
Agents can send messages to guests who submitted viewing requests:
1. Confirmation of viewing appointments
2. Directions to the property
3. Follow-up messages after viewings

## Message Templates

Create these templates in your WhatsApp Business Manager:

### 1. Viewing Request Confirmation
```
Hello {{1}}, your viewing request for {{2}} has been received. We'll contact you shortly to confirm the appointment.
```

### 2. Viewing Appointment Reminder
```
Hi {{1}}, this is a reminder of your property viewing for {{2}} tomorrow at {{3}}. Looking forward to seeing you!
```

### 3. Property Information
```
Hi {{1}}, here are the details for {{2}}: {{3}} bedrooms, {{4}} bathrooms, R{{5}}/month. Would you like to schedule a viewing?
```

## Security Considerations

1. **Verify Token**: Keep your webhook verify token secure
2. **Access Token**: Store access tokens securely (use environment variables in production)
3. **Rate Limits**: Implement rate limiting to avoid hitting API limits
4. **Webhook Security**: Validate webhook signatures in production

## Rate Limits

WhatsApp Business API has rate limits:
- **Messaging**: 1000 messages per day for new numbers (increases with usage)
- **API Calls**: 4000 requests per hour per app
- **Media Upload**: 100 media uploads per day

## Troubleshooting

### Common Issues

1. **Token Expired**: Replace temporary token with permanent token
2. **Phone Number Not Verified**: Complete business verification process
3. **Webhook Not Receiving**: Check webhook URL and verify token
4. **Messages Not Sending**: Verify phone number format and API credentials

### Error Codes

- `400`: Bad Request - Check message format
- `401`: Unauthorized - Invalid access token
- `403`: Forbidden - Phone number not verified or rate limit exceeded
- `404`: Not Found - Invalid phone number ID

### Logs

Check application logs for detailed error messages:
```bash
# View logs in development
dotnet run --verbosity detailed

# Check specific WhatsApp service logs
grep "WhatsApp" logs/app.log
```

## Production Deployment

1. **Environment Variables**: Store sensitive data in environment variables
2. **HTTPS**: Ensure webhook URL uses HTTPS
3. **Monitoring**: Set up monitoring for message delivery rates
4. **Backup**: Keep backup of important configurations

## Support

For additional support:
- **WhatsApp Business API Documentation**: https://developers.facebook.com/docs/whatsapp
- **Meta Business Help Center**: https://business.facebook.com/help
- **Developer Community**: https://developers.facebook.com/community/

## Next Steps

1. Set up message templates in WhatsApp Business Manager
2. Implement advanced features like interactive buttons
3. Add analytics and reporting for message delivery
4. Integrate with CRM for better customer management 