var MONGODB_USERNAME = 'capwusr';
var MONGODB_USERPASSWORD = 'capwusr123';
const conn = new Mongo();
const admin = conn.getDB('admin');
admin.auth('root', 'netwitness');
db = admin.getSiblingDB('capwatchDB');
admin.createUser({
    user: MONGODB_USERNAME,
    pwd: MONGODB_USERPASSWORD,
    roles: [{ role: "readWrite", db: "capwatchDB" }]
});
db.createCollection('stores');
db.stores.insertMany([
    { _id: UUID("9c9cee44-c839-48f2-b54e-237d95fe5d7f"), secret: UUID("00000000-0000-0000-0000-000000000001"), name: "Ikea", street: "Zürcherstrasse 460", zipCode: "9015", city: "St. Gallen", currentCapacity: 100, maxCapacity: 201, storeType: { _id: "f58957ce-fb83-4f62-ac2c-6d1fe810d85c", description: "Bank" } },
    { _id: UUID("9c9cee44-c839-48f2-b54e-238d95fe5d7f"), secret: UUID("00000000-0000-0000-0000-000000000002"), name: "Zoo Zürich", street: "Zürichbergstrasse 221", zipCode: "8044", city: "Zürich", currentCapacity: 528, maxCapacity: 1125, storeType: { _id: "7b0523b7-4efd-4fdf-b11d-3f4d26cf7b19", description: "Freizeit" } },
    { _id: UUID("9c9cee44-c839-48f2-b54e-239d95fe5d7f"), secret: UUID("00000000-0000-0000-0000-000000000003"), name: "Polenmuseum - Schloss Rapperswil", street: "Zürcherstrasse 460", zipCode: "9015", city: "St. Gallen", currentCapacity: 187, maxCapacity: 201, storeType: { _id: "f58957ce-fb83-4f62-ac2c-6d1fe810d85c", description: "Bank" } }
])
db.createCollection('storeTypes');
db.storeTypes.insertMany([
    { _id: UUID("c73e9c5f-de5c-479a-b116-7ee1b93ab4f9"), description: "Detailhändler" },
    { _id: UUID("7b0523b7-4efd-4fdf-b11d-3f4d26cf7b19"), description: "Freizeit" },
    { _id: UUID("f58957ce-fb83-4f62-ac2c-6d1fe810d85c"), description: "Bank" },
    { _id: UUID("498c4e33-baac-464d-bb15-acac6baf12d5"), description: "Restaurant" },
    { _id: UUID("cb94c2f4-f199-4223-966d-9b6717037943"), description: "Kino" },
    { _id: UUID("80c9d736-4ee1-4c9b-a94c-49edbf864724"), description: "Bar" }
]);