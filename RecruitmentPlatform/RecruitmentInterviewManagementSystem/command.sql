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

CREATE TABLE [Companies] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [Name] nvarchar(255) NULL,
    [TaxCode] nvarchar(50) NULL,
    [Address] nvarchar(255) NULL,
    [Website] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [LogoUrl] nvarchar(500) NULL,
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Companie__3214EC07BB89D494] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ServicePackages] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [Name] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [Price] decimal(18,2) NULL,
    [DurationDays] int NULL,
    [MaxPost] int NULL,
    [IsActive] bit NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__ServiceP__3214EC07AFF571BD] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Skills] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [Name] nvarchar(100) NULL,
    CONSTRAINT [PK__Skills__3214EC0767E0594D] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [Email] nvarchar(255) NOT NULL,
    [PasswordHash] nvarchar(500) NOT NULL,
    [FullName] nvarchar(255) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [Role] int NULL,
    [IsActive] bit NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [Salt] nvarchar(255) NOT NULL DEFAULT N'default_salt_value',
    CONSTRAINT [PK__Users__3214EC074C9A6D4F] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [JobPosts] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CompanyId] uniqueidentifier NOT NULL,
    [Title] nvarchar(255) NULL,
    [Description] nvarchar(max) NULL,
    [Requirement] nvarchar(max) NULL,
    [Benefit] nvarchar(max) NULL,
    [SalaryMin] decimal(18,2) NULL,
    [SalaryMax] decimal(18,2) NULL,
    [Location] nvarchar(255) NULL,
    [JobType] int NULL,
    [ExpireAt] datetime NULL,
    [IsActive] bit NULL DEFAULT CAST(1 AS bit),
    [ViewCount] int NULL DEFAULT 0,
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__JobPosts__3214EC07FC730E1F] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Job_Company] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id])
);
GO

CREATE TABLE [CompanySubscriptions] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CompanyId] uniqueidentifier NOT NULL,
    [ServicePackageId] uniqueidentifier NOT NULL,
    [StartDate] datetime NULL,
    [EndDate] datetime NULL,
    [RemainingPost] int NULL,
    [IsActive] bit NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK__CompanyS__3214EC074F42971F] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__CompanySu__Compa__2FCF1A8A] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]),
    CONSTRAINT [FK__CompanySu__Servi__30C33EC3] FOREIGN KEY ([ServicePackageId]) REFERENCES [ServicePackages] ([Id])
);
GO

CREATE TABLE [CandidateProfiles] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [UserId] uniqueidentifier NOT NULL,
    [DateOfBirth] date NULL,
    [Gender] int NULL,
    [Address] nvarchar(255) NULL,
    [ExperienceYears] int NULL,
    [CurrentSalary] decimal(18,2) NULL,
    [DesiredSalary] decimal(18,2) NULL,
    [JobLevel] nvarchar(100) NULL,
    [Summary] nvarchar(max) NULL,
    CONSTRAINT [PK__Candidat__3214EC071E63B6B1] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Candidate_User] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [EmployerProfiles] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [UserId] uniqueidentifier NOT NULL,
    [CompanyId] uniqueidentifier NOT NULL,
    [Position] nvarchar(255) NULL,
    CONSTRAINT [PK__Employer__3214EC07541EB4AD] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employer_Company] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]),
    CONSTRAINT [FK_Employer_User] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [RefreshTokens] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [UserId] uniqueidentifier NOT NULL,
    [Token] nvarchar(500) NOT NULL,
    [JwtId] nvarchar(255) NULL,
    [IsUsed] bit NULL DEFAULT CAST(0 AS bit),
    [IsRevoked] bit NULL DEFAULT CAST(0 AS bit),
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [ExpiredAt] datetime NOT NULL,
    [RevokedAt] datetime NULL,
    [CreatedByIp] nvarchar(100) NULL,
    [RevokedByIp] nvarchar(100) NULL,
    [DeviceInfo] nvarchar(500) NULL,
    CONSTRAINT [PK__RefreshT__3214EC07A8E2136C] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshToken_User] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [JobSkills] (
    [JobId] uniqueidentifier NOT NULL,
    [SkillId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK__JobSkill__689C99DA801B13C1] PRIMARY KEY ([JobId], [SkillId]),
    CONSTRAINT [FK__JobSkills__JobId__6EF57B66] FOREIGN KEY ([JobId]) REFERENCES [JobPosts] ([Id]),
    CONSTRAINT [FK__JobSkills__Skill__6FE99F9F] FOREIGN KEY ([SkillId]) REFERENCES [Skills] ([Id])
);
GO

