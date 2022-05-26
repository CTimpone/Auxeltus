IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220520201149_InitialJobsStructure')
BEGIN
    CREATE TABLE [Location] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Latitude] float NOT NULL,
        [Longitude] float NOT NULL,
        CONSTRAINT [PK_Location] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220520201149_InitialJobsStructure')
BEGIN
    CREATE TABLE [Role] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Tier] int NOT NULL,
        [MinimumSalary] int NOT NULL,
        [MaximumSalary] int NOT NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220520201149_InitialJobsStructure')
BEGIN
    CREATE TABLE [Job] (
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(max) NOT NULL,
        [Salary] float NULL,
        [EmployeeId] int NULL,
        [ReportingEmployeeId] int NOT NULL,
        [RoleId] int NOT NULL,
        [LocationId] int NOT NULL,
        CONSTRAINT [PK_Job] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Job_Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Location] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Job_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220520201149_InitialJobsStructure')
BEGIN
    CREATE INDEX [IX_Job_LocationId] ON [Job] ([LocationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220520201149_InitialJobsStructure')
BEGIN
    CREATE INDEX [IX_Job_RoleId] ON [Job] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220520201149_InitialJobsStructure')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220520201149_InitialJobsStructure', N'5.0.17');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524150711_JobContraints')
BEGIN
    ALTER TABLE [Job] DROP CONSTRAINT [FK_Job_Location_LocationId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524150711_JobContraints')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Job]') AND [c].[name] = N'LocationId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Job] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Job] ALTER COLUMN [LocationId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524150711_JobContraints')
BEGIN
    ALTER TABLE [Job] ADD [EmployeeType] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524150711_JobContraints')
BEGIN
    ALTER TABLE [Job] ADD [Remote] bit NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524150711_JobContraints')
BEGIN
    EXEC(N'ALTER TABLE [Job] ADD CONSTRAINT [CK_EmployeeHasSalary] CHECK (([EmployeeId] IS NULL AND [Salary] IS NULL) OR ([EmployeeId] IS NOT NULL AND [Salary] IS NOT NULL))');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524150711_JobContraints')
BEGIN
    EXEC(N'ALTER TABLE [Job] ADD CONSTRAINT [CK_EmployeeLocationSanity] CHECK ([LocationId] IS NOT NULL OR [Remote] = 1)');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524150711_JobContraints')
BEGIN
    ALTER TABLE [Job] ADD CONSTRAINT [FK_Job_Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Location] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524150711_JobContraints')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220524150711_JobContraints', N'5.0.17');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Job] DROP CONSTRAINT [FK_Job_Location_LocationId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Job] DROP CONSTRAINT [FK_Job_Role_RoleId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Role] DROP CONSTRAINT [PK_Role];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Location] DROP CONSTRAINT [PK_Location];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Job] DROP CONSTRAINT [PK_Job];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    EXEC sp_rename N'[Role]', N'Roles';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    EXEC sp_rename N'[Location]', N'Locations';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    EXEC sp_rename N'[Job]', N'Jobs';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    EXEC sp_rename N'[Jobs].[IX_Job_RoleId]', N'IX_Jobs_RoleId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    EXEC sp_rename N'[Jobs].[IX_Job_LocationId]', N'IX_Jobs_LocationId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'Title');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Roles] ALTER COLUMN [Title] nvarchar(100) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Jobs]') AND [c].[name] = N'Description');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Jobs] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Jobs] ALTER COLUMN [Description] nvarchar(2000) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Jobs] ADD [Archived] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Roles] ADD CONSTRAINT [PK_Roles] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Locations] ADD CONSTRAINT [PK_Locations] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Jobs] ADD CONSTRAINT [PK_Jobs] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Jobs] ADD CONSTRAINT [FK_Jobs_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    ALTER TABLE [Jobs] ADD CONSTRAINT [FK_Jobs_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524205033_AddJobArchival')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220524205033_AddJobArchival', N'5.0.17');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524210130_AddStringLengthToLocation')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Locations]') AND [c].[name] = N'Name');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Locations] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Locations] ALTER COLUMN [Name] nvarchar(100) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220524210130_AddStringLengthToLocation')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220524210130_AddStringLengthToLocation', N'5.0.17');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220525192836_JobRelatedUniquenessConstraints')
BEGIN
    CREATE UNIQUE INDEX [IX_Roles_Title] ON [Roles] ([Title]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220525192836_JobRelatedUniquenessConstraints')
BEGIN
    CREATE UNIQUE INDEX [IX_Locations_Latitude_Longitude] ON [Locations] ([Latitude], [Longitude]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220525192836_JobRelatedUniquenessConstraints')
BEGIN
    CREATE UNIQUE INDEX [IX_Locations_Name] ON [Locations] ([Name]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220525192836_JobRelatedUniquenessConstraints')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220525192836_JobRelatedUniquenessConstraints', N'5.0.17');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220525205852_AddSalaryType')
BEGIN
    ALTER TABLE [Jobs] ADD [SalaryType] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220525205852_AddSalaryType')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220525205852_AddSalaryType', N'5.0.17');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220526224601_FixJobRemoteCheckConstraint')
BEGIN
    ALTER TABLE [Jobs] DROP CONSTRAINT [CK_EmployeeLocationSanity];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220526224601_FixJobRemoteCheckConstraint')
BEGIN
    EXEC(N'ALTER TABLE [Jobs] ADD CONSTRAINT [CK_EmployeeLocationSanity] CHECK (([LocationId] IS NOT NULL AND [Remote] = 0) OR ([Remote] = 1 AND [LocationId] IS NULL))');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220526224601_FixJobRemoteCheckConstraint')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220526224601_FixJobRemoteCheckConstraint', N'5.0.17');
END;
GO

COMMIT;
GO

