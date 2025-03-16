# Innovation

## CQRS

A simple framework which aims to provide the ability to use a CQRS pattern in your code base,
currently with immediate consistency. It does not yet implement or try support Event Sourcing.

## vNext Version Objectives

1. Improve Command Pipeline Performance
1. Implement CommandResult Errors As Per RFC 7807
1. Use ValueTask Over Task To Better Support Synchronous Operations
1. Update To .Net 9
1. Add Benchmarks

## Tasks Remaining

1. Update readme
1. Document breaking changes
1. Create wiki
1. Code review

## Alpha Warning

Please note, at this point this is a work in progress, therefore it is considered alpha grade software.
Changes to this code base and the api may still change.

## External Dependencies

1. MiniValidation by Damian Edwards [Link](https://github.com/DamianEdwards/MiniValidation). This replaces the outdated self written recursive validator

# Benchmarks

## .NET 9

## ValuStopWatch

|         Method |     Mean |    Error |   StdDev | Allocated |
|--------------- |---------:|---------:|---------:|----------:|
| StopWatchUsage | 28.79 ns | 0.108 ns | 0.101 ns |         - |


## DataAnnotationsValidatorTests

| Method       | Mean        | Error     | StdDev    | Ratio         | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------- |------------:|----------:|----------:|--------------:|--------:|-------:|----------:|------------:|
| TestCurrent  | 3,018.65 ns | 24.846 ns | 20.748 ns |      baseline |         | 0.3624 |    3808 B |             |
| TestNew      |   969.43 ns | 13.008 ns | 10.862 ns |  3.11x faster |   0.04x | 0.1373 |    1440 B |  2.64x less |

## BlankCommandDataAnnotationsValidatorTests

| Method              | Mean      | Error    | StdDev   | Ratio        | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------- |----------:|---------:|---------:|-------------:|--------:|-------:|----------:|------------:|
| BlankCommandCurrent | 517.57 ns | 9.377 ns | 8.771 ns |     baseline |         | 0.1135 |    1192 B |             |
| BlankCommandNew     |  96.64 ns | 0.674 ns | 0.563 ns | 5.36x faster |   0.09x | 0.0083 |      88 B | 13.55x less |

## DispatcherCommandTests

## Before

|  Method |     Mean |    Error |   StdDev | Allocated |
|-------- |---------:|---------:|---------:|----------:|
| Command | 25.09 ms | 0.468 ms | 0.609 ms |  73.08 KB |

## After

| Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
|-------- |---------:|--------:|--------:|-------:|----------:|
| Command | 283.6 ns | 3.34 ns | 2.61 ns | 0.0196 |     208 B |



Old: 40 Operations per second
New: 3 521 126 Operations per second

1 second = 1 000 000 000 ns


## Dispatcher Command Pipeline

Commands: Dispatcher -> Command Reactors -> Command Interceptors -> Command Validators -> Command Handler -> Command Result Reactors -> Audit Store -> Return Result

## Framework Components

### Command Reactors

Command Reactors Are The First Step In The Command Dispatching Pipeline.
They Can Be Used As Example For Logging Or To Prime Other Services About An Impending Command.

While The Command Is Passed In By Reference, It Is Not Advised To Edit The Object.
The Command Reactor Has No Influence Over Pipeline Execution

These Are Run In Parallel On A Background Thread

### Command Interceptors

This Is The Second Step In The Command Dispatching Pipeline.
These Can, Where Required Make Changes To A Command Or Its Properties. They Are Called One After The Other,
Not In Parallel.

### Command Validators

To Allow Better Seperation Of Concerns This Is The Third Step In The Command Dispatch Pipeline. 
Command Validators Can Be Implemented For A Given Command. It Can Validate As Required, If Validation Fails The Command Handler
Will Not Be Called, Instead The Result Of The Validation Will Be Returned.

While There Can Be Multiple Implementations, The Pipeline Will Return After The First Implementation Returns An Error

### Command Result Reactors

Command Result Reactors Are The Final Step In The Command Dispatching Pipeline.
The Can Be Used As Example For Logging Or Auditing. The Command Result Reactor Has No Influence Over Pipeline Execution

These Are Run In Parallel On A Background Thread

### Commands

Command Should Be Used To Alter State In Resources.
Commands Must Implement The ICommand Interface.

There Can Only Be A Single Handler For A Command

### Queries

Queries Are Used To Load Resources.
Queries Must Implement The IQuery Interface.

There Can Only Be A Single Handler For A Query

### Query Results

QueryResult Are Objects Which Are Returned From A Query Handler.
These Objects Must Implement The IQueryResult Interface.
This Interface Is Soley For Tracking Within The Framework And Does Not Impose Any
Field Or Property Requirements.

### Command Validation

If Commands Do Not Have Implementations Of Command Handlers Registered, They Will Be Checked
Firstly By The Microsoft Validator (System.ComponentModel.DataAnnotations.Validator), They Will 
Also Be Checked If They Implement IValidatableObject (System.ComponentModel.DataAnnotations.IValidatableObject).
If Validation Fails The Command Handler Will Not Be Called, Instead The Result Of The Validation Will Be Returned.

### Audit Store

The Framework Supports Centralised Auditing, Where Any Command, Query Or Meassage Can Be Logged.
Implement The IAuditStore Interface, And Reqister With Dependency Injection. If This Interface Is Found,
The Methods Will Be Called. If It Is Not Present, It Is Simply Ignored

### Messages

Messages Can Be Used To Broadcast To Multiple Handlers

### Correlation

The Dispatcher Supports Either Creating Its Own Or Being Supplied With A Correlation Id.
An Implementation Of This Is Available For Asp.Net Core, Using The Well Known `X-Correlation-ID` Header

Command And Query Handlers Can Now Implement The ICorrelationAware Interface.
When The Dispatcher Sees That They Implement This Interface, It Will Set The CorrelationId Before Calling The Handle Method.

### SearchLocations

The Innovation Loader Is Capable Of Loading Assemblies From Specified Locations.
This Is To Support A Modular Approach.

## Dispatcher Context



## Supported .Net Framework

1. .Net 9.x

## Samples

There are two samples. `Innovation.Sample.Console` and `Innovation.Sample.Web`

## Tests

There is a single test project, however there are two other projects in the test directory.
This is to ensure that the dynamic loading capability can be correctly tested.

## Building

In order to build the solution, you will need to following items

1. Visual Studio 2022 >= 17.12.3
3. Latest .Net9 SDK And Runtime [Download Link](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

