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
db.createCollection('capacities');
db.capacities.createIndex({ 'storeId': 2, 'timestamp': -9 });
db.createCollection('stores');
db.stores.insertMany([
    { name: "Migros St. Gallen", _id: 111571, secret: "ff0186c5-c3a5-4668-9642-83fdfc111571" },
    { name: "Saentispark Baeder", _id: 111572, secret: "ff0186c5-c3a5-4668-9642-83fdfc111572" },
    { name: "Interdiscount", _id: 111573, secret: "ff0186c5-c3a5-4668-9642-83fdfc111573" }
]);
db.capacities.insertMany([
    { _id: 211571, timestamp: ISODate("2021-03-29T00:00:00Z"), storeId: 111571, capacity: 70, maxCapacity: 180 },
    { _id: 211572, timestamp: ISODate("2021-03-29T00:01:00Z"), storeId: 111572, capacity: 125, maxCapacity: 150 },
    { _id: 211573, timestamp: ISODate("2021-03-29T00:02:00Z"), storeId: 111573, capacity: 7, maxCapacity: 26 }
]);