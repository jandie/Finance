DROP TABLE Transaction;
DROP TABLE Payment;
DROP TABLE BankAccount;
DROP TABLE User;
DROP TABLE Translation;
DROP TABLE Language;
DROP TABLE Currency;

CREATE TABLE Language (
    Id INT NOT NULL,
    Abbrevation VARCHAR(3) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE Translation (
    Id INT NOT NULL,
    Language_Id INT NOT NULL,
    Translation VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id , Language_Id),
    FOREIGN KEY (Language_Id)
        REFERENCES Language (Id)
);

CREATE TABLE Currency (
    Id INT NOT NULL AUTO_INCREMENT,
    Abbrevation VARCHAR(3) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Html VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `LastName` varchar(255) NOT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Password` varchar(255) NOT NULL,
  `Active` int(11) DEFAULT '1',
  `Currency` int(11) DEFAULT '1',
  `Language` int(11) DEFAULT '0',
  `Token` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email` (`Email`),
  KEY `Currency` (`Currency`),
  CONSTRAINT `user_ibfk_1` FOREIGN KEY (`Currency`) REFERENCES `currency` (`Id`)
);


CREATE TABLE BankAccount (
    Id INT NOT NULL AUTO_INCREMENT,
    User_Id INT,
    Name VARCHAR(255),
    Balance VARCHAR(255),
    Active INT DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (User_Id)
        REFERENCES User (Id)
);

CREATE TABLE Payment (
    Id INT NOT NULL AUTO_INCREMENT,
    User_Id INT,
    Name VARCHAR(255),
    Amount VARCHAR(255),
    Type VARCHAR(255),
    Active INT DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (User_Id)
        REFERENCES User (Id)
);

CREATE TABLE Transaction (
    Id INT NOT NULL AUTO_INCREMENT,
    Payment_Id INT,
    Amount VARCHAR(255) NOT NULL,
    Description VARCHAR(255) NOT NULL,
    DateAdded VARCHAR(255) NOT NULL,
    Active INT DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (Payment_Id)
        REFERENCES Payment (Id)
);

INSERT INTO USER (NAME, LASTNAME, EMAIL, PASSWORD) VALUES ('Jandie', 'Hendriks', 'jandie@live.nl', 'test');

INSERT INTO `finance`.`currency` (`Abbrevation`, `Name`, `Html`) VALUES ('EUR', 'Euro', 'EUR');
INSERT INTO `finance`.`currency` (`Abbrevation`, `Name`, `Html`) VALUES ('USD', 'US Dollar', 'USD');

COMMIT;