# capwatch-backend

CapWatch Backend

## Setup Visual Studio

### Import vssettings

Tools -> Import and Export Settings -> Dialog
Import selected environment settings -> Next -> No, just import new settings... -> Next -> Browse -> *select Settings File* -> Next -> Finish

### Power Commands for Visual Studio

Code autoformatting and import organizer when file saved

Extensions -> Manage Extensions -> Search *Power Commands for Visual Studio* -> Install -> restart Visual Studio

MongoDB Tools -> Manage Extensions -> Search MongoDB Tools -> Install

![Server Connection Properties](.\CapWatchBackend.DataAccess.MongoDB\images\Server_Connection_Properties.png)

Password is capwusr123

## Build and run Docker Container

``` cmd
cd /path/to/capwatch-backend
docker-compose up -d
```

## Code Quality

To ensure our Code Quality we use Sonarqube und Sonarlint.

### Sonarqube Metrics

The current state of the Code Quality for the develop Branch can be viewed at the following Sonarqube [Project Page](https://se1-sonarqube.dev.ifs.hsr.ch/dashboard?id=CapwatchBackend).

### SonarLint Visual Studio Extension

To check SonarLint conformity the extension *SonarLint for Visual Studio 2019* needs to be installed via Extensions -> Manage Extensions -> Online -> Search for the extension -> Install -> Restart Visual Studio

To check the code right click the Solution in the Solution Explorer -> Analyze and Code Cleanup -> Run Code Analysis on Solution => The results of the analysis are displayed in the *Error List* window (make sure to display Warnings and Messages in addition to Errors)

## MongoDB Credentials

For the dev enviroment you have to add the passwords manually at the beginning of the init-mongo.js File at /CapWatchBackend.DataAccess.MongoDB/images/docker-entrypoint-initdb.d/init-mongo.js

`var MONGODB_ADMINPASSWORD = ${MONGODB_ADMINPASSWORD};`  replace the ${MONGODB_ADMINPASSWORD} with the password.

You can lookup the passwords and usernames in Gitlab capwatch-backend project -> settings -> CI/CD -> Variables.