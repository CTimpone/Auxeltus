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

