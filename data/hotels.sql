CREATE TABLE Destinations (
    ID smallint NOT NULL,
    Country character varying(255) NOT NULL,
    City character varying(255) NOT NULL,
	PRIMARY KEY (ID)
);

CREATE TABLE Hotels (
    ID smallint NOT NULL,
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
	Hotel_ID smallint NOT NULL,
	RoomType_ID smallint NOT NULL,
	PRIMARY KEY (ID),
	CONSTRAINT Hotel_ID FOREIGN KEY(Hotel_ID) REFERENCES Hotels(ID),
	CONSTRAINT RoomType_ID FOREIGN KEY(RoomType_ID) REFERENCES HotelRoomTypes(ID)
);

CREATE TABLE HotelRoomAvailabilities (
    ID smallint NOT NULL,
    HotelRoom_ID smallint NOT NULL,
    Quantity smallint NOT NULL,
    Date date NOT NULL,
    PRIMARY KEY (ID),
    CONSTRAINT HotelRoom_ID FOREIGN KEY(HotelRoom_ID) REFERENCES HotelRooms(ID)
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
    Username character varying(32) NOT NULL,
    RoomType_ID smallint NOT NULL, 
    TripReservation_Id uuid NOT NULL UNIQUE, 
    Type character varying(16) NOT NULL,
    Hotel_ID smallint NOT NULL,
    StartDate date NOT NULL,
    EndDate date NOT NULL,
    CONSTRAINT Hotel_ID FOREIGN KEY(Hotel_ID) REFERENCES Hotels(ID)
);