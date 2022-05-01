--CREATE TABLES
CREATE TABLE Destinations (
    ID smallint NOT NULL,
    Country character varying(255) NOT NULL,
    City character varying(255) NOT NULL,
	PRIMARY KEY (ID)
);

CREATE TABLE Hotels (
    ID character varying(36) NOT NULL,
    Name character varying(255) NOT NULL,
	Destination_ID smallint NOT NULL,
	Rating real NOT NULL,
	Food character varying(36) NOT NULL,
	Stars smallint,
	PRIMARY KEY (ID),
	CONSTRAINT Destination_ID FOREIGN KEY(Destination_ID) REFERENCES Destinations(ID)
);

CREATE TABLE HotelRoomTypes (
    ID smallint NOT NULL,
    Name character varying(36) NOT NULL,
    Capacity_people smallint NOT NULL,
    PRIMARY KEY (ID)
);

CREATE TABLE HotelRooms (
	ID smallint NOT NULL,
	Hotel_ID character varying(36) NOT NULL,
	RoomType_ID smallint NOT NULL,
	Quantity smallint NOT NULL,
	PRIMARY KEY (ID),
	CONSTRAINT Hotel_ID FOREIGN KEY(Hotel_ID) REFERENCES Hotels(ID),
	CONSTRAINT RoomType_ID FOREIGN KEY(RoomType_ID) REFERENCES HotelRoomTypes(ID)
);

CREATE TABLE Tours (
    ID character varying(36) NOT NULL,
    Name character varying(255) NOT NULL,
    Country character varying(255) NOT NULL,
	Rating real NOT NULL,
	Food character varying(36) NOT NULL,
	PRIMARY KEY (ID)
);

CREATE TABLE Events (
    ID uuid DEFAULT gen_random_uuid() PRIMARY KEY,
    Type character varying(16) NOT NULL,
    Hotel_ID character varying(36) NOT NULL,
    CONSTRAINT Hotel_ID FOREIGN KEY(Hotel_ID) REFERENCES Hotels(ID)
);

CREATE TABLE EventRooms(
    ID uuid DEFAULT gen_random_uuid() PRIMARY KEY,
    Event_ID uuid NOT NULL,
    Quantity smallint NOT NULL,
    RoomType_ID smallint NOT NULL,
    CONSTRAINT Event_ID FOREIGN KEY(Event_ID) REFERENCES Events(ID) ON DELETE CASCADE
);

--TEST DATA

INSERT INTO Destinations(ID, Country, City)
VALUES
	(1, 'Poland', 'Gdansk'),
	(2, 'Poland', 'Warsaw');
	
INSERT INTO Hotels(ID, Name, Destination_ID, Rating, Food, Stars)
VALUES
	('abcdefg', 'Hotel GDA', 1, 4.7, '2 posilki', 3),
	('hijklmn', 'Mariott', 2, 5.5, '2 posilki', 5);
	
INSERT INTO HotelRoomTypes(ID, Name, Capacity_people)
VALUES 
    (1, 'Small', 1),
    (2, 'Medium', 2),
    (3, 'Large', 3),
    (4, 'Apartment', 4),
    (5, 'Studio', 4);

INSERT INTO HotelRooms(ID, Hotel_ID, RoomType_ID, Quantity)
VALUES
	(1, 'abcdefg', 1, 10),
	(2, 'abcdefg', 2, 6),
	(3, 'hijklmn', 1, 50),
	(4, 'hijklmn', 2, 30),
	(5, 'hijklmn', 4, 10);