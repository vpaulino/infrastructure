# Introduction 
This solution groups a set of projects that are common used accross diferent types of business solutions. 
The main purposes of this framework ( if we can call it like that) are:
1.  Gather in one place several application blocks that are not application related
2.  .NET Knowledge base to know how to work with diferent Microsoft SDKs
3.  Knowledge base to how to work with thirdparty .NET sdks to comunication with diferent types of external dependencies, such as NoSQL Databases
4.  Knowledge base to devops yaml scripts 
5. Allways guarantee that they are reusable 

# Getting Started
They will be available as nuget packages and being versioned with semver. (WIP). To see this libraries as part of an application take a look to repository
weatherforecastspa 

# Libraries

## VPFrameworks.CQRS
This library consists on a CQRS design providing the main extension points of an CQRS Framework. 

## VPFrameworks.Http.HttpClient
This library consists on a Http CLient application block that does all the http work. 

## VPFrameworks.Messaging.Abstractions
Abstractions related to messaging contracts that can support multiple concrete implementations from diferent SDKs, such as Azure Service Bus, Azure Storage Queues and RAbbitMQ.

## VPFrameworks.Messaging.Azure.ServiceBus
Concrete implementation of   VPFrameworks.Messaging.Abstractions to azure service Bus 

## VPFrameworks.Messaging.Azure.StorageQueues
Concrete implementation of   VPFrameworks.Messaging.Abstractions to azure Azure Storage Queues

## VPFrameworks.Messaging.Azure.PushNotifications
Azure Push Notification abstraction. this library is still WIP because it still does not provide an abstraction arround Push Notifications

## VPFrameworks.Persistence.Abstractions
Abstractions related to database persistence. Diferent contracts for read and write operations are align with the usage of the CQRS pattern available in VPFRameworks.CQRS

## VPFrameworks.Persistence.EntityFramework
WIP

## VPFrameworks.Persistence.MongoDb
Concrete VPFrameworks.Persistence.Abstractions implementation to comunicate with mongodb. abstracting sdk details on how to execute read and write operations with diferent concrete implementations.



## VPFrameworks.Messaging.RabbitMQ 
Concrete implementation of   VPFrameworks.Messaging.Abstractions to azure RabbitMQ


 

# Build and Test
There will be unit tests available to them to understand how to used them. Unit tests on application blocks that have depedencies to external comunication is not easy task. Always if possible decoupling will be put in place regarding concrete implementations of external sdks

# Contribute
To contribute to this solution you should create fork on you personal space and after that create pull requests. 