CREATE TABLE [CandidateSkills] (
    [CandidateId] uniqueidentifier NOT NULL,
    [SkillId] uniqueidentifier NOT NULL,
    [Level] int NULL,
    CONSTRAINT [PK__Candidat__B2A99284D1268521] PRIMARY KEY ([CandidateId], [SkillId]),
    CONSTRAINT [FK__Candidate__Candi__72C60C4A] FOREIGN KEY ([CandidateId]) REFERENCES [CandidateProfiles] ([Id]),
    CONSTRAINT [FK__Candidate__Skill__73BA3083] FOREIGN KEY ([SkillId]) REFERENCES [Skills] ([Id])
);
GO

CREATE TABLE [CVs] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CandidateId] uniqueidentifier NOT NULL,
    [FullName] nvarchar(255) NOT NULL,
    [Email] nvarchar(255) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [Address] nvarchar(255) NULL,
    [Birthday] date NULL,
    [Gender] int NULL,
    [Nationality] nvarchar(100) NULL,
    [Position] nvarchar(255) NULL,
    [ExperienceYears] int NULL,
    [EducationSummary] nvarchar(255) NULL,
    [Field] nvarchar(255) NULL,
    [CurrentSalary] decimal(18,2) NULL,
    [FileName] nvarchar(255) NULL,
    [FileUrl] nvarchar(500) NULL,
    [MimeType] nvarchar(100) NULL,
    [IsDefault] bit NULL DEFAULT CAST(0 AS bit),
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [UpdatedAt] datetime NULL,
    CONSTRAINT [PK__CVs__3214EC074E6E5E70] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__CVs__CandidateId__797309D9] FOREIGN KEY ([CandidateId]) REFERENCES [CandidateProfiles] ([Id])
);
GO

CREATE TABLE [SavedJobs] (
    [CandidateId] uniqueidentifier NOT NULL,
    [JobId] uniqueidentifier NOT NULL,
    [SavedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__SavedJob__EF05F290E9B5F635] PRIMARY KEY ([CandidateId], [JobId]),
    CONSTRAINT [FK__SavedJobs__Candi__17036CC0] FOREIGN KEY ([CandidateId]) REFERENCES [CandidateProfiles] ([Id]),
    CONSTRAINT [FK__SavedJobs__JobId__17F790F9] FOREIGN KEY ([JobId]) REFERENCES [JobPosts] ([Id])
);
GO

CREATE TABLE [Orders] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [OrderCode] nvarchar(50) NULL,
    [EmployerId] uniqueidentifier NOT NULL,
    [TotalAmount] decimal(18,2) NULL,
    [Status] int NULL,
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [PaidAt] datetime NULL,
    CONSTRAINT [PK__Orders__3214EC07ABC2DDDE] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__Orders__Employer__22751F6C] FOREIGN KEY ([EmployerId]) REFERENCES [EmployerProfiles] ([Id])
);
GO

CREATE TABLE [Applications] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [JobId] uniqueidentifier NOT NULL,
    [CandidateId] uniqueidentifier NOT NULL,
    [CVId] uniqueidentifier NOT NULL,
    [Status] int NULL,
    [AppliedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Applicat__3214EC07582676B4] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__Applicati__Candi__123EB7A3] FOREIGN KEY ([CandidateId]) REFERENCES [CandidateProfiles] ([Id]),
    CONSTRAINT [FK__Applicati__JobId__114A936A] FOREIGN KEY ([JobId]) REFERENCES [JobPosts] ([Id]),
    CONSTRAINT [FK__Applicatio__CVId__1332DBDC] FOREIGN KEY ([CVId]) REFERENCES [CVs] ([Id])
);
GO

CREATE TABLE [CV_Certificates] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CVId] uniqueidentifier NOT NULL,
    [CertificateName] nvarchar(255) NULL,
    [Organization] nvarchar(255) NULL,
    [IssueDate] date NULL,
    [ExpiredDate] date NULL,
    CONSTRAINT [PK__CV_Certi__3214EC075FC69D71] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__CV_Certifi__CVId__0B91BA14] FOREIGN KEY ([CVId]) REFERENCES [CVs] ([Id])
);
GO

CREATE TABLE [CV_Educations] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CVId] uniqueidentifier NOT NULL,
    [SchoolName] nvarchar(255) NULL,
    [Major] nvarchar(255) NULL,
    [StartDate] date NULL,
    [EndDate] date NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK__CV_Educa__3214EC070F7A288C] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__CV_Educati__CVId__01142BA1] FOREIGN KEY ([CVId]) REFERENCES [CVs] ([Id])
);
GO

CREATE TABLE [CV_Experiences] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CVId] uniqueidentifier NOT NULL,
    [CompanyName] nvarchar(255) NULL,
    [Position] nvarchar(255) NULL,
    [StartDate] date NULL,
    [EndDate] date NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK__CV_Exper__3214EC078322F34E] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__CV_Experie__CVId__7D439ABD] FOREIGN KEY ([CVId]) REFERENCES [CVs] ([Id])
);
GO

