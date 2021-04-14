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

