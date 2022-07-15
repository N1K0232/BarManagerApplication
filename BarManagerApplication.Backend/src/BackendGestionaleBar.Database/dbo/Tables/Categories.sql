CREATE TABLE [dbo].[Categories] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (256)   NOT NULL,
    [Description] NVARCHAR (512)   NULL,
    [CreatedDate]       DATETIME         NOT NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NOT NULL,
    [LastModifiedDate]  DATETIME         NULL,
    [UpdatedBy]         UNIQUEIDENTIFIER NULL,
    
    PRIMARY KEY(Id),
    FOREIGN KEY(CreatedBy) REFERENCES AspNetUsers(Id),
    FOREIGN KEY(UpdatedBy) REFERENCES AspNetUsers(Id)
);