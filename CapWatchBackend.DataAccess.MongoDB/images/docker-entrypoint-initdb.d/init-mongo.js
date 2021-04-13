conn = new Mongo();
admin = conn.getDB('admin');
admin.auth('root', 'netwitness');
admin.createUser({
    user: 'capwadmin',
    pwd: 'capwadmin123',
    roles: ['userAdminAnyDatabase']
});
db = admin.getSiblingDB('capwatchDB');
admin.createUser({
    user: 'capwusr',
    pwd: 'capwusr123',
    roles: [{ role: "readWrite", db: "capwatchDB" }]
});
db.createCollection('stores');