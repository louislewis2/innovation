# Innovation

## CQRS 

A simple framework which aims to provide the ability to use a CQRS pattern in your code base,
currently with immediate consistency. It does not yet implement or support Event Sourcing.

Version 1.X, Will Be The Last To Support Dot Net Core 2.X.
The Next Version Will Support Dot Net Core 3.x

## Dispatcher Command Pipeline

Commands: Dispatcher -> Command Reactors -> Command Interceptors -> Command Validators -> Command Handler -> Command Result Reactors -> Audit Store

## Framework Components

### Command Reactors

Command Reactors Are The First Step In The Command Dispatching Pipeline.
They Can Be Used As Example For Logging Or To Prime Other Services About An Impending Command.

While The Command Is Passed In By Reference, It Is Not Advised To Edit The Object.
The Command Reactor Has No Influence Over Pipeline Execution

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
The Can Be Used As Example For Logging Or Auditing

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

### SearchLocations

Add Details Here

## Supported .Net Frameworks

1. .Net Standard 2.0

## Samples

There are two samples. `Innovation.Sample.Console` and `Innovation.Sample.Web`

## Tests

There is a single test project, however there are two other projects in the test directory.
This is to ensure that the loading capability can be correctly tested.

## Building

In order to build the solution, you will need to following items

1. Visual Studio 2019 >= 16.3.2
2. Visual Studio .Net Framework Targeting Packs and SDK's for .Net 4.5.1 through .Net 4.6.2 (See Visual Studio Installer - Modify - Individual Components)
3. Latest .Net Core SDK [Download Link](https://download.microsoft.com/download/0/F/D/0FD852A4-7EA1-4E2A-983A-0484AC19B92C/dotnet-sdk-2.0.0-win-x64.exe)
4. Latest .Net Core Runtime [Download Link](https://download.microsoft.com/download/5/6/B/56BFEF92-9045-4414-970C-AB31E0FC07EC/dotnet-runtime-2.0.0-win-x64.exe)

