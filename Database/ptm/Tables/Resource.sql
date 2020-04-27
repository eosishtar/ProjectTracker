CREATE TABLE [ptm].[Resource] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50)  NOT NULL,
    [Title]         VARCHAR (50)  NULL,
    [ContactNumber] VARCHAR (50)  NULL,
    [EmailAddress]  VARCHAR (50)  NULL,
    [IsActive]      BIT           NOT NULL,
    [CreatedDate]   DATETIME2 (7) NOT NULL,
    [ModifiedDate]  DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED ([Id] ASC)
);



