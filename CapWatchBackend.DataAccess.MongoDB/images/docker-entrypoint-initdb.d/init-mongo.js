const config = require('./config');
const conn = new Mongo();
const admin = conn.getDB('admin');
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
db.createCollection('storeTypes');
db.storeTypes.insertMany([
    { _id: ObjectId("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"), name: "Detailhändler"},
    { _id: ObjectId("7b0523b7-4efd-4fdf-b11d-3f4d26cf7b19"), name: "Freizeit" },
    { _id: ObjectId("f58957ce-fb83-4f62-ac2c-6d1fe810d85c"), name: "Bank" },
    { _id: ObjectId("498c4e33-baac-464d-bb15-acac6baf12d5"), name: "Restaurant" },
    { _id: ObjectId("cb94c2f4-f199-4223-966d-9b6717037943"), name: "Kino" },
    { _id: ObjectId("80c9d736-4ee1-4c9b-a94c-49edbf864724"), name: "Bar" }
])