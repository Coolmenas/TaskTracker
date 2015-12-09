-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Dec 09, 2015 at 10:03 AM
-- Server version: 5.6.17
-- PHP Version: 5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `task_manager`
--

-- --------------------------------------------------------

--
-- Table structure for table `tasks`
--

CREATE TABLE IF NOT EXISTS `tasks` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Description` varchar(250) NOT NULL,
  `DateCreated` date NOT NULL,
  `DateDue` date NOT NULL,
  `WorkerId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=24 ;

--
-- Dumping data for table `tasks`
--

INSERT INTO `tasks` (`Id`, `Name`, `Description`, `DateCreated`, `DateDue`, `WorkerId`) VALUES
(11, 'Kaire stogo puse', '', '2015-12-08', '2015-12-08', 7),
(12, 'Desine stogo puse', '', '2015-12-08', '2015-12-08', 8),
(13, 'Radiatoriai', '', '2015-12-08', '2015-12-08', 9),
(14, 'Sildomos grindys', 'Isvedzioti sildomas grindis', '2015-12-08', '2015-12-08', 9),
(15, 'Katiline', 'Pastatyti boileri', '2015-12-08', '2015-12-08', 11),
(16, 'Ventiliacija', 'Isvedzioti ventiliacijas.', '2015-12-08', '2015-12-08', 11),
(17, 'Pabaigti murinti', '', '2015-12-08', '2015-12-08', 10),
(18, 'Kondicionierius', 'Pakabinti, Pajungti', '2015-12-08', '2016-01-02', 9),
(19, 'Kriaukles', '', '2015-12-01', '2015-12-10', 9),
(20, 'Vonios', '', '2015-12-08', '2015-12-08', 9),
(21, 'Kranai', 'Suvedzioti ir prisukti', '2015-12-08', '2015-12-17', 9),
(22, 'Tualetas', '', '2015-12-08', '2015-12-08', 9),
(23, 'Kaminas', '', '2015-12-08', '2015-12-08', 10);

-- --------------------------------------------------------

--
-- Table structure for table `teams`
--

CREATE TABLE IF NOT EXISTS `teams` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Date` date NOT NULL,
  `Description` varchar(250) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=13 ;

--
-- Dumping data for table `teams`
--

INSERT INTO `teams` (`Id`, `Name`, `Date`, `Description`) VALUES
(10, 'Stogdengiai', '2015-12-08', 'Dengia stogus.'),
(11, 'Santechnikai', '2015-12-08', 'Daro santechniku dalykus'),
(12, 'Murininkai', '2015-12-08', 'Murina');

-- --------------------------------------------------------

--
-- Table structure for table `workers`
--

CREATE TABLE IF NOT EXISTS `workers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Surname` varchar(50) NOT NULL,
  `Born` date NOT NULL,
  `Joined` date NOT NULL,
  `Telephone` varchar(50) NOT NULL,
  `Address` varchar(50) NOT NULL,
  `TeamId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=12 ;

--
-- Dumping data for table `workers`
--

INSERT INTO `workers` (`Id`, `Name`, `Surname`, `Born`, `Joined`, `Telephone`, `Address`, `TeamId`) VALUES
(7, 'Petras', 'Petraitis', '2005-06-07', '2015-12-08', '516511313', 'Petro Namas 1', 10),
(8, 'Jonas', 'Jonaitis', '2005-06-07', '2015-12-08', '556565656', 'Jono Namas 1', 10),
(9, 'Kestas', 'Kestaitis', '2005-06-07', '2015-12-08', 'Kesto g 1', '555555555', 11),
(10, 'Marius', 'Marijonas', '2005-06-07', '2015-12-08', '66666665', 'Mariaus Garve 1', 12),
(11, 'Vytas', 'Vytaitis', '2015-12-08', '2015-12-08', 'Vyto g. 1', '8465123', 11);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
