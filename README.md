dotnet ef migrations add Initial --project EventDriven.Domain.PoC.Repository.EF
dotnet ef database update --project EventDriven.Domain.PoC.Repository.EF 



## Liveness probe 
- URL: https://localhost:5001/liveness

## Rudimentary HC endpoint
- URL: https://localhost:5001/hc

- URL: https://localhost:5001/healthchecks-ui#/healthchecks

## Jaeger (open tracing)

## Commands

## Domain Events
If the Domain Event should be processed outside of the ongoing transaction, you should define a Notification Object for it. 
This is the object which should be written to the Outbox.

First thing to note is Json.NET library usage to serialize/deserialize event messages. Second thing to note are 2 constructors of CustomerRegisteredNotification class. 
First of them is for creating notification based on Domain Event. 
Second of them is to deserialize message from JSON string which is presented in following section regarding processing.

The most important parts are:
Line 1 – [DisallowConcurrentExecution] attribute means that scheduler will not start new instance of job if other instance of that job is running. 
This is important because we don’t want process Outbox concurrently.
Line 25 – Get all messages to process
Line 30 – Deserialize the message (which is usually a state carried integration event) to a Notification Object
Line 32 – Processing the Notification Object (for example sending event to an event bus, or in our example to a Kafka topic)
Line 38 – Mark message as processed in the outbox

1. Command Handler defines transaction boundary. 
Transaction is started when Command Handler is invoked and committed **at the end**.
2. Each Domain Event handler is invoked in context of the same transaction boundary.
3. If we want to process something **outside** the transaction, or in other words, outside the boundaries of this application, we need to create a public event based on the Domain Event. 
I call this event an Integration event. These events will be inserted into the Outbox before being picked up by a background worker.

We need two decorators. The first one will be for command handlers
we also need a second decorator for the Domain Event handler, which will only publish Domain Events at the very end without committing database transaction

## Integration Events

The second most important thing is when to publish and process Domain Events? Events may be created after each action on the Aggregate, so we must publish them:
– after each Command handling (but BEFORE committing transaction)
– after each Domain Event handling (but WITHOUT committing transaction)



            // [RegisterApplicationUserCommand] will get handled by [RegisterApplicationUserCommandHandler]
            // that one will fire an event [ApplicationUserCreatedDomainEvent] (when the user is created, the user has not yet confirmed his email so his account is not yet fully activated)
            // this will get handled by [ApplicationUserCreatedDomainEventHandler]
            // this will fire [SendAccountVerificationMailCommand]
            // this will get handled by [SendAccountVerificationMailCommandHandler] that will send an email containing the activation link
            // the flow continues when the user clicks on the provided url (found in an email that we had sent to the user)
            // [EmailVerifiedDomainEvent] is then fired, and is handled by [EmailVerifiedDomainEventHandler] and the user will be marked as "verified"
            // finally a greeting mail is sent to the user
            // this will fire a new command: [SendAWelcomeMailCommand]
            // this command will get handled by [SendAWelcomeMailCommandHandler], that will send a greeting email to the users email address
            // a domain event is fired in response to this => [MarkApplicationUserAsWelcomedDomainEvent] is sent
            // and is handled by [MarkApplicationUserAsWelcomedDomainEventHandler] marking the user as "welcomed"


            // Forgot password Flow


            // Resend activation link Flow

            // User 
            created
            deleted
            address changed
            deactivated, reactivated, activated, undeleted

            // Role
            created
            deleted
            deactivated, reactivated, activated, undeleted
            assigned to user, removed from user

            // Address
            created
            deleted
            deactivated, reactivated, activated, undeleted
            user given address, user changed address

## TODO

### HiLo on MSSQL
### Guid as PK (generated up front)