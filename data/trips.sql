CREATE TABLE OrderedTrips
(
    Trip_ID uuid PRIMARY KEY,
    Username character varying(32) NOT NULL,
    Room_Type_Name character varying(32) NOT NULL,
    Country character varying(255) NOT NULL,
    City character varying(255) NOT NULL,
    Food character varying(36) NOT NULL,
    Transport_Type_Name character varying(16) NOT NULL,
    Hotel_Name character varying(255) NOT NULL,
    Persons int NOT NULL,
    StartDate date NOT NULL,
    EndDate date NOT NULL
);