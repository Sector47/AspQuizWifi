
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/08/2021 11:22:11
-- Generated from EDMX file: C:\Users\Sector47\source\repos\AspQuizWifi\Site\Models\DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [dbdev26];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__ANSWER__QUE_ID__5006DFF2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ANSWER] DROP CONSTRAINT [FK__ANSWER__QUE_ID__5006DFF2];
GO
IF OBJECT_ID(N'[dbo].[FK__GRADE__COURSE_QU__5F492382]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GRADE] DROP CONSTRAINT [FK__GRADE__COURSE_QU__5F492382];
GO
IF OBJECT_ID(N'[dbo].[FK__RESPONSE__COURSE__603D47BB]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RESPONSE] DROP CONSTRAINT [FK__RESPONSE__COURSE__603D47BB];
GO
IF OBJECT_ID(N'[dbo].[FK__RESPONSE__QUE_ID__50FB042B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RESPONSE] DROP CONSTRAINT [FK__RESPONSE__QUE_ID__50FB042B];
GO
IF OBJECT_ID(N'[dbo].[FK__ROSTER__COURSE_I__4959E263]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ROSTER] DROP CONSTRAINT [FK__ROSTER__COURSE_I__4959E263];
GO
IF OBJECT_ID(N'[dbo].[FK_COURSE_QUIZ_COURSE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[COURSE_QUIZ] DROP CONSTRAINT [FK_COURSE_QUIZ_COURSE];
GO
IF OBJECT_ID(N'[dbo].[FK_COURSE_QUIZ_QUIZ]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[COURSE_QUIZ] DROP CONSTRAINT [FK_COURSE_QUIZ_QUIZ];
GO
IF OBJECT_ID(N'[dbo].[FK_QUESTION_TYP]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QUESTION] DROP CONSTRAINT [FK_QUESTION_TYP];
GO
IF OBJECT_ID(N'[dbo].[FK_QUI_QUE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QUESTION] DROP CONSTRAINT [FK_QUI_QUE];
GO
IF OBJECT_ID(N'[dbo].[FK_ROSTER_USERS]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ROSTER] DROP CONSTRAINT [FK_ROSTER_USERS];
GO
IF OBJECT_ID(N'[dbo].[fk_user_Response]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RESPONSE] DROP CONSTRAINT [fk_user_Response];
GO
IF OBJECT_ID(N'[dbo].[FK_USER_ID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GRADE] DROP CONSTRAINT [FK_USER_ID];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ANSWER]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ANSWER];
GO
IF OBJECT_ID(N'[dbo].[COURSE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[COURSE];
GO
IF OBJECT_ID(N'[dbo].[COURSE_QUIZ]', 'U') IS NOT NULL
    DROP TABLE [dbo].[COURSE_QUIZ];
GO
IF OBJECT_ID(N'[dbo].[GRADE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GRADE];
GO
IF OBJECT_ID(N'[dbo].[QUESTION]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QUESTION];
GO
IF OBJECT_ID(N'[dbo].[QUIZ]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QUIZ];
GO
IF OBJECT_ID(N'[dbo].[RESPONSE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RESPONSE];
GO
IF OBJECT_ID(N'[dbo].[ROSTER]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ROSTER];
GO
IF OBJECT_ID(N'[dbo].[TYPE]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TYPE];
GO
IF OBJECT_ID(N'[dbo].[USERS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[USERS];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ANSWERs'
CREATE TABLE [dbo].[ANSWERs] (
    [QUE_ID] int  NOT NULL,
    [DESCRIPTION] varchar(max)  NOT NULL,
    [CORRECT_ANS] bit  NOT NULL,
    [ANS_ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'COURSEs'
CREATE TABLE [dbo].[COURSEs] (
    [COU_NAME] varchar(45)  NOT NULL,
    [COU_SEM] varchar(45)  NULL,
    [COU_YEAR] int  NULL,
    [COU_START_DATE] datetime  NULL,
    [COU_END_DATE] datetime  NULL,
    [COURSE_ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'COURSE_QUIZ'
CREATE TABLE [dbo].[COURSE_QUIZ] (
    [QUI_ID] int  NOT NULL,
    [COURSE_ID] int  NOT NULL,
    [COURSE_QUI_ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'GRADEs'
CREATE TABLE [dbo].[GRADEs] (
    [GRA_ID] int IDENTITY(1,1) NOT NULL,
    [USER_ID] int  NOT NULL,
    [COURSE_QUI_ID] int  NOT NULL,
    [GRA_GRADE] int  NOT NULL,
    [GRA_NEEDSGRADING] bit  NOT NULL
);
GO

-- Creating table 'QUESTIONs'
CREATE TABLE [dbo].[QUESTIONs] (
    [QUI_ID] int  NOT NULL,
    [QUE_QUESTION] varchar(max)  NOT NULL,
    [TYPE_ID] nvarchar(50)  NOT NULL,
    [QUE_ID] int IDENTITY(1,1) NOT NULL,
    [QUESTION_ANSWER] varchar(max)  NULL
);
GO

-- Creating table 'QUIZs'
CREATE TABLE [dbo].[QUIZs] (
    [QUI_NAME] varchar(45)  NULL,
    [QUI_NOTES] varchar(200)  NULL,
    [QUI_ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'RESPONSEs'
CREATE TABLE [dbo].[RESPONSEs] (
    [QUE_ID] int  NOT NULL,
    [COMMENTS] varchar(max)  NULL,
    [USER_ID] int  NOT NULL,
    [COURSE_QUI_ID] int  NOT NULL,
    [RESPONSE_ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'ROSTERs'
CREATE TABLE [dbo].[ROSTERs] (
    [USER_ID] int  NOT NULL,
    [COURSE_ID] int  NOT NULL,
    [ROSTER_ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'TYPEs'
CREATE TABLE [dbo].[TYPEs] (
    [TYPE_ID] nvarchar(50)  NOT NULL,
    [TYPE_NAME] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'USERS'
CREATE TABLE [dbo].[USERS] (
    [F_NAME] varchar(15)  NULL,
    [L_NAME] varchar(25)  NULL,
    [IS_INSTRUCTOR] bit  NOT NULL,
    [USERNAME] varchar(25)  NOT NULL,
    [PASSWORD] varchar(25)  NOT NULL,
    [SESSION_ID] varchar(25)  NULL,
    [USER_ID] int IDENTITY(1,1) NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ANS_ID] in table 'ANSWERs'
ALTER TABLE [dbo].[ANSWERs]
ADD CONSTRAINT [PK_ANSWERs]
    PRIMARY KEY CLUSTERED ([ANS_ID] ASC);
GO

-- Creating primary key on [COURSE_ID] in table 'COURSEs'
ALTER TABLE [dbo].[COURSEs]
ADD CONSTRAINT [PK_COURSEs]
    PRIMARY KEY CLUSTERED ([COURSE_ID] ASC);
GO

-- Creating primary key on [COURSE_QUI_ID] in table 'COURSE_QUIZ'
ALTER TABLE [dbo].[COURSE_QUIZ]
ADD CONSTRAINT [PK_COURSE_QUIZ]
    PRIMARY KEY CLUSTERED ([COURSE_QUI_ID] ASC);
GO

-- Creating primary key on [GRA_ID] in table 'GRADEs'
ALTER TABLE [dbo].[GRADEs]
ADD CONSTRAINT [PK_GRADEs]
    PRIMARY KEY CLUSTERED ([GRA_ID] ASC);
GO

-- Creating primary key on [QUE_ID] in table 'QUESTIONs'
ALTER TABLE [dbo].[QUESTIONs]
ADD CONSTRAINT [PK_QUESTIONs]
    PRIMARY KEY CLUSTERED ([QUE_ID] ASC);
GO

-- Creating primary key on [QUI_ID] in table 'QUIZs'
ALTER TABLE [dbo].[QUIZs]
ADD CONSTRAINT [PK_QUIZs]
    PRIMARY KEY CLUSTERED ([QUI_ID] ASC);
GO

-- Creating primary key on [RESPONSE_ID] in table 'RESPONSEs'
ALTER TABLE [dbo].[RESPONSEs]
ADD CONSTRAINT [PK_RESPONSEs]
    PRIMARY KEY CLUSTERED ([RESPONSE_ID] ASC);
GO

-- Creating primary key on [ROSTER_ID] in table 'ROSTERs'
ALTER TABLE [dbo].[ROSTERs]
ADD CONSTRAINT [PK_ROSTERs]
    PRIMARY KEY CLUSTERED ([ROSTER_ID] ASC);
GO

-- Creating primary key on [TYPE_ID] in table 'TYPEs'
ALTER TABLE [dbo].[TYPEs]
ADD CONSTRAINT [PK_TYPEs]
    PRIMARY KEY CLUSTERED ([TYPE_ID] ASC);
GO

-- Creating primary key on [USER_ID] in table 'USERS'
ALTER TABLE [dbo].[USERS]
ADD CONSTRAINT [PK_USERS]
    PRIMARY KEY CLUSTERED ([USER_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [QUE_ID] in table 'ANSWERs'
ALTER TABLE [dbo].[ANSWERs]
ADD CONSTRAINT [FK__ANSWER__QUE_ID__5006DFF2]
    FOREIGN KEY ([QUE_ID])
    REFERENCES [dbo].[QUESTIONs]
        ([QUE_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ANSWER__QUE_ID__5006DFF2'
CREATE INDEX [IX_FK__ANSWER__QUE_ID__5006DFF2]
ON [dbo].[ANSWERs]
    ([QUE_ID]);
GO

-- Creating foreign key on [COURSE_ID] in table 'ROSTERs'
ALTER TABLE [dbo].[ROSTERs]
ADD CONSTRAINT [FK__ROSTER__COURSE_I__4959E263]
    FOREIGN KEY ([COURSE_ID])
    REFERENCES [dbo].[COURSEs]
        ([COURSE_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__ROSTER__COURSE_I__4959E263'
CREATE INDEX [IX_FK__ROSTER__COURSE_I__4959E263]
ON [dbo].[ROSTERs]
    ([COURSE_ID]);
GO

-- Creating foreign key on [COURSE_ID] in table 'COURSE_QUIZ'
ALTER TABLE [dbo].[COURSE_QUIZ]
ADD CONSTRAINT [FK_COURSE_QUIZ_COURSE]
    FOREIGN KEY ([COURSE_ID])
    REFERENCES [dbo].[COURSEs]
        ([COURSE_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_COURSE_QUIZ_COURSE'
CREATE INDEX [IX_FK_COURSE_QUIZ_COURSE]
ON [dbo].[COURSE_QUIZ]
    ([COURSE_ID]);
GO

-- Creating foreign key on [COURSE_QUI_ID] in table 'GRADEs'
ALTER TABLE [dbo].[GRADEs]
ADD CONSTRAINT [FK__GRADE__COURSE_QU__5F492382]
    FOREIGN KEY ([COURSE_QUI_ID])
    REFERENCES [dbo].[COURSE_QUIZ]
        ([COURSE_QUI_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__GRADE__COURSE_QU__5F492382'
CREATE INDEX [IX_FK__GRADE__COURSE_QU__5F492382]
ON [dbo].[GRADEs]
    ([COURSE_QUI_ID]);
GO

-- Creating foreign key on [COURSE_QUI_ID] in table 'RESPONSEs'
ALTER TABLE [dbo].[RESPONSEs]
ADD CONSTRAINT [FK__RESPONSE__COURSE__603D47BB]
    FOREIGN KEY ([COURSE_QUI_ID])
    REFERENCES [dbo].[COURSE_QUIZ]
        ([COURSE_QUI_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__RESPONSE__COURSE__603D47BB'
CREATE INDEX [IX_FK__RESPONSE__COURSE__603D47BB]
ON [dbo].[RESPONSEs]
    ([COURSE_QUI_ID]);
GO

-- Creating foreign key on [QUI_ID] in table 'COURSE_QUIZ'
ALTER TABLE [dbo].[COURSE_QUIZ]
ADD CONSTRAINT [FK_COURSE_QUIZ_QUIZ]
    FOREIGN KEY ([QUI_ID])
    REFERENCES [dbo].[QUIZs]
        ([QUI_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_COURSE_QUIZ_QUIZ'
CREATE INDEX [IX_FK_COURSE_QUIZ_QUIZ]
ON [dbo].[COURSE_QUIZ]
    ([QUI_ID]);
GO

-- Creating foreign key on [USER_ID] in table 'GRADEs'
ALTER TABLE [dbo].[GRADEs]
ADD CONSTRAINT [FK_USER_ID]
    FOREIGN KEY ([USER_ID])
    REFERENCES [dbo].[USERS]
        ([USER_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_USER_ID'
CREATE INDEX [IX_FK_USER_ID]
ON [dbo].[GRADEs]
    ([USER_ID]);
GO

-- Creating foreign key on [QUE_ID] in table 'RESPONSEs'
ALTER TABLE [dbo].[RESPONSEs]
ADD CONSTRAINT [FK__RESPONSE__QUE_ID__50FB042B]
    FOREIGN KEY ([QUE_ID])
    REFERENCES [dbo].[QUESTIONs]
        ([QUE_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__RESPONSE__QUE_ID__50FB042B'
CREATE INDEX [IX_FK__RESPONSE__QUE_ID__50FB042B]
ON [dbo].[RESPONSEs]
    ([QUE_ID]);
GO

-- Creating foreign key on [TYPE_ID] in table 'QUESTIONs'
ALTER TABLE [dbo].[QUESTIONs]
ADD CONSTRAINT [FK_QUESTION_TYP]
    FOREIGN KEY ([TYPE_ID])
    REFERENCES [dbo].[TYPEs]
        ([TYPE_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QUESTION_TYP'
CREATE INDEX [IX_FK_QUESTION_TYP]
ON [dbo].[QUESTIONs]
    ([TYPE_ID]);
GO

-- Creating foreign key on [QUI_ID] in table 'QUESTIONs'
ALTER TABLE [dbo].[QUESTIONs]
ADD CONSTRAINT [FK_QUI_QUE]
    FOREIGN KEY ([QUI_ID])
    REFERENCES [dbo].[QUIZs]
        ([QUI_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QUI_QUE'
CREATE INDEX [IX_FK_QUI_QUE]
ON [dbo].[QUESTIONs]
    ([QUI_ID]);
GO

-- Creating foreign key on [USER_ID] in table 'RESPONSEs'
ALTER TABLE [dbo].[RESPONSEs]
ADD CONSTRAINT [fk_user_Response]
    FOREIGN KEY ([USER_ID])
    REFERENCES [dbo].[USERS]
        ([USER_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'fk_user_Response'
CREATE INDEX [IX_fk_user_Response]
ON [dbo].[RESPONSEs]
    ([USER_ID]);
GO

-- Creating foreign key on [USER_ID] in table 'ROSTERs'
ALTER TABLE [dbo].[ROSTERs]
ADD CONSTRAINT [FK_ROSTER_USERS]
    FOREIGN KEY ([USER_ID])
    REFERENCES [dbo].[USERS]
        ([USER_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ROSTER_USERS'
CREATE INDEX [IX_FK_ROSTER_USERS]
ON [dbo].[ROSTERs]
    ([USER_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------