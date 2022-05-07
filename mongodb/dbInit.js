db.createUser({
    user: 'root',
    pwd: 'example',
    roles: [
        {
            role: 'readWrite',
            db: 'Hotels',
        },
    ],
});

db = new Mongo().getDB("Hotels");

db.createCollection('Descriptions', { capped: false });

db.Descriptions.insert([
    { "_id": 1, "Description": 'opisHotel1' },
    { "_id": 2, "Description": 'opisHotel2' },
    { "_id": 3, "Description": 'opis Hotel Francja' },
    { "_id": 4, "Description": 'opis Hotel Włochy' }
]);