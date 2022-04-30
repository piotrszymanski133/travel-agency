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
    { "_id": 'abcdefg', "Description": 'opisHotel1' },
    { "_id": 'hijklmn', "Description": 'opisHotel2' }
]);