using IdentityService.Data.DatabaseContexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IdentityService.Tests.Integration.Fixtures;
using Framework.Kafka.Core.Contracts;
using IdentityService.Domain.DomainEntities.Inbox;
using IdentityService.Domain.DomainEntities.OutboxPattern;
using SharedKernel.DomainImplementations.BaseClasses;
using IdentityService.Api;

namespace IdentityService.Tests.Integration.Tests
{
    public class BasicInfrastructureTests : TestBase
    {
        public BasicInfrastructureTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public void TestDatabaseAccess()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Try to access the database
            var users = dbContext.ApplicationUsers.ToList();

            // Assert that we can access the database and get users
            Assert.NotNull(users);
        }

        [Fact]
        public void TestOutboxInboxAccess()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if Outbox and Inbox tables are accessible
            var outboxMessages = dbContext.OutboxMessages.ToList();
            var inboxMessages = dbContext.InboxMessages.ToList();

            // Assert that we can access the outbox and inbox
            Assert.NotNull(outboxMessages);
            Assert.NotNull(inboxMessages);

            // Write to the outbox and read back
            var testOutboxMessage = new OutboxMessage(DateTime.UtcNow, "TestType", "This is a test outbox message", EventTypeEnum.VerificationEmailSendFailure);
            dbContext.OutboxMessages.Add(testOutboxMessage);
            dbContext.SaveChanges();

            var retrievedOutboxMessage = dbContext.OutboxMessages.Find(testOutboxMessage.Id);
            Assert.NotNull(retrievedOutboxMessage);
            Assert.Equal(testOutboxMessage.Data, retrievedOutboxMessage.Data);

            // Similarly, write to the inbox and read back
            var testInboxMessage = new InboxMessage(DateTime.UtcNow, "TestType", "This is a test inbox message");
            dbContext.InboxMessages.Add(testInboxMessage);
            dbContext.SaveChanges();

            var retrievedInboxMessage = dbContext.InboxMessages.Find(testInboxMessage.Id);
            Assert.NotNull(retrievedInboxMessage);
            Assert.Equal(testInboxMessage.Data, retrievedInboxMessage.Data);
        }

        [Fact]
        public async Task TestKafkaProducerConsumer()
        {
            using var scope = Factory.Services.CreateScope();
            var kafkaProducer = scope.ServiceProvider.GetRequiredService<IKafkaScheduledProducer>();
            var kafkaConsumer = scope.ServiceProvider.GetRequiredService<IKafkaScheduledConsumer>();

            // Prepare a test message
            var messageType = "TestMessageType";
            var messageData = $"Test message at {DateTime.UtcNow}";

            // Produce a message
            var produceResult = await kafkaProducer.WriteMessageAsync(messageType, messageData);
            Assert.True(produceResult);

            // Consume the message
            var consumeResult = kafkaConsumer.Consume();
            Assert.Null(consumeResult); // Since we're using mock implementations
        }
    }
}
