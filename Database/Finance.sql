--
-- Table structure for table `bankaccount`
--

DROP TABLE IF EXISTS `bankaccount`;

CREATE TABLE `bankaccount` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `User_Id` int(11) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Balance` varchar(50) DEFAULT '0.00',
  `Active` int(11) DEFAULT '1',
  `NameSalt` varchar(30) DEFAULT NULL,
  `BalanceSalt` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `User_Id` (`User_Id`),
  CONSTRAINT `bankaccount_ibfk_1` FOREIGN KEY (`User_Id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8;

--
-- Table structure for table `currency`
--

DROP TABLE IF EXISTS `currency`;

CREATE TABLE `currency` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Abbrevation` varchar(3) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Html` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8;

--
-- Table structure for table `language`
--

DROP TABLE IF EXISTS `language`;

CREATE TABLE `language` (
  `Id` int(11) NOT NULL,
  `Abbrevation` varchar(3) NOT NULL,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `payment`
--

DROP TABLE IF EXISTS `payment`;

CREATE TABLE `payment` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `User_Id` int(11) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Amount` varchar(50) DEFAULT NULL,
  `Type` varchar(30) DEFAULT NULL,
  `Active` int(11) DEFAULT '1',
  `NameSalt` varchar(30) DEFAULT NULL,
  `AmountSalt` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `User_Id` (`User_Id`),
  CONSTRAINT `payment_ibfk_1` FOREIGN KEY (`User_Id`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8;

--
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;

CREATE TABLE `transaction` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Payment_Id` int(11) DEFAULT NULL,
  `Amount` varchar(50) NOT NULL,
  `Description` varchar(1000) NOT NULL,
  `DateAdded` varchar(11) NOT NULL,
  `Active` int(11) DEFAULT '1',
  `AmountSalt` varchar(30) DEFAULT NULL,
  `DescriptionSalt` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Payment_Id` (`Payment_Id`),
  CONSTRAINT `transaction_ibfk_1` FOREIGN KEY (`Payment_Id`) REFERENCES `payment` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8;

--
-- Table structure for table `translation`
--

DROP TABLE IF EXISTS `translation`;

CREATE TABLE `translation` (
  `Id` int(11) NOT NULL,
  `Language_Id` int(11) NOT NULL,
  `Translation` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`,`Language_Id`),
  KEY `Language_Id` (`Language_Id`),
  CONSTRAINT `translation_ibfk_1` FOREIGN KEY (`Language_Id`) REFERENCES `language` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;

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
  `NameSalt` varchar(30) DEFAULT NULL,
  `LastNameSalt` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email` (`Email`),
  KEY `Currency` (`Currency`),
  CONSTRAINT `user_ibfk_1` FOREIGN KEY (`Currency`) REFERENCES `currency` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8;