CREATE TABLE [CV_Projects] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [CVId] uniqueidentifier NOT NULL,
    [ProjectName] nvarchar(255) NULL,
    [Role] nvarchar(255) NULL,
    [StartDate] date NULL,
    [EndDate] date NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK__CV_Proje__3214EC07D91F7C40] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__CV_Project__CVId__04E4BC85] FOREIGN KEY ([CVId]) REFERENCES [CVs] ([Id])
);
GO

CREATE TABLE [CV_Skills] (
    [CVId] uniqueidentifier NOT NULL,
    [SkillName] nvarchar(255) NOT NULL,
    [Level] int NULL,
    CONSTRAINT [PK__CV_Skill__BB2F39F444CE8160] PRIMARY KEY ([CVId], [SkillName]),
    CONSTRAINT [FK__CV_Skills__CVId__07C12930] FOREIGN KEY ([CVId]) REFERENCES [CVs] ([Id])
);
GO

CREATE TABLE [OrderItems] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [OrderId] uniqueidentifier NOT NULL,
    [ServicePackageId] uniqueidentifier NOT NULL,
    [Price] decimal(18,2) NULL,
    [Quantity] int NULL,
    CONSTRAINT [PK__OrderIte__3214EC0752E81A21] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__OrderItem__Order__2645B050] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]),
    CONSTRAINT [FK__OrderItem__Servi__2739D489] FOREIGN KEY ([ServicePackageId]) REFERENCES [ServicePackages] ([Id])
);
GO

CREATE TABLE [Payments] (
    [Id] uniqueidentifier NOT NULL DEFAULT ((newid())),
    [OrderId] uniqueidentifier NOT NULL,
    [PaymentMethod] int NULL,
    [TransactionCode] nvarchar(255) NULL,
    [Amount] decimal(18,2) NULL,
    [Status] int NULL,
    [PaidAt] datetime NULL,
    CONSTRAINT [PK__Payments__3214EC07E13485CE] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__Payments__OrderI__2B0A656D] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id])
);
GO

CREATE INDEX [IX_Applications_CandidateId] ON [Applications] ([CandidateId]);
GO

CREATE INDEX [IX_Applications_CVId] ON [Applications] ([CVId]);
GO

CREATE UNIQUE INDEX [UQ_Application] ON [Applications] ([JobId], [CandidateId]);
GO

CREATE UNIQUE INDEX [UQ__Candidat__1788CC4DB5D0361F] ON [CandidateProfiles] ([UserId]);
GO

CREATE INDEX [IX_CandidateSkills_SkillId] ON [CandidateSkills] ([SkillId]);
GO

CREATE INDEX [IX_CompanySubscriptions_CompanyId] ON [CompanySubscriptions] ([CompanyId]);
GO

CREATE INDEX [IX_CompanySubscriptions_ServicePackageId] ON [CompanySubscriptions] ([ServicePackageId]);
GO

CREATE INDEX [IX_CV_Certificates_CVId] ON [CV_Certificates] ([CVId]);
GO

CREATE INDEX [IX_CV_Educations_CVId] ON [CV_Educations] ([CVId]);
GO

CREATE INDEX [IX_CV_Experiences_CVId] ON [CV_Experiences] ([CVId]);
GO

CREATE INDEX [IX_CV_Projects_CVId] ON [CV_Projects] ([CVId]);
GO

CREATE INDEX [IX_CVs_CandidateId] ON [CVs] ([CandidateId]);
GO

CREATE INDEX [IX_EmployerProfiles_CompanyId] ON [EmployerProfiles] ([CompanyId]);
GO

CREATE UNIQUE INDEX [UQ__Employer__1788CC4D61E45335] ON [EmployerProfiles] ([UserId]);
GO

CREATE INDEX [IX_JobPosts_CompanyId] ON [JobPosts] ([CompanyId]);
GO

CREATE INDEX [IX_JobSkills_SkillId] ON [JobSkills] ([SkillId]);
GO

CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
GO

CREATE INDEX [IX_OrderItems_ServicePackageId] ON [OrderItems] ([ServicePackageId]);
GO

CREATE INDEX [IX_Orders_EmployerId] ON [Orders] ([EmployerId]);
GO

CREATE UNIQUE INDEX [UQ__Orders__999B5229983DD7F5] ON [Orders] ([OrderCode]) WHERE [OrderCode] IS NOT NULL;
GO

CREATE INDEX [IX_Payments_OrderId] ON [Payments] ([OrderId]);
GO

CREATE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);
GO

CREATE INDEX [IX_SavedJobs_JobId] ON [SavedJobs] ([JobId]);
GO

CREATE UNIQUE INDEX [UQ__Skills__737584F6343A08C9] ON [Skills] ([Name]) WHERE [Name] IS NOT NULL;
GO

CREATE UNIQUE INDEX [UQ__Users__A9D105348C8C58BF] ON [Users] ([Email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260225142328_v1', N'8.0.24');
GO

COMMIT;
GO

