DROP TABLE User CASCADE;
DROP TABLE BankAccount CASCADE;
DROP TABLE Payment CASCADE;
DROP TABLE `Transaction` CASCADE;

CREATE TABLE User(
  Id int NOT NULL AUTO_INCREMENT,
  Name varchar(255),
  LastName varchar(255),
  Email varchar(255) UNIQUE,
  Password varchar(255),
  Active int DEFAULT 1,
  PRIMARY KEY (Id)
);

CREATE TABLE BankAccount(
  Id int NOT NULL AUTO_INCREMENT,
  User_Id int,
  Name varchar(255),
  Balance varchar(255),
  Active int DEFAULT 1,
  PRIMARY KEY (Id)
);

CREATE TABLE Payment(
  Id int NOT NULL AUTO_INCREMENT,
  User_Id int,
  Name varchar(255),
  Amount varchar(255),
  Type varchar(255),
  Active int,
  PRIMARY KEY (Id)
);

CREATE TABLE `Transaction`(
  Id int NOT NULL AUTO_INCREMENT,
  Payment_Id int,
  Amount varchar(255),
  Description varchar(255),
  Active int DEFAULT 1,
  PRIMARY KEY (Id)
);

INSERT INTO USER (NAME, LASTNAME, EMAIL, PASSWORD) VALUES ('Jandie', 'Hendriks', 'jandie@live.nl', 'test');
user
COMMIT;