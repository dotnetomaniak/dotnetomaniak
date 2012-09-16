--
-- Create schema kigg
--

CREATE DATABASE IF NOT EXISTS kigg;
USE kigg;

--
-- Definition of table `category`
--

DROP TABLE IF EXISTS `category`;
CREATE TABLE `category` (
  `Id` binary(16) NOT NULL,
  `UniqueName` varchar(64) NOT NULL,
  `Name` varchar(64) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Category_UniqueName_CreatedAt` (`UniqueName`,`CreatedAt`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `commentsubscribtion`
--

DROP TABLE IF EXISTS `commentsubscribtion`;
CREATE TABLE `commentsubscribtion` (
  `StoryId` binary(16) NOT NULL,
  `UserId` binary(16) NOT NULL,
  PRIMARY KEY (`StoryId`,`UserId`),
  KEY `FK_CommentSubscribtion_User` (`UserId`),
  CONSTRAINT `FK_CommentSubscribtion_Story` FOREIGN KEY (`StoryId`) REFERENCES `story` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_CommentSubscribtion_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `fulltextsearchcomment`
--

DROP TABLE IF EXISTS `fulltextsearchcomment`;
CREATE TABLE `fulltextsearchcomment` (
  `Id` binary(16) NOT NULL,
  `TextBody` mediumtext NOT NULL,
  PRIMARY KEY (`Id`),
  FULLTEXT KEY `IX_FullText_TextBody` (`TextBody`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Definition of table `fulltextsearchstory`
--

DROP TABLE IF EXISTS `fulltextsearchstory`;
CREATE TABLE `fulltextsearchstory` (
  `Id` binary(16) NOT NULL,
  `TextDescription` mediumtext NOT NULL,
  `Title` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`),
  FULLTEXT KEY `IX_FULLTEXT` (`Title`,`TextDescription`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Definition of table `knownsource`
--

DROP TABLE IF EXISTS `knownsource`;
CREATE TABLE `knownsource` (
  `Url` varchar(450) NOT NULL,
  `Grade` int(10) NOT NULL,
  PRIMARY KEY (`Url`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `story`
--

DROP TABLE IF EXISTS `story`;
CREATE TABLE `story` (
  `Id` binary(16) NOT NULL,
  `UniqueName` varchar(256) NOT NULL,
  `Title` varchar(256) NOT NULL,
  `HtmlDescription` longtext NOT NULL,
  `TextDescription` mediumtext NOT NULL,
  `Url` varchar(2048) NOT NULL,
  `UrlHash` char(24) NOT NULL,
  `CategoryId` binary(16) NOT NULL,
  `UserId` binary(16) NOT NULL,
  `IPAddress` varchar(15) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `LastActivityAt` datetime NOT NULL,
  `ApprovedAt` datetime DEFAULT NULL,
  `PublishedAt` datetime DEFAULT NULL,
  `Rank` int(10) DEFAULT NULL,
  `LastProcessedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Story_UniqueName` (`UniqueName`),
  UNIQUE KEY `IX_Story_UrlHash` (`UrlHash`),
  KEY `IX_Story_ApprovedAt_PublishedAt` (`ApprovedAt`,`PublishedAt`,`Rank`,`CreatedAt`,`LastActivityAt`),
  KEY `IX_Story_CategoryId` (`CategoryId`),
  KEY `IX_Story_LastProcessedAt` (`LastProcessedAt`),
  KEY `IX_Story_UserId` (`UserId`),
  CONSTRAINT `FK_Story_Category` FOREIGN KEY (`CategoryId`) REFERENCES `category` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_Story_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `insertfulltextsearchstory`
--

CREATE TRIGGER `insertfulltextsearchstory` AFTER INSERT ON `story` FOR EACH ROW INSERT INTO fulltextsearchstory VALUES(NEW.Id, NEW.TextDescription, NEW.Title);

--
-- Definition of trigger `updatefulltextsearchstory`
--

CREATE TRIGGER `updatefulltextsearchstory` AFTER UPDATE ON `story` FOR EACH ROW UPDATE fulltextsearchstory set TextDescription=NEW.TextDescription, Title=NEW.Title where hex(Id) = hex(OLD.Id);

--
-- Definition of trigger `deletefulltextsearchstory`
--

CREATE TRIGGER `deletefulltextsearchstory` AFTER DELETE ON `story` FOR EACH ROW DELETE FROM fulltextsearchstory where hex(Id) = hex(OLD.Id);

--
-- Definition of table `storycomment`
--

DROP TABLE IF EXISTS `storycomment`;
CREATE TABLE `storycomment` (
  `Id` binary(16) NOT NULL,
  `HtmlBody` mediumtext NOT NULL,
  `TextBody` mediumtext NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `StoryId` binary(16) NOT NULL,
  `UserId` binary(16) NOT NULL,
  `IPAddress` varchar(15) NOT NULL,
  `IsOffended` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_StoryComment_StoryId_Created` (`StoryId`,`CreatedAt`),
  KEY `IX_StoryComment_UserId` (`UserId`),
  CONSTRAINT `FK_StoryComment_Story` FOREIGN KEY (`StoryId`) REFERENCES `story` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_StoryComment_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of trigger `insertfulltextsearchcomment`
--

CREATE TRIGGER `insertfulltextsearchcomment` AFTER INSERT ON `storycomment` FOR EACH ROW INSERT INTO fulltextsearchcomment VALUES(NEW.Id, NEW.TextBody);

--
-- Definition of trigger `updatefulltextsearchcomment`
--

CREATE TRIGGER `updatefulltextsearchcomment` AFTER UPDATE ON `storycomment` FOR EACH ROW UPDATE fulltextsearchcomment set TextBody=NEW.TextBody where hex(Id) = hex(OLD.Id);

--
-- Definition of trigger `deletefulltextsearchcomment`
--

CREATE TRIGGER `deletefulltextsearchcomment` AFTER DELETE ON `storycomment` FOR EACH ROW DELETE FROM fulltextsearchcomment where hex(Id) = hex(OLD.Id);
--
-- Definition of table `storymarkasspam`
--

DROP TABLE IF EXISTS `storymarkasspam`;
CREATE TABLE `storymarkasspam` (
  `StoryId` binary(16) NOT NULL,
  `UserId` binary(16) NOT NULL,
  `IPAddress` varchar(15) NOT NULL,
  `Timestamp` datetime NOT NULL,
  PRIMARY KEY (`StoryId`,`UserId`),
  KEY `FK_StoryMarkAsSpam_User` (`UserId`),
  CONSTRAINT `FK_StoryMarkAsSpam_Story` FOREIGN KEY (`StoryId`) REFERENCES `story` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_StoryMarkAsSpam_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `storytag`
--

DROP TABLE IF EXISTS `storytag`;
CREATE TABLE `storytag` (
  `StoryId` binary(16) NOT NULL,
  `TagId` binary(16) NOT NULL,
  PRIMARY KEY (`StoryId`,`TagId`),
  KEY `FK_StoryTag_Tag` (`TagId`),
  CONSTRAINT `FK_StoryTag_Story` FOREIGN KEY (`StoryId`) REFERENCES `story` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_StoryTag_Tag` FOREIGN KEY (`TagId`) REFERENCES `tag` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `storyview`
--

DROP TABLE IF EXISTS `storyview`;
CREATE TABLE `storyview` (
  `Id` bigint(19) NOT NULL AUTO_INCREMENT,
  `StoryId` binary(16) NOT NULL,
  `Timestamp` datetime NOT NULL,
  `IPAddress` varchar(15) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_StoryView_StoryId_Timestamp_` (`StoryId`,`Timestamp`,`IPAddress`),
  CONSTRAINT `FK_StoryView_Story` FOREIGN KEY (`StoryId`) REFERENCES `story` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=424 DEFAULT CHARSET=latin1;

--
-- Definition of table `storyvote`
--

DROP TABLE IF EXISTS `storyvote`;
CREATE TABLE `storyvote` (
  `StoryId` binary(16) NOT NULL,
  `UserId` binary(16) NOT NULL,
  `IPAddress` varchar(15) NOT NULL,
  `Timestamp` datetime NOT NULL,
  PRIMARY KEY (`StoryId`,`UserId`),
  KEY `FK_StoryVote_User` (`UserId`),
  CONSTRAINT `FK_StoryVote_Story` FOREIGN KEY (`StoryId`) REFERENCES `story` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_StoryVote_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `tag`
--

DROP TABLE IF EXISTS `tag`;
CREATE TABLE `tag` (
  `Id` binary(16) NOT NULL,
  `UniqueName` varchar(64) NOT NULL,
  `Name` varchar(64) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Tag_Name` (`Name`),
  KEY `IX_Tag_UniqueName_CreatedAat` (`UniqueName`,`CreatedAt`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `Id` binary(16) NOT NULL,
  `UserName` varchar(256) NOT NULL,
  `Password` varchar(64) DEFAULT NULL,
  `Email` varchar(256) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `IsLockedOut` tinyint(1) NOT NULL,
  `Role` int(10) NOT NULL,
  `LastActivityAt` datetime NOT NULL,
  `CreatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_User_Email` (`Email`),
  KEY `IX_User_LastActivityAt` (`LastActivityAt`),
  KEY `IX_User_Role` (`Role`),
  KEY `IX_User_UserName` (`UserName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of table `userscore`
--

DROP TABLE IF EXISTS `userscore`;
CREATE TABLE `userscore` (
  `Id` bigint(19) NOT NULL AUTO_INCREMENT,
  `UserId` binary(16) NOT NULL,
  `Timestamp` datetime NOT NULL,
  `ActionType` int(10) NOT NULL,
  `Score` decimal(5,2) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserScore_UserId_Timestamp_S` (`UserId`,`Timestamp`,`Score`),
  CONSTRAINT `FK_UserScore_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=6237 DEFAULT CHARSET=latin1;

--
-- Definition of table `usertag`
--

DROP TABLE IF EXISTS `usertag`;
CREATE TABLE `usertag` (
  `UserId` binary(16) NOT NULL,
  `TagId` binary(16) NOT NULL,
  PRIMARY KEY (`UserId`,`TagId`),
  KEY `FK_UserTag_Tag` (`TagId`),
  CONSTRAINT `FK_UserTag_Tag` FOREIGN KEY (`TagId`) REFERENCES `tag` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_UserTag_User` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Definition of function `EFSearchComment`
--

CREATE FUNCTION `EFSearchComment`(
        storyId binary(16),
        srchQry varchar(1000)
) RETURNS tinyint(1)
BEGIN
  declare rows_count int;
  select count(ft.Id) into rows_count
  from fulltextsearchcomment ft
  inner join storycomment on ft.Id= storycomment.Id
  where storycomment.StoryId = storyId
  and match(ft.TextBody) against (srchQry WITH QUERY EXPANSION);
  if rows_count > 0 then
     return 1;
  end if;
  return 0;
END;

--
-- Definition of function `EFSearchStory`
--

CREATE FUNCTION `EFSearchStory`(
        storyId binary(16),
        srchQry varchar(1000)
) RETURNS tinyint(1)
BEGIN
  declare rows_count int;
  select count(ft.Id) into rows_count
  from fulltextsearchstory ft
  inner join story on ft.Id= story.Id
  where ft.Id = storyId
  and match(ft.Title, ft.TextDescription) against (srchQry WITH QUERY EXPANSION);

  if rows_count > 0 then
     return 1;
  end if;
  return 0;
END;
