CREATE DATABASE  IF NOT EXISTS `finance` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `finance`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: finance
-- ------------------------------------------------------
-- Server version	5.7.19-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `balancehistory`
--

DROP TABLE IF EXISTS `balancehistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `balancehistory` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) DEFAULT NULL,
  `BankAccountHistory` varchar(50) DEFAULT NULL,
  `BankAccountHistorySalt` varchar(30) DEFAULT NULL,
  `Date` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserId_idx` (`UserId`),
  CONSTRAINT `UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bankaccount`
--

DROP TABLE IF EXISTS `bankaccount`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `currency`
--

DROP TABLE IF EXISTS `currency`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `currency` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Abbrevation` varchar(3) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Html` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `language`
--

DROP TABLE IF EXISTS `language`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `language` (
  `Id` int(11) NOT NULL,
  `Abbrevation` varchar(3) NOT NULL,
  `Name` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `loginlog`
--

DROP TABLE IF EXISTS `loginlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `loginlog` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Time` varchar(45) DEFAULT NULL,
  `Username` varchar(45) DEFAULT NULL,
  `Succes` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `payment`
--

DROP TABLE IF EXISTS `payment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `translation`
--

DROP TABLE IF EXISTS `translation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `translation` (
  `Id` int(11) NOT NULL,
  `Language_Id` int(11) NOT NULL,
  `Translation` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`,`Language_Id`),
  KEY `Language_Id` (`Language_Id`),
  CONSTRAINT `translation_ibfk_1` FOREIGN KEY (`Language_Id`) REFERENCES `language` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'finance'
--

--
-- Dumping routines for database 'finance'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-10-20 15:09:03
