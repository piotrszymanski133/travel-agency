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

CREATE TABLE HotelRooms (
	ID smallint NOT NULL,
	Hotel_ID character varying(36) NOT NULL,
	Capacity_people smallint NOT NULL,
	Quantity smallint NOT NULL,
	PRIMARY KEY (ID),
	CONSTRAINT Hotel_ID FOREIGN KEY(Hotel_ID) REFERENCES Hotels(ID)
);

CREATE TABLE Tours (
    ID character varying(36) NOT NULL,
    Name character varying(255) NOT NULL,
    Country character varying(255) NOT NULL,
	Rating real NOT NULL,
	Food character varying(36) NOT NULL,
	PRIMARY KEY (ID)
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
	
INSERT INTO HotelRooms(ID, Hotel_ID, Capacity_people, Quantity)
VALUES
	(1, 'abcdefg', 1, 10),
	(2, 'abcdefg', 2, 6),
	(3, 'hijklmn', 1, 50),
	(4, 'hijklmn', 2, 30),
	(5, 'hijklmn', 4, 10);