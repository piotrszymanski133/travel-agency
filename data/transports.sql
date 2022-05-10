CREATE TABLE Places (
    ID bigserial NOT NULL,
    Country character varying(255) NOT NULL,
    City character varying(255) NOT NULL,
    PRIMARY KEY (ID)
);

CREATE TABLE Transport (
    ID bigserial NOT NULL,
    Destination_Places_ID bigserial NOT NULL,
    Source_Places_ID bigserial NOT NULL,
    TransportType character varying(255) NOT NULL,
    TransportDate DATE NOT NULL,
    Places int NOT NULL,
    PRIMARY KEY (ID),
    CONSTRAINT Destination_Places_ID FOREIGN KEY(Destination_Places_ID) REFERENCES Places(ID),
    CONSTRAINT Source_Places_ID FOREIGN KEY(Source_Places_ID) REFERENCES Places(ID)
);
-- ID bigserial NOT NULL,
CREATE TABLE TransportEvent (
    ID uuid DEFAULT gen_random_uuid() ,
    Transport_ID bigserial NOT NULL,
    Event_ID uuid NOT NULL,
    Places int NOT NULL,
    Type character varying(16) NOT NULL,
    Username character varying(32) NOT NULL,
    PRIMARY KEY (ID),
    CONSTRAINT Transport_ID FOREIGN KEY(Transport_ID) REFERENCES Transport(ID)
);