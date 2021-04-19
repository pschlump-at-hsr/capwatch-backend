# Code Quality
To ensure our Code Quality we use Sonarqube und Sonarlint.

## Sonarqube Metrics
The current state of the Code Quality for the develop Branch can be viewed at the following Sonarqube [Project Page](https://se1-sonarqube.dev.ifs.hsr.ch/dashboard?id=CapwatchBackend).

# capwatch-backend

CapWatch Backend

# Setup Visual Studio

## Import vssettings

Tools -> Import and Export Settings -> Dialog
Import selected environment settings -> Next -> No, just import new settings... -> Next -> Browse -> *select Settings File* -> Next -> Finish

## Power Commands for Visual Studio

Code autoformatting and import organizer when file saved

Extensions -> Manage Extensions -> Search *Power Commands for Visual Studio* -> Install -> restart Visual Studio

MongoDB Tools -> Manage Extensions -> Search MongoDB Tools -> Install

![Server Connection Properties](.\CapWatchBackend.DataAccess.MongoDB\images\Server_Connection_Properties.png)

Password is capwusr123

# Build and run Docker Container

```
cd /path/to/capwatch-backend
docker-compose up -d
```

