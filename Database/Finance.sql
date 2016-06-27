DROP TABLE Transaction;
DROP TABLE Payment;
DROP TABLE BankAccount;
DROP TABLE User;
DROP TABLE Language;
DROP TABLE Currency;

CREATE TABLE User (
    Id INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(255),
    LastName VARCHAR(255),
    Email VARCHAR(255) UNIQUE,
    Password VARCHAR(255),
    Active INT DEFAULT 1,
    PRIMARY KEY (Id)
);

CREATE TABLE BankAccount (
    Id INT NOT NULL AUTO_INCREMENT,
    User_Id INT,
    Name VARCHAR(255),
    Balance VARCHAR(255),
    Active INT DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (User_Id) REFERENCES User(Id)
);

CREATE TABLE Payment (
    Id INT NOT NULL AUTO_INCREMENT,
    User_Id INT,
    Name VARCHAR(255),
    Amount VARCHAR(255),
    Type VARCHAR(255),
    Active INT DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (User_Id) REFERENCES User(Id)
);

CREATE TABLE Transaction (
    Id INT NOT NULL AUTO_INCREMENT,
    Payment_Id INT ,
    Amount VARCHAR(255) NOT NULL,
    Description VARCHAR(255) NOT NULL,
    DateAdded VARCHAR(255) NOT NULL,
    Active INT DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (Payment_Id) REFERENCES Payment(Id)
);

CREATE TABLE Language(
	Id INT NOT NULL AUTO_INCREMENT,
    Abbrevation VARCHAR(3) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE Translation(
	Id INT NOT NULL AUTO_INCREMENT,
    Language_Id INT NOT NULL,
    Translation VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (Language_Id) REFERENCES Language(Id)
);

CREATE TABLE Currency(
	Id INT NOT NULL AUTO_INCREMENT,
    Abbrevation VARCHAR(3) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Html VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id)
);

INSERT INTO USER (NAME, LASTNAME, EMAIL, PASSWORD) VALUES ('Jandie', 'Hendriks', 'jandie@live.nl', 'test');

COMMIT;