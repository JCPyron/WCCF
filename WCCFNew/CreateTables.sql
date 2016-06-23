
-- tables
-- Table: ContactPlace
CREATE TABLE ContactPlace (
    Customer_CustomerID int  NOT NULL,
    Email bit  NOT NULL,
    Phone bit  NOT NULL,
    Twitter bit  NOT NULL,
    Facebook bit  NOT NULL,
    CONSTRAINT ContactPlace_pk PRIMARY KEY  (Customer_CustomerID)
)
;





-- Table: ContactTime
CREATE TABLE ContactTime (
    Customer_CustomerID int  NOT NULL,
    Morning bit  NOT NULL,
    Afternoon bit  NOT NULL,
    Evening bit  NOT NULL,
    CONSTRAINT ContactTime_pk PRIMARY KEY  (Customer_CustomerID)
)
;





-- Table: Email
CREATE TABLE Email (
    Customer_CustomerID int  NOT NULL,
    EmailAddress nvarchar(50)  NOT NULL,
    CONSTRAINT Email_pk PRIMARY KEY  (Customer_CustomerID)
);

-- Table: Phone
CREATE TABLE Phone (
    Customer_CustomerID int  NOT NULL,
    Phone nvarchar(10)  NOT NULL,
    CONSTRAINT Phone_pk PRIMARY KEY  (Customer_CustomerID)
);

CREATE TABLE Twitter
(
	Id INT NOT NULL PRIMARY KEY, 
    AToken NCHAR(100) NOT NULL, 
    ASecret NCHAR(100) NOT NULL, 
    UserId BIGINT NOT NULL, 
    ScreenName NCHAR(100) NOT NULL
);

CREATE TABLE Face
(
	Id INT NOT NULL PRIMARY KEY, 
    AToken NCHAR(200) NOT NULL
);

CREATE TABLE UMail
(
	Id INT NOT NULL PRIMARY KEY, 
    UserName NCHAR(100) NOT NULL, 
    Password NCHAR(100) NOT NULL
);

-- Table: SMtype
CREATE TABLE SMtype (
    SocialMedia_SMtyKey int  NOT NULL,
    Type nvarchar  NOT NULL,
    CONSTRAINT SMtype_pk PRIMARY KEY  (SocialMedia_SMtyKey)
);

-- Table: SocialMedia
CREATE TABLE SocialMedia (
    Customer_CustomerID int  NOT NULL,
    SMHandle nvarchar (50) NOT NULL,
    SMtyKey int  NOT NULL,
    CONSTRAINT SocialMedia_pk PRIMARY KEY  (SMHandle));

-- Table: Customers
CREATE TABLE Customers (
    CustomerID int  NOT NULL,
    FirstName nvarchar(max)  NOT NULL,
    LastName nvarchar(max)  NOT NULL,
    isSubbed bit  NOT NULL,
    CONSTRAINT Customers_pk PRIMARY KEY  (CustomerID)
);

-- Table: addresses
CREATE TABLE addresses (
    Customer_CustomerID int  NOT NULL,
    StreetAddress nvarchar(50)  NOT NULL,
    city nvarchar(max)  NOT NULL,
    state nvarchar(2)  NOT NULL,
    zip int  NOT NULL,
    CONSTRAINT addresses_pk PRIMARY KEY  (Customer_CustomerID)
);

-- foreign keys
-- Reference:  ContactPlace_Customer (table: ContactPlace)

ALTER TABLE ContactPlace ADD CONSTRAINT ContactPlace_Customer 
    FOREIGN KEY (Customer_CustomerID)
    REFERENCES Customers (CustomerID)
;

-- Reference:  Contact_Customer (table: ContactTime)

ALTER TABLE ContactTime ADD CONSTRAINT Contact_Customer 
    FOREIGN KEY (Customer_CustomerID)
    REFERENCES Customers (CustomerID)
;

-- Reference:  SMAccountInfo_SMtype (table: SMAccountInfo)

ALTER TABLE SMAccountInfo ADD CONSTRAINT SMAccountInfo_SMtype 
    FOREIGN KEY (SMType)
    REFERENCES SMtype (SocialMedia_SMtyKey)
;

-- Reference:  SocialMedia_SMtype (table: SocialMedia)

ALTER TABLE SocialMedia ADD CONSTRAINT SocialMedia_SMtype 
    FOREIGN KEY (SMtyKey)
    REFERENCES SMtype (SocialMedia_SMtyKey)
;

-- Reference:  SocialMedia_Customer (table: SocialMedia)

ALTER TABLE SocialMedia ADD CONSTRAINT SocialMedia_Customer 
    FOREIGN KEY (Customer_CustomerID)
    REFERENCES Customers (CustomerID)
;

-- Reference:  Table_10_Customer (table: Phone)

ALTER TABLE Phone ADD CONSTRAINT Phone_Customer 
    FOREIGN KEY (Customer_CustomerID)
    REFERENCES Customers (CustomerID)
;

-- Reference:  Table_9_Customer (table: Email)

ALTER TABLE Email ADD CONSTRAINT Email_Customer 
    FOREIGN KEY (Customer_CustomerID)
    REFERENCES Customers (CustomerID)
;

-- Reference:  addresses_Customer (table: addresses)

ALTER TABLE addresses ADD CONSTRAINT addresses_Customer 
    FOREIGN KEY (Customer_CustomerID)
    REFERENCES Customers (CustomerID)
;


-- End of file.
