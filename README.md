
# The app (boilerplate for Event Driven Design)

## URLs

- depending on the way in which you start your debugging session the app will be available on different (possibly) URLs
    - the expected URL (debug session started *without* using docker) will be: `https://localhost:5001/swagger/index.html`

## Dependencies on other application stacks

- since the app uses other components (Kafka being the most important one) you must have these components listening on appropriate ports
    - the easiest way to achieve this is to run `docker-compose up` on the provided `docker-compose.yml` file 
    - this will bring up the entire stack 
    - after the stack is "up" you may use the provided `Dockerfile` file (provided within the project directory of `EventDriven.Domain.PoC.Api.Rest`) to begin debugging
        - please note that, if you begin your debugging session in this way, your API is located within a docker container that is *NOT* on the internal network as it is specified
          within the `docker-compose` file, so there will be slight differences in which kafka port must be used by the API
          - if the app container is located within a docker network that is proscribed within the `docker-compose` file, you will be using `kafka:9092`
            if, on the other hand, it is located outside the docker network proscribed within the `docker-compose` file, you will be using `localhost:29092`
            - investigate the docker-compose file, specifically the kafka section for more information on this detail

## How to quicky start debugging the app

1) run docker-compose up, wait for the stack to "settle", I use docker for windows on my Windows 10 development machine
   => I use the following for engine config, take note of the buildkit property 
   <code> 
   {
      "builder": {
        "gc": {
          "defaultKeepStorage": "20GB",
          "enabled": true
        }
      },
      "debug": false,
      "experimental": false,
      "features": {
        "buildkit": true
      },
      "insecure-registries": [],
      "registry-mirrors": []
  }
  </code>
2) from Visual Studio 2022, select `EventDriven.Domain.PoC.Api.Rest` as your "entrypoint" (starting) project and hit debug
   this will bring up the application, that will obviously be located outside the docker network, ergo the proper url to use for communicating with kafka is `localhost:29092`

This is the easiest way to begin debugging.

# EF migrations

The project that is a home for EF context is the `Repository.EF`, therefore the migrations are started using these commands

- `dotnet ef migrations add Initial --project EventDriven.Domain.PoC.Repository.EF`
- `dotnet ef database update --project EventDriven.Domain.PoC.Repository.EF`

# Jaeger and consul (service discovery)

The ideas behind `Consul`, `Eureka` and `Jaeger` are the following:
- I need all services to be registered in a key value store so I can query that store to always know what service is available (and healthy)
    - the key value store is Consul
    - the services register themselves and are then discoverable via Eureka
    - the spans that happen between the services are being traced and logged via Jaeger

## Liveness probe 
- URL: `https://localhost:5001/liveness`
    - will emit a simple 200: Healthy 

## Rudimentary HC endpoint

Basically this is used by load balancers or service discovery subsystem so they know that a service is "alive"
The data presented via the /hc endpoint can also be used for dashboard purposes (Grafana or Kibana) if one so desires

- URL: `https://localhost:5001/hc`

The following is the hc UI which is not all that important, its just an UI over the /hc json payload
Depending on the version of .net core and the version of hc package, there tend to be some incompatibilies, so I usually leave the UI out until it matures 

- URL: `https://localhost:5001/healthchecks-ui#/healthchecks` 

## Jaeger (open tracing)
- Jaeger is located in a docker container (see docker-compose)
- URL: `http://localhost:16686/search`

## Consul
The key values store where we keep the service/app registrations 
- URL: `http://localhost:8500/ui/dc1/services/consul/instances`





# Event driven architecture

## Commands

## Domain Events

If the Domain Event should be processed outside of the ongoing transaction scope, you should define a Notification Object for it. 
This is the object that should be written to the Outbox.

First thing to note is Json.NET library usage to serialize/deserialize event messages. Second thing to note are 2 constructors of CustomerRegisteredNotification class. 
First of them is for creating notification based on Domain Event. 
Second of them is to deserialize message from JSON string which is presented in following section regarding processing.

The most important parts are:

Line 1 – [DisallowConcurrentExecution] attribute means that scheduler will not start a new instance of a job if another instance of that job is already running. 
This is important because we don’t want process Outbox concurrently.
Line 25 – Get all messages to process
Line 30 – Deserialize the message (which is usually a state carried integration event) to a Notification Object
Line 32 – Processing the Notification Object (for example sending event to an event bus, or in our example to a Kafka topic)
Line 38 – Mark message as processed in the outbox

1. Command Handler defines a transaction boundary
A transaction is started when Command Handler is invoked, the transaction is committed **at the end**.
2. Each Domain Event handler is invoked in context of the one and the same transaction boundary (within the boundaries of the same application so to speak)
3. If we want to process something **outside** the boundary of one application, we need to create a *public* event, based on the original Domain Event

We call this event an Integration event. 
These (integration) events will be inserted into the Outbox db table to later be picked up by a background worker (scheduled with the help of Quartz). 
This is done deliberately in this way in order to make sure we wont break the consistency/atomicity rule for a domain event=>integration event transaction.
The integration event is therefore being written into the local db (outbox table) within the same transaction that also completes the mutation on an Aggregate.
Either both succeed or they both fail. Nothing is left hanging / is left in an corrupt state, and no manual compensatory actions are needed.

We need two decorators. The first one will be used for command handlers,
a second decorator will be used for the Domain Event handler, which will be used to publish Domain Events at the very end of the transaction, without committing 

## Integration Events

The second most important thing is to know when it is time to publish our integration events.
Events (many) may be created after any kind of mutation has been applied on an Aggregate, we must publish the change(s) made so other parts of the app stack can react to them:
– this can be done after each Command gets handled (but BEFORE the transaction is commited)
– this can also be done after each Domain Event is handled (but WITHOUT committing transaction)

These are non trivial choices. You must have a solid understanding on the sequence of the "flow".


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


### Jaeger
docker run -d -p 6831:6831/udp -p 6832:6832/udp -p 14268:14268 -p 14250:14250 -p 16686:16686 -p 5778:5778 --name jaeger jaegertracing/all-in-one:1.22
http://localhost:16686/