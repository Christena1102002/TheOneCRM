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

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [FullName] nvarchar(max) NULL,
    [imageUrl] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [chatMessagesChannels] (
    [Id] int NOT NULL IDENTITY,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_chatMessagesChannels] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [notifications] (
    [Id] int NOT NULL IDENTITY,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_notifications] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [pipelineStages] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Order] int NOT NULL,
    [Probability] int NOT NULL,
    [status] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_pipelineStages] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Activities] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [Type] int NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Activities] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Activities_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [leads] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NULL,
    [Phone] nvarchar(max) NULL,
    [Priority] int NOT NULL,
    [status] int NOT NULL,
    [IsActiveCustomer] bit NOT NULL,
    [CreatedById] nvarchar(450) NULL,
    [AssignedToId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_leads] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_leads_AspNetUsers_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_leads_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [supportTickets] (
    [Id] int NOT NULL IDENTITY,
    [TicketNumber] int NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Status] int NOT NULL,
    [priority] int NOT NULL,
    [CreatedById] nvarchar(450) NULL,
    [AssignedToId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_supportTickets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_supportTickets_AspNetUsers_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_supportTickets_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id])
);
GO

CREATE TABLE [deals] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    [statusOfDeal] int NOT NULL,
    [customerId] int NULL,
    [CreatedById] nvarchar(450) NULL,
    [PipelineStagesId] int NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_deals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_deals_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_deals_leads_customerId] FOREIGN KEY ([customerId]) REFERENCES [leads] ([Id]),
    CONSTRAINT [FK_deals_pipelineStages_PipelineStagesId] FOREIGN KEY ([PipelineStagesId]) REFERENCES [pipelineStages] ([Id])
);
GO

CREATE TABLE [Projects] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [status] int NOT NULL,
    [Price] int NOT NULL,
    [Start] datetime2 NOT NULL,
    [End] datetime2 NOT NULL,
    [customerId] int NOT NULL,
    [CreatedById] nvarchar(450) NULL,
    [AssignedToId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Projects_AspNetUsers_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Projects_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_Projects_leads_customerId] FOREIGN KEY ([customerId]) REFERENCES [leads] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [tasks] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Status] int NOT NULL,
    [OrderIndex] int NULL,
    [Description] nvarchar(max) NULL,
    [DueDate] datetime2 NOT NULL,
    [ProjectId] int NOT NULL,
    [CreatedById] nvarchar(450) NULL,
    [AssignedToId] nvarchar(450) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_tasks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_tasks_AspNetUsers_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_tasks_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_tasks_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Activities_UserId] ON [Activities] ([UserId]);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_deals_CreatedById] ON [deals] ([CreatedById]);
GO

CREATE INDEX [IX_deals_customerId] ON [deals] ([customerId]);
GO

CREATE INDEX [IX_deals_PipelineStagesId] ON [deals] ([PipelineStagesId]);
GO

CREATE INDEX [IX_leads_AssignedToId] ON [leads] ([AssignedToId]);
GO

CREATE INDEX [IX_leads_CreatedById] ON [leads] ([CreatedById]);
GO

CREATE INDEX [IX_Projects_AssignedToId] ON [Projects] ([AssignedToId]);
GO

CREATE INDEX [IX_Projects_CreatedById] ON [Projects] ([CreatedById]);
GO

CREATE INDEX [IX_Projects_customerId] ON [Projects] ([customerId]);
GO

CREATE INDEX [IX_supportTickets_AssignedToId] ON [supportTickets] ([AssignedToId]);
GO

CREATE INDEX [IX_supportTickets_CreatedById] ON [supportTickets] ([CreatedById]);
GO

CREATE INDEX [IX_tasks_AssignedToId] ON [tasks] ([AssignedToId]);
GO

CREATE INDEX [IX_tasks_CreatedById] ON [tasks] ([CreatedById]);
GO

CREATE INDEX [IX_tasks_ProjectId] ON [tasks] ([ProjectId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260329205608_intialdb', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [deals] DROP CONSTRAINT [FK_deals_leads_customerId];
GO

ALTER TABLE [leads] DROP CONSTRAINT [FK_leads_AspNetUsers_AssignedToId];
GO

ALTER TABLE [leads] DROP CONSTRAINT [FK_leads_AspNetUsers_CreatedById];
GO

ALTER TABLE [Projects] DROP CONSTRAINT [FK_Projects_leads_customerId];
GO

ALTER TABLE [leads] DROP CONSTRAINT [PK_leads];
GO

EXEC sp_rename N'[leads]', N'customers';
GO

EXEC sp_rename N'[customers].[IX_leads_CreatedById]', N'IX_customers_CreatedById', N'INDEX';
GO

EXEC sp_rename N'[customers].[IX_leads_AssignedToId]', N'IX_customers_AssignedToId', N'INDEX';
GO

ALTER TABLE [customers] ADD CONSTRAINT [PK_customers] PRIMARY KEY ([Id]);
GO

ALTER TABLE [customers] ADD CONSTRAINT [FK_customers_AspNetUsers_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [AspNetUsers] ([Id]);
GO

ALTER TABLE [customers] ADD CONSTRAINT [FK_customers_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]);
GO

ALTER TABLE [deals] ADD CONSTRAINT [FK_deals_customers_customerId] FOREIGN KEY ([customerId]) REFERENCES [customers] ([Id]);
GO

ALTER TABLE [Projects] ADD CONSTRAINT [FK_Projects_customers_customerId] FOREIGN KEY ([customerId]) REFERENCES [customers] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260329205710_intialdb-1', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [deals] DROP CONSTRAINT [FK_deals_AspNetUsers_CreatedById];
GO

ALTER TABLE [deals] ADD [AssignedToId] nvarchar(450) NULL;
GO

CREATE INDEX [IX_deals_AssignedToId] ON [deals] ([AssignedToId]);
GO

ALTER TABLE [deals] ADD CONSTRAINT [FK_deals_AspNetUsers_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [AspNetUsers] ([Id]);
GO

ALTER TABLE [deals] ADD CONSTRAINT [FK_deals_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260329205810_intialdb-2', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [customers] ADD [campaignsId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [customers] ADD [compaignId] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [campaigns] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Source] int NOT NULL,
    [Budget] decimal(18,2) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [AppUserId] nvarchar(450) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_campaigns] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_campaigns_AspNetUsers_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_customers_campaignsId] ON [customers] ([campaignsId]);
GO

CREATE INDEX [IX_campaigns_AppUserId] ON [campaigns] ([AppUserId]);
GO

ALTER TABLE [customers] ADD CONSTRAINT [FK_customers_campaigns_campaignsId] FOREIGN KEY ([campaignsId]) REFERENCES [campaigns] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260426104520_addedEntityCompaign', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [RefreshTokens] (
    [Id] int NOT NULL IDENTITY,
    [TokenHash] nvarchar(max) NOT NULL,
    [OwnerId] nvarchar(max) NOT NULL,
    [RemoteIpAddress] nvarchar(max) NULL,
    [ExpiresAt] datetime2 NOT NULL,
    [IsRevoked] bit NOT NULL,
    [RevokedAt] datetime2 NULL,
    [IsRememberMe] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260428074329_addedForignKey', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [RefreshTokens] ADD [ReplacedByTokenHash] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260428080138_addedForignKey1', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260428082132_addedForignKey2', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [RefreshTokens] ADD [DeviceInfo] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260428082340_addedForignKey3', N'8.0.0');
GO

COMMIT;
GO

