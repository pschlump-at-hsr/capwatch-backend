const config = require('./config');
conn = new Mongo();
admin = conn.getDB('admin');
admin.auth('root', 'netwitness');
admin.createUser({
    user: config.MONGO_ADMINNAME,
    pwd: config.MONGO_ADMINPASSWORD,
    roles: ['userAdminAnyDatabase']
});
db = admin.getSiblingDB('capwatchDB');
admin.createUser({
    user: config.MONGO_USERNAME,
    pwd: config.MONGO_PASSWORD,
    roles: [{ role: "readWrite", db: "capwatchDB" }]
});
db.createCollection('stores');