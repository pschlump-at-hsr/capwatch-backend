const config = require('./config');
const conn = new Mongo();
const admin = conn.getDB('admin');
admin.auth('root', 'netwitness');
admin.createUser({
    user: $MONGODB_ADMINUSER,
    pwd: $MONGODB_ADMINPASSWORD,
    roles: ['userAdminAnyDatabase']
});
db = admin.getSiblingDB('capwatchDB');
admin.createUser({
    user: $MONGODB_USERNAME,
    pwd: $MONGODB_USERPASSWORD,
    roles: [{ role: "readWrite", db: "capwatchDB" }]
});
db.createCollection('stores');
db.createCollection('types');