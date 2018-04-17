
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/08/2016 23:51:42
-- Generated from EDMX file: G:\MiNI\Vsem\dotNET\client-desktop\client-desktop.Model\DatabaseModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DeveloperDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ProviderService]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceSet] DROP CONSTRAINT [FK_ProviderService];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceRegion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceSet] DROP CONSTRAINT [FK_ServiceRegion];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerServiceChoosed]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceChoosedSet] DROP CONSTRAINT [FK_CustomerServiceChoosed];
GO
IF OBJECT_ID(N'[dbo].[FK_ProviderServiceChoosed]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceChoosedSet] DROP CONSTRAINT [FK_ProviderServiceChoosed];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceServiceChoosed]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceChoosedSet] DROP CONSTRAINT [FK_ServiceServiceChoosed];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RateSet] DROP CONSTRAINT [FK_CustomerRate];
GO
IF OBJECT_ID(N'[dbo].[FK_ProviderRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RateSet] DROP CONSTRAINT [FK_ProviderRate];
GO
IF OBJECT_ID(N'[dbo].[FK_RateServiceChoosed]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceChoosedSet] DROP CONSTRAINT [FK_RateServiceChoosed];
GO
IF OBJECT_ID(N'[dbo].[FK_Provider_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSet_Provider] DROP CONSTRAINT [FK_Provider_inherits_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSet_Customer] DROP CONSTRAINT [FK_Customer_inherits_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Admin_inherits_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSet_Admin] DROP CONSTRAINT [FK_Admin_inherits_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[ServiceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceSet];
GO
IF OBJECT_ID(N'[dbo].[RegionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RegionSet];
GO
IF OBJECT_ID(N'[dbo].[ServiceChoosedSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceChoosedSet];
GO
IF OBJECT_ID(N'[dbo].[RateSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RateSet];
GO
IF OBJECT_ID(N'[dbo].[UserSet_Provider]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet_Provider];
GO
IF OBJECT_ID(N'[dbo].[UserSet_Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet_Customer];
GO
IF OBJECT_ID(N'[dbo].[UserSet_Admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet_Admin];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [SecondName] nvarchar(max)  NOT NULL,
    [Activated] bit  NOT NULL
);
GO

-- Creating table 'ServiceSet'
CREATE TABLE [dbo].[ServiceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [ProviderId] int  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [RegionId] int  NOT NULL
);
GO

-- Creating table 'RegionSet'
CREATE TABLE [dbo].[RegionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ServiceChoosedSet'
CREATE TABLE [dbo].[ServiceChoosedSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [ProviderId] int  NOT NULL,
    [ServiceId] int  NOT NULL,
    [Accepted] bit  NOT NULL,
    [CustomerNote] nvarchar(max)  NOT NULL,
    [FinishedByProvider] bit  NOT NULL,
    [FinishedByCustomer] bit  NOT NULL,
    [Rate_Id] int  NULL
);
GO

-- Creating table 'RateSet'
CREATE TABLE [dbo].[RateSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [ProviderId] int  NOT NULL,
    [Mark] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserSet_Provider'
CREATE TABLE [dbo].[UserSet_Provider] (
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'UserSet_Customer'
CREATE TABLE [dbo].[UserSet_Customer] (
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'UserSet_Admin'
CREATE TABLE [dbo].[UserSet_Admin] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceSet'
ALTER TABLE [dbo].[ServiceSet]
ADD CONSTRAINT [PK_ServiceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RegionSet'
ALTER TABLE [dbo].[RegionSet]
ADD CONSTRAINT [PK_RegionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceChoosedSet'
ALTER TABLE [dbo].[ServiceChoosedSet]
ADD CONSTRAINT [PK_ServiceChoosedSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RateSet'
ALTER TABLE [dbo].[RateSet]
ADD CONSTRAINT [PK_RateSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSet_Provider'
ALTER TABLE [dbo].[UserSet_Provider]
ADD CONSTRAINT [PK_UserSet_Provider]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSet_Customer'
ALTER TABLE [dbo].[UserSet_Customer]
ADD CONSTRAINT [PK_UserSet_Customer]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSet_Admin'
ALTER TABLE [dbo].[UserSet_Admin]
ADD CONSTRAINT [PK_UserSet_Admin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ProviderId] in table 'ServiceSet'
ALTER TABLE [dbo].[ServiceSet]
ADD CONSTRAINT [FK_ProviderService]
    FOREIGN KEY ([ProviderId])
    REFERENCES [dbo].[UserSet_Provider]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProviderService'
CREATE INDEX [IX_FK_ProviderService]
ON [dbo].[ServiceSet]
    ([ProviderId]);
GO

-- Creating foreign key on [RegionId] in table 'ServiceSet'
ALTER TABLE [dbo].[ServiceSet]
ADD CONSTRAINT [FK_ServiceRegion]
    FOREIGN KEY ([RegionId])
    REFERENCES [dbo].[RegionSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceRegion'
CREATE INDEX [IX_FK_ServiceRegion]
ON [dbo].[ServiceSet]
    ([RegionId]);
GO

-- Creating foreign key on [CustomerId] in table 'ServiceChoosedSet'
ALTER TABLE [dbo].[ServiceChoosedSet]
ADD CONSTRAINT [FK_CustomerServiceChoosed]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[UserSet_Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerServiceChoosed'
CREATE INDEX [IX_FK_CustomerServiceChoosed]
ON [dbo].[ServiceChoosedSet]
    ([CustomerId]);
GO

-- Creating foreign key on [ProviderId] in table 'ServiceChoosedSet'
ALTER TABLE [dbo].[ServiceChoosedSet]
ADD CONSTRAINT [FK_ProviderServiceChoosed]
    FOREIGN KEY ([ProviderId])
    REFERENCES [dbo].[UserSet_Provider]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProviderServiceChoosed'
CREATE INDEX [IX_FK_ProviderServiceChoosed]
ON [dbo].[ServiceChoosedSet]
    ([ProviderId]);
GO

-- Creating foreign key on [ServiceId] in table 'ServiceChoosedSet'
ALTER TABLE [dbo].[ServiceChoosedSet]
ADD CONSTRAINT [FK_ServiceServiceChoosed]
    FOREIGN KEY ([ServiceId])
    REFERENCES [dbo].[ServiceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceServiceChoosed'
CREATE INDEX [IX_FK_ServiceServiceChoosed]
ON [dbo].[ServiceChoosedSet]
    ([ServiceId]);
GO

-- Creating foreign key on [CustomerId] in table 'RateSet'
ALTER TABLE [dbo].[RateSet]
ADD CONSTRAINT [FK_CustomerRate]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[UserSet_Customer]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerRate'
CREATE INDEX [IX_FK_CustomerRate]
ON [dbo].[RateSet]
    ([CustomerId]);
GO

-- Creating foreign key on [ProviderId] in table 'RateSet'
ALTER TABLE [dbo].[RateSet]
ADD CONSTRAINT [FK_ProviderRate]
    FOREIGN KEY ([ProviderId])
    REFERENCES [dbo].[UserSet_Provider]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProviderRate'
CREATE INDEX [IX_FK_ProviderRate]
ON [dbo].[RateSet]
    ([ProviderId]);
GO

-- Creating foreign key on [Rate_Id] in table 'ServiceChoosedSet'
ALTER TABLE [dbo].[ServiceChoosedSet]
ADD CONSTRAINT [FK_RateServiceChoosed]
    FOREIGN KEY ([Rate_Id])
    REFERENCES [dbo].[RateSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RateServiceChoosed'
CREATE INDEX [IX_FK_RateServiceChoosed]
ON [dbo].[ServiceChoosedSet]
    ([Rate_Id]);
GO

-- Creating foreign key on [Id] in table 'UserSet_Provider'
ALTER TABLE [dbo].[UserSet_Provider]
ADD CONSTRAINT [FK_Provider_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'UserSet_Customer'
ALTER TABLE [dbo].[UserSet_Customer]
ADD CONSTRAINT [FK_Customer_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'UserSet_Admin'
ALTER TABLE [dbo].[UserSet_Admin]
ADD CONSTRAINT [FK_Admin_inherits_User]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------